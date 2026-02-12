using System;
using System.Windows.Forms;
using TienIchToanHocWord.UngDung;
namespace TienIchToanHocWord.GiaoDienVsto.form_GiaoDien // Đảm bảo lớp nằm TRONG namespace này
{
    public partial class ChuanHoaTrangDayHoc : Form
    {
        // Khai báo đối tượng từ lớp nghiệp vụ mới
        private LopChuanHoaTrangDayHoc nghiepVu;

        public ChuanHoaTrangDayHoc()
        {
            InitializeComponent();
            nghiepVu = new LopChuanHoaTrangDayHoc();
        }

        private void btn_Phone_Click(object sender, EventArgs e)
        {
            nghiepVu.ChuanHoaChoPhone();
            this.Close();
        }

        private void btn_Ipad_Click(object sender, EventArgs e)
        {
            nghiepVu.ChuanHoaChoIpad();
            this.Close();
        }

        private void btn_TietKiemA4_Click(object sender, EventArgs e)
        {
            nghiepVu.ChuanHoaTietKiemA4();
            this.Close();
        }
    }
}