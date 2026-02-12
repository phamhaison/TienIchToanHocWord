using System;
using System.Windows.Forms;          // namespace, KHÔNG phải class
using TienIchToanHocWord.UngDung;
using Word = Microsoft.Office.Interop.Word;

namespace TienIchToanHocWord.GiaoDienVsto.form_GiaoDien
{
    // Form giao diện căn chỉnh phương án
    // Ten class phai khop 100% voi file Designer.cs
    public partial class CanChinhPhuonAnPhamVi : Form
    {
        private LopCanChinhPhuongAnTheoPhamVi boCanChinh;

        public CanChinhPhuonAnPhamVi()
        {
            InitializeComponent();

            boCanChinh = new LopCanChinhPhuongAnTheoPhamVi();

            // Giu form luon nam tren cac cua so khac
            this.TopMost = true;

            // Khong hien tren taskbar khi Word full screen
            this.ShowInTaskbar = false;

            // Co dinh vi tri xuat hien
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
                MessageBox.Show("Vui long boi den vung van ban can xu ly.");
                return;
            }

            Globals.ThisAddIn.Application.ScreenUpdating = false;
            try
            {
                hanhDong(vungChon);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi: " + ex.Message);
            }
            finally
            {
                Globals.ThisAddIn.Application.ScreenUpdating = true;
            }
        }
    }
}
