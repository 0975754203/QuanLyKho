using BusinessLogic.Management;
using BusinessLogic.Model;
using BusinessLogic.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using QuanLyKho.Models;
using QuanLyKho.Utility;
using SQLDataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QuanLyKho.Controllers
{
    public class QuanLyTonKhoController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Danh sách tồn kho";
            ViewBag.TitleUrl = " / Danh sách tồn kho";
            using (var uow = new UnitOfWork())
            {
                var dsKho = uow.Repository<Kho>().Query().Get()
                    .OrderBy(x => x.TenKho)
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.TenKho })
                    .ToList();
                dsKho.Insert(0, new SelectListItem { Value = "", Text = "-- Tất cả kho --" });
                ViewBag.ComboKho = dsKho;

                var dsNhaCungCap = uow.Repository<NhaThauCungCap>().Query().Get()
                    .OrderBy(x => x.TenNhaThau)
                    .Select(x => new SelectListItem { Value = x.idNhathaucc.ToString(), Text = x.TenNhaThau })
                    .ToList();
                dsNhaCungCap.Insert(0, new SelectListItem { Value = "", Text = "-- Tất cả nhà cung cấp --" });
                ViewBag.ComboNhaCungCap = dsNhaCungCap;
            }
            return View();
        }

        [HttpGet]
        public ActionResult DanhSachTonKho(int? pageIndex, string sSearch, Guid? idKho, Guid? idNhaCungCap, string soHopDong)
        {
            try
            {
                pageIndex = pageIndex ?? 1;
                var pageSize = HangSo.PageSize;

                var list = KhoTonManager.Instance.Search(sSearch, idKho, idNhaCungCap, soHopDong, (int)pageIndex, pageSize, out int total);

                var pagecount = (int)Math.Ceiling((double)total / (int)pageSize);
                var modelDs = new KhoTon_DSModel((int)pageIndex, pagecount, (int)pageSize, total, list);
                return PartialView(modelDs);
            }
            catch (Exception ex)
            {
                ex.Log();
                return null;
            }
        }

        [HttpGet]
        public JsonResult GoiYSoHopDong(Guid? idNhaCungCap)
        {
            using (var uow = new UnitOfWork())
            {
                var query = uow.Repository<KhoGiaoDich>().Query()
                    .Filter(x => !x.DaXoa
                        && x.LoaiGiaoDich == "NHAP"
                        && x.SoHopDong != null
                        && x.SoHopDong != ""
                        && (!idNhaCungCap.HasValue || x.idNhaCungCap == idNhaCungCap.Value));

                var list = query.Get()
                    .Select(x => x.SoHopDong)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult BaoCaoXuatNhapTon()
        {
            ViewBag.Title = "Báo cáo xuất nhập tồn";
            ViewBag.TitleUrl = " / Báo cáo xuất nhập tồn";
            using (var uow = new UnitOfWork())
            {
                var dsKho = uow.Repository<Kho>().Query().Get()
                    .OrderBy(x => x.TenKho)
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.TenKho })
                    .ToList();
                dsKho.Insert(0, new SelectListItem { Value = "", Text = "-- Chọn kho --" });
                ViewBag.ComboKho = dsKho;
            }
            return View();
        }

        [HttpGet]
        public ActionResult DanhSachBaoCaoXuatNhapTon(int? pageIndex, DateTime? tuNgay, DateTime? denNgay, Guid? idKho, string tenSanPham, string maSanPham)
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

                var list = KhoTonManager.Instance.BaoCaoXuatNhapTon(tuNgay, denNgay, idKho, maSanPham, tenSanPham, pageIndex.Value, pageSize, out int total);
                var pagecount = (int)Math.Ceiling((double)total / pageSize);
                var model = new BaoCaoXuatNhapTon_DSModel(pageIndex.Value, pagecount, pageSize, total, list);
                return PartialView(model);
            }
            catch (Exception ex)
            {
                ex.Log();
                return null;
            }
        }

        [HttpGet]
        public ActionResult XuatExcelBaoCaoXuatNhapTon(DateTime? tuNgay, DateTime? denNgay, Guid? idKho, string tenSanPham, string maSanPham)
        {
            try
            {
                if (tuNgay.HasValue && denNgay.HasValue && tuNgay.Value.Date > denNgay.Value.Date)
                {
                    var tmp = tuNgay;
                    tuNgay = denNgay;
                    denNgay = tmp;
                }

                var excelPath = ExcelBaoCaoXuatNhapTon(1, tuNgay, denNgay, idKho, tenSanPham, maSanPham);
                if (string.IsNullOrWhiteSpace(excelPath))
                {
                    return Json(new { Sucess = false, Message = "Không tạo được file Excel." }, JsonRequestBehavior.AllowGet);
                }

                var fullPath = HostingEnvironment.MapPath("~" + excelPath);
                if (string.IsNullOrWhiteSpace(fullPath) || !System.IO.File.Exists(fullPath))
                {
                    return Json(new { Sucess = false, Message = "Không tìm thấy file Excel để tải." }, JsonRequestBehavior.AllowGet);
                }

                var fileName = string.Format("BaoCaoXuatNhapTon_{0:yyyyMMddHHmmss}.xlsx", DateTime.Now);
                var bytes = System.IO.File.ReadAllBytes(fullPath);
                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                ex.Log();
                return Json(new { Sucess = false, Message = "Xuất Excel thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }

        private string ExcelBaoCaoXuatNhapTon(int? pageIndex, DateTime? tuNgay, DateTime? denNgay, Guid? idKho, string tenSanPham, string maSanPham)
        {
            try
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    string path = @"/Content";
                    string namefile = CFileNameTemplate.BaoCaoXuatNhapTon;
                    var page = pageIndex ?? 1;
                    var tuNgayValue = tuNgay ?? DateTime.Now.Date;
                    var denNgayValue = denNgay ?? DateTime.Now.Date;
                    var list = KhoTonManager.Instance.BaoCaoXuatNhapTon(tuNgayValue, denNgayValue, idKho, maSanPham, tenSanPham, page, 999999, out int total);
                    var fullPath = HostingEnvironment.MapPath(@"~" + path);
                    ReportExcel cReport = new ReportExcel(namefile, fullPath, namefile);
                    cReport.AddFindAndReplaceItem("<TuNgay>", tuNgayValue.ToString("dd/MM/yyyy"));
                    cReport.AddFindAndReplaceItem("<DenNgay>", denNgayValue.ToString("dd/MM/yyyy"));
                    cReport.FindAndReplace();
                    string[] col = { "TenSanPham", "TonDauKy", "NhapTrongKy", "XuatTrongKy", "TonCuoiKy"};
                    cReport.ExportExcelByList(list, 3, 1, col);
                    //string status = cReport.saveFileExcel(stream, path, namefile);

                    return (path + namefile);
                }
            }
            catch (Exception ex)
            {
                ex.Log();
                return "";
            }
        }

        [HttpGet]
        public ActionResult BaoCaoXuatKhoaPhong()
        {
            ViewBag.Title = "Báo cáo xuất kho cho khoa phòng";
            ViewBag.TitleUrl = " / Báo cáo xuất kho cho khoa phòng";
            var dsKho = KhoManager.Instance.SelectAll()
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = string.IsNullOrWhiteSpace(x.MaKho) ? x.TenKho : (x.MaKho + " - " + x.TenKho)
                })
                .ToList();
            dsKho.Insert(0, new SelectListItem { Value = "", Text = "-- Tất cả kho --" });
            ViewBag.ComboKho = dsKho;
            return View();
        }

        [HttpGet]
        public ActionResult DanhSachBaoCaoXuatKhoaPhong(int? pageIndex, DateTime? tuNgay, DateTime? denNgay, Guid? idKho)
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

                var ketQua = KhoGiaoDichManager.Instance.BaoCaoXuatChoKhoaPhong(idKho, tuNgay, denNgay, pageIndex.Value, pageSize, out int total);
                var pagecount = (int)Math.Ceiling((double)total / pageSize);
                var model = new BaoCaoXuatKhoaPhong_DSModel(pageIndex.Value, pagecount, pageSize, total, ketQua.CotSanPham, ketQua.Hang);
                return PartialView(model);
            }
            catch (Exception ex)
            {
                ex.Log();
                return null;
            }
        }

        [HttpGet]
        public ActionResult XuatExcelBaoCaoXuatKhoaPhong(DateTime? tuNgay, DateTime? denNgay, Guid? idKho)
        {
            try
            {
                if (tuNgay.HasValue && denNgay.HasValue && tuNgay.Value.Date > denNgay.Value.Date)
                {
                    var tmp = tuNgay;
                    tuNgay = denNgay;
                    denNgay = tmp;
                }

                var tu = tuNgay ?? DateTime.Now.Date;
                var den = denNgay ?? DateTime.Now.Date;

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var ketQua = KhoGiaoDichManager.Instance.BaoCaoXuatChoKhoaPhong(idKho, tuNgay, denNgay, 1, int.MaxValue, out _);
                var cot = ketQua.CotSanPham ?? new List<BaoCaoXuatKhoaPhongCotSanPham>();
                var list = ketQua.Hang ?? new List<BaoCaoXuatKhoaPhongModel>();

                var templatePath = HostingEnvironment.MapPath("~/Content/Report/Templates/" + CFileNameTemplate.BaoCaoXuatKhoaPhong);
                if (string.IsNullOrEmpty(templatePath) || !System.IO.File.Exists(templatePath))
                {
                    return Json(new { Sucess = false, Message = "Không tìm thấy file mẫu Excel trong thư mục Content/Report/Templates." }, JsonRequestBehavior.AllowGet);
                }

                var templateBytes = System.IO.File.ReadAllBytes(templatePath);
                using (var ms = new MemoryStream(templateBytes))
                using (var package = new ExcelPackage(ms))
                {
                    var ws = package.Workbook.Worksheets[0];

                    if (ws.MergedCells != null)
                    {
                        foreach (var addr in ws.MergedCells.ToList())
                        {
                            try
                            {
                                var a = new ExcelAddress(addr);
                                if (a.Start.Row >= 3)
                                {
                                    ws.Cells[addr].Merge = false;
                                }
                            }
                            catch
                            {
                                // bỏ qua địa chỉ merge không hợp lệ
                            }
                        }
                    }

                    if (cot.Count > 47)
                    {
                        ws.InsertColumn(50, cot.Count - 47);
                    }

                    var lastCol = 3 + cot.Count;
                    var clearToRow = Math.Max(ws.Dimension?.End.Row ?? 48, list.Count + 25);
                    var clearToCol = Math.Max(ws.Dimension?.End.Column ?? 50, lastCol + 5);
                    for (var r = 3; r <= clearToRow; r++)
                    {
                        for (var c = 1; c <= clearToCol; c++)
                        {
                            var cell = ws.Cells[r, c];
                            cell.Formula = null;
                            cell.Value = null;
                        }
                    }

                    GhiTieuDeBaoCaoXuatKhoaPhong(ws, tu, den);

                    ws.Cells[2, 1].Value = "STT";
                    ws.Cells[2, 2].Value = "Tên khoa/ phòng";
                    for (var i = 0; i < cot.Count; i++)
                    {
                        ws.Cells[2, 3 + i].Value = cot[i].TenCot;
                    }
                    ws.Cells[2, lastCol].Value = "Tổng cộng";
                    for (var c = lastCol + 1; c <= clearToCol; c++)
                    {
                        ws.Cells[2, c].Value = null;
                    }

                    var firstDataRow = 3;
                    var stt = 1;
                    var row = firstDataRow;
                    foreach (var item in list)
                    {
                        ws.Cells[row, 1].Value = stt++;
                        ws.Cells[row, 2].Value = item.TenKhoaPhong;
                        for (var i = 0; i < cot.Count; i++)
                        {
                            var idSp = cot[i].IdSanPham;
                            var sl = item.SoLuongTheoSanPham != null && item.SoLuongTheoSanPham.ContainsKey(idSp)
                                ? item.SoLuongTheoSanPham[idSp]
                                : 0m;
                            var cell = ws.Cells[row, 3 + i];
                            if (sl != 0m)
                            {
                                cell.Value = sl;
                            }
                            cell.Style.Numberformat.Format = "#,##0.00";
                        }
                        ws.Cells[row, lastCol].Value = item.TongSoLuong;
                        ws.Cells[row, lastCol].Style.Numberformat.Format = "#,##0.00";
                        row++;
                    }

                    int lastDataRow;
                    if (list.Count == 0)
                    {
                        lastDataRow = firstDataRow;
                    }
                    else
                    {
                        lastDataRow = firstDataRow + list.Count - 1;
                    }

                    var sumRow = lastDataRow + 1;
                    ws.Cells[sumRow, 2].Value = "Tổng cộng";
                    for (var c = 3; c < lastCol; c++)
                    {
                        var colL = ExcelColumnLetter(c);
                        ws.Cells[sumRow, c].Formula = string.Format("SUM({0}{1}:{0}{2})", colL, firstDataRow, lastDataRow);
                    }
                    {
                        var colL = ExcelColumnLetter(lastCol);
                        ws.Cells[sumRow, lastCol].Formula = string.Format("SUM({0}{1}:{0}{2})", colL, firstDataRow, lastDataRow);
                    }

                    var rFooter1 = sumRow + 1;
                    var rFooter2 = sumRow + 2;
                    ws.Cells[rFooter1, 28, rFooter1, 38].Merge = true;
                    ws.Cells[rFooter1, 28].Value = "Hà Nội, ngày       tháng       năm ";
                    ws.Cells[rFooter1, 28].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rFooter2, 8, rFooter2, 12].Merge = true;
                    ws.Cells[rFooter2, 8].Value = "PHÒNG QUẢN TRỊ";
                    ws.Cells[rFooter2, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rFooter2, 15, rFooter2, 16].Merge = true;
                    ws.Cells[rFooter2, 17, rFooter2, 23].Merge = true;
                    ws.Cells[rFooter2, 28, rFooter2, 38].Merge = true;
                    ws.Cells[rFooter2, 28].Value = "NGƯỜI LẬP";
                    ws.Cells[rFooter2, 28].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    var bytes = package.GetAsByteArray();
                    var fileName = string.Format("BaoCaoXuatKhoaPhong_{0:yyyyMMddHHmmss}.xlsx", DateTime.Now);
                    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
            catch (Exception ex)
            {
                ex.Log();
                return Json(new { Sucess = false, Message = "Xuất Excel thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }

        private static void GhiTieuDeBaoCaoXuatKhoaPhong(ExcelWorksheet ws, DateTime tu, DateTime den)
        {
            var cell = ws.Cells["G1"];
            var phu = "(Thời gian từ ngày " + tu.ToString("dd/MM/yyyy") + " đến ngày " + den.ToString("dd/MM/yyyy") + ")";
            try
            {
                cell.RichText.Clear();
                var t1 = cell.RichText.Add("BẢNG TỔNG HỢP XUẤT KHO VẬT TƯ PHÒNG QUẢN TRỊ");
                t1.Bold = true;
                t1.Size = 18;
                t1.FontName = "Times New Roman";
                cell.RichText.Add("\n");
                var t2 = cell.RichText.Add(phu);
                t2.Size = 18;
                t2.FontName = "Times New Roman";
            }
            catch
            {
                cell.Value = "BẢNG TỔNG HỢP XUẤT KHO VẬT TƯ PHÒNG QUẢN TRỊ " + phu;
            }
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        }

        private static string ExcelColumnLetter(int column)
        {
            if (column < 1)
            {
                return "A";
            }
            var s = string.Empty;
            var col = column;
            while (col > 0)
            {
                var remainder = (col - 1) % 26;
                s = (char)('A' + remainder) + s;
                col = (col - 1) / 26;
            }
            return s;
        }
    }
}

