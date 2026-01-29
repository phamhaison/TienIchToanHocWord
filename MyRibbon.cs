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
            // 1. Lấy đường dẫn và kiểm tra tính tồn tại
            string duongDanCasio = Properties.Settings.Default.DuongDanCasio;

            if (string.IsNullOrEmpty(duongDanCasio) || !System.IO.File.Exists(duongDanCasio))
            {
                MessageBox.Show("Hệ thống chưa có đường dẫn máy tính Casio hoặc đường dẫn cũ không còn đúng.\n\nVui lòng chọn file .exe của máy tính Casio.",
                    "Cấu hình hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);

                using (OpenFileDialog openDlg = new OpenFileDialog())
                {
                    openDlg.Filter = "Ứng dụng giả lập (*.exe)|*.exe";
                    openDlg.Title = "Tìm file chạy máy tính Casio";
                    if (openDlg.ShowDialog() == DialogResult.OK)
                    {
                        Properties.Settings.Default.DuongDanCasio = openDlg.FileName;
                        Properties.Settings.Default.Save();
                        duongDanCasio = openDlg.FileName;
                    }
                    else return; // Người dùng hủy chọn
                }
            }

            // 2. Xử lý hiển thị Task Pane
            try
            {
                if (taskPaneHienThi == null)
                {
                    taskPaneCasio controlCasio = new taskPaneCasio();
                    taskPaneHienThi = Globals.ThisAddIn.CustomTaskPanes.Add(controlCasio, "Máy tính Casio FX");
                    taskPaneHienThi.Visible = true;

                    // Gọi hàm nhúng và lấy chiều rộng Pixel
                    int pixelWidth = controlCasio.KhoiDongVaNhungCasio(duongDanCasio);

                    // Cập nhật độ rộng thông minh theo DPI (Hàm đã viết ở bước trước)
                    CapNhatDoRongTaskPaneTheoDpi(pixelWidth);
                }
                else
                {
                    taskPaneHienThi.Visible = !taskPaneHienThi.Visible;
                    if (taskPaneHienThi.Visible)
                    {
                        // Nếu bật lại, ép nhúng lại để tránh mất hình do thay đổi độ phân giải
                        var control = (taskPaneCasio)taskPaneHienThi.Control;
                        control.KhoiDongVaNhungCasio(duongDanCasio);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi động: " + ex.Message);
            }
        }

        /// <summary>
        /// Hàm hỗ trợ tính toán độ rộng Points từ Pixels dựa trên DPI màn hình
        /// </summary>
        private void CapNhatDoRongTaskPaneTheoDpi(int pixelWidth)
        {
            if (pixelWidth <= 0 || taskPaneHienThi == null) return;

            // Lấy thông số màn hình
            IntPtr hdc = WindowsApiHelper.GetDC(IntPtr.Zero);
            int dpiX = WindowsApiHelper.GetDeviceCaps(hdc, WindowsApiHelper.LOGPIXELSX);
            WindowsApiHelper.ReleaseDC(IntPtr.Zero, hdc);

            // Thuật toán Architect: 
            // Trên màn hình 2880x1920, DPI thường là 192 (Scale 200%) hoặc 144 (Scale 150%)
            // Points = Pixels * (72 / DPI)
            float tiLeQuyDoi = 72f / dpiX;

            // Bù trừ lề: Trên màn hình độ phân giải cao, lề Task Pane chiếm nhiều Points hơn
            // Chúng ta dùng 80 Points để đảm bảo an toàn
            int doRongPoint = (int)(pixelWidth * tiLeQuyDoi) + 80;

            if (doRongPoint < 300) doRongPoint = 300;
            if (doRongPoint > 900) doRongPoint = 900; // Nới lỏng giới hạn cho màn hình lớn

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