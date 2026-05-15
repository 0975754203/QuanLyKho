using BusinessLogic.Management;
using BusinessLogic.Model;
using BusinessLogic.Utils;
using QuanLyKho.Models;
using QuanLyKho.Utility;
using SQLDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace QuanLyKho.Controllers
{
    public class QuanLyGiaoDichNhapXuatController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Danh sách nhập kho";
            ViewBag.TitleUrl = " / Danh sách nhập kho";
            PrepareDanhSachNhapKhoViewData();
            PrepareNhapKhoFormViewData();
            return View();
        }

        [HttpGet]
        public ActionResult DanhSachNhapKho(int? pageIndex, string sSearch, Guid? idKho, DateTime? tuNgay, DateTime? denNgay)
        {
            try
            {
                pageIndex = pageIndex ?? 1;
                var pageSize = HangSo.PageSize;
                if (tuNgay.HasValue && denNgay.HasValue && tuNgay.Value.Date > denNgay.Value.Date)
                {
                    var tmp = tuNgay;
                    tuNgay = denNgay;
                    denNgay = tmp;
                }
                var list = KhoGiaoDichManager.Instance.SearchNhapKho(sSearch, idKho, tuNgay, denNgay, pageIndex.Value, pageSize, out int total);
                var pagecount = (int)Math.Ceiling((double)total / pageSize);
                var model = new KhoGiaoDich_DSModel(pageIndex.Value, pagecount, pageSize, total, list);
                return PartialView(model);
            }
            catch (Exception ex)
            {
                ex.Log();
                return null;
            }
        }

        [HttpPost]
        public ActionResult XoaNhapKho(string id)
        {
            try
            {
                var idGuid = Guid.Parse(id);
                var message = KhoGiaoDichManager.Instance.XoaGiaoDich(idGuid);
                var success = string.IsNullOrEmpty(message);
                return Json(new { Sucess = success, Message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ex.Log();
                return Json(new { Sucess = false, Message = "Xóa giao dịch thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult InNhapKho(string id)
        {
            try
            {
                var idGuid = Guid.Parse(id);
                var model = KhoGiaoDichManager.Instance.SelectById(idGuid);
                if (model == null)
                {
                    return HttpNotFound();
                }

                ViewBag.Title = "In phiếu nhập kho";
                return View(model);
            }
            catch (Exception ex)
            {
                ex.Log();
                return HttpNotFound();
            }
        }

        [HttpGet]
        public ActionResult IndexXuatKho()
        {
            ViewBag.Title = "Danh sách xuất kho";
            ViewBag.TitleUrl = " / Danh sách xuất kho";
            PrepareDanhSachXuatKhoViewData();
            PrepareXuatKhoFormViewData();
            return View();
        }

        [HttpGet]
        public ActionResult DanhSachXuatKho(int? pageIndex, string sSearch, Guid? idKho, DateTime? tuNgay, DateTime? denNgay)
        {
            try
            {
                pageIndex = pageIndex ?? 1;
                var pageSize = HangSo.PageSize;
                if (tuNgay.HasValue && denNgay.HasValue && tuNgay.Value.Date > denNgay.Value.Date)
                {
                    var tmp = tuNgay;
                    tuNgay = denNgay;
                    denNgay = tmp;
                }
                var list = KhoGiaoDichManager.Instance.SearchXuatKho(sSearch, idKho, tuNgay, denNgay, pageIndex.Value, pageSize, out int total);
                var pagecount = (int)Math.Ceiling((double)total / pageSize);
                var model = new KhoGiaoDich_DSModel(pageIndex.Value, pagecount, pageSize, total, list);
                return PartialView(model);
            }
            catch (Exception ex)
            {
                ex.Log();
                return null;
            }
        }

        [HttpPost]
        public ActionResult XoaXuatKho(string id)
        {
            try
            {
                var idGuid = Guid.Parse(id);
                var message = KhoGiaoDichManager.Instance.XoaGiaoDich(idGuid);
                var success = string.IsNullOrEmpty(message);
                return Json(new { Sucess = success, Message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ex.Log();
                return Json(new { Sucess = false, Message = "Xóa giao dịch thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult InXuatKho(string id)
        {
            try
            {
                var idGuid = Guid.Parse(id);
                var model = KhoGiaoDichManager.Instance.SelectById(idGuid);
                if (model == null)
                {
                    return HttpNotFound();
                }

                ViewBag.Title = "In phiếu xuất kho";
                return View(model);
            }
            catch (Exception ex)
            {
                ex.Log();
                return HttpNotFound();
            }
        }

        [HttpGet]
        public JsonResult SanPhamCoTonTrongKho(Guid? idKho)
        {
            if (!idKho.HasValue || idKho.Value == Guid.Empty)
            {
                return Json(new object[0], JsonRequestBehavior.AllowGet);
            }
            var list = KhoTonManager.Instance.LaySanPhamCoTonTrongKho(idKho.Value);
            return Json(list.Select(x => new
            {
                id = x.IdSanPham.ToString(),
                text = x.DisplayText,
                name = x.TenSanPham,
                price = x.DonGia.ToString("N0"),
                origin = string.IsNullOrWhiteSpace(x.XuatXu) ? "Chưa cập nhật" : x.XuatXu,
                stock = x.TonKho.ToString("N2"),
                unit = x.DonViTinh ?? string.Empty
            }), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult XuatKho()
        {
            ViewBag.Title = "Xuất kho";
            ViewBag.TitleUrl = " / Xuất kho";

            var model = BuildXuatKhoModel();
            PrepareXuatKhoFormViewData();

            return View(model);
        }

        [HttpGet]
        public ActionResult FormXuatKho()
        {
            var model = BuildXuatKhoModel();
            PrepareXuatKhoFormViewData();
            return PartialView("_FormXuatKho", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XuatKho(KhoGiaoDichModel model)
        {
            try
            {
                var hasIdKho = model.IdKho.HasValue && model.IdKho.Value != Guid.Empty;
                var hasIdKhoaPhong = model.IdKhoaPhong.HasValue && model.IdKhoaPhong.Value != Guid.Empty;
                if (!hasIdKho && !hasIdKhoaPhong)
                {
                    ModelState.AddModelError(nameof(model.IdKho), "Vui lòng chọn kho xuất hoặc khoa/phòng.");
                    ModelState.AddModelError(nameof(model.IdKhoaPhong), "Vui lòng chọn kho xuất hoặc khoa/phòng.");
                }

                if (model.lstChiTiet == null || model.lstChiTiet.Count == 0)
                {
                    ModelState.AddModelError("ChiTiet", "Vui lòng thêm ít nhất 1 sản phẩm.");
                }

                if (model.lstChiTiet != null)
                {
                    for (int i = 0; i < model.lstChiTiet.Count; i++)
                    {
                        if (model.lstChiTiet[i].IdSanPham == Guid.Empty)
                        {
                            ModelState.AddModelError($"ChiTiet[{i}].IdSanPham", "Vui lòng chọn sản phẩm.");
                        }
                        if (model.lstChiTiet[i].SoLuong <= 0)
                        {
                            ModelState.AddModelError($"ChiTiet[{i}].SoLuong", "Số lượng phải > 0.");
                        }
                    }
                }

                if (!ModelState.IsValid)
                {
                    return Json(new { Sucess = false, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
                }

                var details = model.lstChiTiet
                    .Where(x => x.IdSanPham != Guid.Empty && x.SoLuong > 0)
                    .GroupBy(x => x.IdSanPham)
                    .Select(g => new KhoGiaoDichChiTietModel
                    {
                        IdSanPham = g.Key,
                        SoLuong = g.Sum(z => z.SoLuong)
                    })
                    .ToList();

                if (details.Count == 0)
                {
                    ModelState.AddModelError("ChiTiet", "Vui lòng thêm ít nhất 1 sản phẩm hợp lệ.");
                    return Json(new { Sucess = false, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
                }

                if (model.IdKhoNhan.HasValue && model.IdKhoNhan.Value != Guid.Empty
                    && model.IdKho.HasValue && model.IdKhoNhan.Value == model.IdKho.Value)
                {
                    ModelState.AddModelError(nameof(model.IdKhoNhan), "Kho nhận phải khác kho xuất.");
                    return Json(new { Sucess = false, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
                }

                var data = new KhoGiaoDichModel
                {
                    IdNguoiTao = model.IdNguoiTao,
                    IdKhoaPhong = model.IdKhoaPhong,
                    IdKho = model.IdKho,
                    IdKhoNhan = (model.IdKhoNhan.HasValue && model.IdKhoNhan.Value != Guid.Empty) ? model.IdKhoNhan : null,
                    GhiChu = model.GhiChu,
                    NguoiGiaoNhan = string.IsNullOrWhiteSpace(model.NguoiGiaoNhan) ? null : model.NguoiGiaoNhan.Trim(),
                    LoaiGiaoDich = "XUAT",
                    lstChiTiet = details
                };

                var message = KhoGiaoDichManager.Instance.SaveOrUpdate(data);
                if (!string.IsNullOrEmpty(message))
                {
                    ModelState.AddModelError("ChiTiet", message);
                    return Json(new { Sucess = false, Message = message, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { Sucess = true, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ex.Log();
                ModelState.AddModelError("ChiTiet", "Xảy ra lỗi hệ thống. Vui lòng thử lại sau.");
                return Json(new { Sucess = false, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult NhapKho()
        {
            ViewBag.Title = "Nhập kho";
            ViewBag.TitleUrl = " / Nhập kho";

            var model = BuildNhapKhoModel();
            PrepareNhapKhoFormViewData();

            return View(model);
        }

        [HttpGet]
        public ActionResult FormNhapKho()
        {
            var model = BuildNhapKhoModel();
            PrepareNhapKhoFormViewData();
            return PartialView("_FormNhapKho", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NhapKho(KhoGiaoDichModel model)
        {
            try
            {
                if (!model.IdKho.HasValue || model.IdKho.Value == Guid.Empty)
                {
                    ModelState.AddModelError(nameof(model.IdKho), "Vui lòng chọn kho.");
                }

                if (model.lstChiTiet == null || model.lstChiTiet.Count == 0)
                {
                    ModelState.AddModelError("ChiTiet", "Vui lòng thêm ít nhất 1 sản phẩm.");
                }

                if (model.lstChiTiet != null)
                {
                    for (int i = 0; i < model.lstChiTiet.Count; i++)
                    {
                        if (model.lstChiTiet[i].IdSanPham == Guid.Empty)
                        {
                            ModelState.AddModelError($"ChiTiet[{i}].IdSanPham", "Vui lòng chọn sản phẩm.");
                        }
                        if (model.lstChiTiet[i].SoLuong <= 0)
                        {
                            ModelState.AddModelError($"ChiTiet[{i}].SoLuong", "Số lượng phải > 0.");
                        }
                    }
                }

                if (!ModelState.IsValid)
                {
                    return Json(new { Sucess = false, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
                }

                var details = model.lstChiTiet
                    .Where(x => x.IdSanPham != Guid.Empty && x.SoLuong > 0)
                    .GroupBy(x => x.IdSanPham)
                    .Select(g => new KhoGiaoDichChiTietModel
                    {
                        IdSanPham = g.Key,
                        SoLuong = g.Sum(z => z.SoLuong)
                    })
                    .ToList();

                if (details.Count == 0)
                {
                    ModelState.AddModelError("ChiTiet", "Vui lòng thêm ít nhất 1 sản phẩm hợp lệ.");
                    return Json(new { Sucess = false, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
                }

                var data = new KhoGiaoDichModel
                {
                    IdNguoiTao = model.IdNguoiTao,
                    IdKhoaPhong = model.IdKhoaPhong,
                    IdKho = model.IdKho,
                    GhiChu = model.GhiChu,
                    NguoiGiaoNhan = string.IsNullOrWhiteSpace(model.NguoiGiaoNhan) ? null : model.NguoiGiaoNhan.Trim(),
                    LoaiGiaoDich = "NHAP",
                    lstChiTiet = details
                };

                var message = KhoGiaoDichManager.Instance.SaveOrUpdate(data);
                if (!string.IsNullOrEmpty(message))
                {
                    ModelState.AddModelError("ChiTiet", message);
                    return Json(new { Sucess = false, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { Sucess = true, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ex.Log();
                ModelState.AddModelError("ChiTiet", "Xảy ra lỗi hệ thống. Vui lòng thử lại sau.");
                return Json(new { Sucess = false, Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
            }
        }

        private void PrepareDanhSachNhapKhoViewData()
        {
            var dsKho = KhoManager.Instance.SelectAll()
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = string.IsNullOrWhiteSpace(x.MaKho) ? x.TenKho : (x.MaKho + " - " + x.TenKho)
                })
                .ToList();
            dsKho.Insert(0, new SelectListItem { Value = "", Text = "-- Tất cả kho --" });
            ViewBag.ComboKho = dsKho;
        }

        private void PrepareDanhSachXuatKhoViewData()
        {
            var dsKho = KhoManager.Instance.SelectAll()
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = string.IsNullOrWhiteSpace(x.MaKho) ? x.TenKho : (x.MaKho + " - " + x.TenKho)
                })
                .ToList();
            dsKho.Insert(0, new SelectListItem { Value = "", Text = "-- Tất cả kho --" });
            ViewBag.ComboKho = dsKho;
        }

        private KhoGiaoDichModel BuildNhapKhoModel()
        {
            var acc = Global.ThongTinTaiKhoan_Login;
            var model = new KhoGiaoDichModel
            {
                IdNguoiTao = Guid.Parse(acc.Id),
                lstChiTiet = new List<KhoGiaoDichChiTietModel>
                {
                    new KhoGiaoDichChiTietModel { SoLuong = 1 }
                }
            };
            return model;
        }

        private KhoGiaoDichModel BuildXuatKhoModel()
        {
            var acc = Global.ThongTinTaiKhoan_Login;
            var model = new KhoGiaoDichModel
            {
                IdNguoiTao = Guid.Parse(acc.Id),
                lstChiTiet = new List<KhoGiaoDichChiTietModel>
                {
                    new KhoGiaoDichChiTietModel { SoLuong = 1 }
                }
            };
            return model;
        }

        private void PrepareNhapKhoFormViewData()
        {
            using (var uow = new UnitOfWork())
            {
                var dsSanPham = uow.Repository<KhoSanPham>().Query().Get()
                    .OrderBy(x => x.TenSanPham)
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.TenSanPham + "||" + x.DonGia.ToString("N0") + "||" + (string.IsNullOrWhiteSpace(x.XuatXu) ? "Chưa cập nhật" : x.XuatXu)
                    })
                    .ToList();
                dsSanPham.Insert(0, new SelectListItem { Value = "", Text = "-- Chọn sản phẩm --" });
                ViewBag.ComboSanPham = dsSanPham;

                var dsKho = uow.Repository<Kho>().Query().Get()
                    .OrderBy(x => x.TenKho)
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.TenKho })
                    .ToList();
                dsKho.Insert(0, new SelectListItem { Value = "", Text = "-- Chọn kho --" });
                ViewBag.ComboKhoNhap = dsKho;
            }
        }

        private void PrepareXuatKhoFormViewData()
        {
            using (var uow = new UnitOfWork())
            {
                var dsKhoSrc = uow.Repository<Kho>().Query().Get()
                    .OrderBy(x => x.TenKho)
                    .Select(x => new { Value = x.Id.ToString(), Text = x.TenKho })
                    .ToList();

                var dsKho = dsKhoSrc.Select(x => new SelectListItem { Value = x.Value, Text = x.Text }).ToList();
                dsKho.Insert(0, new SelectListItem { Value = "", Text = "-- Chọn kho xuất --" });
                ViewBag.ComboKhoXuat = dsKho;

                var dsKhoNhan = dsKhoSrc.Select(x => new SelectListItem { Value = x.Value, Text = x.Text }).ToList();
                dsKhoNhan.Insert(0, new SelectListItem { Value = "", Text = "-- Không chuyển kho --" });
                ViewBag.ComboKhoNhan = dsKhoNhan;
            }

            var dsKhoaPhong = TuDienManager.Instance.SelectByLoai(cfLoaiTuDien.LoaiKhoaPhong)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.TenTuDien })
                .ToList();
            dsKhoaPhong.Insert(0, new SelectListItem { Value = "", Text = "-- Chọn khoa/phòng --" });
            ViewBag.ComboKhoaPhong = dsKhoaPhong;

            ViewBag.ComboSanPhamXuat = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "-- Chọn kho trước --" }
            };
        }
    }
}
