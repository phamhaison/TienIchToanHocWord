using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;

namespace TienIchToanHocWord.XuLyVoiAi
{
    public class TacVuAiUseCase
    {
        private readonly CauNoiVoiPython _gateway;

        public TacVuAiUseCase(CauNoiVoiPython gateway)
        {
            _gateway = gateway;
        }

        public async Task<KetQuaAI> ThucThiTacVuAsync(string maTacVu, string theLoai, string yeuCauThem, double nhietDo)
        {
            Word.Selection selection = Globals.ThisAddIn.Application.Selection;
            object inputData = null;
            string imgPath = "";

            // LOGIC RẼ NHÁNH: TOÁN HỌC (ẢNH) VS VĂN BẢN (TEXT)
            if (theLoai == "Câu hỏi Toán học")
            {
                // Chụp ảnh vùng chọn vì Word chứa MathType/OMML AI không đọc được text thô
                imgPath = ChupAnhVungChon(selection);
                inputData = imgPath;
            }
            else
            {
                inputData = selection.Text;
            }
            // Tạo JSON đầu vào cho script Python
            var inputJson = new
            {
                db_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ai_toan_hoc.db"),
                output_path = "", // Sẽ được Gateway điền
                ma_tac_vu = maTacVu,
                nhiet_do = nhietDo, // ĐƯA NHIỆT ĐỘ VÀO JSON
                duong_dan_anh = imgPath,
                text_vung_chon = (theLoai == "Văn bản thường") ? selection.Text : "",
                yeu_cau_them = yeuCauThem
            };

            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(inputJson);
            return await _gateway.ThucThiXuLyAiAsync(jsonString, "Tac_Vu_Ai.py");
        }


        // FILE: TacVuAiUseCase.cs

        // FILE: TacVuAiUseCase.cs

        public async Task<KetQuaAI> ThucThiGiaoTiepTangCuongAsync(string yeuCauMoi, string lichSu, string theLoai, string maTacVu)
        {
            Word.Selection selection = Globals.ThisAddIn.Application.Selection;
            string imgPath = "";
            string textVungChon = "";

            // LOGIC RẼ NHÁNH: KIỂM TRA VÙNG CHỌN CỦA WORD
            // Chỉ thực hiện lấy dữ liệu từ Word nếu người dùng có bôi đen (Start != End)
            bool coVungChon = selection != null && selection.Start != selection.End;

            if (coVungChon)
            {
                if (theLoai == "Câu hỏi Toán học")
                {
                    imgPath = ChupAnhVungChon(selection);
                }
                else
                {
                    textVungChon = selection.Text;
                }
            }

            // Đóng gói JSON gửi sang Python
            var inputJson = new
            {
                ma_tac_vu = "CHAT_TANG_CUONG",
                ten_tac_vu_dang_chon = maTacVu, // BỔ SUNG NGỮ CẢNH TÁC VỤ
                the_loai = theLoai,
                yeu_cau_moi = yeuCauMoi,
                lich_su_truoc_do = lichSu,
                text_vung_chon = textVungChon, // Có thể rỗng nếu không chọn gì
                duong_dan_anh = imgPath         // Có thể rỗng nếu không chọn gì
            };

            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(inputJson);
            return await _gateway.ThucThiXuLyAiAsync(jsonString, "Tac_Vu_Ai.py");
        }
        private string ChupAnhVungChon(Word.Selection selection)
        {
            // Logic: Copy vùng chọn sang Clipboard -> Lưu thành ảnh PNG tạm
            selection.CopyAsPicture();
            if (Clipboard.ContainsImage())
            {
                Image img = Clipboard.GetImage();
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"tmp_selection_{Guid.NewGuid()}.png");
                img.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                return path;
            }
            return "";
        }
    }
}