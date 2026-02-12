using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TienIchToanHocWord.MienNghiepVu.PhanTich
{
    public static class LatexFinder
    {
        // =================================================
        // TIM TAT CA CONG THUC LATEX TRONG VAN BAN
        // =================================================
        public static List<Match> Find(string text)
        {
            var results = new List<Match>();

            if (string.IsNullOrEmpty(text))
                return results;

            string pattern =
                @"(\$\$.*?\$\$)|" +
                @"(\$.*?\$)|" +
                @"(\\\[.*?\\\])|" +
                @"(\\\(.*?\\\))";

            MatchCollection matches =
                Regex.Matches(text, pattern, RegexOptions.Singleline);

            for (int i = 0; i < matches.Count; i++)
            {
                results.Add(matches[i]);
            }

            return results;
        }

        // =================================================
        // BO DAU $ $ , \( \) , \[ \]
        // =================================================
        public static string Strip(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return string.Empty;

            string s = raw.Trim();

            if (s.StartsWith("$$") && s.EndsWith("$$"))
                return s.Substring(2, s.Length - 4);

            if (s.StartsWith("$") && s.EndsWith("$"))
                return s.Substring(1, s.Length - 2);

            if (s.StartsWith(@"\[") && s.EndsWith(@"\]"))
                return s.Substring(2, s.Length - 4);

            if (s.StartsWith(@"\(") && s.EndsWith(@"\)"))
                return s.Substring(2, s.Length - 4);

            return s;
        }
    }
}