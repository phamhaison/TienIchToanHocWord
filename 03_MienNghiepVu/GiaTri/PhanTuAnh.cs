// FILE: PhanTuAnh.cs (03_mien_nghiep_vu/gia_tri)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TienIchToanHocWord.MienNghiepVu.GiaTri
{
    /// <summary>
    /// Mo hinh du lieu cho mot phan tu anh/do thi duoc trich xuat tu PDF.
    /// Dung de luu Base64 va Sandwich Context.
    /// </summary>
    public class PhanTuAnh
    {
        public int Index { get; set; }          // Chi muc (HINH 1, HINH 2...)
        public int Page { get; set; }           // Trang so
        public float ToaDoY { get; set; }       // Toa do Y (de sap xep)
        public string Base64Data { get; set; }  // Chuoi Base64 cua anh ([IMAGE_BASE64:image/png:base64_string])
        public string ContextTruoc { get; set; } // Van ban neo truoc anh (prev_anchor)
        public string ContextSau { get; set; }   // Van ban neo sau anh (next_anchor)
        public string DuongDanFileGoc { get; set; } // Duong dan file tam (de chen vao Word)
    }
}