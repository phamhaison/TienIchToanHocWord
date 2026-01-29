using System;
using Word = Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace TienIchToanHocWord
{
    /// <summary>
    /// Class tổng hợp toàn bộ logic tìm kiếm và thay thế từ mã nguồn VBA gốc.
    /// Thiết kế để tái sử dụng cho Azota, SmartTest và căn chỉnh phương án.
    /// </summary>
    public class LopTimKiemThayThe
    {
        /// <summary>
        /// Hàm thay thế tổng hợp (Consolidated Search & Replace)
        /// </summary>
        /// <param name="phamVi">Vùng Range cần xử lý (Selection.Range hoặc Document.Content)</param>
        /// <param name="chuoiTim">sFind</param>
        /// <param name="chuoiThay">sReplace</param>
        /// <param name="dungWildcards">KiTuDaiDien (True nếu dùng Regex của Word)</param>
        /// <param name="phanBietHoaThuong">InHoa (MatchCase)</param>
        /// <param name="lapLai">Thực hiện lặp cho đến khi không tìm thấy (Do While .Execute)</param>
        /// <param name="dungLaiKhiHetVungChon">Bắt buộc dừng khi hết Range (Tránh tràn văn bản)</param>
        /// <param name="inDam">Định dạng In đậm (True/False/Null)</param>
        /// <param name="inNghieng">Định dạng In nghiêng (True/False/Null)</param>
        /// <param name="gachChan">Kiểu gạch chân (WdUnderline)</param>
        /// <param name="mauChu">Màu sắc chữ (WdColor)</param>
        /// <param name="highlight">Tô màu nền (VBA: tosang)</param>
        /// <param name="canLe">Căn lề đoạn văn (VBA: cangiua, cantrai...)</param>
        /// <param name="tenFont">Tên Font chữ</param>
        /// <param name="coChu">Kích thước chữ</param>
        public void ThayTheTongHop(
            Word.Range phamVi,
            string chuoiTim,
            string chuoiThay,
            bool dungWildcards = false,
            bool phanBietHoaThuong = false,
            bool lapLai = false,
            bool dungLaiKhiHetVungChon = true,
            bool? inDam = null,
            bool? inNghieng = null,
            Word.WdUnderline gachChan = Word.WdUnderline.wdUnderlineNone,
            Word.WdColor mauChu = Word.WdColor.wdColorAutomatic,
            bool? highlight = null,
            Word.WdParagraphAlignment? canLe = null,
            string tenFont = "",
            float coChu = 0)
        {
            if (phamVi == null) return;

            // 1. Xử lý lỗi đặc thù của Word: Wildcards không chấp nhận ^p trong chuỗi tìm kiếm
            if (dungWildcards && chuoiTim.Contains("^p"))
            {
                chuoiTim = chuoiTim.Replace("^p", "^13");
            }

            // 2. Cấu hình đối tượng Find
            Word.Find findObject = phamVi.Find;

            // Reset định dạng trước khi thiết lập mới
            findObject.ClearFormatting();
            findObject.Replacement.ClearFormatting();

            findObject.Text = chuoiTim;
            findObject.Replacement.Text = chuoiThay;
            findObject.Forward = true;

            // QUAN TRỌNG: wdFindStop giúp giới hạn chính xác trong vùng bôi đen (VBA logic)
            findObject.Wrap = dungLaiKhiHetVungChon ? Word.WdFindWrap.wdFindStop : Word.WdFindWrap.wdFindContinue;

            findObject.Format = true; // Bật tìm kiếm theo định dạng
            findObject.MatchCase = phanBietHoaThuong;
            findObject.MatchWildcards = dungWildcards;

            // 3. Thiết lập hành động định dạng đầu ra (Replacement)
            if (inDam.HasValue) findObject.Replacement.Font.Bold = inDam.Value ? 1 : 0;
            if (inNghieng.HasValue) findObject.Replacement.Font.Italic = inNghieng.Value ? 1 : 0;

            if (gachChan != Word.WdUnderline.wdUnderlineNone)
                findObject.Replacement.Font.Underline = gachChan;

            if (mauChu != Word.WdColor.wdColorAutomatic)
                findObject.Replacement.Font.Color = mauChu;

            if (!string.IsNullOrEmpty(tenFont))
                findObject.Replacement.Font.Name = tenFont;

            if (coChu > 0)
                findObject.Replacement.Font.Size = coChu;

            if (canLe.HasValue)
                findObject.Replacement.ParagraphFormat.Alignment = canLe.Value;

            // Xử lý Highlight (VBA: tosang)
            if (highlight.HasValue)
                findObject.Replacement.Highlight = highlight.Value ? 1 : 0;

            // 4. Thực thi thay thế
            object replaceAll = Word.WdReplace.wdReplaceAll;

            if (lapLai)
            {
                // Thực hiện vòng lặp (Tương đương Do While .Execute trong VBA)
                // Dùng khi chuỗi thay thế có khả năng tạo ra chuỗi tìm kiếm mới (ví dụ xóa dấu cách thừa)
                while (findObject.Execute(Replace: ref replaceAll))
                {
                    // Vòng lặp tự động chạy cho đến khi không còn kết quả
                }
            }
            else
            {
                // Thực hiện thay thế tất cả trong 1 lần gọi (Tối ưu tốc độ)
                findObject.Execute(Replace: ref replaceAll);
            }

            // 5. Giải phóng bộ nhớ (Best practice cho VSTO)
            // Marshal.ReleaseComObject(findObject);
        }

        /// <summary>
        /// Chuyển đổi tên màu từ chuỗi (VBA style) sang WdColor
        /// </summary>
        public Word.WdColor ChuyenDoiMauVba(string tenMauVba)
        {
            if (string.IsNullOrEmpty(tenMauVba)) return Word.WdColor.wdColorAutomatic;

            switch (tenMauVba.ToLower())
            {
                case "wdblue": case "xanh": return Word.WdColor.wdColorBlue;
                case "wdred": case "do": return Word.WdColor.wdColorRed;
                case "wdgreen": case "xanhla": return Word.WdColor.wdColorGreen;
                case "wdblack": case "den": return Word.WdColor.wdColorBlack;
                case "wdwhite": case "trang": return Word.WdColor.wdColorWhite;
                default: return Word.WdColor.wdColorAutomatic;
            }
        }
    }
}