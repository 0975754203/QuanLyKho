using BusinessLogic.Management;
using BusinessLogic.Utils;
using BusinessLogic;
using SQLDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyKho.Models;
using QuanLyKho.Utility;
using BusinessLogic.Model;
using Aspose.Cells;
using System.Data;
using System.IO;
using System.Web.Hosting;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using static BusinessLogic.Utils.HangSo;

namespace QuanLyKho.Controllers
{
    public class QuanLySanPhamController : BaseController
    {
        // GET: QuanLyTaiKhoan
        public ActionResult Index()
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                ViewBag.Title = "Quản lý vật tư";
                ViewBag.TitleUrl = " / Quản lý vật tư";
                string UserName = Global.ThongTinTaiKhoan_Login.UserName;
                ViewBag.Level = null;

            }
            return View();
        }
        public ActionResult DanhSachSanPham(int? pageIndex, string sSearch, Guid? idNhomVatTu)
        {
            try
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    string UserName = Global.ThongTinTaiKhoan_Login.UserName;


                    pageIndex = pageIndex ?? 1;
                    var pageSize = HangSo.PageSize;

                    var role = Global.ThongTinTaiKhoan_Login.Role;

                    var list = KhoSanPhamManager.Instance.Search(sSearch, idNhomVatTu,
                        (int)pageIndex, pageSize, out int totalPage);

                    var pagecount = (int)Math.Ceiling((double)totalPage / (int)pageSize);
                    var modelDs = new KhoSanPham_DSModel((int)pageIndex, pagecount, (int)pageSize, totalPage, list);
                    return PartialView(modelDs);
                }
            }
            catch (Exception ex)
            {
                ex.Log();
                return null;
            }
        }
        [HttpGet]
        public ActionResult FormSanPham(string id)
        {
            try
            {
                ViewBag.status = "false";
                ViewBag.Status = "false";
                ViewBag.isAdmin = "false";
                if (Global.CoQuyenThaoTacDuLieuKho())
                {
                    ViewBag.isAdmin = "true";
                }
                if (!string.IsNullOrEmpty(id))
                {
                    ViewBag.TitleModal = "Cập nhật vật tư";
                    var sanpham = KhoSanPhamManager.Instance.SelectById(Guid.Parse(id));
                    var result = sanpham.CopyAs<KhoSanPhamForm>();
                    result.IdNhomVatTu = sanpham.IdNhomVatTu ?? Guid.Empty;
                    return PartialView(result);
                }
                else
                {
                    KhoSanPhamForm model = new KhoSanPhamForm();
                    ViewBag.TitleModal = "Thêm mới vật tư";
                    return PartialView(model);
                }
            }
            catch (Exception ex)
            {
                ex.Log();
                return null;
            }
        }
        [HttpPost]
        public ActionResult FormSanPham(KhoSanPhamForm model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!Global.CoQuyenThaoTacDuLieuKho())
                    {
                        ModelState.AddModelError("MaSanPham", "Không có quyền thao tác.");
                        return Json(new { Sucess = false, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
                    }
                    if (string.IsNullOrEmpty(model.Id.ToString()))
                    {
                        model.Id = Guid.NewGuid();
                    }

                    string message = "";
                    var data = model.CopyAs<KhoSanPhamModel>();
                    data.IdNhomVatTu = model.IdNhomVatTu;
                    data.XuatXu = model.XuatXu;
                    message = KhoSanPhamManager.Instance.SaveOrUpdate(data);

                    if (!string.IsNullOrEmpty(message))
                    {
                        ModelState.AddModelError("MaSanPham", message);
                        return Json(new { Sucess = false, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { Sucess = true, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Sucess = false, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ex.Log();
                return null;
            }
        }
        public ActionResult XoaSanPham(string id)
        {
            try
            {
                if (!Global.CoQuyenThaoTacDuLieuKho())
                {
                    return Json(new { Sucess = false, Errors = ModelState.Errors(), Msg = "Không có quyền thao tác." }, JsonRequestBehavior.AllowGet);
                }
                var idGuid = Guid.Parse(id);
                using (UnitOfWork uow = new UnitOfWork())
                {
                    var sp = uow.Repository<KhoSanPham>().Query().Filter(x => x.Id == idGuid).FirstOrDefault();
                    if (sp != null)
                    {
                        var checkDaSuDung = uow.Repository<KhoGiaoDichChiTiet>().Query().Filter(y => y.IdSanPham == sp.Id).FirstOrDefault();
                        if (checkDaSuDung != null)
                        {
                            return Json(new { Sucess = false, Errors = ModelState.Errors(), Msg = "Sản phẩm đã được sử dụng." }, JsonRequestBehavior.AllowGet);
                        }
                        sp.State = EDataState.Deleted;
                        uow.Repository<KhoSanPham>().Delete(sp);
                        uow.Save();
                        return Json(new { Sucess = true, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { Sucess = false, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ex.Log();
                return null;
            }

        }
    }
}
