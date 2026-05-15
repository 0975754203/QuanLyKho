using System;

namespace BusinessLogic.Model
{
    /// <summary>
    /// Một dòng tồn kho dùng cho combobox xuất kho (text hiển thị tên + tồn + đơn vị).
    /// </summary>
    public class KhoSanPhamTrongKhoOption
    {
        public Guid IdSanPham { get; set; }
        public string DisplayText { get; set; }
        public string TenSanPham { get; set; }
        public decimal TonKho { get; set; }
        public string DonViTinh { get; set; }
        public decimal DonGia { get; set; }
        public string XuatXu { get; set; }
    }
}
