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
    }
}
