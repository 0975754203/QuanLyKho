using BusinessLogic.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyKho.Models
{
    public class NhaCungCapForm : IValidatableObject
    {
        public Guid? Id { get; set; }
        public string TenNhaCungCap { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(TenNhaCungCap))
                yield return new ValidationResult("Vui lòng nhập tên nhà cung cấp", new[] { nameof(TenNhaCungCap) });
        }
    }

    public class NhaCungCap_DSModel
    {
        public NhaCungCap_DSModel(int pageIndex, int pagecount, int pageSize, int total, IList<NhaCungCapModel> data)
        {
            PageIndex = pageIndex;
            Pagecount = pagecount;
            PageSize = pageSize;
            Total = total;
            TBL_NhaCungCap = data;
        }

        public int PageIndex { get; set; }
        public int Pagecount { get; set; }
        public int PageSize { get; private set; }
        public int Total { get; set; }
        public IList<NhaCungCapModel> TBL_NhaCungCap { get; set; }
    }
}
