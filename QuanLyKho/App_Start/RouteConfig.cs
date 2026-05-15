using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QuanLyKho
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "DangNhap",
                url: "dang-nhap",
                defaults: new { controller = "DangNhap", action = "Index", id = UrlParameter.Optional, loai_menu_cn = 1 }
            );

            routes.MapRoute(
                name: "DangXuat",
                url: "dang-xuat",
                defaults: new { controller = "DangNhap", action = "DoLogout", id = UrlParameter.Optional, loai_menu_cn = 1 }
            );
            routes.MapRoute(
                name: "DoiMatKhau",
                url: "doi-mat-khau",
                defaults: new { controller = "Home", action = "DoiMatKhau", id = UrlParameter.Optional }
            );
        

            routes.MapRoute(
                 name: "QuanLySanPham",
                 url: "he-thong/quan-ly-san-pham",
                 defaults: new { controller = "QuanLySanPham", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
              name: "QuanLyTaiKhoan",
              url: "quan-ly-tai-khoan",
              defaults: new { controller = "QuanLyTaiKhoan", action = "Index", id = UrlParameter.Optional }
         );
            routes.MapRoute(
              name: "QuanLyTaiKhoanHeThong",
              url: "he-thong/quan-ly-tai-khoan",
              defaults: new { controller = "QuanLyTaiKhoan", action = "Index", id = UrlParameter.Optional }
         );

            routes.MapRoute(
                 name: "QuanLyDanhMucKho",
                 url: "he-thong/danh-muc-kho",
                 defaults: new { controller = "QuanLyDanhMucKho", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                 name: "QuanLyNhomVatTu",
                 url: "he-thong/nhom-vat-tu",
                 defaults: new { controller = "QuanLyNhomVatTu", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                 name: "QuanLyTonKho",
                 url: "he-thong/ton-kho",
                 defaults: new { controller = "QuanLyTonKho", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                 name: "BaoCaoXuatNhapTon",
                 url: "he-thong/bao-cao-xuat-nhap-ton",
                 defaults: new { controller = "QuanLyTonKho", action = "BaoCaoXuatNhapTon", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                 name: "BaoCaoXuatKhoaPhong",
                 url: "he-thong/bao-cao-xuat-khoa-phong",
                 defaults: new { controller = "QuanLyTonKho", action = "BaoCaoXuatKhoaPhong", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                 name: "TheKho",
                 url: "he-thong/the-kho",
                 defaults: new { controller = "QuanLyTheKho", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DanhSachNhapKho",
                url: "kho/danh-sach-nhap-kho",
                defaults: new { controller = "QuanLyGiaoDichNhapXuat", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
              name: "NhapKho",
              url: "kho/nhap-kho",
              defaults: new { controller = "QuanLyGiaoDichNhapXuat", action = "NhapKho", id = UrlParameter.Optional }
          );
            routes.MapRoute(
           name: "DanhSachXuatKho",
           url: "kho/danh-sach-xuat-kho",
           defaults: new { controller = "QuanLyGiaoDichNhapXuat", action = "IndexXuatKho", id = UrlParameter.Optional }
       );
            routes.MapRoute(
           name: "XuatKho",
           url: "kho/xuat-kho",
           defaults: new { controller = "QuanLyGiaoDichNhapXuat", action = "XuatKho", id = UrlParameter.Optional }
       );


            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

       
         
 

        }
    }
}
