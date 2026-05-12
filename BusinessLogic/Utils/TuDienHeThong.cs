using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Utils
{
    public class cfLoaiTuDien
    {
        public const string LoaiDonViTinh = "LoaiDonViTinh";
        public const string LoaiKhoaPhong = "LoaiKhoaPhong";
    }
    public class cfLoaiHanhKiem
    {
        public const string T = "T";
        public const string K = "K";
        public const string D = "D";
        public const string C = "C";
        public const string X = "X";
    }
    public class cfTenLoaiHanhKiem
    {
        public const string T = "Tốt";
        public const string K = "Khá";
        public const string D = "Đạt";
        public const string C = "Chưa đạt";
        public const string X = "Không xếp loại";
    }
    public class cfLoaiHocLuc
    {
        public const string T = "T";
        public const string K = "K";
        public const string D = "D";
        public const string C = "C";
        public const string X = "X";
    }
    public class cfTenLoaiHocLuc
    {
        public const string T = "Tốt";
        public const string K = "Khá";
        public const string D = "Đạt";
        public const string C = "Chưa đạt";
        public const string X = "Không xếp loại";
    }
    public class cfLoaiTN
    {
        public const string D = "D";
        public const string K = "K";
    }
    public class cfTenLoaiTN
    {
        public const string D = "Đỗ";
        public const string K = "Hỏng";
    }
    public class cfDoiTuongXetLai
    {
        public const string Loai1 = "Dự kiểm tra lại Văn, Toán";
        public const string Loai2 = "Phải xếp loại lại hạnh kiểm";
        public const string Loai3 = "Phải xếp loại lại học lực";
        public const string Loai4 = "Phải xếp loại lại HK và HL";
    }
    public class cfGiaiHSGTinh
    {
        public const string Giai0 = "Không có";
        public const string Giai1 = "Giải nhất";
        public const string Giai2 = "Giải nhì";
        public const string Giai3 = "Giải ba";
        public const string Giai4 = "Khuyến khích";
    }
    public class cLoaiDoiTuongKhuyenKhich
    {
        public const string CuocThiKHKT = "Cuộc thi khoa học kỹ thuật cấp tỉnh";
        public const string GiaiVHVN_TDTT = "Hội thi Giai điệu tuổi hồng cấp tỉnh";
        public const string GiaiCuocThiKhac = "Giải kỳ thi, cuộc thi, hội thi khác";
        //public const string GiaiThiKhuVuc_QT_Bo = "Cuộc thi Thể dục thể thao cấp tỉnh";
    }
    public class cDoiTuongKhuyenKhichGiai
    {
        public const string Giai0 = "Không có";
        public const string Giai1 = "Giải Nhất";
        public const string Giai2 = "Giải Nhì";
        public const string Giai3 = "Giải Ba";
        public const string Giai4 = "Khuyến khích";
    }
    public class cfGiaiKH_KT
    {
        public const string Giai0 = "Không có";
        public const string Giai1 = "Giải nhất";
        public const string Giai2 = "Giải nhì";
        public const string Giai3 = "Giải ba";
        public const string Giai4 = "Khuyến khích";
    }
    public class cfGiaiVanHoa
    {
        public const string Giai0 = "Không có";
        public const string Giai1 = "Giải nhất";
        public const string Giai2 = "Giải nhì";
        public const string Giai3 = "Giải ba";
        public const string Giai4 = "Khuyến khích";
    }
    public class cfChungChiTiengAnh
    {
        public const string ChungChi0 = "Không có";
        public const string ChungChi1 = "C2";
        public const string ChungChi2 = "C1";
        public const string ChungChi3 = "B2";
        public const string ChungChi4 = "B1";
        public const string ChungChi5 = "KNLNN - Bậc 1";
        public const string ChungChi6 = "KNLNN - Bậc 2";
        public const string ChungChi7 = "KNLNN - Bậc 3";
        public const string ChungChi8 = "KNLNN - Bậc 4";
        public const string ChungChi9 = "KNLNN - Bậc 5";
        public const string ChungChi10 = "KNLNN - Bậc 6";
    }
    public class CFileNameTemplate
    {
        public const string BaoCaoXuatNhapTon = "BaoCaoXuatNhapTon.xlsx";
        public const string BaoCaoXuatKhoaPhong = "BaoCaoXuatKhoaPhong.xlsx";
    }
    public class CProcName
    {
        public const string procKetQuaXetTotNghiep = "procKetQuaXetTotNghiep";
    }
    public class CTuDienLoaiTruong
    {
        public const string THPT = "THPT";
        public const string THCS = "THCS";
        public const string GDTX = "GDTX";
    }
    public class cCauHinh
    {
        public const string MonThiChung = "MonThiChung";
        public const string HeSoMonThi = "HeSoVaChiTieu";
        public const string PHIENBAN = "PHIENBAN";
        public const string MonNgoaiNgu = "MonNgoaiNgu";
        public const string MonChuyenTuNhien = "MonChuyenTuNhien";
    }
    public class cDoiTuongUuTien
    {
        public const string UuTien1 = "Con liệt sĩ";
        public const string UuTien2 = "Con thương binh, bệnh binh hoặc hưởng như thương binh trên 81%";
        public const string UuTien3 = "Con của người nhiễm chất độc hóa học";
        public const string UuTien4 = "Con của người hoạt động cách mạng đến khởi nghĩa tháng tám năm 1945";
        public const string UuTien5 = "Con Anh hùng lao động, con Anh hùng LLVT, con bà mẹ Việt Nam anh hùng";
        public const string UuTien6 = "Con thương binh, bệnh binh hoặc hưởng như thương binh dưới 81%";
        public const string UuTien7 = "Học sinh là người dân tộc thiểu số hoặc có cha mẹ là người dân tộc thiểu số";
        public const string UuTien8 = "Học sinh đang sinh sống ở vùng có điều kiện KT – XH khó khăn";
        //public const string UuTien9 = "Học sinh tàn tật, khuyết tật, kém phát triển, nhiễm CĐHH, mồ côi, diện hộ đói nghèo";
    }
}
