using System;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;

namespace TienIchToanHocWord
{
    /// <summary>
    /// Class xử lý logic chuẩn hóa trang in và văn bản.
    /// Tách biệt hoàn toàn với giao diện Form.
    /// </summary>
    public class LopChuanHoaTrangDayHoc
    {
        private Word.Application ungDungWord;
        private Word.Document taiLieu;

        public LopChuanHoaTrangDayHoc()
        {
            ungDungWord = Globals.ThisAddIn.Application;
            taiLieu = ungDungWord.ActiveDocument;
        }

        // 1. Chuẩn hóa cho Phone (VBA trang 87)
        public void ChuanHoaChoPhone()
        {
            ThucThiChuanHoaTongThe(10.0f, 29.7f, 0.5f, 0.5f, 0.5f, 0.5f);
        }

        // 2. Chuẩn hóa cho Ipad (VBA trang 88)
        public void ChuanHoaChoIpad()
        {
            ThucThiChuanHoaTongThe(13.0f, 29.7f, 0.5f, 0.5f, 0.5f, 0.5f);
        }

        // 3. Chuẩn hóa tiết kiệm A4 (VBA trang 88)
        public void ChuanHoaTietKiemA4()
        {
            ThucThiChuanHoaTongThe(21.0f, 29.7f, 1.0f, 1.0f, 1.25f, 1.25f);
        }

        private void ThucThiChuanHoaTongThe(float rong, float cao, float tren, float duoi, float trai, float phai)
        {
            if (taiLieu == null) return;

            ungDungWord.ScreenUpdating = false;
            ungDungWord.DisplayAlerts = Word.WdAlertLevel.wdAlertsNone;

            try
            {
                // Chuyển số thứ tự tự động thành văn bản để tránh nhảy số khi đổi khổ giấy
                taiLieu.Range().ListFormat.ConvertNumbersToText();

                // Thiết lập trang in (VBA: thietdat_trangchinh)
                Word.PageSetup setup = taiLieu.PageSetup;
                setup.PageWidth = ungDungWord.CentimetersToPoints(rong);
                setup.PageHeight = ungDungWord.CentimetersToPoints(cao);
                setup.TopMargin = ungDungWord.CentimetersToPoints(tren);
                setup.BottomMargin = ungDungWord.CentimetersToPoints(duoi);
                setup.LeftMargin = ungDungWord.CentimetersToPoints(trai);
                setup.RightMargin = ungDungWord.CentimetersToPoints(phai);
                setup.HeaderDistance = 0;
                setup.FooterDistance = 0;

                // Xóa ngắt trang/đoạn (VBA: Delete_all_breaks)
                XoaNgatTrangDoan();

                // Chuẩn hóa lề đoạn văn (VBA: removethutledaudong)
                ChuanHoaDoanVan();

                // Chuẩn hóa khoảng trắng phương án (VBA: chuanhoacacphuongan)
                ChuanHoaPhuongAn();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                ungDungWord.ScreenUpdating = true;
                ungDungWord.DisplayAlerts = Word.WdAlertLevel.wdAlertsAll;
            }
        }

        private void XoaNgatTrangDoan()
        {
            string[] cacKyTu = { "^b", "^m", "^n" };
            foreach (string k in cacKyTu)
            {
                Word.Find find = taiLieu.Content.Find;
                find.ClearFormatting();
                find.Replacement.ClearFormatting();
                find.Text = k;
                find.Replacement.Text = "";
                find.Execute(Replace: Word.WdReplace.wdReplaceAll);
            }
        }

        private void ChuanHoaDoanVan()
        {
            Word.Paragraphs paras = taiLieu.Paragraphs;
            paras.Format.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;
            paras.Format.LeftIndent = 0;
            paras.Format.RightIndent = 0;
            paras.Format.FirstLineIndent = 0;
        }

        private void ChuanHoaPhuongAn()
        {
            Word.Find find = taiLieu.Content.Find;
            find.ClearFormatting();
            find.Replacement.ClearFormatting();
            find.Text = "([AaBbCcDd])(.)([! ])";
            find.Replacement.Text = @"\1\2 \3";
            find.MatchWildcards = true;
            find.Execute(Replace: Word.WdReplace.wdReplaceAll);
        }
    }
}