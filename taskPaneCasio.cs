using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace TienIchToanHocWord
{
    // Kế thừa từ UserControl để có hàm Dispose và các thuộc tính WinForms
    public partial class taskPaneCasio : UserControl
    {
        private Process quyTrinhCasio;

        public taskPaneCasio()
        {
            // Hàm này bây giờ đã tồn tại nhờ Bước 1
            InitializeComponent();
        }
        public int LayDoRongChuan(string duongDanExe)
        {
            if (quyTrinhCasio != null && !quyTrinhCasio.HasExited) return this.pnlCasio.Width;

            try
            {
                quyTrinhCasio = Process.Start(duongDanExe);

                int demCho = 0;
                while (quyTrinhCasio.MainWindowHandle == IntPtr.Zero && demCho < 40)
                {
                    Thread.Sleep(200);
                    quyTrinhCasio.Refresh();
                    demCho++;
                }

                IntPtr handleCasio = quyTrinhCasio.MainWindowHandle;
                if (handleCasio != IntPtr.Zero)
                {
                    // 1. Gọt viền và ép kiểu cửa sổ con (WS_CHILD)
                    int styleHienTai = WindowsApiHelper.GetWindowLong(handleCasio, WindowsApiHelper.GWL_STYLE);
                    int styleMoi = (int)((styleHienTai & ~WindowsApiHelper.WS_POPUP & ~WindowsApiHelper.WS_CAPTION & ~WindowsApiHelper.WS_THICKFRAME) | WindowsApiHelper.WS_CHILD);
                    WindowsApiHelper.SetWindowLong(handleCasio, WindowsApiHelper.GWL_STYLE, styleMoi);

                    // 2. Nhúng vào Panel của Word
                    WindowsApiHelper.SetParent(handleCasio, this.pnlCasio.Handle);

                    // 3. Lấy kích thước thực tế
                    WindowsApiHelper.RECT rect;
                    WindowsApiHelper.GetClientRect(handleCasio, out rect);
                    int w = rect.Right - rect.Left;
                    int h = rect.Bottom - rect.Top;

                    // 4. Ép vẽ lại nội dung (Sửa lỗi màn hình trắng)
                    WindowsApiHelper.SetWindowPos(handleCasio, IntPtr.Zero, 0, 0, w, h,
                        WindowsApiHelper.SWP_NOZORDER | WindowsApiHelper.SWP_FRAMECHANGED | WindowsApiHelper.SWP_SHOWWINDOW);

                    return w;
                }
            }
            catch (Exception) { /* Xử lý log nếu cần */ }
            return 0;
        }
        public int LayChieuRongPixelThucTe(string duongDanExe)
        {
            try
            {
                if (!System.IO.File.Exists(duongDanExe)) return 0;
                quyTrinhCasio = Process.Start(duongDanExe);

                int demCho = 0;
                while (quyTrinhCasio.MainWindowHandle == IntPtr.Zero && demCho < 30)
                {
                    Thread.Sleep(200);
                    quyTrinhCasio.Refresh();
                    demCho++;
                }

                IntPtr handleCasio = quyTrinhCasio.MainWindowHandle;
                if (handleCasio != IntPtr.Zero)
                {
                    WindowsApiHelper.RECT rect;
                    WindowsApiHelper.GetWindowRect(handleCasio, out rect);
                    int pixelWidth = rect.Right - rect.Left;

                    // Nhúng vào Panel
                    WindowsApiHelper.SetParent(handleCasio, this.pnlCasio.Handle);
                    // Ép cửa sổ Casio tràn đầy Panel của chúng ta
                    WindowsApiHelper.MoveWindow(handleCasio, 0, 0, pixelWidth, rect.Bottom - rect.Top, true);

                    return pixelWidth;
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return 0;
        }
        public int KhoiDongVaNhungCasio(string duongDanExe)
        {
            try
            {
                if (!System.IO.File.Exists(duongDanExe)) return 0;

                quyTrinhCasio = Process.Start(duongDanExe);

                int demCho = 0;
                while (quyTrinhCasio.MainWindowHandle == IntPtr.Zero && demCho < 30)
                {
                    Thread.Sleep(200);
                    quyTrinhCasio.Refresh();
                    demCho++;
                }

                IntPtr handleCasio = quyTrinhCasio.MainWindowHandle;

                if (handleCasio != IntPtr.Zero)
                {
                    // Lấy kích thước thực tế của cửa sổ Casio
                    WindowsApiHelper.RECT kichThuocCuaSo;
                    WindowsApiHelper.GetWindowRect(handleCasio, out kichThuocCuaSo);
                    int chieuRongPixel = kichThuocCuaSo.Right - kichThuocCuaSo.Left;
                    int chieuCaoPixel = kichThuocCuaSo.Bottom - kichThuocCuaSo.Top;

                    // Thực hiện nhúng vào Panel
                    WindowsApiHelper.SetParent(handleCasio, this.pnlCasio.Handle);

                    // Căn chỉnh cửa sổ Casio tràn đầy Panel trong Task Pane
                    WindowsApiHelper.MoveWindow(handleCasio, 0, 0, chieuRongPixel, chieuCaoPixel, true);

                    // Trả về chiều rộng Pixel gốc
                    return chieuRongPixel;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Loi nhung Casio: " + ex.Message);
            }
            return 0;
        }
    }
}