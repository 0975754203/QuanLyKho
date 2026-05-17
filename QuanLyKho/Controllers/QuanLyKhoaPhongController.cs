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
    public class QuanLyKhoaPhongController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Quản lý Khoa Phòng";
            ViewBag.TitleUrl = " / Quản lý Khoa Phòng";
            return View();
        }

        [HttpGet]
        public ActionResult DanhSachKhoaPhong(int? pageIndex, string sSearch)
        {
            try
            {
                pageIndex = pageIndex ?? 1;
                var pageSize = HangSo.PageSize;
                var term = (sSearch ?? string.Empty).Trim().ToLower();

                var list = TuDienManager.Instance.SelectByLoai(cfLoaiTuDien.LoaiKhoaPhong);
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
                var model = new KhoaPhong_DSModel(pageIndex.Value, pagecount, pageSize, total, data);
                return PartialView(model);
            }
            catch (Exception ex)
            {
                ex.Log();
                return null;
            }
        }

        [HttpGet]
        public ActionResult FormKhoaPhong(string id)
        {
            try
            {
                var model = new KhoaPhongForm();
                if (!string.IsNullOrEmpty(id))
                {
                    var khoaPhong = TuDienManager.Instance.SelectById(Guid.Parse(id));
                    if (khoaPhong != null)
                    {
                        model.Id = khoaPhong.Id;
                        model.MaTuDien = khoaPhong.MaTuDien;
                        model.TenTuDien = khoaPhong.TenTuDien;
                        model.GhiChu = khoaPhong.GhiChu;
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
        public ActionResult FormKhoaPhong(KhoaPhongForm model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { Sucess = false, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);

                var loaiKhoaPhong = TuDienManager.Instance.SelectLoaiTuDienByMa(cfLoaiTuDien.LoaiKhoaPhong);
                if (loaiKhoaPhong == null)
                {
                    ModelState.AddModelError("MaTuDien", "Chưa cấu hình loại từ điển khoa/phòng.");
                    return Json(new { Sucess = false, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
                }

                var data = new TuDienModel
                {
                    Id = model.Id ?? Guid.Empty,
                    IdLoaiTuDien = loaiKhoaPhong.Id,
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
        public ActionResult XoaKhoaPhong(string id)
        {
            try
            {
                var idGuid = Guid.Parse(id);
                using (var uow = new UnitOfWork())
                {
                    var daSuDung = uow.Repository<KhoGiaoDich>().Query()
                        .Filter(x => x.IdKhoaPhong == idGuid)
                        .FirstOrDefault();
                    if (daSuDung != null)
                    {
                        return Json(new { Sucess = false, Message = "Không thể xóa: khoa/phòng đang được phiếu xuất sử dụng." }, JsonRequestBehavior.AllowGet);
                    }
                }

                TuDienManager.Instance.Delete(idGuid);
                return Json(new { Sucess = true, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ex.Log();
                return Json(new { Sucess = false, Message = "Không xóa được khoa/phòng." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
