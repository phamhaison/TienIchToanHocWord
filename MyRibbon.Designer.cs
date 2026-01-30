namespace TienIchToanHocWord
{
    partial class MyRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public MyRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyRibbon));
            this.tab1 = this.Factory.CreateRibbonTab();
            this.group_DayHoc = this.Factory.CreateRibbonGroup();
            this.btn_TaoBookMark = this.Factory.CreateRibbonButton();
            this.btn_ChuanHoaHienThi = this.Factory.CreateRibbonButton();
            this.btn_CanChinh_PA_PhamVi = this.Factory.CreateRibbonButton();
            this.btn_HienThiLuoi = this.Factory.CreateRibbonButton();
            this.btn_AnHienThiLuoi = this.Factory.CreateRibbonButton();
            this.btn_Xuat_PDF = this.Factory.CreateRibbonButton();
            this.btn_Casio = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.group_DayHoc.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.group_DayHoc);
            this.tab1.Label = "Công cụ C#";
            this.tab1.Name = "tab1";
            // 
            // group_DayHoc
            // 
            this.group_DayHoc.Items.Add(this.btn_TaoBookMark);
            this.group_DayHoc.Items.Add(this.btn_ChuanHoaHienThi);
            this.group_DayHoc.Items.Add(this.btn_CanChinh_PA_PhamVi);
            this.group_DayHoc.Items.Add(this.btn_HienThiLuoi);
            this.group_DayHoc.Items.Add(this.btn_AnHienThiLuoi);
            this.group_DayHoc.Items.Add(this.btn_Xuat_PDF);
            this.group_DayHoc.Items.Add(this.btn_Casio);
            this.group_DayHoc.Label = "Công cụ dạy học";
            this.group_DayHoc.Name = "group_DayHoc";
            // 
            // btn_TaoBookMark
            // 
            this.btn_TaoBookMark.Image = ((System.Drawing.Image)(resources.GetObject("btn_TaoBookMark.Image")));
            this.btn_TaoBookMark.Label = "Tạo Book mark";
            this.btn_TaoBookMark.Name = "btn_TaoBookMark";
            this.btn_TaoBookMark.ShowImage = true;
            this.btn_TaoBookMark.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_TaoBookMark_Click);
            // 
            // btn_ChuanHoaHienThi
            // 
            this.btn_ChuanHoaHienThi.Image = ((System.Drawing.Image)(resources.GetObject("btn_ChuanHoaHienThi.Image")));
            this.btn_ChuanHoaHienThi.Label = "Chuẩn hóa hiển thị";
            this.btn_ChuanHoaHienThi.Name = "btn_ChuanHoaHienThi";
            this.btn_ChuanHoaHienThi.ShowImage = true;
            this.btn_ChuanHoaHienThi.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_ChuanHoaHienThi_Click);
            // 
            // btn_CanChinh_PA_PhamVi
            // 
            this.btn_CanChinh_PA_PhamVi.Image = ((System.Drawing.Image)(resources.GetObject("btn_CanChinh_PA_PhamVi.Image")));
            this.btn_CanChinh_PA_PhamVi.Label = "Căn chỉnh PA theo phạm vi";
            this.btn_CanChinh_PA_PhamVi.Name = "btn_CanChinh_PA_PhamVi";
            this.btn_CanChinh_PA_PhamVi.ShowImage = true;
            this.btn_CanChinh_PA_PhamVi.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_CanChinh_PA_PhamVi_Click);
            // 
            // btn_HienThiLuoi
            // 
            this.btn_HienThiLuoi.Image = ((System.Drawing.Image)(resources.GetObject("btn_HienThiLuoi.Image")));
            this.btn_HienThiLuoi.Label = "Hiển thị đường lưới";
            this.btn_HienThiLuoi.Name = "btn_HienThiLuoi";
            this.btn_HienThiLuoi.ShowImage = true;
            this.btn_HienThiLuoi.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_HienThiLuoi_Click);
            // 
            // btn_AnHienThiLuoi
            // 
            this.btn_AnHienThiLuoi.Image = ((System.Drawing.Image)(resources.GetObject("btn_AnHienThiLuoi.Image")));
            this.btn_AnHienThiLuoi.Label = "Ẩn hiển thị đường lưới";
            this.btn_AnHienThiLuoi.Name = "btn_AnHienThiLuoi";
            this.btn_AnHienThiLuoi.ShowImage = true;
            this.btn_AnHienThiLuoi.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_AnHienThiLuoi_Click);
            // 
            // btn_Xuat_PDF
            // 
            this.btn_Xuat_PDF.Image = ((System.Drawing.Image)(resources.GetObject("btn_Xuat_PDF.Image")));
            this.btn_Xuat_PDF.Label = "Xuất ra PDF";
            this.btn_Xuat_PDF.Name = "btn_Xuat_PDF";
            this.btn_Xuat_PDF.ShowImage = true;
            this.btn_Xuat_PDF.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_Xuat_PDF_Click);
            // 
            // btn_Casio
            // 
            this.btn_Casio.Image = ((System.Drawing.Image)(resources.GetObject("btn_Casio.Image")));
            this.btn_Casio.Label = "Casio FX";
            this.btn_Casio.Name = "btn_Casio";
            this.btn_Casio.ShowImage = true;
            this.btn_Casio.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_Casio_Click);
            // 
            // MyRibbon
            // 
            this.Name = "MyRibbon";
            this.RibbonType = "Microsoft.Word.Document";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.MyRibbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group_DayHoc.ResumeLayout(false);
            this.group_DayHoc.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group_DayHoc;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_TaoBookMark;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_ChuanHoaHienThi;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_CanChinh_PA_PhamVi;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_HienThiLuoi;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_AnHienThiLuoi;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_Xuat_PDF;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_Casio;
    }

    partial class ThisRibbonCollection
    {
        internal MyRibbon MyRibbon
        {
            get { return this.GetRibbon<MyRibbon>(); }
        }
    }
}
