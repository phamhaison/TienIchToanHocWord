namespace TienIchToanHocWord.GiaoDienVsto.form_GiaoDien
{
    partial class FormNhapApiKey
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtDanhSachApiKey = new System.Windows.Forms.TextBox();
            this.btn_luu = new System.Windows.Forms.Button();
            this.btn_huy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label1.Location = new System.Drawing.Point(12, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(204, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Dán danh sách api key vào đây";
            // 
            // txtDanhSachApiKey
            // 
            this.txtDanhSachApiKey.Location = new System.Drawing.Point(12, 71);
            this.txtDanhSachApiKey.Multiline = true;
            this.txtDanhSachApiKey.Name = "txtDanhSachApiKey";
            this.txtDanhSachApiKey.Size = new System.Drawing.Size(256, 311);
            this.txtDanhSachApiKey.TabIndex = 1;
            // 
            // btn_luu
            // 
            this.btn_luu.Location = new System.Drawing.Point(12, 406);
            this.btn_luu.Name = "btn_luu";
            this.btn_luu.Size = new System.Drawing.Size(123, 34);
            this.btn_luu.TabIndex = 2;
            this.btn_luu.Text = "Lưu lại";
            this.btn_luu.UseVisualStyleBackColor = true;
            this.btn_luu.Click += new System.EventHandler(this.btn_luu_Click);
            // 
            // btn_huy
            // 
            this.btn_huy.Location = new System.Drawing.Point(145, 406);
            this.btn_huy.Name = "btn_huy";
            this.btn_huy.Size = new System.Drawing.Size(123, 34);
            this.btn_huy.TabIndex = 3;
            this.btn_huy.Text = "Hủy bỏ";
            this.btn_huy.UseVisualStyleBackColor = true;
            this.btn_huy.Click += new System.EventHandler(this.btn_huy_Click);
            // 
            // FormNhapApiKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 467);
            this.Controls.Add(this.btn_huy);
            this.Controls.Add(this.btn_luu);
            this.Controls.Add(this.txtDanhSachApiKey);
            this.Controls.Add(this.label1);
            this.Name = "FormNhapApiKey";
            this.Text = "Nhập các api key";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDanhSachApiKey;
        private System.Windows.Forms.Button btn_luu;
        private System.Windows.Forms.Button btn_huy;
    }
}