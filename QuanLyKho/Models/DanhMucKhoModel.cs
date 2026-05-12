using BusinessLogic.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyKho.Models
{
    public class DanhMucKhoForm : IValidatableObject
    {
        public Guid? Id { get; set; }
        public string MaKho { get; set; }
        public string TenKho { get; set; }
        public string DiaChi { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(MaKho))
                yield return new ValidationResult("Vui lòng nhập mã kho", new[] { nameof(MaKho) });
            if (string.IsNullOrWhiteSpace(TenKho))
                yield return new ValidationResult("Vui lòng nhập tên kho", new[] { nameof(TenKho) });
        }
    }

    public class DanhMucKho_DSModel
    {
        public DanhMucKho_DSModel(int pageIndex, int pagecount, int pageSize, int total, IList<KhoModel> tbl_DanhMucKho)
        {
            PageIndex = pageIndex;
            Pagecount = pagecount;
            Total = total;
            PageSize = pageSize;
            TBL_DanhMucKho = tbl_DanhMucKho;
        }

        public int PageIndex { get; set; }
        public int Pagecount { get; set; }
        public int Total { get; set; }
        public int PageSize { get; private set; }

        public IList<KhoModel> TBL_DanhMucKho { get; set; }
    }
}

