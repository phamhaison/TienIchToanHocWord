namespace TienIchToanHocWord.GiaoDienVsto.form_GiaoDien
{
    partial class frm_TaoBookMark
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
            this.btn_BookMarkCauHoi = new System.Windows.Forms.Button();
            this.btn_BookMarkTuyChinh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_BookMarkCauHoi
            // 
            this.btn_BookMarkCauHoi.Location = new System.Drawing.Point(36, 32);
            this.btn_BookMarkCauHoi.Name = "btn_BookMarkCauHoi";
            this.btn_BookMarkCauHoi.Size = new System.Drawing.Size(145, 23);
            this.btn_BookMarkCauHoi.TabIndex = 0;
            this.btn_BookMarkCauHoi.Text = "Tạo dấu trang câu hỏi";
            this.btn_BookMarkCauHoi.UseVisualStyleBackColor = true;
            this.btn_BookMarkCauHoi.Click += new System.EventHandler(this.btn_BookMarkCauHoi_Click);
            // 
            // btn_BookMarkTuyChinh
            // 
            this.btn_BookMarkTuyChinh.Location = new System.Drawing.Point(36, 89);
            this.btn_BookMarkTuyChinh.Name = "btn_BookMarkTuyChinh";
            this.btn_BookMarkTuyChinh.Size = new System.Drawing.Size(145, 23);
            this.btn_BookMarkTuyChinh.TabIndex = 1;
            this.btn_BookMarkTuyChinh.Text = "Tạo dấu trang tùy chỉnh";
            this.btn_BookMarkTuyChinh.UseVisualStyleBackColor = true;
            this.btn_BookMarkTuyChinh.Click += new System.EventHandler(this.btn_BookMarkTuyChinh_Click);
            // 
            // frm_TaoBookMark
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(223, 145);
            this.Controls.Add(this.btn_BookMarkTuyChinh);
            this.Controls.Add(this.btn_BookMarkCauHoi);
            this.Name = "frm_TaoBookMark";
            this.Text = "Tạo dấu trang";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_BookMarkCauHoi;
        private System.Windows.Forms.Button btn_BookMarkTuyChinh;
    }
}