
using Microsoft.Office.Interop.Word;
using Microsoft.Office.Tools;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TienIchToanHocWord.GiaoDienVsto.form_GiaoDien;
using TienIchToanHocWord.GiaoDienVsto.task_panel;
using TienIchToanHocWord.HaTang.HeThong;
using TienIchToanHocWord.HaTang.LuuTru;
using TienIchToanHocWord.UngDung;
using TienIchToanHocWord.XuLyVoiAi;
using Word = Microsoft.Office.Interop.Word;

namespace TienIchToanHocWord.GiaoDienVsto.Ribbon
{
    public partial class MyRibbon
    {
        private LopLatexToEquation _boXuLyCongThuc;
        private frm_TaoBookMark _formBM;
        private CanChinhPhuonAnPhamVi _formCC;
        private CustomTaskPane _ctpCasio;

        private void MyRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            // Ép hệ thống đợi Word ổn định rồi mới nạp logic
            try
            {
                // Khởi tạo lớp xử lý công thức toán một cách an toàn
                if (_boXuLyCongThuc == null)
                {
                    _boXuLyCongThuc = new LopLatexToEquation();
                }
            }
            catch (Exception ex)
            {
                // Ghi log lỗi vào cửa sổ Output của Visual Studio thay vì hiện thông báo làm sập Ribbon
                System.Diagnostics.Debug.WriteLine("Lỗi nạp Ribbon: " + ex.Message);
            }
        }

        private frm_ChuyenDoiPDFIMG2DOC _frmChuyenDoi;

        private void btn_TacVuAi_Click(object sender, Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs e)
        {
            // Đảm bảo đã có using System.Linq; ở đầu file để dùng FirstOrDefault

            // SỬA LỖI 1: Đổi .Content thành .Control
            var taskPane = Globals.ThisAddIn.CustomTaskPanes
                .Cast<Microsoft.Office.Tools.CustomTaskPane>()
                .FirstOrDefault(tp => tp.Control is TienIchToanHocWord.GiaoDienVsto.task_panel.TaskPanel_TacVuAi);

            if (taskPane != null)
            {
                taskPane.Visible = !taskPane.Visible;
            }
            else
            {
                // SỬA LỖI 2: Bây giờ boTacVuAiUseCase đã tồn tại trong ThisAddIn sau khi làm Bước 1
                var control = new TienIchToanHocWord.GiaoDienVsto.task_panel.TaskPanel_TacVuAi(Globals.ThisAddIn.boTacVuAiUseCase);
                var newPane = Globals.ThisAddIn.CustomTaskPanes.Add(control, "Tác vụ AI");
                newPane.Width = 350;
                newPane.Visible = true;
            }
        }
        private void btn_ChuyenDoiPdf_Click(object sender, RibbonControlEventArgs e)
        {
            if (_frmChuyenDoi == null || _frmChuyenDoi.IsDisposed)
            {
                // LẤY USE CASE TỪ COMPOSITION ROOT (ThisAddIn)
                XuLyChuyenDoiTaiLieuUseCase useCase = Globals.ThisAddIn.boXuLyChuyenDoiTaiLieu;

                if (useCase == null)
                {
                    // THONG BAO LOI KHI CHUA KHOI TAO
                    MessageBox.Show("He thong chuyen doi AI chua san sang. Vui long khoi dong lai Word.", "Loi Khoi Tao He Thong");
                    return;
                }

                // KHỞI TẠO FORM VÀ TRUYỀN USE CASE QUA CONSTRUCTOR
                _frmChuyenDoi = new frm_ChuyenDoiPDFIMG2DOC(useCase);
            }

            _frmChuyenDoi.Show();
            _frmChuyenDoi.BringToFront();
        }




        // [FILE: MyRibbon.cs]
        // Đảm bảo trong file Ribbon.xml, id của nút là "btn_ChuyenDoiLT_EQ" 
        // và onAction trỏ đến "btn_ChuyenDoiLT_EQ_Click"
        private void btn_NhapApiKey_Click(object sender, RibbonControlEventArgs e)
        {
            // Thay đổi kiểu dữ liệu và tên biến property từ ThisAddIn
            TienIchToanHocWord.HaTang.LuuTru.RepositoryAI repo = Globals.ThisAddIn.repositoryAI;

            using (var form = new TienIchToanHocWord.GiaoDienVsto.form_GiaoDien.FormNhapApiKey(repo))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    CapNhatTrangThaiNutApiKey();
                }
            }
        }

        private void CapNhatTrangThaiNutApiKey()
        {
            // Truy cập vào Repository mới đã được khởi tạo trong ThisAddIn_Startup
            var repo = Globals.ThisAddIn.repositoryAI;

            if (repo != null)
            {
                int soLuong = repo.DemSoApiKey(); // Sử dụng hàm đã viết trong RepositoryAI
                if (soLuong > 0)
                {
                    btn_NhapApiKey.Label = soLuong + " API key";
                }
                else
                {
                    btn_NhapApiKey.Label = "Nhập API Key";
                }
            }
        }

        private void btn_CauHinhPython_Click(object sender, RibbonControlEventArgs e)
        {
            using (OpenFileDialog openDlg = new OpenFileDialog())
            {
                // ... (Khai báo OpenFileDialog giữ nguyên) ...
                if (openDlg.ShowDialog() == DialogResult.OK)
                {
                    // SỬ DỤNG NAMESPACE ĐẦY ĐỦ VÀ LƯU PATH
                    TienIchToanHocWord.Properties.Settings.Default.DuongDanPythonExe = openDlg.FileName;
                    TienIchToanHocWord.Properties.Settings.Default.Save();

                    // ==============================================================
                    // BỔ SUNG: RE-INITIALIZE AI GATEWAY NGAY LẬP TỨC
                    // ==============================================================
                    try
                    {
                        // Gọi lại logic khởi tạo (Composition Root) cho AI Gateway
                        string duongDanScripts = AppDomain.CurrentDomain.BaseDirectory;
                        // Khởi tạo lại boCauNoiVoiPython
                        Globals.ThisAddIn.boCauNoiVoiPython = new CauNoiVoiPython(duongDanScripts);
                        MessageBox.Show("Đã lưu đường dẫn Python THÀNH CÔNG và khởi tạo lại hệ thống AI.", "Cấu hình thành công");
                    }
                    catch (FileNotFoundException ex)
                    {
                        // Bắt lỗi nếu sau khi lưu Python.exe, script xu_ly_ai.py vẫn không tìm thấy
                        MessageBox.Show($"Lỗi: {ex.Message}. Vui lòng kiểm tra file xu_ly_ai.py.", "Lỗi Cấu Hình AI");
                    }
                    catch (Exception ex)
                    {
                        // Bắt lỗi khác
                        MessageBox.Show($"Lỗi khởi tạo hệ thống: {ex.Message}", "Lỗi Cấu Hình");
                    }

                    // XÓA DÒNG CŨ: MessageBox.Show("Đã lưu đường dẫn Python. Vui lòng khởi động lại Word để áp dụng.", "Cấu hình thành công");
                }
            }
        }

        // FIX: Đổi tên sự kiện Click để khớp với yêu cầu mới và file Ribbon XML/Designer
        private void btn_ChuyenDoiLT_EQ_Click(object sender, RibbonControlEventArgs e)
        {
            Word.Range vungChon = Globals.ThisAddIn.Application.Selection.Range;
            Globals.ThisAddIn.boXuLyCongThuc?.ChuyenDoiLT_SangEQ(vungChon);
        }

        // FIX: Cập nhật lại các hàm bị gọi sai tên (lỗi CS1061)
        private void btn_LaTex2MT_Click(object sender, RibbonControlEventArgs e)
        {
            Word.Range vungChon = Globals.ThisAddIn.Application.Selection.Range;
            Globals.ThisAddIn.boChuyenCongThucSangMT?.LatexSangMathTypeVungChon(vungChon);
        }

        private void btn_Selec2MT_Click(object sender, RibbonControlEventArgs e)
        {
            Word.Range vungChon = Globals.ThisAddIn.Application.Selection.Range;
            Globals.ThisAddIn.boChuyenCongThucSangMT?.LuaChonSangMathType(vungChon);
        }

        private void btn_TaoBookMark_Click(object sender, RibbonControlEventArgs e)
        {
            if (_formBM == null || _formBM.IsDisposed) _formBM = new frm_TaoBookMark();
            _formBM.Show();
            _formBM.Activate();
        }

        private void btn_ChuanHoaHienThi_Click(object sender, RibbonControlEventArgs e)
        {
            using (var frm = new ChuanHoaTrangDayHoc())
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog();
            }
        }

        private void btn_CanChinh_PA_PhamVi_Click(object sender, RibbonControlEventArgs e)
        {
            if (_formCC == null || _formCC.IsDisposed) _formCC = new CanChinhPhuonAnPhamVi();
            _formCC.Show();
            _formCC.Activate();
        }

        private void btn_Xuat_PDF_Click(object sender, RibbonControlEventArgs e)
        {
            new xuLyXuatPDF().ThucHienXuatPDF(Globals.ThisAddIn.Application.ActiveDocument);
        }

        private void btn_Casio_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                // 1. Lấy và kiểm tra đường dẫn file thực thi
                string duongDanCasio = Properties.Settings.Default.DuongDanCasio;

                // Kiểm tra tính tồn tại thực tế của file
                if (string.IsNullOrEmpty(duongDanCasio) || !System.IO.File.Exists(duongDanCasio))
                {
                    MessageBox.Show("Hệ thống không tìm thấy file chạy máy tính Casio.\nVui lòng chọn đường dẫn đến file .exe của ứng dụng.",
                                    "Cấu hình hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    using (OpenFileDialog openDlg = new OpenFileDialog())
                    {
                        openDlg.Filter = "Ứng dụng giả lập (*.exe)|*.exe";
                        openDlg.Title = "Chọn file chạy máy tính Casio";
                        openDlg.CheckFileExists = true;

                        if (openDlg.ShowDialog() == DialogResult.OK)
                        {
                            Properties.Settings.Default.DuongDanCasio = openDlg.FileName;
                            Properties.Settings.Default.Save();
                            duongDanCasio = openDlg.FileName;
                        }
                        else
                        {
                            return; // Người dùng hủy chọn
                        }
                    }
                }

                // 2. Quản lý hiển thị Task Pane
                if (_ctpCasio == null)
                {
                    // Khởi tạo UserControl và Task Pane lần đầu
                    taskPaneCasio controlCasio = new taskPaneCasio();
                    _ctpCasio = Globals.ThisAddIn.CustomTaskPanes.Add(controlCasio, "Máy tính Casio FX");

                    // Hiển thị trước để lấy Handle của Panel
                    _ctpCasio.Visible = true;

                    // Thực hiện nhúng và lấy chiều rộng Pixel thực tế
                    int pixelWidth = controlCasio.LayDoRongChuan(duongDanCasio);

                    // Cập nhật độ rộng Task Pane phù hợp với DPI màn hình (2880x1920)
                    CapNhatDoRongTaskPane(pixelWidth);
                }
                else
                {
                    // Đảo ngược trạng thái hiển thị
                    _ctpCasio.Visible = !_ctpCasio.Visible;

                    // Nếu bật lại, kiểm tra xem ứng dụng Casio còn sống không, nếu không thì nhúng lại
                    if (_ctpCasio.Visible && _ctpCasio.Control is taskPaneCasio control)
                    {
                        int pixelWidth = control.LayDoRongChuan(duongDanCasio);
                        CapNhatDoRongTaskPane(pixelWidth);
                    }
                }
            }
            catch (Exception ex)
            {
                // Bẫy lỗi để tránh việc Word vô hiệu hóa Add-in
                MessageBox.Show("Không thể khởi động máy tính Casio.\nChi tiết lỗi: " + ex.Message,
                                "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Xóa cache đường dẫn nếu lỗi liên quan đến file
                if (ex is System.ComponentModel.Win32Exception)
                {
                    Properties.Settings.Default.DuongDanCasio = "";
                    Properties.Settings.Default.Save();
                }
            }
        }

        /// <summary>
        /// Hàm bổ trợ tính toán độ rộng chuẩn xác cho màn hình độ phân giải cao
        /// </summary>
        private void CapNhatDoRongTaskPane(int pxWidth)
        {
            if (pxWidth <= 0 || _ctpCasio == null) return;

            try
            {
                // Lấy chỉ số DPI thực tế của màn hình
                IntPtr hdc = WindowsApiHelper.GetDC(IntPtr.Zero);
                if (hdc != IntPtr.Zero)
                {
                    int dpiX = WindowsApiHelper.GetDeviceCaps(hdc, 88); // 88 = LOGPIXELSX
                    WindowsApiHelper.ReleaseDC(IntPtr.Zero, hdc);

                    // Công thức quy đổi: Points = Pixels * (72 / DPI)
                    float heSoQuyDoi = 72f / dpiX;

                    // Tính toán độ rộng: Chiều rộng máy tính + 85 Points lề đệm (Padding)
                    int doRongPoint = (int)(pxWidth * heSoQuyDoi) + 85;

                    // Giới hạn an toàn cho giao diện Word
                    if (doRongPoint < 300) doRongPoint = 300;
                    if (doRongPoint > 900) doRongPoint = 900;

                    _ctpCasio.Width = doRongPoint;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi tính DPI: " + ex.Message);
                // Nếu lỗi DPI, đặt độ rộng mặc định an toàn
                _ctpCasio.Width = 450;
            }
        }

        private void btn_HienThiLuoi_Click(object sender, RibbonControlEventArgs e)
        {
            var app = Globals.ThisAddIn.Application;
            app.ActiveDocument.GridDistanceHorizontal = app.CentimetersToPoints(0.3f);
            app.ActiveDocument.GridDistanceVertical = app.CentimetersToPoints(0.3f);
            app.Options.DisplayGridLines = true;
        }

        private void btn_AnHienThiLuoi_Click(object sender, RibbonControlEventArgs e)
        {
            Globals.ThisAddIn.Application.Options.DisplayGridLines = false;
        }
    }
}