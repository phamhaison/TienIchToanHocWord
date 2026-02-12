using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Windows.Forms; // Cần dùng để hiển thị OpenFileDialog
using TienIchToanHocWord.Properties; // Truy cập Settings.Default

namespace TienIchToanHocWord.XuLyVoiAi
{
    /// <summary>
    /// Lop chua ket qua tra ve tu AI, bao gom noi dung van ban va duong dan chua tai nguyen (anh).
    /// </summary>
    public class KetQuaAI
    {
        public string VanBan { get; set; }
        public string ThuMucTam { get; set; }
    }

    /// <summary>
    /// Lop AI Gateway (Cau noi). Chuyen trach thuc thi Python Process va quan ly I/O file tam thoi.
    /// Logic tu dong tim kiem thong minh ket hop voi co che tu phuc hoi duong dan.
    /// </summary>
    public class CauNoiVoiPython
    {
        // =================================================================
        // KHAI BÁO NỘI BỘ (Infrastructure Details)
        // =================================================================
        private readonly string _duongDanPythonExe;     // Duong dan python.exe hop le
        private readonly string _thuMucScript;          // Thu muc chua cac file script .py
        private const string MAIN_SCRIPT = "xu_ly_ai.py"; // Script goc de dinh vi thu muc

        /// <summary>
        /// Constructor: Tu dong tim kiem moi truong Python va Script.
        /// Neu thieu se tu dong hien thi hop thoai yeu cau nguoi dung chi dinh.
        /// </summary>
        /// <param name="baseDir">Thu muc bat dau tim kiem (BaseDirectory).</param>
        public CauNoiVoiPython(string baseDir)
        {
            // 1. DINH VI PYTHON.EXE (Tu dong -> Settings -> Thu cong)
            _duongDanPythonExe = DinhViPythonExeChuyenNghiep();

            // 2. DINH VI THU MUC CHUA SCRIPTS (Dua tren file xu_ly_ai.py)
            _thuMucScript = DinhViThuMucScriptChuyenNghiep(baseDir);
        }

        // =========================================================
        // LOGIC ĐỊNH VỊ PYTHON.EXE
        // =========================================================
        private string DinhViPythonExeChuyenNghiep()
        {
            string path = TimDuongDanPythonHopLe();
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                path = Settings.Default.DuongDanPythonExe;
            }

            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                MessageBox.Show("He thong khong tim thay moi truong Python. Vui long chi dinh file 'python.exe'.", "Cau hinh AI", MessageBoxButtons.OK, MessageBoxIcon.Information);
                using (OpenFileDialog dlg = new OpenFileDialog { Filter = "Python Executable|python.exe", Title = "Chon file python.exe" })
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        path = dlg.FileName;
                        Settings.Default.DuongDanPythonExe = path;
                        Settings.Default.Save();
                    }
                    else throw new Exception("Hanh lang AI: Khong co python.exe, khong the thuc thi.");
                }
            }
            return path;
        }

        // =========================================================
        // LOGIC ĐỊNH VỊ THƯ MỤC SCRIPTS
        // =========================================================
        private string DinhViThuMucScriptChuyenNghiep(string baseDir)
        {
            string foundFilePath = "";
            string currentCheckDir = baseDir;

            // Chien luoc Parent Crawling: Tim file xu_ly_ai.py de xac dinh thu muc goc cua AI
            for (int i = 0; i < 5; i++)
            {
                string p1 = Path.Combine(currentCheckDir, MAIN_SCRIPT);
                string p2 = Path.Combine(currentCheckDir, "05_XuLyVoiAi", MAIN_SCRIPT);
                string p3 = Path.Combine(currentCheckDir, "07_xu_ly_ai", MAIN_SCRIPT);

                if (File.Exists(p1)) { foundFilePath = p1; break; }
                if (File.Exists(p2)) { foundFilePath = p2; break; }
                if (File.Exists(p3)) { foundFilePath = p3; break; }

                var parent = Directory.GetParent(currentCheckDir);
                if (parent == null) break;
                currentCheckDir = parent.FullName;
            }

            if (string.IsNullOrEmpty(foundFilePath))
            {
                MessageBox.Show($"Khong tim thay bo kich ban AI ({MAIN_SCRIPT}). Vui long chi dinh file nay.", "Cau hinh AI", MessageBoxButtons.OK, MessageBoxIcon.Information);
                using (OpenFileDialog dlg = new OpenFileDialog { Filter = "Python Script|*.py", FileName = MAIN_SCRIPT })
                {
                    if (dlg.ShowDialog() == DialogResult.OK) foundFilePath = dlg.FileName;
                    else throw new Exception("Hanh lang AI: Thieu file kich ban, khong the xu ly.");
                }
            }
            return Path.GetDirectoryName(foundFilePath);
        }

        // =========================================================
        // PHƯƠNG THỨC GIAO TIẾP CHÍNH (PUBLIC)
        // =========================================================
        /// <summary>
        /// Thuc thi xu ly AI. Cho phep tuy chon Script (Mac dinh la xu_ly_ai.py).
        /// </summary>
        /// <param name="jsonInput">Du lieu JSON tu Use Case.</param>
        /// <param name="scriptName">Ten file script muon chay (vd: Tac_Vu_Ai.py).</param>
        public async Task<KetQuaAI> ThucThiXuLyAiAsync(string jsonInput, string scriptName = "xu_ly_ai.py")
        {
            string fullScriptPath = Path.Combine(_thuMucScript, scriptName);

            if (!File.Exists(fullScriptPath))
            {
                throw new FileNotFoundException($"Khong tim thay script yeu cau: {scriptName} tai {_thuMucScript}");
            }

            return await ThucThiScriptAsync(fullScriptPath, jsonInput);
        }

        // =========================================================
        // LOGIC THỰC THI PROCESS (PRIVATE)
        // =========================================================
        private async Task<KetQuaAI> ThucThiScriptAsync(string scriptPath, string jsonInput)
        {
            string baseAppDir = AppDomain.CurrentDomain.BaseDirectory;
            string tempDir = Path.Combine(baseAppDir, "AiTemp_" + Guid.NewGuid().ToString("N").Substring(0, 8));
            Directory.CreateDirectory(tempDir);

            string fileInputJson = Path.Combine(tempDir, "input.json");
            string fileOutputTxt = Path.Combine(tempDir, "output.txt");
            bool canGiuLaiThuMuc = false;

            try
            {
                JObject jo = JObject.Parse(jsonInput);
                jo["output_path"] = fileOutputTxt;
                jo["db_path"] = Path.Combine(baseAppDir, "ai_toan_hoc.db");

                File.WriteAllText(fileInputJson, jo.ToString(), new UTF8Encoding(false));

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = _duongDanPythonExe,
                    Arguments = $"\"{scriptPath}\" \"{fileInputJson}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8
                };

                using (Process p = Process.Start(psi))
                {
                    Task<string> errorTask = p.StandardError.ReadToEndAsync();
                    bool finished = await Task.Run(() => p.WaitForExit(120000));
                    string stderr = await errorTask;

                    if (!finished) { p.Kill(); throw new TimeoutException("Python bi treo (120s)."); }
                    if (p.ExitCode != 0) throw new Exception($"Python Error (Code {p.ExitCode}): {stderr}");

                    if (File.Exists(fileOutputTxt))
                    {
                        string text = File.ReadAllText(fileOutputTxt, Encoding.UTF8);
                        if (!string.IsNullOrWhiteSpace(text))
                        {
                            canGiuLaiThuMuc = true;
                            return new KetQuaAI { VanBan = text, ThuMucTam = tempDir };
                        }
                    }
                    throw new Exception("AI khong tra ve ket qua.");
                }
            }
            catch (Exception) { canGiuLaiThuMuc = false; throw; }
            finally
            {
                if (!canGiuLaiThuMuc && Directory.Exists(tempDir))
                {
                    try { Directory.Delete(tempDir, true); } catch { }
                }
            }
        }

        // =========================================================
        // HÀM TÌM KIẾM PYTHON (STATIC)
        // =========================================================
        private static string TimDuongDanPythonHopLe()
        {
            string local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string[] paths = {
                Path.Combine(local, @"Programs\Python\Python312\python.exe"),
                Path.Combine(local, @"Programs\Python\Python311\python.exe"),
                @"C:\Python312\python.exe", @"C:\Python311\python.exe"
            };
            foreach (string p in paths) if (File.Exists(p)) return p;

            string pathEnv = Environment.GetEnvironmentVariable("PATH");
            if (pathEnv != null)
                foreach (string p in pathEnv.Split(';'))
                {
                    string full = Path.Combine(p.Trim(), "python.exe");
                    if (File.Exists(full)) return full;
                }
            return null;
        }
    }
}