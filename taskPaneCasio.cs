using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace TienIchToanHocWord
{
    public partial class taskPaneCasio : UserControl
    {
        private Process quyTrinhCasio;

        public taskPaneCasio()
        {
            InitializeComponent();
        }

        public int KhoiDongVaNhungCasio(string duongDanExe)
        {
            try
            {
                // Nếu đã chạy rồi thì chỉ cần căn chỉnh lại
                if (quyTrinhCasio != null && !quyTrinhCasio.HasExited)
                {
                    return DinhViLaiCuaSo();
                }

                quyTrinhCasio = Process.Start(duongDanExe);

                // Chờ ứng dụng khởi tạo (Tăng thời gian chờ cho máy độ phân giải cao)
                int demCho = 0;
                while (quyTrinhCasio.MainWindowHandle == IntPtr.Zero && demCho < 50)
                {
                    Thread.Sleep(200);
                    quyTrinhCasio.Refresh();
                    demCho++;
                }

                IntPtr handleCasio = quyTrinhCasio.MainWindowHandle;
                if (handleCasio != IntPtr.Zero)
                {
                    // KỸ THUẬT SỬA LỖI BIẾN MẤT TRÊN MÀN HÌNH 4K:
                    // 1. Ép Style cửa sổ thành Child và loại bỏ viền hệ thống
                    int styleCu = WindowsApiHelper.GetWindowLong(handleCasio, WindowsApiHelper.GWL_STYLE);
                    WindowsApiHelper.SetWindowLong(handleCasio, WindowsApiHelper.GWL_STYLE, (styleCu | (int)WindowsApiHelper.WS_CHILD) & ~(int)WindowsApiHelper.WS_BORDER);

                    // 2. Nhúng vào Panel
                    WindowsApiHelper.SetParent(handleCasio, this.pnlCasio.Handle);

                    // 3. Căn chỉnh và ép vẽ lại (FrameChanged)
                    return DinhViLaiCuaSo();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nhúng ứng dụng: " + ex.Message);
            }
            return 0;
        }

        private int DinhViLaiCuaSo()
        {
            if (quyTrinhCasio == null) return 0;
            IntPtr handle = quyTrinhCasio.MainWindowHandle;

            // Lấy kích thước thực tế của Casio
            WindowsApiHelper.RECT rect;
            WindowsApiHelper.GetClientRect(handle, out rect);
            int w = rect.Right - rect.Left;
            int h = rect.Bottom - rect.Top;

            // Ép cửa sổ hiển thị tại tọa độ (0,0) của Panel và ép Redraw
            WindowsApiHelper.SetWindowPos(handle, IntPtr.Zero, 0, 0, w, h,
                WindowsApiHelper.SWP_NOZORDER | WindowsApiHelper.SWP_FRAMECHANGED | WindowsApiHelper.SWP_SHOWWINDOW);

            return w;
        }
    }
}