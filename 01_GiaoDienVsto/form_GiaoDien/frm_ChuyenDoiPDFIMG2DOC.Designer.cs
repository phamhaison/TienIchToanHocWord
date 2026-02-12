using System;
using System.ComponentModel;
using System.Windows.Forms;
using TienIchToanHocWord.XuLyVoiAi;


namespace TienIchToanHocWord.GiaoDienVsto.form_GiaoDien
{
    // LƯU Ý: Namespace này phải khớp 100% với file frm_ChuyenDoiPDFIMG2DOC.cs
    partial class frm_ChuyenDoiPDFIMG2DOC
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
            this.textBox_DuongDanPDF = new System.Windows.Forms.TextBox();
            this.rad_ChiLayNoiDungTuText = new System.Windows.Forms.RadioButton();
            this.rad_LayNoiDungDungNguyenAnh = new System.Windows.Forms.RadioButton();
            this.rad_TuAnh = new System.Windows.Forms.RadioButton();
            this.rad_TuPDF_Anh = new System.Windows.Forms.RadioButton();
            this.btn_chonPDF = new System.Windows.Forms.Button();
            this.btn_ThucThiChuyenDoi = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox_DanAnh = new System.Windows.Forms.PictureBox();
            this.richTextBox_TienTrinh = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_DanAnh)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox_DuongDanPDF
            // 
            this.textBox_DuongDanPDF.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_DuongDanPDF.Location = new System.Drawing.Point(159, 60);
            this.textBox_DuongDanPDF.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBox_DuongDanPDF.Name = "textBox_DuongDanPDF";
            this.textBox_DuongDanPDF.Size = new System.Drawing.Size(377, 23);
            this.textBox_DuongDanPDF.TabIndex = 0;
            // 
            // rad_ChiLayNoiDungTuText
            // 
            this.rad_ChiLayNoiDungTuText.Checked = true;
            this.rad_ChiLayNoiDungTuText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rad_ChiLayNoiDungTuText.Location = new System.Drawing.Point(29, 32);
            this.rad_ChiLayNoiDungTuText.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rad_ChiLayNoiDungTuText.Name = "rad_ChiLayNoiDungTuText";
            this.rad_ChiLayNoiDungTuText.Size = new System.Drawing.Size(166, 28);
            this.rad_ChiLayNoiDungTuText.TabIndex = 4;
            this.rad_ChiLayNoiDungTuText.TabStop = true;
            this.rad_ChiLayNoiDungTuText.Text = "Chỉ lấy nội dung văn bản";
            this.rad_ChiLayNoiDungTuText.CheckedChanged += new System.EventHandler(this.rad_All_CheckedChanged);
            // 
            // rad_LayNoiDungDungNguyenAnh
            // 
            this.rad_LayNoiDungDungNguyenAnh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rad_LayNoiDungDungNguyenAnh.Location = new System.Drawing.Point(29, 66);
            this.rad_LayNoiDungDungNguyenAnh.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rad_LayNoiDungDungNguyenAnh.Name = "rad_LayNoiDungDungNguyenAnh";
            this.rad_LayNoiDungDungNguyenAnh.Size = new System.Drawing.Size(156, 28);
            this.rad_LayNoiDungDungNguyenAnh.TabIndex = 5;
            this.rad_LayNoiDungDungNguyenAnh.Text = "Lấy cả văn bản và Ảnh";
            this.rad_LayNoiDungDungNguyenAnh.CheckedChanged += new System.EventHandler(this.rad_All_CheckedChanged);
            // 
            // rad_TuAnh
            // 
            this.rad_TuAnh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rad_TuAnh.Location = new System.Drawing.Point(274, 32);
            this.rad_TuAnh.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rad_TuAnh.Name = "rad_TuAnh";
            this.rad_TuAnh.Size = new System.Drawing.Size(149, 28);
            this.rad_TuAnh.TabIndex = 6;
            this.rad_TuAnh.Text = "Chuyển đổi từ Ảnh";
            this.rad_TuAnh.CheckedChanged += new System.EventHandler(this.rad_All_CheckedChanged);
            // 
            // rad_TuPDF_Anh
            // 
            this.rad_TuPDF_Anh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rad_TuPDF_Anh.Location = new System.Drawing.Point(274, 66);
            this.rad_TuPDF_Anh.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rad_TuPDF_Anh.Name = "rad_TuPDF_Anh";
            this.rad_TuPDF_Anh.Size = new System.Drawing.Size(187, 28);
            this.rad_TuPDF_Anh.TabIndex = 7;
            this.rad_TuPDF_Anh.Text = "Từ PDF tạo bởi các Ảnh";
            this.rad_TuPDF_Anh.CheckedChanged += new System.EventHandler(this.rad_All_CheckedChanged);
            // 
            // btn_chonPDF
            // 
            this.btn_chonPDF.Location = new System.Drawing.Point(38, 57);
            this.btn_chonPDF.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_chonPDF.Name = "btn_chonPDF";
            this.btn_chonPDF.Size = new System.Drawing.Size(113, 27);
            this.btn_chonPDF.TabIndex = 1;
            this.btn_chonPDF.Text = "Chọn PDF";
            this.btn_chonPDF.UseVisualStyleBackColor = true;
            this.btn_chonPDF.Click += new System.EventHandler(this.btn_chonPDF_Click);
            // 
            // btn_ThucThiChuyenDoi
            // 
            this.btn_ThucThiChuyenDoi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(87)))), ((int)(((byte)(154)))));
            this.btn_ThucThiChuyenDoi.FlatAppearance.BorderSize = 0;
            this.btn_ThucThiChuyenDoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ThucThiChuyenDoi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ThucThiChuyenDoi.ForeColor = System.Drawing.Color.White;
            this.btn_ThucThiChuyenDoi.Location = new System.Drawing.Point(121, 564);
            this.btn_ThucThiChuyenDoi.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_ThucThiChuyenDoi.Name = "btn_ThucThiChuyenDoi";
            this.btn_ThucThiChuyenDoi.Size = new System.Drawing.Size(288, 48);
            this.btn_ThucThiChuyenDoi.TabIndex = 2;
            this.btn_ThucThiChuyenDoi.Text = "Thực thi";
            this.btn_ThucThiChuyenDoi.UseVisualStyleBackColor = false;
            this.btn_ThucThiChuyenDoi.Click += new System.EventHandler(this.btn_ThucThiChuyenDoi_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.rad_TuPDF_Anh);
            this.groupBox1.Controls.Add(this.rad_LayNoiDungDungNguyenAnh);
            this.groupBox1.Controls.Add(this.rad_TuAnh);
            this.groupBox1.Controls.Add(this.rad_ChiLayNoiDungTuText);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(38, 279);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Size = new System.Drawing.Size(497, 113);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Chọn chế độ chuyển đổi";
            // 
            // pictureBox_DanAnh
            // 
            this.pictureBox_DanAnh.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_DanAnh.BackColor = System.Drawing.Color.White;
            this.pictureBox_DanAnh.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_DanAnh.Location = new System.Drawing.Point(38, 123);
            this.pictureBox_DanAnh.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBox_DanAnh.Name = "pictureBox_DanAnh";
            this.pictureBox_DanAnh.Size = new System.Drawing.Size(497, 144);
            this.pictureBox_DanAnh.TabIndex = 9;
            this.pictureBox_DanAnh.TabStop = false;
            // 
            // richTextBox_TienTrinh
            // 
            this.richTextBox_TienTrinh.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox_TienTrinh.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox_TienTrinh.Location = new System.Drawing.Point(38, 431);
            this.richTextBox_TienTrinh.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBox_TienTrinh.Name = "richTextBox_TienTrinh";
            this.richTextBox_TienTrinh.Size = new System.Drawing.Size(497, 106);
            this.richTextBox_TienTrinh.TabIndex = 10;
            this.richTextBox_TienTrinh.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 404);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 15);
            this.label1.TabIndex = 11;
            this.label1.Text = "Tiến trình xử lý";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 96);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 15);
            this.label2.TabIndex = 12;
            this.label2.Text = "Dán ảnh vào đây";
            // 
            // frm_ChuyenDoiPDFIMG2DOC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(242)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(580, 643);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox_TienTrinh);
            this.Controls.Add(this.pictureBox_DanAnh);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox_DuongDanPDF);
            this.Controls.Add(this.btn_chonPDF);
            this.Controls.Add(this.btn_ThucThiChuyenDoi);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(49)))), ((int)(((byte)(48)))));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "frm_ChuyenDoiPDFIMG2DOC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chuyển đổi IMG & PDF sang DOC (Phạm Hải Sơn)";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_DanAnh)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_DuongDanPDF;
        private System.Windows.Forms.RadioButton rad_ChiLayNoiDungTuText;
        private System.Windows.Forms.RadioButton rad_LayNoiDungDungNguyenAnh;
        private System.Windows.Forms.RadioButton rad_TuAnh;
        private System.Windows.Forms.RadioButton rad_TuPDF_Anh;
        private System.Windows.Forms.Button btn_chonPDF;
        private System.Windows.Forms.Button btn_ThucThiChuyenDoi;

        private GroupBox groupBox1;
        private PictureBox pictureBox_DanAnh;
        private RichTextBox richTextBox_TienTrinh;
        private Label label1;
        private Label label2;
    }
}