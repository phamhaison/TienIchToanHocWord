using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TienIchToanHocWord.XuLyVoiAi; // Dependency: AI Gateway & KetQuaAI
using TienIchToanHocWord.MienNghiepVu.GiaTri; // Value Object: PhanTuAnh

namespace TienIchToanHocWord.HaTang.Pdf
{
    /// <summary>
    /// Lop Client chuyen trach goi script Python de thuc hien Phan tich Bounding Box (Toa do, Context) cua hinh anh trong PDF.
    /// Dong vai tro la mot Utility Client, phu thuoc vao AI Gateway (CauNoiVoiPython) de thuc thi process.
    /// </summary>
    public class LopPhanTichPdf
    {
        // ======================================================
        // DEPENDENCIES & CONSTANTS
        // ======================================================
        private readonly CauNoiVoiPython _cauNoiVoiPython;
        private const string SCRIPT_NAME = "pdf_analyzer.py";

        // ĐỊNH NGHĨA CÁC CHUỖI PHÂN ĐỊNH ĐỘC NHẤT (PHẢI KHỚP VỚI PYTHON)
        private const string JSON_START_DELIMITER = "---JSON_DATA_START---";
        private const string JSON_END_DELIMITER = "---JSON_DATA_END---";

        /// <summary>
        /// Constructor. Nhan AI Gateway qua Dependency Injection.
        /// </summary>
        public LopPhanTichPdf(CauNoiVoiPython cauNoiVoiPython)
        {
            _cauNoiVoiPython = cauNoiVoiPython ?? throw new ArgumentNullException(nameof(cauNoiVoiPython));
        }

        // ======================================================
        // FUNCTION: PhanTichAsync
        // Chức năng: Thực hiện gọi script Python phân tích PDF và xử lý chuỗi JSON trả về.
        // ======================================================
        /// <summary>
        /// Thuc hien Phan tich toan bo PDF de trich xuat anh va context.
        /// </summary>
        /// <param name="duongDanPdf">Duong dan den file PDF can phan tich.</param>
        /// <returns>Danh sach cac PhanTuAnh (Vi tri, Base64, Context).</returns>
        public async Task<List<PhanTuAnh>> PhanTichAsync(string duongDanPdf)
        {
            // 1. Xay dung JSON Input (Chi can Path)
            var doi_tuong_input = new { duong_dan_pdf = duongDanPdf };
            string chuoi_json_input = JsonConvert.SerializeObject(doi_tuong_input);

            // 2. Goi Python thong qua Gateway
            // FIX: Hung ket qua bang doi tuong KetQuaAI thay vi string
            KetQuaAI ket_qua_tu_gateway = await _cauNoiVoiPython.ThucThiXuLyAiAsync(chuoi_json_input);

            if (ket_qua_tu_gateway == null || string.IsNullOrWhiteSpace(ket_qua_tu_gateway.VanBan))
            {
                throw new Exception("Hanh lang AI: Script Phan tich PDF khong tra ve du lieu.");
            }

            // Lay thong tin tu doi tuong ket qua
            string chuoi_ket_qua_tho = ket_qua_tu_gateway.VanBan;
            string thu_muc_tam_thoi = ket_qua_tu_gateway.ThuMucTam;

            try
            {
                // =================================================
                // BƯỚC 3: TRÍCH XUẤT JSON SẠCH (Self-Delimiting Protocol)
                // =================================================
                int vi_tri_bat_dau = chuoi_ket_qua_tho.IndexOf(JSON_START_DELIMITER);
                int vi_tri_ket_thuc = chuoi_ket_qua_tho.LastIndexOf(JSON_END_DELIMITER);

                if (vi_tri_bat_dau == -1 || vi_tri_ket_thuc == -1)
                {
                    // Lỗi giao thức Python trả về
                    string thong_bao_loi = $"Loi giao thuc: Khong tim thay delimiter JSON. Du lieu: {chuoi_ket_qua_tho.Substring(0, Math.Min(200, chuoi_ket_qua_tho.Length))}";
                    throw new Exception(thong_bao_loi);
                }

                // Cắt lấy phần JSON nằm giữa 2 thẻ đánh dấu
                int payloadStart = vi_tri_bat_dau + JSON_START_DELIMITER.Length;
                string chuoi_json_sach = chuoi_ket_qua_tho.Substring(payloadStart, vi_tri_ket_thuc - payloadStart);

                // =================================================
                // BƯỚC 4: PARSE JSON SANG OBJECT C#
                // =================================================
                List<PhanTuAnh> ket_qua_cuoi_cung = JsonConvert.DeserializeObject<List<PhanTuAnh>>(chuoi_json_sach);

                return ket_qua_cuoi_cung ?? new List<PhanTuAnh>();
            }
            catch (Exception ex)
            {
                // Log loi chi tiet cho qua trinh Parse
                string thong_bao_loi = $"Loi phan tich cau truc JSON tu AI: {ex.Message}";
                throw new Exception(thong_bao_loi, ex);
            }
            finally
            {
                // =================================================
                // BƯỚC 5: DỌN DẸP TÀI NGUYÊN (QUAN TRỌNG)
                // =================================================
                // Vi LopPhanTichPdf chi trich xuat du lieu vao RAM, khong dung anh vat ly 
                // nen ta can xoa ngay thu muc tam de tranh day o cung.
                try
                {
                    if (!string.IsNullOrEmpty(thu_muc_tam_thoi) && Directory.Exists(thu_muc_tam_thoi))
                    {
                        Directory.Delete(thu_muc_tam_thoi, true);
                    }
                }
                catch { /* Bo qua loi neu tep tin dang bi khoa boi tien trinh khac */ }
            }
        }
    }
}