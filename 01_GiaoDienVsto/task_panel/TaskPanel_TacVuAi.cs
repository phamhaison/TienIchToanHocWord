using System;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;
using TienIchToanHocWord.XuLyVoiAi;
using Word = Microsoft.Office.Interop.Word;

namespace TienIchToanHocWord.GiaoDienVsto.task_panel
{
    public partial class TaskPanel_TacVuAi : UserControl
    {
        private readonly TacVuAiUseCase _tacVuAiUseCase;

        public TaskPanel_TacVuAi()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor chinh thuc cua Task Panel. 
        /// Khoi tao moi truong, gia tri mac dinh va dang ky su kien Ctrl + Enter cho o nhap lieu.
        /// </summary>
        public TaskPanel_TacVuAi(TacVuAiUseCase tacVuAiUseCase) : this()
        {
            // Gán UseCase thông qua Dependency Injection
            _tacVuAiUseCase = tacVuAiUseCase ?? throw new ArgumentNullException(nameof(tacVuAiUseCase));

            // 1. CẤU HÌNH TRACKBAR NHIỆT ĐỘ
            // Quy ước: Value 6 tương ứng Nhiệt độ 0.6
            trackBar_NhietDo.Minimum = 0;
            trackBar_NhietDo.Maximum = 20;
            trackBar_NhietDo.Value = 6;
            trackBar_NhietDo.TickFrequency = 2;

            // 2. KHỞI TẠO DANH SÁCH THỂ LOẠI (Toán, Văn bản, Prompt, Tự do)
            KhoiTaoGiaTriTheLoai();

            // 3. ĐĂNG KÝ SỰ KIỆN CHỦ ĐỘNG (Manual Event Hookup)
            // Viec dang ky bang code dam bao logic khong bi mat khi Designer bi crash
            this.comboBox_ChonTheLoai.SelectedIndexChanged += new System.EventHandler(this.comboBox_ChonTheLoai_SelectedIndexChanged);

            // Đăng ký sự kiện KeyDown để bắt tổ hợp phím Ctrl + Enter
            this.textBox_NhapNoiDungTangCuong.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_NhapNoiDungTangCuong_KeyDown);

            // Đăng ký các nút bấm chức năng
            this.button_ThucThi.Click += new System.EventHandler(this.button_ThucThi_Click);
            this.button_ChenWord.Click += new System.EventHandler(this.button_ChenWord_Click);
            this.button_GuiAiTangCuong.Click += new System.EventHandler(this.button_GuiAiTangCuong_Click);

            // 4. ĐỒNG BỘ DANH SÁCH TÁC VỤ
            CapNhatDanhSachTacVu();

            GhiLog("Hệ thống Tác vụ AI đã sẵn sàng. Nhấn Ctrl + Enter để gửi nhanh yêu cầu.");
        }
        // Tách logic cập nhật danh sách tác vụ ra hàm riêng để tái sử dụng
        private void CapNhatDanhSachTacVu()
        {
            if (comboBox_ChonTheLoai.SelectedItem == null) return;
            string selected = comboBox_ChonTheLoai.SelectedItem.ToString();
            comboBox_ChonTacVu.Items.Clear();

            switch (selected)
            {
                case "Câu hỏi Toán học":
                    comboBox_ChonTacVu.Items.AddRange(new string[] {
                "Hướng dẫn giải", "Lời giải chi tiết", "Câu 4 lựa chọn",
                "Câu dạng Đúng/Sai", "Câu trả lời ngắn", "Câu tự luận",
                "Thiết kế dạy học", "Thiết kế HĐ CV5512", "Viết lại GA 5512"
            });
                    break;
                case "Văn bản thường":
                    comboBox_ChonTacVu.Items.AddRange(new string[] {
                "Tóm tắt nội dung", "Sửa lại câu từ", "Văn học", "Báo trí",
                "Hành chính", "Dịch thuật EV", "Tham luận", "KĐ Cá nhân", "KĐ Đảng viên"
            });
                    break;
                case "Viết Prompt":
                    comboBox_ChonTacVu.Items.AddRange(new string[] {
                "CSharp", "Python", "CSharp&Python"
            });
                    break;
                case "Tự do":
                    comboBox_ChonTacVu.Items.Add("Tự do");
                    break;
            }
            if (comboBox_ChonTacVu.Items.Count > 0) comboBox_ChonTacVu.SelectedIndex = 0;
        }

        private void KhoiTaoGiaTriTheLoai()
        {
            comboBox_ChonTheLoai.Items.Clear();
            comboBox_ChonTheLoai.Items.AddRange(new string[] {
        "Câu hỏi Toán học",
        "Văn bản thường",
        "Viết Prompt",
        "Tự do"
        });
            comboBox_ChonTheLoai.SelectedIndex = 0;
        }
        private void KhoiTaoGiaTri()
        {
            comboBox_ChonTheLoai.Items.Clear();
            comboBox_ChonTheLoai.Items.Add("Câu hỏi Toán học");
            comboBox_ChonTheLoai.Items.Add("Văn bản thường");
            comboBox_ChonTheLoai.SelectedIndex = 0;
        }

        // Sự kiện chạy khi người dùng bấm chọn trên giao diện
        private void comboBox_ChonTheLoai_SelectedIndexChanged(object sender, EventArgs e)
        {
            CapNhatDanhSachTacVu();
            GhiLog("Đã cập nhật danh sách tác vụ cho: " + comboBox_ChonTheLoai.SelectedItem.ToString());
        }

        // SỬA TÊN HÀM: btn_ThucThi -> button_ThucThi
        private async void button_ThucThi_Click(object sender, EventArgs e)
        {
            await ThucHienGoiAi();
        }

        private async Task ThucHienGoiAi(string yeuCauThem = "")
        {
            // 1. KHAI BAO BIEN TRUNG GIAN (De lay du lieu tu luong UI sang luong Task)
            string maTacVu = "";
            string theLoai = "";
            double nhietDoAi = 0.6; // Mac dinh 0.6

            // 2. DOC DU LIEU TU UI AN TOAN (Su dung Invoke)
            this.Invoke(new Action(() => {
                if (comboBox_ChonTacVu.SelectedItem != null)
                    maTacVu = comboBox_ChonTacVu.SelectedItem.ToString();

                if (comboBox_ChonTheLoai.SelectedItem != null)
                    theLoai = comboBox_ChonTheLoai.SelectedItem.ToString();

                // QUY DOI NHIET DO: TrackBar (0-20) -> AI Temperature (0.0-2.0)
                nhietDoAi = trackBar_NhietDo.Value / 10.0;

                // Thiet lap trang thai dang xu ly cho UI
                button_ThucThi.Enabled = false;
                progressBar_ThanhTienTrinh.Value = 15;
                progressBar_ThanhTienTrinh.Style = ProgressBarStyle.Marquee; // Chay lien tuc khi cho AI
            }));

            // Neu chua chon tac vu thi dung lai
            if (string.IsNullOrEmpty(maTacVu))
            {
                this.Invoke(new Action(() => { button_ThucThi.Enabled = true; }));
                return;
            }

            try
            {
                GhiLog($"Dang thuc thi: {maTacVu} (Nhiet do: {nhietDoAi})...");

                // 3. GOI NGHERP VU (USE CASE) - Thuc thi tai luong Task (Background)
                // UseCase se tu dong chup anh vung chon neu la Toan hoc hoac lay Text neu la Van ban
                KetQuaAI ketQua = await _tacVuAiUseCase.ThucThiTacVuAsync(maTacVu, theLoai, yeuCauThem, nhietDoAi);

                // 4. CAP NHAT KET QUA LEN GIAO DIEN (Invoke ve luong UI)
                this.Invoke(new Action(() => {
                    // Giai ma Unicode Escape de hien thi tieng Viet co dau va LaTeX chuan
                    string cleanText = System.Text.RegularExpressions.Regex.Unescape(ketQua.VanBan);
                    // Thay the "**" bang mot khoang trang hoac chuoi rong tuy y ban (o day toi dung khoang trang " ")
                    cleanText = cleanText.Replace("**", " ");

                    richTextBox_NoiDungAiTraVe.Text = cleanText;

                    progressBar_ThanhTienTrinh.Style = ProgressBarStyle.Blocks;
                    progressBar_ThanhTienTrinh.Value = 100;
                    GhiLog("AI da hoan tat phan hoi.");

                    // 5. DON DEP TAI NGUYEN (Quan trong: Xoa folder chua anh tam cua phien nay)
                    if (!string.IsNullOrEmpty(ketQua.ThuMucTam) && Directory.Exists(ketQua.ThuMucTam))
                    {
                        try { Directory.Delete(ketQua.ThuMucTam, true); } catch { }
                    }
                }));
            }
            catch (Exception ex)
            {
                // Xu ly loi va bao cao len giao dien
                this.Invoke(new Action(() => {
                    GhiLog($"LOI HE THONG: {ex.Message}");
                    progressBar_ThanhTienTrinh.Style = ProgressBarStyle.Blocks;
                    progressBar_ThanhTienTrinh.Value = 0;
                }));
            }
            finally
            {
                // 6. KHOI PHUC TRANG THAI NUT BAM
                this.Invoke(new Action(() => {
                    button_ThucThi.Enabled = true;
                }));
            }
        }

        /// <summary>
        /// Xu ly phim tat Ctrl + Enter de gui nhanh yeu cau tang cuong AI.
        /// </summary>
        private async void textBox_NhapNoiDungTangCuong_KeyDown(object sender, KeyEventArgs e)
        {
            // Kiem tra to hop phim Ctrl + Enter
            if (e.Control && e.KeyCode == Keys.Enter)
            {
                // 1. Ngăn chặn ký tự xuống dòng được thêm vào textbox khi nhấn Enter
                // và ngăn chặn tiếng "beep" khó chịu của Windows
                e.SuppressKeyPress = true;

                string noiDungThem = textBox_NhapNoiDungTangCuong.Text.Trim();

                if (!string.IsNullOrEmpty(noiDungThem))
                {
                    // 2. Ghi log yeu cau vao lich su (tuong tu nhu nut Gui)
                    this.Invoke(new Action(() => {
                        string maTacVu = comboBox_ChonTacVu.Text;
                        richTextBox_NoiDungAiTraVe.AppendText($"\n\n[YÊU CẦU: {maTacVu}]\n{noiDungThem}\n");
                        richTextBox_NoiDungAiTraVe.ScrollToCaret();

                        // Xóa nội dung ô nhập sau khi đã lấy dữ liệu
                        textBox_NhapNoiDungTangCuong.Clear();
                    }));

                    // 3. Kich hoat luong goi AI tang cuong
                    // Ham nay da duoc chung ta "cung hoa" voi Invoke ben trong o cac buoc truoc
                    await ThucHienGoiAiTangCuong(noiDungThem, comboBox_ChonTacVu.Text);
                }
            }
        }

        // SỬA TÊN: button_ChenWord
        private void button_ChenWord_Click(object sender, EventArgs e)
        {
            string content = richTextBox_NoiDungAiTraVe.Text;
            if (string.IsNullOrWhiteSpace(content)) return;

            try
            {
                Word.Selection sel = Globals.ThisAddIn.Application.Selection;
                sel.TypeText(content);
                GhiLog("Đã chèn nội dung vào Word.");
            }
            catch (Exception ex) { GhiLog($"Lỗi chèn: {ex.Message}"); }
        }
        // FILE: TaskPanel_TacVuAi.cs

        // Đăng ký sự kiện Click cho nút Gửi trong Constructor hoặc InitializeComponent
        // this.button_GuiAiTangCuong.Click += new System.EventHandler(this.button_GuiAiTangCuong_Click);

        private async void button_GuiAiTangCuong_Click(object sender, EventArgs e)
        {
            string noiDungNhap = "";
            string maTacVu = "";
            string theLoai = "";

            // Đọc UI an toàn
            this.Invoke(new Action(() => {
                noiDungNhap = textBox_NhapNoiDungTangCuong.Text.Trim();
                maTacVu = comboBox_ChonTacVu.Text;
                theLoai = comboBox_ChonTheLoai.Text;

                if (!string.IsNullOrEmpty(noiDungNhap))
                {
                    // Hiển thị yêu cầu kèm ngữ cảnh tác vụ lên richTextBox lịch sử
                    richTextBox_NoiDungAiTraVe.AppendText($"\n\n[YÊU CẦU: {maTacVu}]\n{noiDungNhap}\n");
                    richTextBox_NoiDungAiTraVe.ScrollToCaret();
                    textBox_NhapNoiDungTangCuong.Clear();
                }
            }));

            if (string.IsNullOrEmpty(noiDungNhap)) return;

            // Gọi hàm thực thi (đã có Invoke bên trong như tôi hướng dẫn ở lượt trước)
            await ThucHienGoiAiTangCuong(noiDungNhap, maTacVu);
        }

        // Sửa lại hàm trợ lý để truyền thêm maTacVu
        /// <summary>
        /// Ham thuc thi giao tiep tang cuong (Chat Contextual).
        /// Hop nhat logic: Doc lich su, Quy doi nhiet do, Goi AI va Lam sach Markdown.
        /// </summary>
        /// <param name="currentRequest">Noi dung nguoi dung vua nhap vao o Tang cuong.</param>
        /// <param name="maTacVu">Ten tac vu dang chon tu ComboBox (de lam ngu canh).</param>
        private async Task ThucHienGoiAiTangCuong(string currentRequest, string maTacVu)
        {
            // 1. KHAI BAO BIEN TRUNG GIAN (Chuan bi du lieu tu luong UI)
            string lichSu = "";
            string theLoai = "";
            double nhietDoAi = 0.6;

            // 2. DOC DU LIEU TU UI AN TOAN (Su dung Invoke)
            this.Invoke(new Action(() => {
                lichSu = richTextBox_NoiDungAiTraVe.Text;
                theLoai = comboBox_ChonTheLoai.Text;

                // Lay nhiet do tu TrackBar (Quy doi 0-20 sang 0.0-2.0)
                nhietDoAi = trackBar_NhietDo.Value / 10.0;

                // Khoa UI va thiet lap tien trinh
                button_GuiAiTangCuong.Enabled = false;
                button_ThucThi.Enabled = false;
                progressBar_ThanhTienTrinh.Style = ProgressBarStyle.Marquee;
                progressBar_ThanhTienTrinh.Value = 10;
            }));

            try
            {
                GhiLog($"Dang gui yeu cau bo sung (Nhiet do: {nhietDoAi})...");

                // 3. GOI NGHERP VU (USE CASE) - Thuc thi tai luong Background
                // Tham so bao gom: Yeu cau moi, Lich su cu, The loai va Ma tac vu de AI khong quen vai tro
                KetQuaAI ketQua = await _tacVuAiUseCase.ThucThiGiaoTiepTangCuongAsync(currentRequest, lichSu, theLoai, maTacVu);

                // 4. XU LY KET QUA VA LAM SACH (Thuc hien tren luong Background truoc khi day len UI)
                // Buoc A: Giai ma Unicode Escape (\u1ebf -> ế)
                string textPhanHoi = System.Text.RegularExpressions.Regex.Unescape(ketQua.VanBan);

                // Buoc B: Xoa bo ky tu Markdown ** (Thay bang khoang trang theo yeu cau)
                textPhanHoi = textPhanHoi.Replace("**", " ");

                // 5. CAP NHAT GIAO DIEN (Invoke ve luong UI)
                this.Invoke(new Action(() => {
                    // Hien thi noi dung sach vao o tra ve
                    richTextBox_NoiDungAiTraVe.Text = textPhanHoi;

                    progressBar_ThanhTienTrinh.Style = ProgressBarStyle.Blocks;
                    progressBar_ThanhTienTrinh.Value = 100;
                    GhiLog("AI da hoan thanh yeu cau bo sung.");

                    // 6. DON DEP TAI NGUYEN (Folder anh tam neu co)
                    if (!string.IsNullOrEmpty(ketQua.ThuMucTam) && Directory.Exists(ketQua.ThuMucTam))
                    {
                        try { Directory.Delete(ketQua.ThuMucTam, true); } catch { }
                    }
                }));
            }
            catch (Exception ex)
            {
                // Xu ly loi va bao cao len log giao dien
                this.Invoke(new Action(() => {
                    GhiLog($"LOI TANG CUONG: {ex.Message}");
                    progressBar_ThanhTienTrinh.Style = ProgressBarStyle.Blocks;
                    progressBar_ThanhTienTrinh.Value = 0;
                }));
            }
            finally
            {
                // 7. KHOI PHUC TRANG THAI CAC NUT BAM
                this.Invoke(new Action(() => {
                    button_GuiAiTangCuong.Enabled = true;
                    button_ThucThi.Enabled = true;
                }));
            }
        }

        private void GhiLog(string message)
        {
            if (richTextBox_LogTienTrinh.InvokeRequired)
            {
                // Nếu đang ở luồng phụ, gọi đệ quy qua Invoke
                richTextBox_LogTienTrinh.Invoke(new Action(() => GhiLog(message)));
                return;
            }

            // Nếu đã ở luồng chính, thực hiện ghi log bình thường
            richTextBox_LogTienTrinh.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\n");
            richTextBox_LogTienTrinh.ScrollToCaret();
        }
    }
}