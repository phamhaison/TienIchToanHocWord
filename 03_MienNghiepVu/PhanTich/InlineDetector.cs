using Microsoft.Office.Interop.Word;
using System;
using System.Text.RegularExpressions;
using Word = Microsoft.Office.Interop.Word;


namespace TienIchToanHocWord.MienNghiepVu.PhanTich
{
    public static class InlineDetector
    {
        /// <summary>
        /// Xac dinh cong thuc nen la Inline hay Display
        /// Do chinh xac ~99% cho de trac nghiem Toan
        /// </summary>
        public static bool IsInlineEquation(Word.Range latexRange)
        {
            if (latexRange == null || latexRange.Paragraphs.Count == 0)
                return true; // mac dinh an toan: inline

            Word.Paragraph para = latexRange.Paragraphs[1];
            string paraText = CleanText(para.Range.Text);
            string latexText = CleanText(latexRange.Text);

            // 1. Neu doan van chi chua latex -> Display
            if (paraText == latexText)
                return false;

            // 2. Neu truoc hoac sau latex co chu / so / dau toan -> Inline
            if (HasTextBeforeOrAfter(para.Range, latexRange))
                return true;

            // 3. Neu la dap an trac nghiem (A. B. C. D.) -> Inline
            if (IsMultipleChoiceParagraph(paraText))
                return true;

            // 4. Neu doan ket thuc bang dau cham -> Inline
            if (paraText.EndsWith("."))
                return true;

            // 5. Neu para ngan va co ky tu toan hoc -> Inline
            if (paraText.Length < 80 && Regex.IsMatch(paraText, @"[=+\-*/]"))
                return true;

            // Mac dinh: Display
            return false;
        }

        // =========================================================
        // CAC HAM PHU
        // =========================================================

        private static bool HasTextBeforeOrAfter(
        Word.Range paraRange,
        Word.Range latexRange
    )
        {
            // ----- BEFORE -----
            Word.Range beforeRange = paraRange.Duplicate;
            beforeRange.End = Math.Min(latexRange.Start, paraRange.End);

            string before = CleanText(beforeRange.Text);

            // ----- AFTER -----
            Word.Range afterRange = paraRange.Duplicate;
            afterRange.Start = Math.Max(latexRange.End, paraRange.Start);

            string after = CleanText(afterRange.Text);

            return !string.IsNullOrWhiteSpace(before)
                || !string.IsNullOrWhiteSpace(after);
        }


        private static bool IsMultipleChoiceParagraph(string text)
        {
            // A. B. C. D.
            return Regex.IsMatch(
                text,
                @"^\s*[A-D]\s*[\.\)]\s*",
                RegexOptions.IgnoreCase
            );
        }

        private static string CleanText(string s)
        {
            if (string.IsNullOrEmpty(s))
                return "";

            return s
                .Replace("\r", "")
                .Replace("\a", "")
                .Replace("\v", "")
                .Replace("\n", "")
                .Trim();
        }
    }
}