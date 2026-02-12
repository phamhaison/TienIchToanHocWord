// [FILE: LopChuyenCongThucSangMT]
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;
using System.Threading; // Cần thêm thư viện này cho Thread.Sleep

namespace TienIchToanHocWord.UngDung
{
    /// <summary>
    /// Class chuyên trách xử lý chuyển đổi liên quan đến MathType (MTCommand_TeXToggle)
    /// </summary>
    public class LopChuyenCongThucSangMT
    {
        private Word.Application UngDungWord => Globals.ThisAddIn.Application;

        // Hàm này chuyển đổi LaTeX trong vùng chọn thành MathType (dùng TeXToggle)
        public void LatexSangMathTypeVungChon(Word.Range vungChon)
        {
            if (vungChon == null || vungChon.Start == vungChon.End)
            {
                MessageBox.Show("Vui lòng bôi đen vùng văn bản chứa mã LaTeX cần chuyển đổi.", "Thông báo");
                return;
            }

            UngDungWord.ScreenUpdating = false;
            try
            {
                // Logic: Chọn vùng và gọi lệnh TeXToggle.
                vungChon.Select();
                UngDungWord.Run("MTCommand_TeXToggle");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi chuyển LaTeX sang MathType: " + ex.Message, "Lỗi");
            }
            finally
            {
                UngDungWord.ScreenUpdating = true;
            }
        }

        /// <summary>
        /// KHÔI PHỤC CHỨC NĂNG GỐC: Cắt vùng chọn -> Gọi MathType -> Dán -> Đóng MT
        /// (Đối chiếu chính xác logic VBA: LuaChon_Den_MathType)
        /// </summary>
        public void LuaChonSangMathType(Word.Range vungChon)
        {
            if (vungChon == null || vungChon.Start == vungChon.End)
            {
                MessageBox.Show("Vui lòng bôi đen vùng văn bản cần chuyển đổi thành công thức MathType.", "Thông báo");
                return;
            }

            UngDungWord.ScreenUpdating = false;
            try
            {
                // 1. Cắt vùng chọn vào Clipboard (tương đương phamvi.Cut trong VBA)
                vungChon.Cut();

                // 2. Gọi cửa sổ nhập công thức MathType (tương đương Application.Run MacroName:="MTCommand_InsertInlineEqn")
                // Lưu ý: Cần đảm bảo MathType Add-in đã được cài đặt và kích hoạt
                UngDungWord.Run("MTCommand_InsertInlineEqn");

                // 3. Đợi 1 giây (1000ms) để MathType khởi động và mở cửa sổ nhập
                // (Tương đương Sleep 2000 trong VBA cũ, giảm xuống 1s để tối ưu)
                Thread.Sleep(1000);

                // 4. Gửi tổ hợp phím Ctrl+V để Dán nội dung từ Clipboard vào MathType
                // Tham số True đảm bảo lệnh được xử lý trước khi tiếp tục
                SendKeys.SendWait("^v");

                // 5. Đợi thêm 200ms để đảm bảo nội dung đã được dán hoàn tất
                Thread.Sleep(200);

                // 6. Gửi tổ hợp phím Alt+F4 để đóng cửa sổ MathType
                // Việc đóng cửa sổ sẽ tự động chèn công thức đã dán vào Word
                SendKeys.SendWait("%{F4}"); // % là Alt, {F4} là phím F4
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi chuyển vùng chọn sang MathType: " + ex.Message, "Lỗi");
            }
            finally
            {
                UngDungWord.ScreenUpdating = true;
            }
        }
    }
}