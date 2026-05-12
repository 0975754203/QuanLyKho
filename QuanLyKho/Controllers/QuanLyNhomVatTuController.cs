using BusinessLogic.Management;
using BusinessLogic.Model;
using BusinessLogic.Utils;
using QuanLyKho.Models;
using QuanLyKho.Utility;
using System;
using System.Web.Mvc;

namespace QuanLyKho.Controllers
{
    public class QuanLyNhomVatTuController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Danh mục nhóm vật tư";
            ViewBag.TitleUrl = " / Danh mục nhóm vật tư";
            return View();
        }

        public ActionResult DanhSachNhomVatTu(int? pageIndex, string name)
        {
            try
            {
                pageIndex = pageIndex ?? 1;
                var pageSize = HangSo.PageSize;

                var list = NhomVatTuManager.Instance.SearchByPage(name,
                    (int)pageIndex, pageSize, out int totalPage);

                var pagecount = (int)Math.Ceiling((double)totalPage / (int)pageSize);
                var modelDs = new NhomVatTu_DSModel((int)pageIndex, pagecount, (int)pageSize, totalPage, list);
                return PartialView(modelDs);
            }
            catch (Exception ex)
            {
                ex.Log();
                return null;
            }
        }

        [HttpGet]
        public ActionResult FormNhomVatTu(string id)
        {
            try
            {
                NhomVatTuForm model = new NhomVatTuForm();
                ViewBag.status = "false";
                ViewBag.Status = "false";
                ViewBag.isSua = "false";

                if (!string.IsNullOrEmpty(id))
                {
                    ViewBag.TitleModal = "Cập nhật nhóm vật tư";
                    model.Id = Guid.Parse(id);
                    var nhom = NhomVatTuManager.Instance.SelectById(model.Id.Value);
                    if (nhom != null)
                    {
                        model.MaNhom = nhom.MaNhom;
                        model.TenNhom = nhom.TenNhom;
                    }
                    ViewBag.isSua = "true";
                }
                else
                {
                    ViewBag.TitleModal = "Thêm mới nhóm vật tư";
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
        public ActionResult FormNhomVatTu(NhomVatTuForm model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = new NhomVatTuModel
                    {
                        Id = model.Id.HasValue ? model.Id.Value : Guid.Empty,
                        MaNhom = model.MaNhom,
                        TenNhom = model.TenNhom
                    };

                    var message = NhomVatTuManager.Instance.SaveOrUpdate(data);
                    if (!string.IsNullOrEmpty(message))
                    {
                        ModelState.AddModelError("MaNhom", message);
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

        public ActionResult XoaNhomVatTu(string id)
        {
            try
            {
                var idGuid = Guid.Parse(id);
                var message = NhomVatTuManager.Instance.Delete(idGuid);
                if (!string.IsNullOrEmpty(message))
                {
                    return Json(new { Sucess = false, Message = message }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Sucess = true, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ex.Log();
                return null;
            }
        }
    }
}
