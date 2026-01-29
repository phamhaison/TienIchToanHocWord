namespace TienIchToanHocWord
{
    partial class ChuanHoaTrangDayHoc
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
            this.btn_Phone = new System.Windows.Forms.Button();
            this.btn_Ipad = new System.Windows.Forms.Button();
            this.btn_TietKiemA4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_Phone
            // 
            this.btn_Phone.Location = new System.Drawing.Point(33, 41);
            this.btn_Phone.Name = "btn_Phone";
            this.btn_Phone.Size = new System.Drawing.Size(216, 23);
            this.btn_Phone.TabIndex = 0;
            this.btn_Phone.Text = "Chuẩn hóa cho Phone";
            this.btn_Phone.UseVisualStyleBackColor = true;
            this.btn_Phone.Click += new System.EventHandler(this.btn_Phone_Click);
            // 
            // btn_Ipad
            // 
            this.btn_Ipad.Location = new System.Drawing.Point(33, 88);
            this.btn_Ipad.Name = "btn_Ipad";
            this.btn_Ipad.Size = new System.Drawing.Size(216, 23);
            this.btn_Ipad.TabIndex = 1;
            this.btn_Ipad.Text = "Chuẩn hóa cho Ipad";
            this.btn_Ipad.UseVisualStyleBackColor = true;
            this.btn_Ipad.Click += new System.EventHandler(this.btn_Ipad_Click);
            // 
            // btn_TietKiemA4
            // 
            this.btn_TietKiemA4.Location = new System.Drawing.Point(33, 136);
            this.btn_TietKiemA4.Name = "btn_TietKiemA4";
            this.btn_TietKiemA4.Size = new System.Drawing.Size(216, 23);
            this.btn_TietKiemA4.TabIndex = 2;
            this.btn_TietKiemA4.Text = "Chuẩn hóa tiết kiệm A4";
            this.btn_TietKiemA4.UseVisualStyleBackColor = true;
            this.btn_TietKiemA4.Click += new System.EventHandler(this.btn_TietKiemA4_Click);
            // 
            // ChuanHoaTrangDayHoc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 191);
            this.Controls.Add(this.btn_TietKiemA4);
            this.Controls.Add(this.btn_Ipad);
            this.Controls.Add(this.btn_Phone);
            this.Name = "ChuanHoaTrangDayHoc";
            this.Text = "Chuẩn hóa trang dạy học";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Phone;
        private System.Windows.Forms.Button btn_Ipad;
        private System.Windows.Forms.Button btn_TietKiemA4;
    }
}