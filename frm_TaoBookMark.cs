using System;
using System.Windows.Forms;

namespace TienIchToanHocWord
{
    public partial class frm_TaoBookMark : Form
    {
        public frm_TaoBookMark()
        {
            InitializeComponent();
        }

        // Nút: Tạo dấu trang câu hỏi
        private Microsoft.Office.Tools.CustomTaskPane taskPaneCauHoi;

        private void btn_BookMarkCauHoi_Click(object sender, EventArgs e)
        {
            if (taskPaneCauHoi == null)
            {
                TaskPanel_DauTrangCauHoi control = new TaskPanel_DauTrangCauHoi();
                taskPaneCauHoi = Globals.ThisAddIn.CustomTaskPanes.Add(control, "Danh sách câu hỏi");
                taskPaneCauHoi.Width = 300;
            }
            taskPaneCauHoi.Visible = true;
        }

        // Nút: Tạo dấu trang tùy chỉnh
        private void btn_BookMarkTuyChinh_Click(object sender, EventArgs e)
        {
            // Logic xử lý sẽ viết ở bước tiếp theo
            MessageBox.Show("Chuc nang Tao dau trang tuy chinh dang duoc thiet ke.");
        }
    }
}