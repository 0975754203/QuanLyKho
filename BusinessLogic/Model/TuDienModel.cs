using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Model
{
    public class TuDienModel
    {
        public Guid? Id { get; set; }
        public Guid IdLoaiTuDien { get; set; }
        public string MaTuDien { get; set; }
        public string TenTuDien { get; set; }
        public string GhiChu { get; set; }
        public int NamHoc { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string NgayBatDaus { get; set; }
        public string NgayKetThucs { get; set; }
        public bool IsChuyen { get; set; }
        public bool IsDong { get; set; }
        public string IDHoiDongThi { get; set; }
        ///ex
        public DateTime KyThiNgayDangKyTu { get; set; }
        public DateTime KyThiNgayDangKyDen { get; set; }
        public string sKyThiNgayDangKyTu { get; set; }
        public string sKyThiNgayDangKyDen { get; set; }

    }
    public class TuDienLoaiModel
    {
        public Guid Id { get; set; }
        public string MaLoai { get; set; }
        public string TenLoai { get; set; }
        public string GhiChu { get; set; }
    }
}
