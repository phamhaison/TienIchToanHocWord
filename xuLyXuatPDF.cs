using System;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Word = Microsoft.Office.Interop.Word;

namespace TienIchToanHocWord
{
    /// <summary>
    /// Class chuyên trách xử lý xuất bản tài liệu sang định dạng PDF
    /// Đảm bảo giữ nguyên định dạng trang in, màu sắc và chữ viết tay (Ink)
    /// </summary>
    public class xuLyXuatPDF
    {
        /// <summary>
        /// Thực hiện xuất tài liệu Word đang mở ra file PDF tại Desktop
        /// </summary>
        /// <param name="taiLieu">Đối tượng tài liệu Word cần xử lý</param>
        public void ThucHienXuatPDF(Word.Document taiLieu)
        {
            // 1. Kiểm tra tính hợp lệ của đối tượng đầu vào
            if (taiLieu == null)
            {
                MessageBox.Show("Không tìm thấy tài liệu đang mở để xuất PDF.", "Thông báo");
                return;
            }

            try
            {
                // 2. Xử lý tên tệp tin
                // Lấy tên file (ví dụ: DeThiToan.docx -> DeThiToan)
                string tenFileGoc = taiLieu.Name;
                string tenFileKhongDuoi = Path.GetFileNameWithoutExtension(tenFileGoc);
                string tenFilePdf = tenFileKhongDuoi + ".pdf";

                // 3. Xác định đường dẫn Desktop của máy tính hiện tại
                string duongDanDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string duongDanLuu = Path.Combine(duongDanDesktop, tenFilePdf);

                // 4. Cấu hình các tham số xuất PDF tối ưu (Dựa trên logic Module COnvert2Pdf2Docx)
                // Sử dụng wdExportDocumentWithMarkup để giữ nguyên Ink/Handwriting
                // Sử dụng wdExportOptimizeForPrint để giữ nguyên độ phân giải màu sắc
                taiLieu.ExportAsFixedFormat(
                    OutputFileName: duongDanLuu,
                    ExportFormat: Word.WdExportFormat.wdExportFormatPDF,
                    OpenAfterExport: true, // Mở ngay sau khi xuất để giáo viên kiểm tra
                    OptimizeFor: Word.WdExportOptimizeFor.wdExportOptimizeForPrint, // Chất lượng in ấn cao nhất
                    Range: Word.WdExportRange.wdExportAllDocument, // Xuất toàn bộ tài liệu
                    From: 1,
                    To: 1,
                    Item: Word.WdExportItem.wdExportDocumentWithMarkup, // QUAN TRỌNG: Giữ chữ viết tay và màu sắc
                    IncludeDocProps: true, // Giữ thuộc tính file
                    KeepIRM: true,
                    CreateBookmarks: Word.WdExportCreateBookmarks.wdExportCreateHeadingBookmarks,
                    DocStructureTags: true,
                    BitmapMissingFonts: true,
                    UseISO19005_1: false
                );
            }
            catch (COMException ex)
            {
                // Xử lý lỗi đặc thù khi file PDF đang mở hoặc Word bị khóa
                MessageBox.Show("Lỗi Office: Có thể file PDF đang được mở bởi một chương trình khác hoặc tài liệu chưa được lưu.\n\nChi tiết: " + ex.Message, "Lỗi hệ thống");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi không xác định: " + ex.Message, "Thông báo lỗi");
            }
        }
    }
}