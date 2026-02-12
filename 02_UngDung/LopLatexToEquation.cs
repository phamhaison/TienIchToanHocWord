using Microsoft.Office.Interop.Word;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using TienIchToanHocWord;
using Word = Microsoft.Office.Interop.Word;
using TienIchToanHocWord.MienNghiepVu.PhanTich;
using TienIchToanHocWord.HaTang.LaTex;

namespace TienIchToanHocWord.UngDung
{
    public class LopLatexToEquation
    {
        // ================== CACHE ==================
        private const string CACHE_FILE = "latex_eq_cache.json";
        private readonly string _cachePath;
        private readonly Dictionary<string, string> _cache = new Dictionary<string, string>();

        private bool _dirty = false;

        private Word.Application App => Globals.ThisAddIn.Application;

        // ================== KHOI TAO ==================
        public LopLatexToEquation()
        {
            string dir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "TienIchToanHocWord"
            );

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            _cachePath = Path.Combine(dir, CACHE_FILE);
            TaiCache();
        }

        private static void XoaManualLineBreak(
        Word.Application app,
        Word.Range range
    )
        {
            if (app == null || range == null)
                return;

            Word.Find find = range.Find;

            find.ClearFormatting();
            find.Replacement.ClearFormatting();

            // ^11 = Manual Line Break (Shift+Enter)
            find.Text = "^11";
            find.Replacement.Text = "";

            find.Forward = true;
            find.Wrap = Word.WdFindWrap.wdFindStop;

            // RAT QUAN TRONG: ^11 CHI HOAT DONG KHI BAT WILDCARD
            find.MatchWildcards = true;

            find.Execute(
                Replace: Word.WdReplace.wdReplaceAll
            );
        }
        public void ChuyenDoiLT_SangEQ(Word.Range vungChon)
        {
            var sel = App.Selection;
            if (sel == null || string.IsNullOrWhiteSpace(sel.Text))
                return;

            var matches = LatexFinder.Find(sel.Text);

            for (int i = matches.Count - 1; i >= 0; i--)
            {
                Match m = matches[i];

                Word.Range r = sel.Range.Duplicate;
                r.Start = sel.Range.Start + m.Index;
                r.End = r.Start + m.Length;

                string latex = LatexFinder.Strip(m.Value);

                bool isInline = InlineDetector.IsInlineEquation(r);

                PandocBridge.ChenLaTeXSangWordEquation(
                    App,
                    r,
                    latex,
                    isInline
                );
            }

            // =================================================
            // SAU CUNG: XOA ^11 DE FIX LOI XUONG DONG CONG THUC
            // =================================================
            XoaManualLineBreak(App, sel.Range);
        }


        // =================================================
        // CACHE
        // =================================================
        private void TaiCache()
        {
            try
            {
                if (!File.Exists(_cachePath))
                    return;

                string json = File.ReadAllText(_cachePath);
                var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                if (data == null)
                    return;

                _cache.Clear();
                foreach (var kv in data)
                    _cache[kv.Key] = kv.Value;
            }
            catch { }
        }

        private void LuuCacheVaoFile()
        {
            if (!_dirty)
                return;

            try
            {
                string json = JsonConvert.SerializeObject(_cache, Formatting.Indented);
                File.WriteAllText(_cachePath, json);
                _dirty = false;
            }
            catch { }
        }

        // =================================================
        // HASH
        // =================================================
        public void KetThucPhienLamViec()
        {
            LuuCacheVaoFile();
        }

        private string MD5Hash(string s)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] b = md5.ComputeHash(Encoding.UTF8.GetBytes(s));
                StringBuilder sb = new StringBuilder();
                foreach (byte x in b)
                    sb.Append(x.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}