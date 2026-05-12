using System;
using System.Collections.Generic;

namespace BusinessLogic.Model
{
    /// <summary>Cột động trên báo cáo: một sản phẩm = một cột.</summary>
    public class BaoCaoXuatKhoaPhongCotSanPham
    {
        public Guid IdSanPham { get; set; }
        /// <summary>Tiêu đề cột (tên sản phẩm).</summary>
        public string TenCot { get; set; }
    }

    /// <summary>Kết quả báo cáo (cột + dòng đã phân trang).</summary>
    public class BaoCaoXuatKhoaPhongKetQua
    {
        public List<BaoCaoXuatKhoaPhongCotSanPham> CotSanPham { get; set; }
        public List<BaoCaoXuatKhoaPhongModel> Hang { get; set; }
    }

    /// <summary>
    /// Một dòng khoa/phòng: số lượng xuất theo từng sản phẩm (khóa = IdSanPham).
    /// </summary>
    public class BaoCaoXuatKhoaPhongModel
    {
        public Guid IdKhoaPhong { get; set; }
        public string TenKhoaPhong { get; set; }
        public Dictionary<Guid, decimal> SoLuongTheoSanPham { get; set; }
        public decimal TongSoLuong { get; set; }
    }
}
