using BusinessLogic;
using BusinessLogic.Management;
using BusinessLogic.Model;
using BusinessLogic.Utils;
using OfficeOpenXml.FormulaParsing.ExpressionGraph.FunctionCompilers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyKho.Models;
using QuanLyKho.Utility;

namespace QuanLyKho.Controllers
{
    public class HomeController : BaseController
    {
        string Key = ConfigurationManager.AppSettings["KeyPhienBan"];
        public ActionResult Index()

        {
            try
            {
                if (Global.ThongTinTaiKhoan_Login == null)
                {
                    return Redirect("/dang-nhap");
                }

                return Redirect("/kho/danh-sach-nhap-kho");
            }
            catch (Exception ex)
            {
                ex.Log();
                return View();
            }
        }
        public ActionResult DoiMatKhau()
        {
            try
            {
                ViewBag.Title = "Bệnh viện K";
                return View();
            }
            catch (Exception ex)
            {
                ex.Log();
                return View();
            }
        }
        public ActionResult DoiMatKhau1(DangNhapModel model)
        {
            try
            {
                if (model.Password.IsNullOrEmpty() || model.PasswordNew.IsNullOrEmpty() || model.PasswordNewComfirm.IsNullOrEmpty())
                {
                    return Json(new { Sucess = false, Errors = "Vui lòng nhập đủ thông tin." }, JsonRequestBehavior.AllowGet);
                }
                if (model.PasswordNew.Length > 30 || model.PasswordNew.Length < 6)
                {
                    return Json(new { Sucess = false, Errors = "Mật khẩu phải từ 6-30 ký tự." }, JsonRequestBehavior.AllowGet);
                }
                if (model.PasswordNew != model.PasswordNewComfirm)
                {
                    return Json(new { Sucess = false, Errors = "Mật khẩu không khớp nhau." }, JsonRequestBehavior.AllowGet);
                }
                string sPass = BusinessUtils.MD5Hash(model.Password);
                string sPassNew = BusinessUtils.MD5Hash(model.PasswordNew);
                var TaiKhoan = TaiKhoanManager.Instance.SelectByUser(Global.ThongTinTaiKhoan_Login.UserName);
                if (TaiKhoan != null)
                {
                    if (sPass != TaiKhoan.Pass)
                    {
                        return Json(new { Sucess = false, Errors = "Mật khẩu cũ không đúng." }, JsonRequestBehavior.AllowGet);
                    }
                    TaiKhoan.Pass = sPassNew;
                    TaiKhoanManager.Instance.DoiMatKhau(TaiKhoan);
                }
                return Json(new { Sucess = true, Errors = "Đổi mật khẩu thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ex.Log();
                return Json(new { Sucess = false, Errors = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
    }

    public static class ModelStateHelper
    {
        public static IEnumerable Errors(this ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return modelState.ToDictionary(kvp => kvp.Key,
                    kvp => kvp.Value.Errors
                                    .Select(e => e.ErrorMessage).ToArray())
                                    .Where(m => m.Value.Any());
            }
            return null;
        }
    }
}
