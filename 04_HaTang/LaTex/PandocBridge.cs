using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Office.Interop.Word;
using Word = Microsoft.Office.Interop.Word;

namespace TienIchToanHocWord.HaTang.LaTex
{
    public static class PandocBridge
    {
        // =================================================
        // API CHINH: CHEN LaTeX -> OMath Word
        // =================================================
        public static bool ChenLaTeXSangWordEquation(
            Word.Application app,
            Word.Range targetRange,
            string latex,
            bool isInline
        )
        {
            if (app == null || targetRange == null)
                return false;

            if (string.IsNullOrWhiteSpace(latex))
                return false;

            string tempDir = null;
            string texFile = null;
            string docxFile = null;

            try
            {
                // =================================================
                // 1. TAO THU MUC TAM
                // =================================================
                tempDir = Path.Combine(
                    Path.GetTempPath(),
                    "LatexToOMML_" + Guid.NewGuid().ToString("N")
                );
                Directory.CreateDirectory(tempDir);

                texFile = Path.Combine(tempDir, "math.tex");
                docxFile = Path.Combine(tempDir, "out.docx");

                // =================================================
                // 2. TAO NOI DUNG LaTeX DUNG NGU NGHIA
                // =================================================
                string texContent = TaoNoiDungLaTeX(latex, isInline);
                File.WriteAllText(texFile, texContent);

                // =================================================
                // 3. GOI PANDOC CLI
                // =================================================
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "pandoc",
                    Arguments = $"\"{texFile}\" -o \"{docxFile}\"",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true
                };

                using (Process p = Process.Start(psi))
                {
                    if (!p.WaitForExit(8000))
                    {
                        try { p.Kill(); } catch { }
                        return false;
                    }

                    if (p.ExitCode != 0)
                        return false;
                }

                // =================================================
                // 4. MO DOCX TAM – COPY OMath
                // =================================================
                Word.Document tempDoc = app.Documents.Open(
                    docxFile,
                    ReadOnly: true,
                    Visible: false
                );

                try
                {
                    if (tempDoc.OMaths.Count == 0)
                        return false;

                    // COPY OMath dau tien
                    Word.Range mathRange = tempDoc.OMaths[1].Range;
                    mathRange.Copy();

                    // =================================================
                    // 5. PASTE DE WORD TU THAY THE LaTeX
                    // TUYET DOI KHONG targetRange.Text = ""
                    // =================================================
                    targetRange.Paste();
                }
                finally
                {
                    tempDoc.Close(false);
                }

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                // =================================================
                // 6. DON DEP
                // =================================================
                try
                {
                    if (!string.IsNullOrEmpty(tempDir) && Directory.Exists(tempDir))
                        Directory.Delete(tempDir, true);
                }
                catch { }
            }
        }

        // =================================================
        // OVERLOAD: MAC DINH DISPLAY
        // =================================================
        public static bool ChenLaTeXSangWordEquation(
            Word.Application app,
            Word.Range targetRange,
            string latex
        )
        {
            return ChenLaTeXSangWordEquation(app, targetRange, latex, false);
        }

        // =================================================
        // TAO NOI DUNG LaTeX
        // =================================================
        private static string TaoNoiDungLaTeX(string latex, bool isInline)
        {
            latex = latex.Trim();

            if (isInline)
            {
                // INLINE: KHONG TAO PARAGRAPH MOI
                return
    $@"\documentclass{{article}}
\usepackage{{amsmath,amssymb}}
\pagestyle{{empty}}
\begin{{document}}
${latex}$
\end{{document}}";
            }

            // DISPLAY: DUNG \[ \]
            return
    $@"\documentclass{{article}}
\usepackage{{amsmath,amssymb}}
\pagestyle{{empty}}
\begin{{document}}
\[
{latex}
\]
\end{{document}}";
        }
    }
}