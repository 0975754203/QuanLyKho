using BusinessLogic.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyKho.Models
{
    public class KhoaPhongForm : IValidatableObject
    {
        public Guid? Id { get; set; }
        public string MaTuDien { get; set; }
        public string TenTuDien { get; set; }
        public string GhiChu { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(MaTuDien))
                yield return new ValidationResult("Vui lòng nhập mã khoa/phòng", new[] { nameof(MaTuDien) });
            if (string.IsNullOrWhiteSpace(TenTuDien))
                yield return new ValidationResult("Vui lòng nhập tên khoa/phòng", new[] { nameof(TenTuDien) });
        }
    }

    public class KhoaPhong_DSModel
    {
        public KhoaPhong_DSModel(int pageIndex, int pagecount, int pageSize, int total, IList<TuDienModel> data)
        {
            PageIndex = pageIndex;
            Pagecount = pagecount;
            PageSize = pageSize;
            Total = total;
            TBL_KhoaPhong = data;
        }

        public int PageIndex { get; set; }
        public int Pagecount { get; set; }
        public int PageSize { get; private set; }
        public int Total { get; set; }
        public IList<TuDienModel> TBL_KhoaPhong { get; set; }
    }
}
