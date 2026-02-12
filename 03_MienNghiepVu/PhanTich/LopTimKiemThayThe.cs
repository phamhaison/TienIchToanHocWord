using System;
using Word = Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.Office.Interop.Word;

namespace TienIchToanHocWord.MienNghiepVu.PhanTich
{
    public class LopTimKiemThayThe
    {
        public void ThayTheTongHop(
            Word.Range phamVi,
            string chuoiTim,
            string chuoiThay,
            bool dungWildcards = false,
            bool phanBietHoaThuong = false,
            bool khopToanBoTu = false,
            bool lapLai = false,
            bool dungLaiKhiHetVungChon = true,
            bool? inDam = null,
            bool? inNghieng = null,
            bool? gachChan = null,
            Word.WdColor mauChu = Word.WdColor.wdColorAutomatic,
            bool? highlight = null,
            Word.WdParagraphAlignment? canLe = null,
            string tenFont = "",
            float coChu = 0)
        {
            if (phamVi == null) return;

            Word.Application ungDung = phamVi.Application;
            Word.UndoRecord banGhiUndo = null;

            // KIỂM TRA AN TOÀN: Chỉ bật UndoRecord nếu Word đang sẵn sàng
            try { banGhiUndo = ungDung.UndoRecord; } catch { }

            if (banGhiUndo != null)
            {
                try { banGhiUndo.StartCustomRecord("ThayThe_" + chuoiTim); } catch { banGhiUndo = null; }
            }

            // Sử dụng Duplicate để bảo vệ Range gốc
            Word.Range vungLamViec = phamVi.Duplicate;
            Word.Find findObject = vungLamViec.Find;

            try
            {
                // 1. XỬ LÝ CHUỖI ĐẶC THÙ
                if (dungWildcards && chuoiTim.Contains("^p"))
                {
                    chuoiTim = chuoiTim.Replace("^p", "^13");
                }

                // 2. THIẾT LẬP CẤU HÌNH
                findObject.ClearFormatting();
                findObject.Replacement.ClearFormatting();

                findObject.Text = chuoiTim;
                findObject.Replacement.Text = chuoiThay;
                findObject.Forward = true;
                findObject.Wrap = dungLaiKhiHetVungChon ? Word.WdFindWrap.wdFindStop : Word.WdFindWrap.wdFindContinue;

                findObject.Format = true;
                findObject.MatchCase = phanBietHoaThuong;
                findObject.MatchWholeWord = khopToanBoTu;
                findObject.MatchWildcards = dungWildcards;

                // 3. THIẾT LẬP ĐỊNH DẠNG (Sử dụng ép kiểu an toàn)
                if (inDam.HasValue) findObject.Replacement.Font.Bold = inDam.Value ? 1 : 0;
                if (inNghieng.HasValue) findObject.Replacement.Font.Italic = inNghieng.Value ? 1 : 0;

                if (gachChan.HasValue)
                    findObject.Replacement.Font.Underline = gachChan.Value ? Word.WdUnderline.wdUnderlineSingle : Word.WdUnderline.wdUnderlineNone;

                if (mauChu != Word.WdColor.wdColorAutomatic)
                    findObject.Replacement.Font.Color = mauChu;

                if (highlight.HasValue)
                    findObject.Replacement.Highlight = highlight.Value ? (int)Word.WdColorIndex.wdYellow : (int)Word.WdColorIndex.wdNoHighlight;

                if (!string.IsNullOrEmpty(tenFont))
                    findObject.Replacement.Font.Name = tenFont;

                if (coChu > 0)
                    findObject.Replacement.Font.Size = coChu;

                if (canLe.HasValue)
                    findObject.Replacement.ParagraphFormat.Alignment = canLe.Value;

                // 4. THỰC THI
                object replaceAll = Word.WdReplace.wdReplaceAll;

                if (lapLai)
                {
                    int counter = 0;
                    while (findObject.Execute(Replace: ref replaceAll) && counter < 100)
                    {
                        counter++;
                    }
                }
                else
                {
                    findObject.Execute(Replace: ref replaceAll);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Lỗi thực thi thay thế: " + ex.Message);
            }
            finally
            {
                // 5. GIẢI PHÓNG VÀ KẾT THÚC
                if (banGhiUndo != null)
                {
                    try { banGhiUndo.EndCustomRecord(); } catch { }
                }

                // Giải phóng đối tượng COM để tránh treo Ribbon
                if (findObject != null) Marshal.ReleaseComObject(findObject);
                if (vungLamViec != null) Marshal.ReleaseComObject(vungLamViec);
            }
        }

        public Word.WdColor ChuyenDoiMauVba(string tenMauVba)
        {
            if (string.IsNullOrEmpty(tenMauVba)) return Word.WdColor.wdColorAutomatic;
            switch (tenMauVba.ToLower())
            {
                case "wdblue": return Word.WdColor.wdColorBlue;
                case "wdred": return Word.WdColor.wdColorRed;
                case "wdgreen": return Word.WdColor.wdColorGreen;
                case "wdblack": return Word.WdColor.wdColorBlack;
                default: return Word.WdColor.wdColorAutomatic;
            }
        }

        internal static void XoaManualLineBreak(Application app, Range range)
        {
            throw new NotImplementedException();
        }
    }
}