namespace TienIchToanHocWord.GiaoDienVsto.Ribbon
{
    partial class MyRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        private System.ComponentModel.IContainer components = null;

        public MyRibbon() : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) { components.Dispose(); }
            base.Dispose(disposing);
        }

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
            this.group_XuLyCongThuc = this.Factory.CreateRibbonGroup();
            this.btn_LaTex2MT = this.Factory.CreateRibbonButton();
            this.btn_Selec2MT = this.Factory.CreateRibbonButton();
            this.btn_ChuyenDoiLT_EQ = this.Factory.CreateRibbonButton();
            this.group_TichHopAi = this.Factory.CreateRibbonGroup();
            this.btn_NhapApiKey = this.Factory.CreateRibbonButton();
            this.btn_ChuyenDoiPdf = this.Factory.CreateRibbonButton();
            this.btn_TacVuAi = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.group_DayHoc.SuspendLayout();
            this.group_XuLyCongThuc.SuspendLayout();
            this.group_TichHopAi.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.Groups.Add(this.group_DayHoc);
            this.tab1.Groups.Add(this.group_XuLyCongThuc);
            this.tab1.Groups.Add(this.group_TichHopAi);
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
            // group_XuLyCongThuc
            // 
            this.group_XuLyCongThuc.Items.Add(this.btn_LaTex2MT);
            this.group_XuLyCongThuc.Items.Add(this.btn_Selec2MT);
            this.group_XuLyCongThuc.Items.Add(this.btn_ChuyenDoiLT_EQ);
            this.group_XuLyCongThuc.Label = "Xử lý công thức toán";
            this.group_XuLyCongThuc.Name = "group_XuLyCongThuc";
            // 
            // btn_LaTex2MT
            // 
            this.btn_LaTex2MT.Label = "LaTex toggle MathType";
            this.btn_LaTex2MT.Name = "btn_LaTex2MT";
            this.btn_LaTex2MT.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_LaTex2MT_Click);
            // 
            // btn_Selec2MT
            // 
            this.btn_Selec2MT.Label = "Lựa chọn sang MathType";
            this.btn_Selec2MT.Name = "btn_Selec2MT";
            this.btn_Selec2MT.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_Selec2MT_Click);
            // 
            // btn_ChuyenDoiLT_EQ
            // 
            this.btn_ChuyenDoiLT_EQ.Label = "Chuyển đổi LT sang EQ";
            this.btn_ChuyenDoiLT_EQ.Name = "btn_ChuyenDoiLT_EQ";
            this.btn_ChuyenDoiLT_EQ.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_ChuyenDoiLT_EQ_Click);
            // 
            // group_TichHopAi
            // 
            this.group_TichHopAi.Items.Add(this.btn_NhapApiKey);
            this.group_TichHopAi.Items.Add(this.btn_ChuyenDoiPdf);
            this.group_TichHopAi.Items.Add(this.btn_TacVuAi);
            this.group_TichHopAi.Label = "Công cụ AI";
            this.group_TichHopAi.Name = "group_TichHopAi";
            // 
            // btn_NhapApiKey
            // 
            this.btn_NhapApiKey.Label = "Nhập API";
            this.btn_NhapApiKey.Name = "btn_NhapApiKey";
            this.btn_NhapApiKey.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_NhapApiKey_Click);
            // 
            // btn_ChuyenDoiPdf
            // 
            this.btn_ChuyenDoiPdf.Label = "Chuyển đổi từ PDF";
            this.btn_ChuyenDoiPdf.Name = "btn_ChuyenDoiPdf";
            this.btn_ChuyenDoiPdf.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_ChuyenDoiPdf_Click);
            // 
            // btn_TacVuAi
            // 
            this.btn_TacVuAi.Label = "Tác vụ Ai";
            this.btn_TacVuAi.Name = "btn_TacVuAi";
            this.btn_TacVuAi.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_TacVuAi_Click);
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
            this.group_XuLyCongThuc.ResumeLayout(false);
            this.group_XuLyCongThuc.PerformLayout();
            this.group_TichHopAi.ResumeLayout(false);
            this.group_TichHopAi.PerformLayout();
            this.ResumeLayout(false);

        }

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group_DayHoc;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_TaoBookMark;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_ChuanHoaHienThi;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_CanChinh_PA_PhamVi;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_HienThiLuoi;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_AnHienThiLuoi;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_Xuat_PDF;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_Casio;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group_XuLyCongThuc;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_LaTex2MT;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_Selec2MT;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_ChuyenDoiLT_EQ;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group_TichHopAi;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_NhapApiKey;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_ChuyenDoiPdf;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_TacVuAi;
    }
}