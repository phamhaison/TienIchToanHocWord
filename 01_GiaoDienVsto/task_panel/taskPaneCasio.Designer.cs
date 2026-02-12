namespace TienIchToanHocWord.GiaoDienVsto.task_panel
{
    // Đảm bảo tên lớp là taskPaneCasio (không có chữ l)
    partial class taskPaneCasio
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlCasio = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlCasio
            // 
            this.pnlCasio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCasio.Location = new System.Drawing.Point(0, 0);
            this.pnlCasio.Name = "pnlCasio";
            this.pnlCasio.Size = new System.Drawing.Size(300, 500);
            this.pnlCasio.TabIndex = 0;
            // 
            // taskPaneCasio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlCasio);
            this.Name = "taskPaneCasio";
            this.Size = new System.Drawing.Size(300, 500);
            this.ResumeLayout(false);
        }

        // Khai báo biến pnlCasio ở đây để file chính có thể nhìn thấy
        public System.Windows.Forms.Panel pnlCasio;
    }
}