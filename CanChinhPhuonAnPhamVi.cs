using System;
using System.Windows.Forms;
// Sửa lỗi CS0246: Thêm khai báo định danh Word
using Word = Microsoft.Office.Interop.Word;

namespace TienIchToanHocWord
{
    // Đảm bảo tên class này khớp chính xác với file .Designer.cs
    public partial class CanChinhPhuonAnPhamVi : Form
    {
        private LopCanChinhPhuongAnTheoPhamVi boCanChinh;

        public CanChinhPhuonAnPhamVi()
        {
            InitializeComponent();
            boCanChinh = new LopCanChinhPhuongAnTheoPhamVi();

            // GIẢI PHÁP (A): 
            // 1. Giữ Form luôn nằm trên các cửa sổ khác
            this.TopMost = true;
            // 2. Cho phép Form hiển thị ngay cả khi Word đang ở chế độ Full Screen
            this.ShowInTaskbar = false;
            // 3. Đặt vị trí xuất hiện cố định để không che khuất văn bản chính giữa
            this.StartPosition = FormStartPosition.Manual;
            this.Left = 100;
            this.Top = 100;
        }

        private void btn_CacPhuongAnCungDong_Click(object sender, EventArgs e)
        {
            ThucThiNghiepVu(r => boCanChinh.CanChinh_4PA_Tren_1Dong(r));
        }

        private void btn_HaiPhuongAnMotDong_Click(object sender, EventArgs e)
        {
            ThucThiNghiepVu(r => boCanChinh.CanChinh_2PA_Tren_1Dong(r));
        }

        private void btn_MoiPhuongAnMotDong_Click(object sender, EventArgs e)
        {
            ThucThiNghiepVu(r => boCanChinh.CanChinh_MoiPA_1Dong(r));
        }

        private void btn_TuDongCanChinhThongMinh_Click(object sender, EventArgs e)
        {
            ThucThiNghiepVu(r => boCanChinh.TuDongCanChinhThongMinh(r));
        }

        private void ThucThiNghiepVu(Action<Word.Range> hanhDong)
        {
            Word.Selection luaChon = Globals.ThisAddIn.Application.Selection;
            Word.Range vungChon = luaChon.Range;

            if (vungChon.Start == vungChon.End)
            {
                MessageBox.Show("Vui lòng bôi đen vùng văn bản cần xử lý.");
                return;
            }

            Globals.ThisAddIn.Application.ScreenUpdating = false;
            try
            {
                hanhDong(vungChon);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                Globals.ThisAddIn.Application.ScreenUpdating = true;
                // ĐÃ XÓA this.Close() - Form sẽ giữ nguyên trạng thái
            }
        }
    }
}