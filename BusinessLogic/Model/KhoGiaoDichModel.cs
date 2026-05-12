using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Model
{
    public class KhoGiaoDichModel
    {
        public Guid Id { get; set; }
        public string MaGiaoDich { get; set; }
        public DateTime NgayTao { get; set; }
        public Guid IdNguoiTao { get; set; }
        public string LoaiGiaoDich { get; set; }
        public bool DaXoa { get; set; }
        public Guid? IdKhoaPhong { get; set; }
        public Guid? IdKho { get; set; }
        public Guid? IdGiaoDichCha { get; set; }
        public string GhiChu { get; set; }
        public string NguoiGiaoNhan { get; set; }
        public string sTenKho { get; set; }
        public string sTenKhoaPhong { get; set; }

        /// <summary>
        /// Kho nhận khi tạo phiếu xuất kho. Nếu có giá trị, sẽ tự sinh giao dịch nhập kho đi kèm
        /// với IdGiaoDichCha = Id của giao dịch xuất.
        /// </summary>
        public Guid? IdKhoNhan { get; set; }

        public List<KhoGiaoDichChiTietModel> lstChiTiet { get; set; }

    }
    public class KhoGiaoDichChiTietModel
    {
        public Guid Id { get; set; }
        public Guid IdGiaoDich { get; set; }
        public Guid IdSanPham { get; set; }
        public decimal SoLuong { get; set; }
        public string MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string sDonVi { get; set; }
    }
}
