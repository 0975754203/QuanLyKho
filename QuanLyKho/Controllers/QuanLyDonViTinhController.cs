using BusinessLogic.Management;
using BusinessLogic.Model;
using BusinessLogic.Utils;
using QuanLyKho.Models;
using SQLDataAccess;
using System;
using System.Linq;
using System.Web.Mvc;

namespace QuanLyKho.Controllers
{
    public class QuanLyDonViTinhController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Đơn vị tính";
            ViewBag.TitleUrl = " / Đơn vị tính";
            return View();
        }

        [HttpGet]
        public ActionResult DanhSachDonViTinh(int? pageIndex, string sSearch)
        {
            try
            {
                pageIndex = pageIndex ?? 1;
                var pageSize = HangSo.PageSize;
                var term = (sSearch ?? string.Empty).Trim().ToLower();

                var list = TuDienManager.Instance.SelectByLoai(cfLoaiTuDien.LoaiDonViTinh);
                if (!string.IsNullOrWhiteSpace(term))
                {
                    list = list.Where(x =>
                        (!string.IsNullOrWhiteSpace(x.MaTuDien) && x.MaTuDien.ToLower().Contains(term))
                        || (!string.IsNullOrWhiteSpace(x.TenTuDien) && x.TenTuDien.ToLower().Contains(term)))
                        .ToList();
                }

                var total = list.Count;
                var data = list
                    .OrderBy(x => x.TenTuDien)
                    .Skip((pageIndex.Value - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var pagecount = (int)Math.Ceiling((double)total / pageSize);
                var model = new DonViTinh_DSModel(pageIndex.Value, pagecount, pageSize, total, data);
                return PartialView(model);
            }
            catch (Exception ex)
            {
                ex.Log();
                return null;
            }
        }

        [HttpGet]
        public ActionResult FormDonViTinh(string id)
        {
            try
            {
                var model = new DonViTinhForm();
                if (!string.IsNullOrEmpty(id))
                {
                    var donVi = TuDienManager.Instance.SelectById(Guid.Parse(id));
                    if (donVi != null)
                    {
                        model.Id = donVi.Id;
                        model.MaTuDien = donVi.MaTuDien;
                        model.TenTuDien = donVi.TenTuDien;
                        model.GhiChu = donVi.GhiChu;
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
        public ActionResult FormDonViTinh(DonViTinhForm model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { Sucess = false, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);

                var loaiDonViTinh = TuDienManager.Instance.SelectLoaiTuDienByMa(cfLoaiTuDien.LoaiDonViTinh);
                if (loaiDonViTinh == null)
                {
                    ModelState.AddModelError("MaTuDien", "Chưa cấu hình loại từ điển đơn vị tính.");
                    return Json(new { Sucess = false, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
                }

                var data = new TuDienModel
                {
                    Id = model.Id ?? Guid.Empty,
                    IdLoaiTuDien = loaiDonViTinh.Id,
                    MaTuDien = (model.MaTuDien ?? string.Empty).Trim(),
                    TenTuDien = (model.TenTuDien ?? string.Empty).Trim(),
                    GhiChu = model.GhiChu
                };

                try
                {
                    TuDienManager.Instance.SaveOrUpdate(data);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("MaTuDien", ex.Message);
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
        public ActionResult XoaDonViTinh(string id)
        {
            try
            {
                var idGuid = Guid.Parse(id);
                using (var uow = new UnitOfWork())
                {
                    var daSuDung = uow.Repository<KhoSanPham>().Query()
                        .Filter(x => x.IdDonViTinh == idGuid)
                        .FirstOrDefault();
                    if (daSuDung != null)
                    {
                        return Json(new { Sucess = false, Message = "Không thể xóa: đơn vị tính đang được vật tư sử dụng." }, JsonRequestBehavior.AllowGet);
                    }
                }

                TuDienManager.Instance.Delete(idGuid);
                return Json(new { Sucess = true, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ex.Log();
                return Json(new { Sucess = false, Message = "Không xóa được đơn vị tính." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
