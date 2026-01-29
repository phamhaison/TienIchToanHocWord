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
            string duongDanCasio = Properties.Settings.Default.DuongDanCasio;
            // ... (Giữ nguyên logic kiểm tra file tồn tại) ...

            try
            {
                if (taskPaneHienThi == null)
                {
                    taskPaneCasio controlCasio = new taskPaneCasio();
                    taskPaneHienThi = Globals.ThisAddIn.CustomTaskPanes.Add(controlCasio, "Máy tính Casio FX");

                    int pixelWidth = controlCasio.LayDoRongChuan(duongDanCasio);

                    if (pixelWidth > 0)
                    {
                        // Lấy DPI màn hình để quy đổi đơn vị
                        IntPtr hdc = WindowsApiHelper.GetDC(IntPtr.Zero);
                        int dpiX = WindowsApiHelper.GetDeviceCaps(hdc, WindowsApiHelper.LOGPIXELSX);
                        WindowsApiHelper.ReleaseDC(IntPtr.Zero, hdc);

                        float tiLeQuyDoi = 72f / dpiX;

                        // TÍNH TOÁN ĐỘ RỘNG THÔNG MINH:
                        // 1. Quy đổi chiều rộng máy tính sang Point
                        // 2. Cộng thêm 75 Points (~2.6cm) để bù cho lề Task Pane, thanh cuộn và khung Windows
                        int doRongPoint = (int)(pixelWidth * tiLeQuyDoi) + 75;

                        if (doRongPoint < 300) doRongPoint = 300;
                        if (doRongPoint > 800) doRongPoint = 800;

                        taskPaneHienThi.Width = doRongPoint;
                    }
                    taskPaneHienThi.Visible = true;
                }
                else
                {
                    taskPaneHienThi.Visible = !taskPaneHienThi.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị: " + ex.Message);
            }
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