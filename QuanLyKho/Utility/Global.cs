using BusinessLogic.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuanLyKho.Models;
using BusinessLogic.Utils;
using BusinessLogic.Model;

namespace QuanLyKho.Utility
{
    public class Global
    {
        public static DangNhapModel TaiKhoan_Login
        {
            get
            {
                if (!HttpContext.Current.Request.IsAuthenticated)
                {
                    return new DangNhapModel();
                }
                var json = HttpContext.Current.User.Identity.Name;
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<DangNhapModel>(json);
                return obj;
            }
        }

        //Khi nào cần lấy thêm thông tin tài khoản login ngoài thông tin cơ bản trong DangNhapModel thì bổ sung vào hàm này
        public static DangNhapModel ThongTinTaiKhoan_Login
        {
            get
            {
                var currentAcc = TaiKhoan_Login;
                if (currentAcc != null && !string.IsNullOrEmpty(currentAcc.Id))
                {
                    var TaiKhoan = TaiKhoanManager.Instance.SelectById(Guid.Parse(currentAcc.Id));

                    if (TaiKhoan != null)
                    {
                        return new DangNhapModel
                        {
                            Id = TaiKhoan.Id.ToString(),
                            UserName = TaiKhoan.UserName,
                            Role = TaiKhoan.Role,
                        };

                    }
                }
                return null;
            }
        }
        /// <summary>Role Admin trong DB (SelectData): giá trị "admin", so khớp không phân biệt hoa thường.</summary>
        public static bool IsAdmin()
        {
            var tk = ThongTinTaiKhoan_Login;
            if (tk == null || string.IsNullOrWhiteSpace(tk.Role))
            {
                return false;
            }

            return string.Equals(tk.Role.Trim(), "admin", StringComparison.OrdinalIgnoreCase);
        }
    }
}