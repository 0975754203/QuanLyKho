using BusinessLogic;
using BusinessLogic.Management;
using BusinessLogic.Model;
using BusinessLogic.Utils;
using SQLDataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QuanLyKho.Models
{

    public class KhoSanPhamForm : IValidatableObject
    {
        public Guid? Id { get; set; }
        public string MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public Guid IdDonViTinh { get; set; }
        public Guid IdNhomVatTu { get; set; }
        public decimal? DonGia { get; set; }
        public string XuatXu { get; set; }
        public string KyhieuNhanmac { get; set; }
        public string ThoiGianBaoHanh { get; set; }
        public string GhiChu { get; set; }
        public string sDonViTinh { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(TenSanPham))
                yield return new ValidationResult("Vui lòng nhập tên sản phẩm", new[] { nameof(TenSanPham) });
            if (IdDonViTinh == Guid.Empty)
                yield return new ValidationResult("Vui lòng chọn đơn vị tính", new[] { nameof(IdDonViTinh) });
            if (IdNhomVatTu == Guid.Empty)
                yield return new ValidationResult("Vui lòng chọn nhóm vật tư", new[] { nameof(IdNhomVatTu) });
            if (DonGia.HasValue && DonGia.Value < 0)
                yield return new ValidationResult("Đơn giá không được nhỏ hơn 0", new[] { nameof(DonGia) });
        }
    }
    public class KhoSanPham_DSModel
    {
        public KhoSanPham_DSModel(int pageIndex, int pagecount, int pageSize, int total, IList<KhoSanPhamModel> tbl_SanPham)
        {
            PageIndex = pageIndex;
            Pagecount = pagecount;
            Total = total;
            PageSize = pageSize;
            TBL_SanPham = tbl_SanPham;
        }

        public int PageIndex { get; set; }
        public int Pagecount { get; set; }
        public int Total { get; set; }
        public int PageSize { get; private set; }

        public IList<KhoSanPhamModel> TBL_SanPham { get; set; }
    }
}
