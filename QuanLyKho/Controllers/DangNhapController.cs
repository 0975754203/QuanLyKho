using BusinessLogic;
using BusinessLogic.Management;
using BusinessLogic.Utils;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using QuanLyKho.Models;
using QuanLyKho.Utility;

namespace QuanLyKho.Controllers
{
    public class DangNhapController : Controller
    {
        // GET: DangNhap
        public ActionResult Index()
        {
            try
            {
                ViewBag.Title = "Đăng nhập hệ thống " ;
                return View();
            }
            catch (Exception ex)
            {
                ex.Log();
                return View();
            }
        }

        public ActionResult DoLogin(DangNhapModel model, string RedirectUrl, string subdomain)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string sPass = BusinessUtils.MD5Hash(model.Password);
                    var TaiKhoan = TaiKhoanManager.Instance.SelectByUserLogin(model.UserName, sPass);
                    if (TaiKhoan != null)
                    {
                      
                        var taikhoan_login = new DangNhapModel
                        {
                            Id = TaiKhoan.Id.ToString(),
                            UserName = TaiKhoan.UserName,
                            Role = TaiKhoan.Role,
                        };

                        var culture = "vi-VN";
                        var cookieLang = new HttpCookie(Helper.APP_LOGIN, culture)
                        {
                            Expires = DateTime.Now.AddDays(3)
                        };

                        System.Web.HttpContext.Current.Response.Cookies.Add(cookieLang);
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(taikhoan_login);
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1,
                          json,
                          DateTime.Now,
                          DateTime.Now.AddDays(3),
                          true,
                          string.Empty);

                        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                        System.Web.HttpCookie authCookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                        if (authTicket.IsPersistent)
                        {
                            authCookie.Expires = authTicket.Expiration;
                        }
                        System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);

                        string backurl = Request.QueryString["backurl"];
                        if (!string.IsNullOrEmpty(RedirectUrl))
                        {
                            RedirectUrl = Uri.UnescapeDataString(RedirectUrl);
                            return Redirect(Server.UrlDecode(RedirectUrl));
                        }
                        else if (string.IsNullOrEmpty(backurl))
                        {
                            backurl = "/kho/danh-sach-nhap-kho";
                        }
                        return Json(new { Sucess = true, Errors = ModelState.Errors(), Url = backurl }, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "Tài khoản đăng nhập không đúng.");
                        return Json(new { Sucess = false, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new
                    {
                        Sucess = false,
                        Errors = ModelState.Errors()
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ex.Log();
                return Json(new { Sucess = false, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DoLogout()
        {
            try
            {
                FormsAuthentication.SignOut();
                //Clear session
                var current = System.Web.HttpContext.Current;
                current.Session.Clear();
                current.Session.Abandon();
                //Clears out Session
                current.Response.Cookies.Clear();
                // clear authentication cookie
                current.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
                current.Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
                HttpCookie cookie = current.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    current.Response.Cookies.Add(cookie);
                }
                return Redirect(Server.UrlDecode("/dang-nhap"));
            }
            catch (Exception ex)
            {
                ex.Log();
                return Redirect(Server.UrlDecode("/dang-nhap"));
            }
        }

    }
}
