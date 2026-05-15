using System;
using System.Web.Mvc;
using System.Web.Security;
using QuanLyKho.Utility;
using BusinessLogic.Utils;
using System.Configuration;
using BusinessLogic;
using System.Web;
using BusinessLogic.Extensions;
using NLog;

namespace QuanLyKho.Controllers
{
    public class BaseController : Controller
    {
        private static ILogger logger = LogExtension.GetLogger("BaseController");
        string Key = ConfigurationManager.AppSettings["KeyPhienBan"];
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            try
            {
                try
                {
                    // Add thêm thời gian khi vẫn còn hoạt động, Tránh timeout;
                    var checkCookie = false;
                    foreach (var cookey in requestContext.HttpContext.Request.Cookies.AllKeys)
                    {
                        if (cookey == FormsAuthentication.FormsCookieName || cookey.ToLower() == "asp.net_sessionid")
                        {
                            var reqCookie = requestContext.HttpContext.Request.Cookies[cookey];

                            if (reqCookie != null)
                            {
                                System.Web.HttpCookie respCookie = new System.Web.HttpCookie(reqCookie.Name, reqCookie.Value);
                                respCookie.Expires = DateTime.Now.AddMinutes(360);

                                requestContext.HttpContext.Response.Cookies.Set(respCookie);
                                checkCookie = true;
                            }
                            break;
                        }
                    }
                    if (!checkCookie)
                    {
                        System.Web.HttpCookie respCookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName);
                        respCookie.Expires = DateTime.Now.AddMinutes(360);
                        requestContext.HttpContext.Response.Cookies.Set(respCookie);
                        checkCookie = true;
                    }
                }
                catch (Exception ex)
                {
                    ex.Log();
                    throw;
                }
                if (!requestContext.HttpContext.Request.IsAuthenticated)
                {
                    requestContext.HttpContext.Response.Redirect(GetLoginUrl(requestContext));
                }
                var controller = requestContext.RouteData.Values["controller"].ToString().Trim().ToUpper();
                var action = requestContext.RouteData.Values["action"].ToString().Trim().ToUpper();
                var currentAcc = Global.TaiKhoan_Login;
                if (currentAcc != null)
                {
                    //Phân quyền nếu sử dụng
                }
                else if (controller == "DANGNHAP")
                {
                    requestContext.HttpContext.Response.Redirect(GetLoginUrl(requestContext));
                }


                base.Initialize(requestContext);
            }
            catch (Exception ex)
            {
                ex.Log();
                if (Key.IsNotNullOrEmpty() && Key == "DangKyOnline")
                {
                    // đăng ký online 
                    requestContext.HttpContext.Response.Redirect("/DangKyTuyenSinhOnline/Index");
                }
                else
                {
                    requestContext.HttpContext.Response.Redirect("/dang-nhap");
                }
            }
        }

        private string GetLoginUrl(System.Web.Routing.RequestContext requestContext)
        {
            var redirectUrl = requestContext.HttpContext.Server.UrlEncode(requestContext.HttpContext.Request.Url.PathAndQuery);
            if (Key.IsNotNullOrEmpty() && Key == "DangKyOnline")
            {
                // đăng ký online 
                return "/DangKyTuyenSinhOnline/Index";
            }
            else
            {
                return string.Format("/dang-nhap?backurl={0}", redirectUrl);
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            ViewBag.LaAdmin = Global.IsAdmin();
            ViewBag.CoQuyenThaoTacDuLieuKho = Global.CoQuyenThaoTacDuLieuKho();

            if (Global.IsAdmin())
            {
                return;
            }

            var req = filterContext.HttpContext.Request;
            var action = filterContext.ActionDescriptor.ActionName ?? "";
            var ctrl = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName ?? "";

            if (string.Equals(ctrl, "Home", StringComparison.OrdinalIgnoreCase)
                && string.Equals(action, "DoiMatKhau1", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // Nhân viên không được truy cập module Quản lý tài khoản (chỉ Admin)
            if (Global.LaNhanVien() && string.Equals(ctrl, "QuanLyTaiKhoan", StringComparison.OrdinalIgnoreCase))
            {
                SetAdminRequiredResult(filterContext, action, "Chỉ tài khoản Admin mới được quản lý tài khoản.");
                return;
            }

            if (Global.CoQuyenThaoTacDuLieuKho())
            {
                return;
            }

            var method = (req.HttpMethod ?? "").ToUpperInvariant();
            var needsAdmin = false;

            if (method == "POST" || method == "PUT" || method == "DELETE")
            {
                needsAdmin = true;
            }
            else if (method == "GET")
            {
                if (action.StartsWith("Xoa", StringComparison.OrdinalIgnoreCase)
                    || action.StartsWith("Form", StringComparison.OrdinalIgnoreCase))
                {
                    needsAdmin = true;
                }
                else if (string.Equals(action, "NhapKho", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(action, "XuatKho", StringComparison.OrdinalIgnoreCase))
                {
                    needsAdmin = true;
                }
            }

            if (!needsAdmin)
            {
                return;
            }

            if (method == "GET" && (string.Equals(action, "NhapKho", StringComparison.OrdinalIgnoreCase)
                || string.Equals(action, "XuatKho", StringComparison.OrdinalIgnoreCase)))
            {
                var url = string.Equals(action, "NhapKho", StringComparison.OrdinalIgnoreCase)
                    ? "/kho/danh-sach-nhap-kho"
                    : "/kho/danh-sach-xuat-kho";
                filterContext.Controller.TempData["PermissionMsg"] = "Chỉ tài khoản Admin hoặc Nhân viên mới được thêm, sửa hoặc xóa dữ liệu.";
                filterContext.Result = new RedirectResult(url);
                return;
            }

            SetAdminRequiredResult(filterContext, action);
        }

        private static void SetAdminRequiredResult(ActionExecutingContext filterContext, string action, string thongBao = null)
        {
            var msg = string.IsNullOrWhiteSpace(thongBao)
                ? "Chỉ tài khoản Admin hoặc Nhân viên mới được thêm, sửa hoặc xóa dữ liệu."
                : thongBao.Trim();
            var req = filterContext.HttpContext.Request;
            var isAjax = req.IsAjaxRequest()
                || string.Equals(req.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.OrdinalIgnoreCase);

            var isGetForm = string.Equals(req.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase)
                && !string.IsNullOrEmpty(action)
                && action.StartsWith("Form", StringComparison.OrdinalIgnoreCase);

            if (isGetForm && isAjax)
            {
                filterContext.Result = new ContentResult
                {
                    Content = "<div class=\"alert alert-warning m-3 mb-0\" role=\"alert\"><strong>Không có quyền.</strong> " + System.Web.HttpUtility.HtmlEncode(msg) + "</div>",
                    ContentType = "text/html; charset=utf-8"
                };
                return;
            }

            if (isAjax)
            {
                filterContext.Result = new JsonResult
                {
                    Data = new { Sucess = false, Errors = (object)null, Msg = msg },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
                return;
            }

            filterContext.Result = new HttpStatusCodeResult(403, msg);
        }
    }
}