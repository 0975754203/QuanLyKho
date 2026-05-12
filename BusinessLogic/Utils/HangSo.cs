using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessLogic.Utils
{
    public static class HangSo
    {
        public static int PageSize = 20;

        public class PhanLoaiDonVi
        {
            public const int Truong = 1;
            public const int Phong = 2;
            public const int So = 3;
        };
        public class PhienBan { 
            public const string TuyenSinh = "1";
            public const string SinhPhach = "2";
            public const string NhapDiem = "3";
            public const string KhopDiem = "4";
        }; 
        public class sLoaiDoiTuongCu
        {
            public const string Loai1 = "Dự kiểm tra lại Văn, Toán";
            public const string Loai2 = "Phải xếp loại lại hạnh kiểm";
            public const string Loai3 = "Phải xếp loại lại học lực";
            public const string Loai4 = "Phải xếp loại lại HK và HL";
        };
    }
}