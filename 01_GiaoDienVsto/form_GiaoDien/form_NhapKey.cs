using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TienIchToanHocWord.HaTang.LuuTru; // Chua lop RepositoryAI moi

namespace TienIchToanHocWord.GiaoDienVsto.form_GiaoDien
{
    /// <summary>
    /// Form nhap lieu API Key cho nguoi dung.
    /// Su dung RepositoryAI duy nhat de tuong tac voi Database.
    /// </summary>
    public partial class FormNhapApiKey : Form
    {
        // ======================================================
        // DEPENDENCY: REPOSITORY DUY NHẤT (THEO KIẾN TRÚC MỚI)
        // ======================================================
        private readonly RepositoryAI _repository;

        /// <summary>
        /// Constructor. Nhan Repository duy nhat qua Dependency Injection.
        /// </summary>
        /// <param name="repository">Lop quan ly database sau khi da hop nhat.</param>
        public FormNhapApiKey(RepositoryAI repository)
        {
            InitializeComponent();

            // Guard Clause: Đảm bảo dependency không null
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // ======================================================
        // FUNCTION: btn_luu_Click
        // Chức năng: Đọc, làm sạch chuỗi nhập và lưu vào DB qua Repository.
        // ======================================================
        private void btn_luu_Click(object sender, EventArgs e)
        {
            // 1. Lấy nội dung thô từ textbox
            string noiDungNhap = txtDanhSachApiKey.Text;

            // 2. Logic xử lý chuỗi (Tách dòng, Trim khoảng trắng, Lọc key rác, Loại trùng)
            if (!string.IsNullOrWhiteSpace(noiDungNhap))
            {
                List<string> danhSachKey = noiDungNhap
                    .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .Where(x => x.Length > 10) // Điều kiện lọc key hợp lệ (Gemini key thường dài)
                    .Distinct()                // Loại bỏ các key trùng lặp trong nội dung nhập
                    .ToList();

                try
                {
                    // 3. Gọi Repository tập trung để lưu vào SQLite
                    _repository.LuuDanhSachApiKey(danhSachKey);

                    // Thông báo cho người dùng (Tùy chọn)
                    // MessageBox.Show($"Đã lưu thành công {danhSachKey.Count} API Key.", "Thông báo");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lưu API Key: {ex.Message}", "Lỗi Cơ Sở Dữ Liệu");
                    return;
                }
            }

            // 4. Đóng form và trả về kết quả thành công cho Ribbon cập nhật UI
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // ======================================================
        // FUNCTION: btn_huy_Click
        // Chức năng: Hủy bỏ thao tác nhập liệu.
        // ======================================================
        private void btn_huy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}