using BusinessLogic.Management;
using BusinessLogic.Utils;
using QuanLyKho.Models;
using QuanLyKho.Utility;
using SQLDataAccess;
using System;
using System.Linq;
using System.Web.Mvc;

namespace QuanLyKho.Controllers
{
    public class QuanLyTheKhoController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Thẻ kho";
            ViewBag.TitleUrl = " / Thẻ kho";

            using (var uow = new UnitOfWork())
            {
                var dsKho = uow.Repository<Kho>().Query().Get()
                    .OrderBy(x => x.TenKho)
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.TenKho })
                    .ToList();
                dsKho.Insert(0, new SelectListItem { Value = "", Text = "-- Chọn kho --" });
                ViewBag.ComboKho = dsKho;
            }

            ViewBag.ComboSanPham = new System.Collections.Generic.List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "-- Chọn kho trước --" }
            };

            return View();
        }

        [HttpGet]
        public JsonResult SanPhamTrongKho(Guid? idKho)
        {
            if (!idKho.HasValue || idKho.Value == Guid.Empty)
            {
                return Json(new object[0], JsonRequestBehavior.AllowGet);
            }
            var list = KhoTonManager.Instance.LaySanPhamTrongKho(idKho.Value);
            return Json(list.Select(x => new { id = x.IdSanPham.ToString(), text = x.DisplayText }), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DanhSachTheKho(int? pageIndex, Guid? idKho, Guid? idSanPham, DateTime? tuNgay, DateTime? denNgay)
        {
            try
            {
                pageIndex = pageIndex ?? 1;
                var pageSize = HangSo.PageSize;

                if (!idKho.HasValue || idKho.Value == Guid.Empty
                    || !idSanPham.HasValue || idSanPham.Value == Guid.Empty)
                {
                    var empty = new TheKho_DSModel(1, 0, pageSize, 0, new System.Collections.Generic.List<BusinessLogic.Model.TheKhoModel>(), 0m);
                    return PartialView(empty);
                }

                if (tuNgay.HasValue && denNgay.HasValue && tuNgay.Value.Date > denNgay.Value.Date)
                {
                    var tmp = tuNgay;
                    tuNgay = denNgay;
                    denNgay = tmp;
                }

                var list = KhoTonManager.Instance.LayLichSuTheKho(
                    idSanPham.Value,
                    idKho.Value,
                    tuNgay,
                    denNgay,
                    pageIndex.Value,
                    pageSize,
                    out int total,
                    out decimal tongSoLuong);

                var pagecount = (int)Math.Ceiling((double)total / pageSize);
                var model = new TheKho_DSModel(pageIndex.Value, pagecount, pageSize, total, list, tongSoLuong);
                return PartialView(model);
            }
            catch (Exception ex)
            {
                ex.Log();
                return null;
            }
        }
    }
}
