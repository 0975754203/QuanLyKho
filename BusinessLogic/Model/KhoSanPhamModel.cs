using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Model
{
    public class KhoSanPhamModel
    {
        public Guid Id { get; set; }
        public string MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public Guid IdDonViTinh { get; set; }
        public Guid? IdNhomVatTu { get; set; }
        public string GhiChu { get; set; }
        public string XuatXu { get; set; }
        public string sDonViTinh { get; set; }
        public string TenNhomVatTu { get; set; }
    }
}
