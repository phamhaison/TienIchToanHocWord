namespace TienIchToanHocWord.GiaoDienVsto.form_GiaoDien
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
            this.btn_Phone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Phone.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(212)))));
            this.btn_Phone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Phone.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Phone.ForeColor = System.Drawing.Color.Yellow;
            this.btn_Phone.Location = new System.Drawing.Point(25, 33);
            this.btn_Phone.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_Phone.Name = "btn_Phone";
            this.btn_Phone.Size = new System.Drawing.Size(162, 35);
            this.btn_Phone.TabIndex = 0;
            this.btn_Phone.Text = "Chuẩn hóa cho Phone";
            this.btn_Phone.UseVisualStyleBackColor = false;
            this.btn_Phone.Click += new System.EventHandler(this.btn_Phone_Click);
            // 
            // btn_Ipad
            // 
            this.btn_Ipad.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Ipad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(212)))));
            this.btn_Ipad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Ipad.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Ipad.ForeColor = System.Drawing.Color.Yellow;
            this.btn_Ipad.Location = new System.Drawing.Point(25, 80);
            this.btn_Ipad.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_Ipad.Name = "btn_Ipad";
            this.btn_Ipad.Size = new System.Drawing.Size(162, 35);
            this.btn_Ipad.TabIndex = 1;
            this.btn_Ipad.Text = "Chuẩn hóa cho Ipad";
            this.btn_Ipad.UseVisualStyleBackColor = false;
            this.btn_Ipad.Click += new System.EventHandler(this.btn_Ipad_Click);
            // 
            // btn_TietKiemA4
            // 
            this.btn_TietKiemA4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_TietKiemA4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(212)))));
            this.btn_TietKiemA4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_TietKiemA4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_TietKiemA4.ForeColor = System.Drawing.Color.Yellow;
            this.btn_TietKiemA4.Location = new System.Drawing.Point(25, 127);
            this.btn_TietKiemA4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_TietKiemA4.Name = "btn_TietKiemA4";
            this.btn_TietKiemA4.Size = new System.Drawing.Size(162, 35);
            this.btn_TietKiemA4.TabIndex = 2;
            this.btn_TietKiemA4.Text = "Chuẩn hóa tiết kiệm A4";
            this.btn_TietKiemA4.UseVisualStyleBackColor = false;
            this.btn_TietKiemA4.Click += new System.EventHandler(this.btn_TietKiemA4_Click);
            // 
            // ChuanHoaTrangDayHoc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(209, 182);
            this.Controls.Add(this.btn_TietKiemA4);
            this.Controls.Add(this.btn_Ipad);
            this.Controls.Add(this.btn_Phone);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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