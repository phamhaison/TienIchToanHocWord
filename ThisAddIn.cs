using System;
using System.Windows.Forms;
using System.Diagnostics;
using TienIchToanHocWord.XuLyVoiAi;
using TienIchToanHocWord.HaTang.LuuTru;
using TienIchToanHocWord.UngDung;

namespace TienIchToanHocWord
{
    /// <summary>
    /// Lop Composition Root - Noi khoi tao va ket noi toan bo cac thanh phan cua Add-in.
    /// </summary>
    public partial class ThisAddIn
    {
        // =================================================================
        // 1. CÁC LỚP NGHIỆP VỤ CŨ (WORD INTEROP)
        // =================================================================
        public LopLatexToEquation boXuLyCongThuc;
        public LopChuyenCongThucSangMT boChuyenCongThucSangMT;

        // =================================================================
        // 2. CÁC LỚP HỆ THỐNG AI MỚI
        // =================================================================
        public CauNoiVoiPython boCauNoiVoiPython;
        public XuLyChuyenDoiTaiLieuUseCase boXuLyChuyenDoiTaiLieu;

        // Repository duy nhat quan ly Database (Thay the cho 5 file cu)
        public RepositoryAI repositoryAI;
        // THÊM DÒNG NÀY: Khai báo Use Case cho Tác vụ AI
        public TacVuAiUseCase boTacVuAiUseCase;

        // =================================================================
        // HÀM KHỞI CHẠY HỆ THỐNG (STARTUP)
        // =================================================================
        private void ThisAddIn_Startup(object sender, EventArgs e)
        {
            try
            {
                // BƯỚC 1: KHỞI TẠO REPOSITORY & CO SƠ DU LIEU
                // Python se tu truy cap vao file DB nay de lay Key va Prompt
                repositoryAI = new RepositoryAI();
                repositoryAI.DamBaoHeThongSanSang(); // Tao file .db va cac bang neu chua co
                repositoryAI.DatLaiTrangThaiLoi();    // Reset trang thai key de bat dau phien lam viec moi

                // BƯỚC 2: KHỞI TẠO AI GATEWAY (CAU NOI PYTHON)
                // Logic tu dong tim kiem thong minh nằm trong Constructor
                string folderBase = AppDomain.CurrentDomain.BaseDirectory;
                boCauNoiVoiPython = new CauNoiVoiPython(folderBase);

                // BƯỚC 3: KHỞI TẠO USE CASE (APPLICATION LAYER)
                // Use Case chi phu thuoc vao Gateway theo dung SOLID
                boXuLyChuyenDoiTaiLieu = new XuLyChuyenDoiTaiLieuUseCase(boCauNoiVoiPython);
                boTacVuAiUseCase = new TacVuAiUseCase(boCauNoiVoiPython);

                // BƯỚC 4: KHỞI TẠO CÁC DỊCH VỤ WORD INTEROP LEGACY
                boXuLyCongThuc = new LopLatexToEquation();
                boChuyenCongThucSangMT = new LopChuyenCongThucSangMT();
                // KHỞI TẠO Use Case mới tại đây
                
                
            }
            catch (Exception ex)
            {
                // Thong bao loi dích danh de de dang debug moi truong AI
                MessageBox.Show(
                    $"He thong AI khong the khoi dong.\nChi tiet: {ex.Message}",
                    "Loi Khoi Tao He Thong",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        // =================================================================
        // HÀM ĐÓNG HỆ THỐNG (SHUTDOWN)
        // =================================================================
        private void ThisAddIn_Shutdown(object sender, EventArgs e)
        {
            try
            {
                // Giai phong tai nguyen, luu cache neu co
                if (boXuLyCongThuc is LopLatexToEquation xuLyCache)
                {
                    xuLyCache.KetThucPhienLamViec();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Loi khi tat Add-in: " + ex.Message);
            }
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new EventHandler(ThisAddIn_Startup);
            this.Shutdown += new EventHandler(ThisAddIn_Shutdown);
        }

        #endregion
    }
}