namespace TienIchToanHocWord
{
    partial class CanChinhPhuonAnPhamVi
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_CacPhuongAnCungDong = new System.Windows.Forms.Button();
            this.btn_HaiPhuongAnMotDong = new System.Windows.Forms.Button();
            this.btn_MoiPhuongAnMotDong = new System.Windows.Forms.Button();
            this.btn_TuDongCanChinhThongMinh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_CacPhuongAnCungDong
            // 
            this.btn_CacPhuongAnCungDong.Location = new System.Drawing.Point(39, 32);
            this.btn_CacPhuongAnCungDong.Name = "btn_CacPhuongAnCungDong";
            this.btn_CacPhuongAnCungDong.Size = new System.Drawing.Size(292, 27);
            this.btn_CacPhuongAnCungDong.TabIndex = 0;
            this.btn_CacPhuongAnCungDong.Text = "Các phương án ở cùng một dòng";
            this.btn_CacPhuongAnCungDong.UseVisualStyleBackColor = true;
            this.btn_CacPhuongAnCungDong.Click += new System.EventHandler(this.btn_CacPhuongAnCungDong_Click);
            // 
            // btn_HaiPhuongAnMotDong
            // 
            this.btn_HaiPhuongAnMotDong.Location = new System.Drawing.Point(39, 79);
            this.btn_HaiPhuongAnMotDong.Name = "btn_HaiPhuongAnMotDong";
            this.btn_HaiPhuongAnMotDong.Size = new System.Drawing.Size(292, 25);
            this.btn_HaiPhuongAnMotDong.TabIndex = 1;
            this.btn_HaiPhuongAnMotDong.Text = "Hai phương án ở cùng một dòng";
            this.btn_HaiPhuongAnMotDong.UseVisualStyleBackColor = true;
            this.btn_HaiPhuongAnMotDong.Click += new System.EventHandler(this.btn_HaiPhuongAnMotDong_Click);
            // 
            // btn_MoiPhuongAnMotDong
            // 
            this.btn_MoiPhuongAnMotDong.Location = new System.Drawing.Point(39, 126);
            this.btn_MoiPhuongAnMotDong.Name = "btn_MoiPhuongAnMotDong";
            this.btn_MoiPhuongAnMotDong.Size = new System.Drawing.Size(292, 26);
            this.btn_MoiPhuongAnMotDong.TabIndex = 2;
            this.btn_MoiPhuongAnMotDong.Text = "Mỗi phương án ở một dòng";
            this.btn_MoiPhuongAnMotDong.UseVisualStyleBackColor = true;
            this.btn_MoiPhuongAnMotDong.Click += new System.EventHandler(this.btn_MoiPhuongAnMotDong_Click);
            // 
            // btn_TuDongCanChinhThongMinh
            // 
            this.btn_TuDongCanChinhThongMinh.Location = new System.Drawing.Point(39, 173);
            this.btn_TuDongCanChinhThongMinh.Name = "btn_TuDongCanChinhThongMinh";
            this.btn_TuDongCanChinhThongMinh.Size = new System.Drawing.Size(292, 26);
            this.btn_TuDongCanChinhThongMinh.TabIndex = 3;
            this.btn_TuDongCanChinhThongMinh.Text = "Tự động căn chỉnh thông minh";
            this.btn_TuDongCanChinhThongMinh.UseVisualStyleBackColor = true;
            this.btn_TuDongCanChinhThongMinh.Click += new System.EventHandler(this.btn_TuDongCanChinhThongMinh_Click);
            // 
            // CanChinhPhuonAnPhamVi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 229);
            this.Controls.Add(this.btn_TuDongCanChinhThongMinh);
            this.Controls.Add(this.btn_MoiPhuongAnMotDong);
            this.Controls.Add(this.btn_HaiPhuongAnMotDong);
            this.Controls.Add(this.btn_CacPhuongAnCungDong);
            this.Name = "CanChinhPhuonAnPhamVi";
            this.Text = "Căn chỉnh phương án theo phạm vi lựa chọn";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_CacPhuongAnCungDong;
        private System.Windows.Forms.Button btn_HaiPhuongAnMotDong;
        private System.Windows.Forms.Button btn_MoiPhuongAnMotDong;
        private System.Windows.Forms.Button btn_TuDongCanChinhThongMinh;
    }
}