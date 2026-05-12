using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Model
{
    public class KhoTonModel
    {
        public Guid Id { get; set; }
        public Guid IdSanPham { get; set; }
        public Guid IdKho { get; set; }
        public decimal SoLuong { get; set; }

        // Thông tin hiển thị cho danh sách tồn kho
        public string MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string DonViTinh { get; set; }
        public string TenKho { get; set; }

        // Alias để dùng thống nhất ở view hiện tại
        public decimal TonKho { get; set; }
    }
}
