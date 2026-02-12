using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TienIchToanHocWord.XuLyVoiAi; // Chỉ còn phụ thuộc vào Gateway (CauNoiVoiPython)

namespace TienIchToanHocWord.XuLyVoiAi
{
    /// <summary>
    /// Use Case xu ly chuyen doi tai lieu (PDF/Anh/Text) sang Word bang AI.
    /// Trach nhiem: Xay dung JSON Input, Goi Gateway (CauNoiVoiPython), Xu ly ket qua.
    /// Khong biet ve API Key, Prompt, Database hay Process.
    /// </summary>
    public class XuLyChuyenDoiTaiLieuUseCase
    {
        // =================================================================
        // DEPENDENCIES (Application Layer)
        // =================================================================
        private readonly CauNoiVoiPython _cauNoiVoiPython;

        /// <summary>
        /// Constructor. Nhan AI Gateway qua Dependency Injection.
        /// </summary>
        public XuLyChuyenDoiTaiLieuUseCase(CauNoiVoiPython cauNoiVoiPython)
        {
            // Guard Clause cho Dependency
            _cauNoiVoiPython = cauNoiVoiPython ?? throw new ArgumentNullException(nameof(cauNoiVoiPython));
        }

        // =================================================================
        // HÀM CHÍNH (Business Rule Execution)
        // =================================================================
        /// <summary>
        /// Thuc thi luong chuyen doi tai lieu tu A sang B bang AI.
        /// Tra ve doi tuong KetQuaAI chua ca Van ban va Duong dan thu muc tam.
        /// </summary>
        public async Task<KetQuaAI> ThucThiChuyenDoiAsync(string ma_che_do, object noi_dung_dau_vao)
        {
            // 1. KIEM TRA PHONG THU (Guard Clauses)
            if (string.IsNullOrEmpty(ma_che_do))
            {
                throw new ArgumentException("Ma che do thuc thi khong duoc de trong.");
            }

            if (noi_dung_dau_vao == null)
            {
                throw new ArgumentNullException(nameof(noi_dung_dau_vao), "Du lieu dau vao (Duong dan hoac Danh sach anh) bi null.");
            }

            // =================================================
            // BƯỚC 1: Đóng gói JSON (Protocol Definition)
            // =================================================
            // Newtonsoft.Json se tu dong xu ly tinh da hinh:
            // - Neu noi_dung_dau_vao la string -> JSON: "duong_dan_pdf": "C:\\..."
            // - Neu noi_dung_dau_vao la string[] -> JSON: "duong_dan_pdf": ["path1", "path2"]
            var du_lieu_gui_python = new
            {
                duong_dan_pdf = noi_dung_dau_vao, // Giu key nay de tuong thich logic Python hien tai
                ma_che_do = ma_che_do
            };

            // Chuyen doi doi tuong sang chuoi JSON thuan
            string chuoi_json_input = JsonConvert.SerializeObject(du_lieu_gui_python);

            // Ghi log de kiem chung du lieu truoc khi day sang Python Process
            Debug.WriteLine($"[UseCase] Goi Python voi JSON: {chuoi_json_input}");

            // =================================================
            // BƯỚC 2: Gọi Gateway thực thi (Anti-Corruption Layer)
            // =================================================
            // Gateway se tu dong: 
            // - Tim Python.exe, Tim xu_ly_ai.py
            // - Bo sung db_path dích danh tu C#
            // - Tao output_path cho Python ghi file
            // - Tra ve doi tuong KetQuaAI gom (.VanBan, .ThuMucTam)
            KetQuaAI ket_qua_tu_gateway = await _cauNoiVoiPython.ThucThiXuLyAiAsync(chuoi_json_input);

            // =================================================
            // BƯỚC 3: Hậu kiểm kết quả (Validation)
            // =================================================
            // Truong hop A: Gateway tra ve null (Loi logic nghiem trong)
            if (ket_qua_tu_gateway == null)
            {
                throw new Exception("He thong AI Gateway khong phan hoi ket qua.");
            }

            // Truong hop B: Van ban tra ve bi trong (Loi API Key hoac Model AI)
            if (string.IsNullOrWhiteSpace(ket_qua_tu_gateway.VanBan))
            {
                throw new Exception("AI khong trich xuat duoc noi dung. Vui long kiem tra Internet, API Key hoac file dau vao.");
            }

            // Ghi log trang thai thanh cong
            Debug.WriteLine($"[UseCase] Nhan du lieu thanh cong tu thu muc: {ket_qua_tu_gateway.ThuMucTam}");

            // 4. Tra ve KetQuaAI cho tang UI (Form) de hien thi Word va don dep
            return ket_qua_tu_gateway;
        }
    }
}