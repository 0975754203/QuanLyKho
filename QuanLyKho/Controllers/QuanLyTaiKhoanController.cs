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
    public class QuanLyTaiKhoanController : BaseController
    {
        // GET: QuanLyTaiKhoan
        public ActionResult Index()
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                ViewBag.Title = "Quản lý tài khoản";
                ViewBag.TitleUrl = " / Quản lý tài khoản";
                string linkDownload = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Authority + "/Content/Report/Templates/FileMauImportTaiKhoan.xlsx";
                ViewBag.LinkMau = linkDownload;
                string UserName = Global.ThongTinTaiKhoan_Login.UserName;
                ViewBag.Level = null;
                var checkTK = uow.Repository<TaiKhoan>().Query().Filter(x => x.UserName == UserName).FirstOrDefault();

            }
            return View();
        }
        public ActionResult DanhSachTaiKhoan(int? pageIndex, string name)
        {
            try
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    string UserName = Global.ThongTinTaiKhoan_Login.UserName;
                    var checkTK = uow.Repository<TaiKhoan>().Query().Filter(x => x.UserName == UserName).FirstOrDefault();

                    pageIndex = pageIndex ?? 1;
                    var pageSize = HangSo.PageSize;

                    var role = Global.ThongTinTaiKhoan_Login.Role;

                    var list = TaiKhoanManager.Instance.SearchByPageStatus(name,
                        (int)pageIndex, pageSize, out int totalPage, Global.ThongTinTaiKhoan_Login.UserName);

                    var pagecount = (int)Math.Ceiling((double)totalPage / (int)pageSize);
                    var modelDs = new TaiKhoan_DSModel((int)pageIndex, pagecount, (int)pageSize, totalPage, list);
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
        public ActionResult FormTaiKhoan(string id)
        {
            try
            {
                TaiKhoanForm model = new TaiKhoanForm();
                ViewBag.status = "false";
                if (!string.IsNullOrEmpty(id) || id != "")
                {
                    ViewBag.TitleModal = "Cập nhật tài khoản";
                    model.Id = Guid.Parse(id);
                    var taikhoan = TaiKhoanManager.Instance.SelectById((Guid)model.Id);
                    model.UserName = taikhoan.UserName;
                    model.Role = taikhoan.Role;
                    using (UnitOfWork uow = new UnitOfWork())
                    {
                        var user = uow.Repository<TaiKhoan>().Query().Filter(x => x.Id == model.Id).FirstOrDefault();
                        ViewBag.isSua = "false";
                        var checkTk = uow.Repository<TaiKhoan>().Query().Filter(y => y.UserName == taikhoan.UserName).FirstOrDefault();
                        if (checkTk != null)
                        {
                            ViewBag.isSua = "true";
                        }
                    }
                }
                else
                {
                    ViewBag.TitleModal = "Thêm mới tài khoản";
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
        public ActionResult FormTaiKhoan(TaiKhoanForm model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new TaiKhoanModel()
                    { 
                        UserName = model.UserName,
                        Role = model.Role,
                        Pass = model.Pass
                    };

                    if (string.IsNullOrEmpty(model.Id.ToString()))
                    {
                        user.Id = Guid.NewGuid();
                    }
                    else
                    {
                        user.Id = Guid.Parse(model.Id.ToString());
                    }
                    string message = "";
                    message = TaiKhoanManager.Instance.SaveOrUpdateTaiKhoan(user);

                    if (!string.IsNullOrEmpty(message))
                    {
                        ModelState.AddModelError("UserName", message);
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
        public ActionResult XoaTaiKhoan(string id)
        {
            try
            {
                var idGuid = Guid.Parse(id);
                using (UnitOfWork uow = new UnitOfWork())
                {
                    var user = uow.Repository<TaiKhoan>().Query().Filter(x => x.Id == idGuid).FirstOrDefault();
                    if (user != null)
                    {
                        user.State = EDataState.Deleted;
                        uow.Repository<TaiKhoan>().Delete(user);
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
        //[HttpPost]
        //public ActionResult KhoaOrMo(Guid Id, bool IsDaKhoa)
        //{
        //    try
        //    {
        //        using (UnitOfWork uow = new UnitOfWork())
        //        {
        //            var value = uow.Repository<TaiKhoan>().Query().Filter(x => x.Id == Id).FirstOrDefault();
        //            if (value != null)
        //            {
        //                value.TrangThai = IsDaKhoa;
        //                value.State = EDataState.Modified;
        //                uow.Repository<TaiKhoan>().InsertOrUpdate(value);
        //                uow.Save();
        //            }
        //        }
        //        return Json(new { Sucess = true, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Log();
        //        return Json(new { Sucess = false, Errors = ex }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        private DataTable GetExcelWorksheetData(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");

            Workbook workBook = new Workbook(fileName);
            Worksheet workSheet = workBook.Worksheets[0]; // Assumption is that we are working with one worksheet
            DataTable dataTable = new DataTable();
            ExportTableOptions options = new ExportTableOptions();
            options.ExportColumnName = true;
            options.IsVertical = true;
            options.ExportAsString = true;
            dataTable = workSheet.Cells.ExportDataTable(0, 0, workSheet.Cells.MaxDataRow + 1, workSheet.Cells.MaxColumn + 1, options);
            return dataTable;
        }
    }
}