using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using TienIchToanHocWord.XuLyVoiAi;
using Word = Microsoft.Office.Interop.Word;

namespace TienIchToanHocWord.GiaoDienVsto.form_GiaoDien
{
    /// <summary>
    /// Lop UI dieu khien chuc nang chuyen doi PDF/IMG sang Word.
    /// Ho tro: Dan nhieu anh tu Clipboard, Chuyen PDF scan, PDF van ban giu vi tri anh.
    /// </summary>
    public partial class frm_ChuyenDoiPDFIMG2DOC : Form
    {
        // ======================================================
        // DEPENDENCIES & FIELDS
        // ======================================================
        private readonly XuLyChuyenDoiTaiLieuUseCase _useCase;
        private List<string> _danhSachAnhTam = new List<string>();
        private string _cheDoXuLy = "CHI_TEXT";

        // ======================================================
        // CONSTRUCTORS (GIAI PHAP CHO DESIGNER & RUNTIME)
        // ======================================================

        public frm_ChuyenDoiPDFIMG2DOC()
        {
            InitializeComponent();
        }

        public frm_ChuyenDoiPDFIMG2DOC(XuLyChuyenDoiTaiLieuUseCase useCase) : this()
        {
            _useCase = useCase ?? throw new ArgumentNullException(nameof(useCase));

            // Cấu hình bắt phím tắt toàn Form
            this.KeyPreview = true;
            this.KeyDown += Frm_ChuyenDoiPDFIMG2DOC_KeyDown;

            // Khởi tạo trạng thái giao diện
            CapNhatTrangThaiUI();
            GhiLog("He thong chuyen doi da san sang.");
        }

        // ======================================================
        // LOGIC GHI LOG (Bao ve da luong)
        // ======================================================
        private void GhiLog(string noiDung)
        {
            if (richTextBox_TienTrinh.InvokeRequired)
            {
                richTextBox_TienTrinh.Invoke(new Action(() => GhiLog(noiDung)));
                return;
            }
            string thoiGian = DateTime.Now.ToString("HH:mm:ss");
            richTextBox_TienTrinh.AppendText($"[{thoiGian}] {noiDung}\n");
            richTextBox_TienTrinh.ScrollToCaret();
        }

        // ======================================================
        // LOGIC DIEU KHIEN TRANG THAI UI
        // ======================================================
        private void CapNhatTrangThaiUI()
        {
            bool laCheDoTuAnh = rad_TuAnh.Checked;

            // PictureBox: Chỉ mở khi chọn "Từ ảnh"
            pictureBox_DanAnh.Enabled = laCheDoTuAnh;
            pictureBox_DanAnh.BackColor = laCheDoTuAnh ? Color.White : Color.FromArgb(240, 240, 240);

            // Button/TextBox PDF: Khóa khi chọn "Từ ảnh"
            btn_chonPDF.Enabled = !laCheDoTuAnh;
            textBox_DuongDanPDF.Enabled = !laCheDoTuAnh;

            if (laCheDoTuAnh)
            {
                textBox_DuongDanPDF.Text = "Vui lòng dán ảnh vào khung (Ctrl+V)";
            }
            else
            {
                pictureBox_DanAnh.Image = null;
                _danhSachAnhTam.Clear();
                textBox_DuongDanPDF.Text = string.Empty;
            }
        }

        // ======================================================
        // LOGIC XU LY CLIPBOARD (DAN NHIEU ANH)
        // ======================================================
        private void Frm_ChuyenDoiPDFIMG2DOC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V && rad_TuAnh.Checked)
            {
                ThucHienDanAnh();
            }
        }

        private void ThucHienDanAnh()
        {
            try
            {
                if (Clipboard.ContainsImage())
                {
                    Image img = Clipboard.GetImage();
                    pictureBox_DanAnh.Image = img;
                    pictureBox_DanAnh.SizeMode = PictureBoxSizeMode.Zoom;

                    // Tạo thư mục lưu clips tạm
                    string tempFolder = Path.Combine(Path.GetTempPath(), "TienIchWordAI_Clips");
                    if (!Directory.Exists(tempFolder)) Directory.CreateDirectory(tempFolder);

                    // Lưu file ảnh vật lý để gửi cho Python
                    string path = Path.Combine(tempFolder, $"clip_{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid().ToString("N").Substring(0, 8)}.png");
                    img.Save(path, System.Drawing.Imaging.ImageFormat.Png);

                    _danhSachAnhTam.Add(path);
                    GhiLog($"Da nhan anh vao danh sach (Tong: {_danhSachAnhTam.Count} anh).");
                }
            }
            catch (Exception ex)
            {
                GhiLog($"Loi khi dan anh: {GiaiMaUnicodeAnToan(ex.Message)}");
            }
        }

        // ======================================================
        // THUC THI CHINH (ASYNCHRONOUS)
        // ======================================================
        private void btn_chonPDF_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "PDF files (*.pdf)|*.pdf";
                if (dlg.ShowDialog() == DialogResult.OK) textBox_DuongDanPDF.Text = dlg.FileName;
            }
        }

        private async void btn_ThucThiChuyenDoi_Click(object sender, EventArgs e)
        {
            if (_useCase == null) return;

            string maCheDo = LayMaCheDoHienTai();
            object noiDungDauVao = null;

            // 1. Thu thap dau vao theo rẽ nhánh
            if (rad_TuAnh.Checked)
            {
                if (_danhSachAnhTam.Count == 0) { MessageBox.Show("Vui lòng dán ảnh!"); return; }
                noiDungDauVao = _danhSachAnhTam.ToArray();
            }
            else
            {
                string path = textBox_DuongDanPDF.Text;
                if (string.IsNullOrWhiteSpace(path) || !File.Exists(path)) { MessageBox.Show("File không tồn tại!"); return; }
                noiDungDauVao = path;
            }

            // 2. Chuan bi UI
            this.Invoke(new Action(() => { btn_ThucThiChuyenDoi.Enabled = false; }));
            GhiLog($"Dang thuc thi che do {maCheDo}. Vui long cho AI phan hoi...");
            Globals.ThisAddIn.Application.ScreenUpdating = false;

            try
            {
                // 3. Goi Use Case (Async)
                KetQuaAI ketQuaObj = await _useCase.ThucThiChuyenDoiAsync(maCheDo, noiDungDauVao);

                // 4. Giai ma Unicode nhung bao ve LaTeX
                string vanBanHienThi = GiaiMaUnicodeAnToan(ketQuaObj.VanBan);

                // 5. Xuat Word (Invoke ve luong UI)
                this.Invoke(new Action(() => {
                    if (maCheDo == "TEXT_GIU_ANH")
                        ChenVaoWordThongMinh(vanBanHienThi, ketQuaObj.ThuMucTam);
                    else
                        ChenVaoWord(vanBanHienThi);

                    GhiLog("Hoan tat xuat du lieu ra Word.");

                    // 6. Don dep thu muc tam sau khi chen xong
                    try { if (Directory.Exists(ketQuaObj.ThuMucTam)) Directory.Delete(ketQuaObj.ThuMucTam, true); } catch { }
                }));

                // 7. Don dep danh sach anh clipboard
                if (rad_TuAnh.Checked)
                {
                    foreach (var p in _danhSachAnhTam) { try { if (File.Exists(p)) File.Delete(p); } catch { } }
                    _danhSachAnhTam.Clear();
                    this.Invoke(new Action(() => { pictureBox_DanAnh.Image = null; }));
                }
            }
            catch (Exception ex)
            {
                string msg = GiaiMaUnicodeAnToan(ex.Message);
                GhiLog($"LOI: {msg}");
                this.Invoke(new Action(() => { MessageBox.Show(msg, "Loi AI"); }));
            }
            finally
            {
                this.Invoke(new Action(() => { btn_ThucThiChuyenDoi.Enabled = true; }));
                Globals.ThisAddIn.Application.ScreenUpdating = true;
            }
        }

        // ======================================================
        // HELPER METHODS (PRIVATE)
        // ======================================================

        private string LayMaCheDoHienTai()
        {
            if (rad_ChiLayNoiDungTuText.Checked) return "CHI_TEXT";
            if (rad_LayNoiDungDungNguyenAnh.Checked) return "TEXT_GIU_ANH";
            if (rad_TuAnh.Checked) return "TU_ANH";
            if (rad_TuPDF_Anh.Checked) return "PDF_ANH";
            return _cheDoXuLy;
        }

        private string GiaiMaUnicodeAnToan(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            // Giải mã \uXXXX nhưng giữ nguyên \int, \frac, \mathbb (LaTeX)
            return Regex.Replace(input, @"\\u([0-9a-fA-F]{4})", m =>
            {
                try { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); }
                catch { return m.Value; }
            });
        }

        // --- TRONG frm_ChuyenDoiPDFIMG2DOC.cs ---
        private void ChenVaoWordThongMinh(string fullText, string folderTam)
        {
            Word.Application app = Globals.ThisAddIn.Application;
            if (app.ActiveDocument == null) return;
            Word.Selection selection = app.Selection;

            // REGEX NÂNG CAO: 
            // 1. Nhận diện nhãn [[[HINH X]]] hoặc [[HINH X]]
            // 2. (?:\s*\2)? -> "Nuốt" luôn khoảng trắng và con số lặp lại chính xác của ID hình đó (nếu có)
            // Ví dụ: [[[HINH 1]]] 1 -> Sẽ bị khớp toàn bộ và thay thế bằng ảnh, số 1 thừa sẽ biến mất.
            string pattern = @"(\[\[{2,3}HINH(?:\sANH)?\s(\d+)\]{2,3}(?:\s*\2)?)";

            // Sử dụng Regex.Split với Capturing Group để giữ lại các phần tử trong mảng
            string[] parts = Regex.Split(fullText, pattern);

            foreach (var part in parts)
            {
                if (string.IsNullOrEmpty(part)) continue;

                // Kiểm tra xem đoạn này có phải là nhãn ảnh (đã kèm số thừa) không
                var match = Regex.Match(part, @"HINH(?:\sANH)?\s(\d+)");

                if (match.Success)
                {
                    string indexStr = match.Groups[1].Value;
                    string imagePath = Path.Combine(folderTam, "images", $"hinh_{indexStr}.png");

                    if (File.Exists(imagePath))
                    {
                        try
                        {
                            // Chèn ảnh vật lý
                            selection.InlineShapes.AddPicture(imagePath);
                            selection.TypeParagraph(); // Xuống dòng sau ảnh
                        }
                        catch
                        {
                            // Nếu lỗi chèn ảnh, in lại nhãn gốc (không kèm số thừa) để tránh mất dấu
                            selection.TypeText($" [[[HINH ANH {indexStr}]]] ");
                        }
                    }
                    else
                    {
                        // Nếu không thấy file ảnh, in nhãn để người dùng biết
                        selection.TypeText($" [[[HINH ANH {indexStr}]]] ");
                    }
                }
                else
                {
                    // Kiểm tra xem đoạn văn bản này có phải là con số mồ côi (1, 2) do AI tạo ra không
                    // Nếu đoạn text chỉ chứa đúng số ID của hình vừa chèn và rất ngắn, ta có thể bỏ qua
                    // Tuy nhiên, logic Regex (?:\s*\2)? ở trên đã xử lý 99% trường hợp này.
                    selection.TypeText(part);
                }
            }
        }

        private void ChenVaoWord(string text)
        {
            Word.Application app = Globals.ThisAddIn.Application;
            if (app.ActiveDocument == null) return;
            Word.Range vungChen = app.Selection?.Range;
            if (vungChen != null && vungChen.Start != vungChen.End) vungChen.Text = text;
            else app.ActiveDocument.Content.InsertAfter(text);
            app.ActiveDocument.ActiveWindow.View.Type = app.ActiveDocument.ActiveWindow.View.Type;
        }

        private void rad_All_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton rb && rb.Checked)
            {
                CapNhatTrangThaiUI();
                _cheDoXuLy = LayMaCheDoHienTai();
                GhiLog($"Da chuyen sang che do: {rb.Text}");
            }
        }

        private void btn_XuatWord_Rieng_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chuc nang xuat Word rieng dang duoc phat trien.");
        }
    }
}