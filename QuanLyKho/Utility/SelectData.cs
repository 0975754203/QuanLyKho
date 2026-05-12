using BusinessLogic;
using BusinessLogic.Extensions;
using BusinessLogic.Management;
using BusinessLogic.Model;
using BusinessLogic.Utils;
using NLog;
using SQLDataAccess;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKho.Utility
{
    public class SelectData
    {
        private static ILogger logger = LogExtension.GetLogger();

        public static IEnumerable<SelectListItem> ComboDoiLoaiTaiKhoan()
        {
            var lisItem = new List<SelectListItem>();
            lisItem.Add(new SelectListItem { Text = "-- Chọn loại --", Value = "" });
            lisItem.Add(new SelectListItem { Text = "Admin", Value = "admin" });
            lisItem.Add(new SelectListItem { Text = "Nhân viên", Value = "nhanvien" });
            lisItem.Add(new SelectListItem { Text = "Thợ", Value = "tho" });
            return lisItem;
        }

        public static IEnumerable<SelectListItem> CombDoiTuongKhuyenKhichLoai()
        {
            var lisItem = new List<SelectListItem>();
            lisItem.Add(new SelectListItem { Text = "-- Đối tượng khuyến khích --", Value = "" });
            lisItem.Add(new SelectListItem { Text = cLoaiDoiTuongKhuyenKhich.CuocThiKHKT, Value = "CuocThiKHKT" });
            lisItem.Add(new SelectListItem { Text = cLoaiDoiTuongKhuyenKhich.GiaiVHVN_TDTT, Value = "GiaiVHVN_TDTT" });
            lisItem.Add(new SelectListItem { Text = cLoaiDoiTuongKhuyenKhich.GiaiCuocThiKhac, Value = "GiaiCuocThiKhac" });
            //lisItem.Add(new SelectListItem { Text = cLoaiDoiTuongKhuyenKhich.GiaiThiKhuVuc_QT_Bo, Value = "GiaiThiKhuVuc_QT_Bo" });
            return lisItem;
        }
        public static IEnumerable<SelectListItem> ComboDoiTuongKhuyenKhichGiai()
        {
            var lisItem = new List<SelectListItem>();
            lisItem.Add(new SelectListItem { Text = "-- Chọn giải --", Value = "" });
            lisItem.Add(new SelectListItem { Text = cDoiTuongKhuyenKhichGiai.Giai1, Value = "GiaiNhat" });
            lisItem.Add(new SelectListItem { Text = cDoiTuongKhuyenKhichGiai.Giai2, Value = "GiaiNhi" });
            lisItem.Add(new SelectListItem { Text = cDoiTuongKhuyenKhichGiai.Giai3, Value = "GiaiBa" });
            lisItem.Add(new SelectListItem { Text = cDoiTuongKhuyenKhichGiai.Giai4, Value = "KhuyenKhich" });
            //lisItem.Add(new SelectListItem { Text = cDoiTuongKhuyenKhichGiai.HCV, Value = "HCV" });
            //lisItem.Add(new SelectListItem { Text = cDoiTuongKhuyenKhichGiai.HCB, Value = "HCB" });
            //lisItem.Add(new SelectListItem { Text = cDoiTuongKhuyenKhichGiai.HCD, Value = "HCD" });
            //lisItem.Add(new SelectListItem { Text = cDoiTuongKhuyenKhichGiai.BangKhen, Value = "BangKhen" });
            return lisItem;
        }

        public static IEnumerable<SelectListItem> ComboGiaiHSGTinh()
        {
            var lisItem = new List<SelectListItem>();
            lisItem.Add(new SelectListItem { Text = cfGiaiHSGTinh.Giai0, Value = "0" });
            lisItem.Add(new SelectListItem { Text = cfGiaiHSGTinh.Giai1, Value = "1" });
            lisItem.Add(new SelectListItem { Text = cfGiaiHSGTinh.Giai2, Value = "2" });
            lisItem.Add(new SelectListItem { Text = cfGiaiHSGTinh.Giai3, Value = "3" });
            lisItem.Add(new SelectListItem { Text = cfGiaiHSGTinh.Giai4, Value = "4" });
            return lisItem;
        }
        public static IEnumerable<SelectListItem> ComboChungChiTiengAnh()
        {
            var lisItem = new List<SelectListItem>();
            lisItem.Add(new SelectListItem { Text = cfChungChiTiengAnh.ChungChi0, Value = "" });
            lisItem.Add(new SelectListItem { Text = cfChungChiTiengAnh.ChungChi1, Value = "C1" });
            lisItem.Add(new SelectListItem { Text = cfChungChiTiengAnh.ChungChi2, Value = "C2" });
            lisItem.Add(new SelectListItem { Text = cfChungChiTiengAnh.ChungChi3, Value = "B1" });
            lisItem.Add(new SelectListItem { Text = cfChungChiTiengAnh.ChungChi4, Value = "B2" });
            lisItem.Add(new SelectListItem { Text = cfChungChiTiengAnh.ChungChi5, Value = "A1" });
            lisItem.Add(new SelectListItem { Text = cfChungChiTiengAnh.ChungChi6, Value = "A2" });
            lisItem.Add(new SelectListItem { Text = cfChungChiTiengAnh.ChungChi7, Value = "A3" });
            lisItem.Add(new SelectListItem { Text = cfChungChiTiengAnh.ChungChi8, Value = "A4" });
            lisItem.Add(new SelectListItem { Text = cfChungChiTiengAnh.ChungChi9, Value = "A5" });
            lisItem.Add(new SelectListItem { Text = cfChungChiTiengAnh.ChungChi10, Value = "A6" });
            return lisItem;
        }
        public static IEnumerable<SelectListItem> ComBoCoOrKhong()
        {
            var lisItem = new List<SelectListItem>();
            lisItem.Add(new SelectListItem { Text = "Có", Value = "1" });
            lisItem.Add(new SelectListItem { Text = "Không", Value = "0" });
            return lisItem;
        }





        public static IEnumerable<SelectListItem> ComboDanhMucLoaiDoiTuongCu(string title)
        {
            var lisItem = new List<SelectListItem>();
            lisItem.Add(new SelectListItem { Text = cfDoiTuongXetLai.Loai1, Value = "1" });
            lisItem.Add(new SelectListItem { Text = cfDoiTuongXetLai.Loai2, Value = "2" });
            lisItem.Add(new SelectListItem { Text = cfDoiTuongXetLai.Loai3, Value = "3" });
            lisItem.Add(new SelectListItem { Text = cfDoiTuongXetLai.Loai4, Value = "4" });
            return lisItem;
        }

        public static IEnumerable<SelectListItem> ComboGiaiVanHoa()
        {
            var lisItem = new List<SelectListItem>();
            lisItem.Add(new SelectListItem { Text = "Chọn giải", Value = "0" });
            lisItem.Add(new SelectListItem { Text = "Giải nhất", Value = "1" });
            lisItem.Add(new SelectListItem { Text = "Giải nhì", Value = "2" });
            lisItem.Add(new SelectListItem { Text = "Giải ba", Value = "3" });
            lisItem.Add(new SelectListItem { Text = "Khuyến khích", Value = "4" });
            return lisItem;
        }
        public static IEnumerable<SelectListItem> ComboMonVanHoaLop8()
        {
            var lisItem = new List<SelectListItem>();
            lisItem.Add(new SelectListItem { Text = "Chọn môn", Value = "" });
            lisItem.Add(new SelectListItem { Text = "Toán", Value = "T" });
            lisItem.Add(new SelectListItem { Text = "Ngữ văn", Value = "V" });
            lisItem.Add(new SelectListItem { Text = "Tiếng Anh", Value = "A" });
            return lisItem;
        }
        public static IEnumerable<SelectListItem> ComboMonVanHoaLop9()
        {
            var lisItem = new List<SelectListItem>();
            lisItem.Add(new SelectListItem { Text = "Chọn môn", Value = "" });
            lisItem.Add(new SelectListItem { Text = "KHTN (Chủ đề Năng lượng và sự biến đổi)", Value = "L" });
            lisItem.Add(new SelectListItem { Text = "KHTN (Chủ đề Chất và sự biến đổi của chất)", Value = "H" });
            lisItem.Add(new SelectListItem { Text = "KHTN (Chủ đề Vật sống)", Value = "S" });
            lisItem.Add(new SelectListItem { Text = "Lịch sử và Địa lí (Phân môn Lịch sử)", Value = "U" });
            lisItem.Add(new SelectListItem { Text = "Lịch sử và Địa lí (Phân môn Địa lí)", Value = "D" });
            lisItem.Add(new SelectListItem { Text = "Toán", Value = "T" });
            lisItem.Add(new SelectListItem { Text = "Ngữ văn", Value = "V" });
            lisItem.Add(new SelectListItem { Text = "Tiếng Anh", Value = "A" });
            lisItem.Add(new SelectListItem { Text = "Tiếng Nga", Value = "N" });
            lisItem.Add(new SelectListItem { Text = "Tiếng Pháp", Value = "P" });
            lisItem.Add(new SelectListItem { Text = "Tin", Value = "I" });
            return lisItem;
        }
        public static IEnumerable<SelectListItem> ComboHoanThanh()
        {
            var lisItem = new List<SelectListItem>();
            lisItem.Add(new SelectListItem { Text = "Đã hoàn thành", Value = "1" });
            lisItem.Add(new SelectListItem { Text = "Chưa hoàn thành", Value = "0" });
            return lisItem;
        }
        public static IEnumerable<SelectListItem> ComboLoaiTotNghiep()
        {
            var lisItem = new List<SelectListItem>();
            lisItem.Add(new SelectListItem { Text = "Đạt", Value = "D" });
            lisItem.Add(new SelectListItem { Text = "Không đạt", Value = "K" });
            return lisItem;
        }
        public static IEnumerable<SelectListItem> ComboDonViTinh()
        {
            var lisItem = new List<SelectListItem>();
            lisItem.Add(new SelectListItem { Text = "Chọn đơn vị ", Value = Guid.Empty.ToString() });

            var lstDonVi = TuDienManager.Instance.SelectByLoai(cfLoaiTuDien.LoaiDonViTinh);
            foreach (var item in lstDonVi)
            {
                lisItem.Add(new SelectListItem { Text = item.TenTuDien, Value = item.Id.ToString() });
            }
            return lisItem;

        }

        public static IEnumerable<SelectListItem> ComboNhomVatTu()
        {
            var lisItem = new List<SelectListItem>();
            lisItem.Add(new SelectListItem { Text = "-- Chọn nhóm vật tư --", Value = Guid.Empty.ToString() });

            foreach (var item in NhomVatTuManager.Instance.SelectAll())
            {
                lisItem.Add(new SelectListItem { Text = item.TenNhom + " (" + item.MaNhom + ")", Value = item.Id.ToString() });
            }
            return lisItem;
        }

        /// <summary>Combo lọc danh sách sản phẩm theo nhóm (giá trị rỗng = tất cả).</summary>
        public static IEnumerable<SelectListItem> ComboNhomVatTuTimKiem()
        {
            var lisItem = new List<SelectListItem>();
            lisItem.Add(new SelectListItem { Text = "-- Tất cả nhóm --", Value = "" });

            foreach (var item in NhomVatTuManager.Instance.SelectAll())
            {
                lisItem.Add(new SelectListItem { Text = item.TenNhom + " (" + item.MaNhom + ")", Value = item.Id.ToString() });
            }
            return lisItem;
        }
    }
}