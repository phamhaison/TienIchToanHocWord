using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using TienIchToanHocWord.HaTang.HeThong;

namespace TienIchToanHocWord.GiaoDienVsto.task_panel
{
    public partial class taskPaneCasio : UserControl
    {
        private Process quyTrinhCasio;

        public taskPaneCasio() { InitializeComponent(); }

        public int LayDoRongChuan(string path)
        {
            if (quyTrinhCasio != null && !quyTrinhCasio.HasExited) return this.pnlCasio.Width;
            try
            {
                quyTrinhCasio = Process.Start(path);
                int wait = 0;
                while (quyTrinhCasio.MainWindowHandle == IntPtr.Zero && wait < 50) { Thread.Sleep(200); quyTrinhCasio.Refresh(); wait++; }
                IntPtr handle = quyTrinhCasio.MainWindowHandle;
                if (handle != IntPtr.Zero)
                {
                    int style = WindowsApiHelper.GetWindowLong(handle, WindowsApiHelper.GWL_STYLE);
                    WindowsApiHelper.SetWindowLong(handle, WindowsApiHelper.GWL_STYLE, (style | (int)WindowsApiHelper.WS_CHILD) & ~(int)WindowsApiHelper.WS_CAPTION);
                    WindowsApiHelper.SetParent(handle, this.pnlCasio.Handle);
                    WindowsApiHelper.RECT r;
                    WindowsApiHelper.GetClientRect(handle, out r);
                    int w = r.Right - r.Left;
                    WindowsApiHelper.SetWindowPos(handle, IntPtr.Zero, 0, 0, w, r.Bottom - r.Top, WindowsApiHelper.SWP_FRAMECHANGED | WindowsApiHelper.SWP_SHOWWINDOW);
                    return w;
                }
            }
            catch { }
            return 0;
        }
    }
}