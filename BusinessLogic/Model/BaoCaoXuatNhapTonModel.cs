using System;

namespace BusinessLogic.Model
{
    public class BaoCaoXuatNhapTonModel
    {
        public Guid IdSanPham { get; set; }
        public string MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public decimal TonDauKy { get; set; }
        public decimal NhapTrongKy { get; set; }
        public decimal XuatTrongKy { get; set; }
        public decimal TonCuoiKy { get; set; }
    }
}
