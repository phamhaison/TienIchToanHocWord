using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace TienIchToanHocWord
{
    public class LopCanChinhPhuongAnTheoPhamVi
    {
        private Word.Application ungDungWord;
        private LopTimKiemThayThe boTimKiem;

        public LopCanChinhPhuongAnTheoPhamVi()
        {
            ungDungWord = Globals.ThisAddIn.Application;
            boTimKiem = new LopTimKiemThayThe();
        }

        public void TuDongCanChinhThongMinh(Word.Range vungChon)
        {
            if (vungChon == null) return;
            ungDungWord.ScreenUpdating = false;

            try
            {
                // 1. Chuẩn hóa ban đầu (Cố định số thứ tự)
                vungChon.ListFormat.ConvertNumbersToText();

                // 2. Lấy danh sách từng câu hỏi để xử lý riêng biệt
                List<Word.Range> dsCauHoi = LayDanhSachRangeCauHoi(vungChon);

                foreach (Word.Range r in dsCauHoi)
                {
                    ThucThiLogicThongMinh(r);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống căn chỉnh: " + ex.Message);
            }
            finally
            {
                ungDungWord.ScreenUpdating = true;
                ungDungWord.ScreenRefresh();
            }
        }

        private void ThucThiLogicThongMinh(Word.Range cauRange)
        {
            // Xác định vùng chứa 4 phương án (từ A. đến hết câu)
            Word.Range paRange = cauRange.Duplicate;
            Word.Find f = paRange.Find;
            f.Text = "[Aa].[ ]{1,}";
            f.MatchWildcards = true;
            if (!f.Execute()) return;
            paRange.End = cauRange.End;

            // --- BƯỚC 1: THỬ KỊCH BẢN 4 PHƯƠNG ÁN / 1 DÒNG ---
            CanChinh_4PA_Tren_1Dong(paRange);
            EpWordCapNhatLayout(paRange);

            float yA = LayY(paRange, "A.");
            float yB = LayY(paRange, "B.");
            float yC = LayY(paRange, "C.");
            float yD = LayY(paRange, "D.");

            // Kiểm tra: Nếu A, B, C, D không cùng nằm trên 1 đường thẳng (sai số 10pt cho tích phân)
            bool biTranDong1 = (Math.Abs(yA - yB) > 10 || Math.Abs(yA - yC) > 10 || Math.Abs(yA - yD) > 10);

            if (biTranDong1)
            {
                // --- BƯỚC 2: THỬ KỊCH BẢN 2 PHƯƠNG ÁN / 1 DÒNG (2-2) ---
                CanChinh_2PA_Tren_1Dong(paRange);
                EpWordCapNhatLayout(paRange);

                yA = LayY(paRange, "A.");
                yB = LayY(paRange, "B.");
                yC = LayY(paRange, "C.");
                yD = LayY(paRange, "D.");

                // Kiểm tra: A-B phải cùng hàng VÀ C-D phải cùng hàng
                bool hang1Loi = Math.Abs(yA - yB) > 10;
                bool hang2Loi = Math.Abs(yC - yD) > 10;

                if (hang1Loi || hang2Loi)
                {
                    // --- BƯỚC 3: ÉP VỀ MỖI PHƯƠNG ÁN 1 DÒNG ---
                    CanChinh_MoiPA_1Dong(paRange);
                }
            }
        }

        #region HÀM THỰC THI ĐỊNH DẠNG (TÁI SỬ DỤNG LOP TIM KIEM)

        public void CanChinh_4PA_Tren_1Dong(Word.Range r)
        {
            CleanSpace(r);
            boTimKiem.ThayTheTongHop(r, "^13([BbCcDd])([.)])", "^t\\1\\2", true, lapLai: true);
            SetTabs(r, 4);
        }

        public void CanChinh_2PA_Tren_1Dong(Word.Range r)
        {
            CleanSpace(r);
            boTimKiem.ThayTheTongHop(r, "^13([BbDd])([.)])", "^t\\1\\2", true, lapLai: true);
            boTimKiem.ThayTheTongHop(r, "^t([Cc])([.)])", "^p\\1\\2", true, lapLai: true);
            SetTabs(r, 2);
        }

        public void CanChinh_MoiPA_1Dong(Word.Range r)
        {
            CleanSpace(r);
            boTimKiem.ThayTheTongHop(r, "^t([BbCcDd])([.)])", "^p\\1\\2", true, lapLai: true);
            r.ParagraphFormat.TabStops.ClearAll();
        }

        #endregion

        #region TRỢ LÝ LAYOUT (CORE HELPERS)

        private void EpWordCapNhatLayout(Word.Range r)
        {
            // Lệnh này ép Word phải tính toán lại toàn bộ Geometry của văn bản
            int dummy = r.ComputeStatistics(Word.WdStatistic.wdStatisticLines);
            ungDungWord.ScreenRefresh();
            System.Windows.Forms.Application.DoEvents();
        }

        private float LayY(Word.Range phamVi, string pa)
        {
            Word.Range r = phamVi.Duplicate;
            r.Find.ClearFormatting();
            r.Find.Text = pa;
            r.Find.Forward = true;
            r.Find.Wrap = Word.WdFindWrap.wdFindStop;
            if (r.Find.Execute())
            {
                return (float)r.get_Information(Word.WdInformation.wdVerticalPositionRelativeToPage);
            }
            return 0;
        }

        private void CleanSpace(Word.Range r)
        {
            boTimKiem.ThayTheTongHop(r, "([ ]{2,})", " ", true, lapLai: true);
        }

        private void SetTabs(Word.Range r, int soCot)
        {
            float width = r.PageSetup.PageWidth - r.PageSetup.LeftMargin - r.PageSetup.RightMargin;
            r.ParagraphFormat.TabStops.ClearAll();
            for (int i = 1; i < soCot; i++)
            {
                r.ParagraphFormat.TabStops.Add((width / soCot) * i, Word.WdTabAlignment.wdAlignTabLeft);
            }
        }

        private List<Word.Range> LayDanhSachRangeCauHoi(Word.Range vungChon)
        {
            List<Word.Range> ketQua = new List<Word.Range>();
            Word.Range finder = vungChon.Duplicate;
            Word.Find f = finder.Find;
            f.Text = "Câu [0-9]{1,4}[.:)]";
            f.MatchWildcards = true;

            while (f.Execute())
            {
                if (finder.Start >= vungChon.End) break;
                Word.Range r = finder.Duplicate;
                r.MoveEndUntil("C", Word.WdConstants.wdForward);
                if (r.End > vungChon.End) r.End = vungChon.End;
                ketQua.Add(r);
                finder.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
            }
            return ketQua;
        }
        #endregion
    }
}