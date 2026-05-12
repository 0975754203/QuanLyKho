using BusinessLogic.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyKho.Models
{
    public class NhomVatTuForm : IValidatableObject
    {
        public Guid? Id { get; set; }
        public string MaNhom { get; set; }
        public string TenNhom { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(MaNhom))
                yield return new ValidationResult("Vui lòng nhập mã nhóm", new[] { nameof(MaNhom) });
            if (string.IsNullOrWhiteSpace(TenNhom))
                yield return new ValidationResult("Vui lòng nhập tên nhóm", new[] { nameof(TenNhom) });
        }
    }

    public class NhomVatTu_DSModel
    {
        public NhomVatTu_DSModel(int pageIndex, int pagecount, int pageSize, int total, IList<NhomVatTuModel> tbl_NhomVatTu)
        {
            PageIndex = pageIndex;
            Pagecount = pagecount;
            Total = total;
            PageSize = pageSize;
            TBL_NhomVatTu = tbl_NhomVatTu;
        }

        public int PageIndex { get; set; }
        public int Pagecount { get; set; }
        public int Total { get; set; }
        public int PageSize { get; private set; }

        public IList<NhomVatTuModel> TBL_NhomVatTu { get; set; }
    }
}
