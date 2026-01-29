using System;
using Microsoft.Office.Tools.Ribbon;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Office.Tools;
using System.Windows.Forms;
using System.IO;

namespace TienIchToanHocWord
{
    public partial class MyRibbon
    {
        // Khai báo biến toàn cục để quản lý Task Pane
        private CustomTaskPane taskPaneHienThi;

        /// <summary>
        /// Hàm khởi tạo Ribbon - Designer yêu cầu hàm này
        /// </summary>
        private void MyRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            // Có thể để trống hoặc khởi tạo các thiết lập mặc định tại đây
        }

        /// <summary>
        /// Xử lý nút bấm Casio FX: Kiểm tra đường dẫn, lưu settings và nhúng App
        /// </summary>
        private void btn_Casio_Click(object sender, RibbonControlEventArgs e)
        {
            // 1. Lấy đường dẫn hợp lệ (Sử dụng hàm kiểm tra đã viết ở bước trước)
            string duongDanHopLe = LayDuongDanCasioHopLe();

            // Nếu không chọn được file hoặc file không tồn tại, dừng thực thi
            if (string.IsNullOrEmpty(duongDanHopLe)) return;

            try
            {
                if (taskPaneHienThi == null)
                {
                    // Khởi tạo UserControl mới
                    taskPaneCasio controlCasio = new taskPaneCasio();

                    // Thêm vào tập hợp CustomTaskPanes
                    taskPaneHienThi = Globals.ThisAddIn.CustomTaskPanes.Add(controlCasio, "Máy tính Casio FX");
                    taskPaneHienThi.Width = 450; // Độ rộng mặc định ban đầu
                    taskPaneHienThi.Visible = true;

                    // Thực hiện nhúng ứng dụng và lấy chiều rộng thực tế
                    int chieuRongPixel = controlCasio.LayDoRongChuan(duongDanHopLe);

                    // Tự động điều chỉnh độ rộng Task Pane theo DPI (Logic đã tối ưu)
                    CapNhatDoRongTaskPaneTheoDpi(chieuRongPixel);
                }
                else
                {
                    // Đảo ngược trạng thái hiển thị
                    taskPaneHienThi.Visible = !taskPaneHienThi.Visible;

                    if (taskPaneHienThi.Visible)
                    {
                        // SỬA LỖI TẠI ĐÂY: Sử dụng thuộc tính .Control thay vì .ContentControl
                        // Ép kiểu về taskPaneCasio để gọi hàm LayDoRongChuan
                        if (taskPaneHienThi.Control is taskPaneCasio bienDieuKhien)
                        {
                            int chieuRongMoi = bienDieuKhien.LayDoRongChuan(duongDanHopLe);
                            CapNhatDoRongTaskPaneTheoDpi(chieuRongMoi);
                        }
                    }
                }
            }
            catch (Exception ngoaiLe)
            {
                System.Windows.Forms.MessageBox.Show("Lỗi thực thi Task Pane: " + ngoaiLe.Message);
            }
        }

        /// <summary>
        /// Hàm hỗ trợ tính toán độ rộng Points từ Pixels dựa trên DPI màn hình
        /// </summary>
        private void CapNhatDoRongTaskPaneTheoDpi(int pixelWidth)
        {
            if (pixelWidth <= 0 || taskPaneHienThi == null) return;

            IntPtr hdc = WindowsApiHelper.GetDC(IntPtr.Zero);
            int dpiX = WindowsApiHelper.GetDeviceCaps(hdc, WindowsApiHelper.LOGPIXELSX);
            WindowsApiHelper.ReleaseDC(IntPtr.Zero, hdc);

            float tiLeQuyDoi = 72f / dpiX;
            // Cộng thêm 75 Points bù lề khung Task Pane như đã thống nhất
            int doRongPoint = (int)(pixelWidth * tiLeQuyDoi) + 75;

            if (doRongPoint < 300) doRongPoint = 300;
            if (doRongPoint > 800) doRongPoint = 800;

            taskPaneHienThi.Width = doRongPoint;
        }

        /// <summary>
        /// Hàm thông minh: Kiểm tra đường dẫn lưu trữ, nếu sai hoặc không có thì ép chọn lại.
        /// </summary>
        private string LayDuongDanCasioHopLe()
        {
            string duongDanHienTai = Properties.Settings.Default.DuongDanCasio;

            // Kiểm tra file có tồn tại trên máy này không
            if (string.IsNullOrEmpty(duongDanHienTai) || !System.IO.File.Exists(duongDanHienTai))
            {
                MessageBox.Show("Hệ thống không tìm thấy file chạy máy tính Casio trên máy tính này.\n\nVui lòng chọn đường dẫn đến file .exe của ứng dụng.",
                                "Cấu hình máy tính", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                using (OpenFileDialog openDlg = new OpenFileDialog())
                {
                    openDlg.Filter = "Ứng dụng giả lập (*.exe)|*.exe";
                    openDlg.Title = "Chọn file chạy máy tính Casio (fx-580VN X...)";
                    openDlg.CheckFileExists = true;

                    if (openDlg.ShowDialog() == DialogResult.OK)
                    {
                        // Lưu lại đường dẫn mới vào Settings của máy này
                        Properties.Settings.Default.DuongDanCasio = openDlg.FileName;
                        Properties.Settings.Default.Save();
                        return openDlg.FileName;
                    }
                    else
                    {
                        // Người dùng nhấn Cancel, xóa đường dẫn sai trong Settings để lần sau hỏi lại
                        Properties.Settings.Default.DuongDanCasio = "";
                        Properties.Settings.Default.Save();
                        return null;
                    }
                }
            }
            return duongDanHienTai;
        }
        /// <summary>
        /// Hiển thị đường lưới ô vuông (Logic từ Module HienThi_AnLuoi_oVuong)
        /// </summary>
        private void btn_HienThiLuoi_Click(object sender, RibbonControlEventArgs e)
        {
            Word.Application app = Globals.ThisAddIn.Application;
            Word.Document doc = app.ActiveDocument;

            app.ScreenUpdating = false;
            try
            {
                // Thiết lập lưới 0.3cm chuẩn cho vẽ hình Toán học
                doc.GridDistanceHorizontal = app.CentimetersToPoints(0.3f);
                doc.GridDistanceVertical = app.CentimetersToPoints(0.3f);
                doc.GridOriginHorizontal = app.CentimetersToPoints(0.5f);
                doc.GridOriginVertical = app.CentimetersToPoints(0.5f);

                app.Options.DisplayGridLines = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi hien thi luoi: " + ex.Message);
            }
            finally
            {
                app.ScreenUpdating = true;
            }
        }

        /// <summary>
        /// Ẩn hiển thị đường lưới
        /// </summary>
        private void btn_AnHienThiLuoi_Click(object sender, RibbonControlEventArgs e)
        {
            Globals.ThisAddIn.Application.Options.DisplayGridLines = false;
        }

        /// <summary>
        /// Chuẩn hóa hiển thị (Dựa trên logic ChuanHoa_Azota và ChuaHoaVanBan)
        /// </summary>
        private void btn_ChuanHoaHienThi_Click(object sender, RibbonControlEventArgs e)
        {
            // Khởi tạo và hiển thị Form
            ChuanHoaTrangDayHoc frm = new ChuanHoaTrangDayHoc();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog(); // Dùng ShowDialog để người dùng tập trung xử lý
        }

        private void btn_Xuat_PDF_Click(object sender, RibbonControlEventArgs e)
        {
            // 1. Truy cập ứng dụng Word và tài liệu hiện tại
            Word.Application ungDungWord = Globals.ThisAddIn.Application;
            Word.Document taiLieuHienHanh = ungDungWord.ActiveDocument;

            // 2. Khởi tạo đối tượng từ Class chuyên trách
            xuLyXuatPDF boXuatPdf = new xuLyXuatPDF();

            // 3. Tối ưu hiệu năng: Tắt làm mới màn hình trong khi xuất
            ungDungWord.ScreenUpdating = false;

            try
            {
                // 4. Thực thi nghiệp vụ
                boXuatPdf.ThucHienXuatPDF(taiLieuHienHanh);
            }
            finally
            {
                // 5. Luôn bật lại màn hình dù có lỗi hay không
                ungDungWord.ScreenUpdating = true;
            }
        }
        // Khai báo biến ở cấp độ Class để quản lý duy nhất 1 thực thể Form
        private CanChinhPhuonAnPhamVi _formCanChinh;

        private void btn_CanChinh_PA_PhamVi_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                // Kiểm tra nếu Form chưa được tạo hoặc đã bị đóng (Disposed)
                if (_formCanChinh == null || _formCanChinh.IsDisposed)
                {
                    _formCanChinh = new CanChinhPhuonAnPhamVi();
                }

                // Hiển thị Form dưới dạng Modeless (Cho phép chọn văn bản trong Word)
                // Chúng ta truyền đối tượng Window của Word vào để Form luôn nằm trên
                _formCanChinh.Show();

                // Đưa Form lên trên cùng nếu nó đang bị che khuất
                _formCanChinh.Activate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị giao diện: " + ex.Message);
            }
        }
    }
}