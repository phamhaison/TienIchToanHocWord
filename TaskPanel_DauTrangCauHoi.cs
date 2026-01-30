using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using Word = Microsoft.Office.Interop.Word;

namespace TienIchToanHocWord
{
    public partial class TaskPanel_DauTrangCauHoi : UserControl
    {
        private LopTaoBookMark nghiepVu;

        public TaskPanel_DauTrangCauHoi()
        {
            InitializeComponent();
            nghiepVu = new LopTaoBookMark();
            DangKySuKien();
        }

        private void DangKySuKien()
        {
            this.Load += (s, e) => {
                rad_ALL.Checked = true;
                LoadData("ALL");
                // Tự động chỉnh độ rộng khi vừa load xong
                TuDongChinhBeRongTaskPane();
            };

            rad_ALL.CheckedChanged += (s, e) => { if (rad_ALL.Checked) LoadData("ALL"); };
            rad_NB.CheckedChanged += (s, e) => { if (rad_NB.Checked) LoadData("NB"); };
            rad_TH.CheckedChanged += (s, e) => { if (rad_TH.Checked) LoadData("TH"); };
            rad_VD.CheckedChanged += (s, e) => { if (rad_VD.Checked) LoadData("VD"); };

            chk_HienLoiGiai.CheckedChanged += (s, e) => nghiepVu.AnHienLoiGiai(chk_HienLoiGiai.Checked);

            chk_NgatTrang.CheckedChanged += (s, e) => {
                nghiepVu.XuLyNgatTrang(chk_NgatTrang.Checked);
                string mucDo = rad_NB.Checked ? "NB" : (rad_TH.Checked ? "TH" : (rad_VD.Checked ? "VD" : "ALL"));
                LoadData(mucDo);
            };

            lsb_CauHoi.SelectedIndexChanged += Lsb_CauHoi_SelectedIndexChanged;
        }

        private void LoadData(string mucDo)
        {
            List<string> ds = nghiepVu.TaoDauTrangCauHoi(mucDo);
            lsb_CauHoi.Items.Clear();
            foreach (string cau in ds) lsb_CauHoi.Items.Add(cau);
        }

        /// <summary>
        /// Thuật toán tự động tính toán độ rộng cần thiết dựa trên DPI màn hình
        /// </summary>
        public void TuDongChinhBeRongTaskPane()
        {
            try
            {
                // 1. Tìm CustomTaskPane chứa UserControl này
                // Trong VSTO, chúng ta cần truy cập thông qua Globals
                Microsoft.Office.Tools.CustomTaskPane hienTai = null;
                foreach (var pane in Globals.ThisAddIn.CustomTaskPanes)
                {
                    if (pane.Control == this) { hienTai = pane; break; }
                }

                if (hienTai != null)
                {
                    // 2. Tính toán chiều rộng Pixels lớn nhất của các linh kiện (ListBox hoặc GroupBox)
                    // Cộng thêm 25 pixels dự phòng cho thanh cuộn và lề
                    int doRongPixel = Math.Max(lsb_CauHoi.Width, 200) + 25;

                    // 3. Lấy chỉ số DPI thực tế của màn hình (Quan trọng cho màn hình 2880x1920)
                    using (Graphics g = this.CreateGraphics())
                    {
                        float dpiX = g.DpiX;
                        // Công thức quy đổi: Points = Pixels * (72 / DPI)
                        float heSoQuyDoi = 72f / dpiX;
                        int doRongPoint = (int)(doRongPixel * heSoQuyDoi);

                        // 4. Áp dụng chiều rộng (Giới hạn an toàn từ 150 đến 400 points)
                        if (doRongPoint < 150) doRongPoint = 150;
                        if (doRongPoint > 400) doRongPoint = 400;

                        hienTai.Width = doRongPoint;
                    }
                }
            }
            catch { /* Bỏ qua lỗi nếu Task Pane chưa sẵn sàng */ }
        }

        private void Lsb_CauHoi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsb_CauHoi.SelectedItem == null) return;

            try
            {
                string tenHienThi = lsb_CauHoi.SelectedItem.ToString();
                string tenBookmark = tenHienThi.Replace(" ", "_");

                Word.Application app = Globals.ThisAddIn.Application;
                Word.Document doc = app.ActiveDocument;

                if (doc.Bookmarks.Exists(tenBookmark))
                {
                    app.ScreenUpdating = true;
                    Word.Range vungCauHoi = doc.Bookmarks[tenBookmark].Range;
                    vungCauHoi.Select();

                    // Tái hiện hàm VerticalLineScroll bằng dynamic (Sửa lỗi CS1061)
                    try
                    {
                        int soDongTuyetDoi = (int)vungCauHoi.get_Information(Word.WdInformation.wdFirstCharacterLineNumber);
                        dynamic activePaneDynamic = app.ActiveWindow.ActivePane;
                        activePaneDynamic.VerticalLineScroll = soDongTuyetDoi;
                    }
                    catch
                    {
                        app.ActiveWindow.ScrollIntoView(vungCauHoi, true);
                    }

                    app.ActiveWindow.ActivePane.SmallScroll(0);
                    app.ActiveWindow.Activate();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi điều hướng: " + ex.Message);
            }
        }
    }
}