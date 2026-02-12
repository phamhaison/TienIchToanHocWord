namespace TienIchToanHocWord.GiaoDienVsto.task_panel
{
    partial class TaskPanel_DauTrangCauHoi
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.lsb_CauHoi = new System.Windows.Forms.ListBox();
            this.chk_HienLoiGiai = new System.Windows.Forms.CheckBox();
            this.chk_NgatTrang = new System.Windows.Forms.CheckBox();
            this.group_PhanLoai = new System.Windows.Forms.GroupBox();
            this.rad_NB = new System.Windows.Forms.RadioButton();
            this.rad_TH = new System.Windows.Forms.RadioButton();
            this.rad_VD = new System.Windows.Forms.RadioButton();
            this.rad_ALL = new System.Windows.Forms.RadioButton();
            this.group_PhanLoai.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Danh sách cẩu hỏi";
            // 
            // lsb_CauHoi
            // 
            this.lsb_CauHoi.FormattingEnabled = true;
            this.lsb_CauHoi.Location = new System.Drawing.Point(17, 40);
            this.lsb_CauHoi.Name = "lsb_CauHoi";
            this.lsb_CauHoi.Size = new System.Drawing.Size(123, 394);
            this.lsb_CauHoi.TabIndex = 1;
            // 
            // chk_HienLoiGiai
            // 
            this.chk_HienLoiGiai.AutoSize = true;
            this.chk_HienLoiGiai.Location = new System.Drawing.Point(17, 451);
            this.chk_HienLoiGiai.Name = "chk_HienLoiGiai";
            this.chk_HienLoiGiai.Size = new System.Drawing.Size(80, 17);
            this.chk_HienLoiGiai.TabIndex = 8;
            this.chk_HienLoiGiai.Text = "Hiện lời giải";
            this.chk_HienLoiGiai.UseVisualStyleBackColor = true;
            // 
            // chk_NgatTrang
            // 
            this.chk_NgatTrang.AutoSize = true;
            this.chk_NgatTrang.Location = new System.Drawing.Point(17, 609);
            this.chk_NgatTrang.Name = "chk_NgatTrang";
            this.chk_NgatTrang.Size = new System.Drawing.Size(76, 17);
            this.chk_NgatTrang.TabIndex = 9;
            this.chk_NgatTrang.Text = "Ngắt trang";
            this.chk_NgatTrang.UseVisualStyleBackColor = true;
            // 
            // group_PhanLoai
            // 
            this.group_PhanLoai.Controls.Add(this.rad_ALL);
            this.group_PhanLoai.Controls.Add(this.rad_VD);
            this.group_PhanLoai.Controls.Add(this.rad_TH);
            this.group_PhanLoai.Controls.Add(this.rad_NB);
            this.group_PhanLoai.Location = new System.Drawing.Point(17, 488);
            this.group_PhanLoai.Name = "group_PhanLoai";
            this.group_PhanLoai.Size = new System.Drawing.Size(123, 90);
            this.group_PhanLoai.TabIndex = 11;
            this.group_PhanLoai.TabStop = false;
            this.group_PhanLoai.Text = "Phân loại";
            // 
            // rad_NB
            // 
            this.rad_NB.AutoSize = true;
            this.rad_NB.Location = new System.Drawing.Point(10, 27);
            this.rad_NB.Name = "rad_NB";
            this.rad_NB.Size = new System.Drawing.Size(40, 17);
            this.rad_NB.TabIndex = 0;
            this.rad_NB.TabStop = true;
            this.rad_NB.Text = "NB";
            this.rad_NB.UseVisualStyleBackColor = true;
            // 
            // rad_TH
            // 
            this.rad_TH.AutoSize = true;
            this.rad_TH.Location = new System.Drawing.Point(81, 27);
            this.rad_TH.Name = "rad_TH";
            this.rad_TH.Size = new System.Drawing.Size(40, 17);
            this.rad_TH.TabIndex = 1;
            this.rad_TH.TabStop = true;
            this.rad_TH.Text = "TH";
            this.rad_TH.UseVisualStyleBackColor = true;
            // 
            // rad_VD
            // 
            this.rad_VD.AutoSize = true;
            this.rad_VD.Location = new System.Drawing.Point(10, 64);
            this.rad_VD.Name = "rad_VD";
            this.rad_VD.Size = new System.Drawing.Size(40, 17);
            this.rad_VD.TabIndex = 12;
            this.rad_VD.TabStop = true;
            this.rad_VD.Text = "VD";
            this.rad_VD.UseVisualStyleBackColor = true;
            // 
            // rad_ALL
            // 
            this.rad_ALL.AutoSize = true;
            this.rad_ALL.Location = new System.Drawing.Point(81, 64);
            this.rad_ALL.Name = "rad_ALL";
            this.rad_ALL.Size = new System.Drawing.Size(36, 17);
            this.rad_ALL.TabIndex = 13;
            this.rad_ALL.TabStop = true;
            this.rad_ALL.Text = "All";
            this.rad_ALL.UseVisualStyleBackColor = true;
            // 
            // TaskPanel_DauTrangCauHoi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.group_PhanLoai);
            this.Controls.Add(this.chk_NgatTrang);
            this.Controls.Add(this.chk_HienLoiGiai);
            this.Controls.Add(this.lsb_CauHoi);
            this.Controls.Add(this.label1);
            this.Name = "TaskPanel_DauTrangCauHoi";
            this.Size = new System.Drawing.Size(159, 653);
            this.group_PhanLoai.ResumeLayout(false);
            this.group_PhanLoai.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lsb_CauHoi;
        private System.Windows.Forms.CheckBox chk_HienLoiGiai;
        private System.Windows.Forms.CheckBox chk_NgatTrang;
        private System.Windows.Forms.GroupBox group_PhanLoai;
        private System.Windows.Forms.RadioButton rad_VD;
        private System.Windows.Forms.RadioButton rad_NB;
        private System.Windows.Forms.RadioButton rad_TH;
        private System.Windows.Forms.RadioButton rad_ALL;
    }
}
