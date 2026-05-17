using BusinessLogic.Management;
using BusinessLogic.Model;
using BusinessLogic.Utils;
using QuanLyKho.Models;
using QuanLyKho.Utility;
using System;
using System.Web.Mvc;

namespace QuanLyKho.Controllers
{
    public class QuanLyNhaCungCapController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Quản lý Nhà cung cấp";
            ViewBag.TitleUrl = " / Quản lý Nhà cung cấp";
            return View();
        }

        [HttpGet]
        public ActionResult DanhSachNhaCungCap(int? pageIndex, string sSearch)
        {
            try
            {
                pageIndex = pageIndex ?? 1;
                var pageSize = HangSo.PageSize;
                var list = NhaCungCapManager.Instance.SearchByPage(sSearch, pageIndex.Value, pageSize, out int total);
                var pagecount = (int)Math.Ceiling((double)total / pageSize);
                var model = new NhaCungCap_DSModel(pageIndex.Value, pagecount, pageSize, total, list);
                return PartialView(model);
            }
            catch (Exception ex)
            {
                ex.Log();
                return null;
            }
        }

        [HttpGet]
        public ActionResult FormNhaCungCap(string id)
        {
            try
            {
                var model = new NhaCungCapForm();
                if (!string.IsNullOrEmpty(id))
                {
                    var nhaCungCap = NhaCungCapManager.Instance.SelectById(Guid.Parse(id));
                    if (nhaCungCap != null)
                    {
                        model.Id = nhaCungCap.Id;
                        model.TenNhaCungCap = nhaCungCap.TenNhaCungCap;
                    }
                }

                return PartialView(model);
            }
            catch (Exception ex)
            {
                ex.Log();
                return null;
            }
        }

        [HttpPost]
        public ActionResult FormNhaCungCap(NhaCungCapForm model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { Sucess = false, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);

                var data = new NhaCungCapModel
                {
                    Id = model.Id.HasValue ? model.Id.Value : Guid.Empty,
                    TenNhaCungCap = model.TenNhaCungCap
                };

                var message = NhaCungCapManager.Instance.SaveOrUpdate(data);
                if (!string.IsNullOrEmpty(message))
                {
                    ModelState.AddModelError("TenNhaCungCap", message);
                    return Json(new { Sucess = false, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { Sucess = true, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ex.Log();
                return null;
            }
        }

        [HttpPost]
        public ActionResult XoaNhaCungCap(string id)
        {
            try
            {
                var message = NhaCungCapManager.Instance.Delete(Guid.Parse(id));
                if (!string.IsNullOrEmpty(message))
                    return Json(new { Sucess = false, Message = message }, JsonRequestBehavior.AllowGet);

                return Json(new { Sucess = true, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ex.Log();
                return Json(new { Sucess = false, Message = "Không xóa được nhà cung cấp." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
