using System;
using System.Runtime.InteropServices;

namespace TienIchToanHocWord
{
    public static class WindowsApiHelper
    {
        // --- CÁC HÀM WINDOWS API (DLL IMPORT) ---

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        // Thêm vào WindowsApiHelper.cs
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public const int SW_SHOW = 5;
        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_MDICHILD = 0x00000040;

        // --- CÁC HẰNG SỐ (CONSTANTS) ---

        public const int GWL_STYLE = -16;

        // Styles
        public const uint WS_VISIBLE = 0x10000000;
        public const uint WS_CHILD = 0x40000000;
        public const uint WS_POPUP = 0x80000000;
        public const int WS_CAPTION = 0x00C00000;
        public const int WS_THICKFRAME = 0x00040000;

        // SetWindowPos Flags
        public const uint SWP_NOSIZE = 0x0001;
        public const uint SWP_NOMOVE = 0x0002;
        public const uint SWP_NOZORDER = 0x0004;
        public const uint SWP_SHOWWINDOW = 0x0040;
        public const uint SWP_FRAMECHANGED = 0x0020;
        public const uint SWP_NOSENDCHANGING = 0x0400;

        // DPI Capability
        public const int LOGPIXELSX = 88;



        // --- CẤU TRÚC DỮ LIỆU (STRUCTS) ---

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }
}