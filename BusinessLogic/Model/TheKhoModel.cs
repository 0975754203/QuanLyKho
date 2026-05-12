using System;

namespace BusinessLogic.Model
{
    public class TheKhoModel
    {
        public Guid IdGiaoDich { get; set; }
        public Guid IdChiTiet { get; set; }
        public string MaGiaoDich { get; set; }
        public DateTime NgayGiaoDich { get; set; }
        public string LoaiGiaoDich { get; set; }
        public decimal SoLuong { get; set; }
        public decimal SoLuongHienThi { get; set; }
    }
}
