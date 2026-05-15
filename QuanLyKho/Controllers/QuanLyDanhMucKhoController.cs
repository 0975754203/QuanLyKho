using BusinessLogic.Management;
using BusinessLogic.Model;
using BusinessLogic.Utils;
using QuanLyKho.Models;
using QuanLyKho.Utility;
using System;
using System.Web.Mvc;

namespace QuanLyKho.Controllers
{
    public class QuanLyDanhMucKhoController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Danh mục kho";
            ViewBag.TitleUrl = " / Danh mục kho";
            return View();
        }

        [HttpGet]
        public ActionResult DanhSachDanhMucKho(int? pageIndex, string sSearch)
        {
            try
            {
                pageIndex = pageIndex ?? 1;
                var pageSize = HangSo.PageSize;

                var list = KhoManager.Instance.SearchByPage(sSearch, (int)pageIndex, pageSize, out int total);

                var pagecount = (int)Math.Ceiling((double)total / (int)pageSize);
                var modelDs = new DanhMucKho_DSModel((int)pageIndex, pagecount, (int)pageSize, total, list);
                return PartialView(modelDs);
            }
            catch (Exception ex)
            {
                ex.Log();
                return null;
            }
        }

        [HttpGet]
        public ActionResult FormDanhMucKho(string id)
        {
            try
            {
                DanhMucKhoForm model = new DanhMucKhoForm();
                ViewBag.status = "false";
                ViewBag.isSua = "false";

                if (!string.IsNullOrEmpty(id))
                {
                    ViewBag.TitleModal = "Cập nhật kho";
                    model.Id = Guid.Parse(id);
                    var kho = KhoManager.Instance.SelectById((Guid)model.Id);
                    if (kho != null)
                    {
                        model.MaKho = kho.MaKho;
                        model.TenKho = kho.TenKho;
                        model.DiaChi = kho.DiaChi;
                    }
                    ViewBag.isSua = "true";
                }
                else
                {
                    ViewBag.TitleModal = "Thêm mới kho";
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
        public ActionResult FormDanhMucKho(DanhMucKhoForm model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = new KhoModel
                    {
                        Id = model.Id.HasValue ? model.Id.Value : Guid.Empty,
                        MaKho = model.MaKho,
                        TenKho = model.TenKho,
                        DiaChi = model.DiaChi
                    };

                    var message = KhoManager.Instance.SaveOrUpdate(data);
                    if (!string.IsNullOrEmpty(message))
                    {
                        ModelState.AddModelError("MaKho", message);
                        return Json(new { Sucess = false, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { Sucess = true, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { Sucess = false, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ex.Log();
                return null;
            }
        }

        [HttpPost]
        public ActionResult XoaDanhMucKho(string id)
        {
            try
            {
                var idGuid = Guid.Parse(id);
                var ok = KhoManager.Instance.Delete(idGuid);
                return Json(new { Sucess = ok, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ex.Log();
                return null;
            }
        }
    }
}

