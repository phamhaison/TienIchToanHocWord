using System;
using System.Collections.Generic;
using Word = Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;
using TienIchToanHocWord.MienNghiepVu.PhanTich;

namespace TienIchToanHocWord.HaTang.WordInterop
{
    public class LopTaoBookMark
    {
        private Word.Application ungDungWord;
        private Word.Document taiLieu;
        private LopTimKiemThayThe boTimKiem;

        public LopTaoBookMark()
        {
            ungDungWord = Globals.ThisAddIn.Application;
            taiLieu = ungDungWord.ActiveDocument;
            boTimKiem = new LopTimKiemThayThe();
        }

        public List<string> TaoDauTrangCauHoi(string mucDo)
        {
            List<string> dsHienThi = new List<string>();
            if (taiLieu == null) return dsHienThi;

            // Cố định số thứ tự (VBA trang 515)
            taiLieu.Range().ListFormat.ConvertNumbersToText();

            XoaDauTrangCu("Câu_");
            XoaDauTrangCu("loiGiai_");

            Word.Range r = taiLieu.Content;
            Word.Find f = r.Find;
            f.ClearFormatting();

            string pattern = (mucDo == "ALL") ? @"(Câu[ ]{1,2}[0-9]{1,4}[.:])" : @"(Câu[ ]{1,2}[0-9]{1,4}[.:])([ ]{1,4})(\(" + mucDo + @"\))";
            f.Text = pattern;
            f.MatchWildcards = true;
            f.Forward = true;

            while (f.Execute())
            {
                if (r.Start >= taiLieu.Content.End) break;
                string textGoc = r.Text.Trim();
                string tenBM = textGoc.Replace(" ", "_").Replace(".", "").Replace(":", "").Replace(")", "");

                taiLieu.Bookmarks.Add(tenBM, r);
                dsHienThi.Add(textGoc.TrimEnd('.', ':'));
                r.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
            }
            return dsHienThi;
        }

        /// <summary>
        /// Xử lý ngắt trang thông minh (Sửa lỗi nhảy sai vị trí câu)
        /// </summary>
        public void XuLyNgatTrang(bool batNgatTrang)
        {
            if (taiLieu == null) return;

            ungDungWord.ScreenUpdating = false;
            try
            {
                // 1. Xóa tất cả dấu ngắt trang cũ (^m) để đưa về trạng thái chuẩn
                boTimKiem.ThayTheTongHop(taiLieu.Content, "^m", "", false);

                if (batNgatTrang)
                {
                    // 2. Tìm lại các vị trí "Câu..." và chèn ngắt trang vào TRƯỚC nó
                    Word.Range r = taiLieu.Content;
                    Word.Find f = r.Find;
                    f.ClearFormatting();
                    f.Text = @"(Câu[ ]{1,2}[0-9]{1,4}[.:])";
                    f.MatchWildcards = true;

                    while (f.Execute())
                    {
                        // Không ngắt trang ở Câu 1 (đầu tài liệu)
                        if (r.Start > 1)
                        {
                            Word.Range rNgat = taiLieu.Range(r.Start, r.Start);
                            rNgat.InsertBreak(Word.WdBreakType.wdPageBreak);
                        }
                        r.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
                    }
                }

                // 3. Ép Word tính toán lại toàn bộ Layout
                taiLieu.Repaginate();
            }
            finally
            {
                ungDungWord.ScreenUpdating = true;
            }
        }

        public void AnHienLoiGiai(bool hienThi)
        {
            foreach (Word.Bookmark bm in taiLieu.Bookmarks)
            {
                if (bm.Name.StartsWith("loiGiai_"))
                {
                    bm.Range.Font.Hidden = hienThi ? 0 : 1;
                }
            }
        }

        private void XoaDauTrangCu(string tienTo)
        {
            for (int i = taiLieu.Bookmarks.Count; i >= 1; i--)
            {
                if (taiLieu.Bookmarks[i].Name.StartsWith(tienTo)) taiLieu.Bookmarks[i].Delete();
            }
        }
    }
}