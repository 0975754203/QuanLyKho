using BusinessLogic.Model;
using BusinessLogic.Utils;
using SQLDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.Management
{
    public class KhoTonManager
    {
        #region Singleton
        private static KhoTonManager _instance;
        private KhoTonManager() { }
        public static KhoTonManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new KhoTonManager();
                }
                return _instance;
            }
        }
        #endregion

        #region Public Interface
        public List<KhoTonModel> Search(string sSearch, Guid? idKho, int pageIndex, int pageSize, out int total)
        {
            using (var uow = new UnitOfWork())
            {
                var query = uow.Repository<KhoTon>()
                    .Query()
                    .Filter(x =>
                        (string.IsNullOrEmpty(sSearch)
                            || (x.KhoSanPham != null
                                && (
                                    (x.KhoSanPham.MaSanPham != null && x.KhoSanPham.MaSanPham.Contains(sSearch))
                                    || (x.KhoSanPham.TenSanPham != null && x.KhoSanPham.TenSanPham.Contains(sSearch))
                                )))
                        && (!idKho.HasValue || x.IdKho == idKho.Value));

                var pageItems = query
                    .OrderBy(x => x.OrderBy(y => y.KhoSanPham.MaSanPham))
                    .GetPage(pageIndex, pageSize, out total);

                return pageItems.Select(x => new KhoTonModel
                {
                    Id = x.Id,
                    IdSanPham = x.IdSanPham,
                    IdKho = x.IdKho,
                    SoLuong = x.SoLuong,
                    TonKho = x.SoLuong,
                    MaSanPham = x.KhoSanPham != null ? x.KhoSanPham.MaSanPham : null,
                    TenSanPham = x.KhoSanPham != null ? x.KhoSanPham.TenSanPham : null,
                    DonViTinh = (x.KhoSanPham != null && x.KhoSanPham.TuDien != null) ? x.KhoSanPham.TuDien.TenTuDien : null,
                    TenKho = x.Kho != null ? x.Kho.TenKho : null,
                    DonGia = x.KhoSanPham != null ? x.KhoSanPham.DonGia : 0,
                    ThoiGianBaoHanh = x.KhoSanPham != null ? x.KhoSanPham.ThoiGianBaoHanh : null
                }).ToList();
            }
        }

        /// <summary>
        /// Sản phẩm có tồn &gt; 0 trong kho (dùng cho form xuất kho).
        /// </summary>
        public List<KhoSanPhamTrongKhoOption> LaySanPhamCoTonTrongKho(Guid idKho)
        {
            using (var uow = new UnitOfWork())
            {
                var query = uow.Repository<KhoTon>()
                    .Query()
                    .Filter(x => x.IdKho == idKho && x.SoLuong > 0)
                    .Include(x => x.KhoSanPham);

                var list = query
                    .OrderBy(x => x.OrderBy(y => y.KhoSanPham.TenSanPham))
                    .Get()
                    .ToList();

                var idDonVis = list.Where(x => x.KhoSanPham != null).Select(x => x.KhoSanPham.IdDonViTinh).Distinct().ToList();
                var mapDv = new Dictionary<Guid, string>();
                foreach (var idDv in idDonVis)
                {
                    var td = TuDienManager.Instance.SelectById(idDv);
                    mapDv[idDv] = td != null ? td.TenTuDien : string.Empty;
                }

                return list.Select(x =>
                {
                    var ten = x.KhoSanPham != null ? x.KhoSanPham.TenSanPham : string.Empty;
                    var dvt = string.Empty;
                    if (x.KhoSanPham != null && mapDv.TryGetValue(x.KhoSanPham.IdDonViTinh, out var dvTen))
                    {
                        dvt = dvTen;
                    }
                    var tonStr = x.SoLuong.ToString("n2");
                    var display = string.IsNullOrWhiteSpace(dvt)
                        ? string.Format("{0} — Tồn: {1}", ten, tonStr)
                        : string.Format("{0} — Tồn: {1} {2}", ten, tonStr, dvt);
                    return new KhoSanPhamTrongKhoOption
                    {
                        IdSanPham = x.IdSanPham,
                        DisplayText = display,
                        TenSanPham = ten,
                        TonKho = x.SoLuong,
                        DonViTinh = dvt,
                        DonGia = x.KhoSanPham != null ? x.KhoSanPham.DonGia : 0,
                        XuatXu = x.KhoSanPham != null ? x.KhoSanPham.XuatXu : string.Empty
                    };
                }).ToList();
            }
        }

        /// <summary>
        /// Lấy danh sách sản phẩm có trong kho (kể cả tồn = 0) phục vụ thẻ kho.
        /// </summary>
        public List<KhoSanPhamTrongKhoOption> LaySanPhamTrongKho(Guid idKho)
        {
            using (var uow = new UnitOfWork())
            {
                var query = uow.Repository<KhoTon>()
                    .Query()
                    .Filter(x => x.IdKho == idKho)
                    .Include(x => x.KhoSanPham);

                var list = query
                    .OrderBy(x => x.OrderBy(y => y.KhoSanPham.TenSanPham))
                    .Get()
                    .ToList();

                return list.Select(x =>
                {
                    var ma = x.KhoSanPham != null ? x.KhoSanPham.MaSanPham : string.Empty;
                    var ten = x.KhoSanPham != null ? x.KhoSanPham.TenSanPham : string.Empty;
                    var display = string.IsNullOrWhiteSpace(ma) ? ten : string.Format("{0} - {1}", ma, ten);
                    return new KhoSanPhamTrongKhoOption
                    {
                        IdSanPham = x.IdSanPham,
                        DisplayText = display
                    };
                }).ToList();
            }
        }

        /// <summary>
        /// Lấy lịch sử nhập/xuất kho (thẻ kho) theo sản phẩm và kho.
        /// Số lượng âm với giao dịch xuất, dương với giao dịch nhập.
        /// </summary>
        public List<TheKhoModel> LayLichSuTheKho(
            Guid idSanPham,
            Guid idKho,
            DateTime? tuNgay,
            DateTime? denNgay,
            int pageIndex,
            int pageSize,
            out int total,
            out decimal tongSoLuong)
        {
            using (var uow = new UnitOfWork())
            {
                var query = uow.Repository<KhoGiaoDichChiTiet>()
                    .Query()
                    .Filter(x => x.IdSanPham == idSanPham
                        && x.KhoGiaoDich != null
                        && !x.KhoGiaoDich.DaXoa
                        && x.KhoGiaoDich.IdKho == idKho
                        && (x.KhoGiaoDich.LoaiGiaoDich == "NHAP" || x.KhoGiaoDich.LoaiGiaoDich == "XUAT"))
                    .Include(x => x.KhoGiaoDich);

                if (tuNgay.HasValue)
                {
                    var t = tuNgay.Value.Date;
                    query = query.Filter(x => x.KhoGiaoDich.NgayTao >= t);
                }
                if (denNgay.HasValue)
                {
                    var d = denNgay.Value.Date.AddDays(1);
                    query = query.Filter(x => x.KhoGiaoDich.NgayTao < d);
                }

                tongSoLuong = query.Get().Sum(x =>
                    x.KhoGiaoDich.LoaiGiaoDich == "XUAT" ? -x.SoLuong : x.SoLuong);

                var pageItems = query
                    .OrderBy(x => x.OrderBy(y => y.KhoGiaoDich.NgayTao).ThenBy(y => y.KhoGiaoDich.MaGiaoDich))
                    .GetPage(pageIndex, pageSize, out total);

                return pageItems.Select(x =>
                {
                    var laXuat = x.KhoGiaoDich.LoaiGiaoDich == "XUAT";
                    return new TheKhoModel
                    {
                        IdChiTiet = x.Id,
                        IdGiaoDich = x.IdGiaoDich,
                        MaGiaoDich = x.KhoGiaoDich.MaGiaoDich,
                        NgayGiaoDich = x.KhoGiaoDich.NgayTao,
                        LoaiGiaoDich = x.KhoGiaoDich.LoaiGiaoDich,
                        SoLuong = x.SoLuong,
                        SoLuongHienThi = laXuat ? -x.SoLuong : x.SoLuong
                    };
                }).ToList();
            }
        }

        public List<BaoCaoXuatNhapTonModel> BaoCaoXuatNhapTon(
            DateTime? tuNgay,
            DateTime? denNgay,
            Guid? idKho,
            string maSanPham,
            string tenSanPham,
            int pageIndex,
            int pageSize,
            out int total)
        {
            using (var uow = new UnitOfWork())
            {
                maSanPham = string.IsNullOrWhiteSpace(maSanPham) ? null : maSanPham.Trim();
                tenSanPham = string.IsNullOrWhiteSpace(tenSanPham) ? null : tenSanPham.Trim();

                var spQuery = uow.Repository<KhoSanPham>()
                    .Query()
                    .Filter(x =>
                        (string.IsNullOrEmpty(maSanPham) || (x.MaSanPham != null && x.MaSanPham.Contains(maSanPham)))
                        && (string.IsNullOrEmpty(tenSanPham) || (x.TenSanPham != null && x.TenSanPham.Contains(tenSanPham))));

                var dsSanPham = spQuery
                    .OrderBy(x => x.OrderBy(y => y.TenSanPham))
                    .GetPage(pageIndex, pageSize, out total)
                    .ToList();

                if (dsSanPham.Count == 0)
                {
                    return new List<BaoCaoXuatNhapTonModel>();
                }

                var tuNgayDate = tuNgay.HasValue ? tuNgay.Value.Date : (DateTime?)null;
                var denNgayDate = denNgay.HasValue ? denNgay.Value.Date.AddDays(1) : (DateTime?)null;
                var ids = dsSanPham.Select(x => x.Id).ToList();

                var chiTietQuery = uow.Repository<KhoGiaoDichChiTiet>()
                    .Query()
                    .Filter(x => ids.Contains(x.IdSanPham)
                        && x.KhoGiaoDich != null
                        && !x.KhoGiaoDich.DaXoa
                        && (!idKho.HasValue || x.KhoGiaoDich.IdKho == idKho.Value)
                        && (x.KhoGiaoDich.LoaiGiaoDich == "NHAP" || x.KhoGiaoDich.LoaiGiaoDich == "XUAT"))
                    .Include(x => x.KhoGiaoDich);

                if (denNgayDate.HasValue)
                {
                    var den = denNgayDate.Value;
                    chiTietQuery = chiTietQuery.Filter(x => x.KhoGiaoDich.NgayTao < den);
                }

                var dsChiTiet = chiTietQuery.Get().ToList();
                var mapTheoSanPham = dsChiTiet.GroupBy(x => x.IdSanPham).ToDictionary(g => g.Key, g => g.ToList());

                return dsSanPham.Select(sp =>
                {
                    var tonDauKy = 0m;
                    var nhapTrongKy = 0m;
                    var xuatTrongKy = 0m;

                    if (mapTheoSanPham.TryGetValue(sp.Id, out var chiTietSanPham))
                    {
                        foreach (var ct in chiTietSanPham)
                        {
                            var ngayGd = ct.KhoGiaoDich.NgayTao;
                            var laNhap = ct.KhoGiaoDich.LoaiGiaoDich == "NHAP";
                            var laXuat = ct.KhoGiaoDich.LoaiGiaoDich == "XUAT";

                            var laTruocKy = tuNgayDate.HasValue && ngayGd < tuNgayDate.Value;
                            var trongKhoangTuNgay = !tuNgayDate.HasValue || ngayGd >= tuNgayDate.Value;
                            var trongKhoangDenNgay = !denNgayDate.HasValue || ngayGd < denNgayDate.Value;
                            var laTrongKy = trongKhoangTuNgay && trongKhoangDenNgay;

                            if (laTruocKy)
                            {
                                tonDauKy += laNhap ? ct.SoLuong : -ct.SoLuong;
                            }

                            if (laTrongKy)
                            {
                                if (laNhap)
                                {
                                    nhapTrongKy += ct.SoLuong;
                                }
                                else if (laXuat)
                                {
                                    xuatTrongKy += ct.SoLuong;
                                }
                            }
                        }
                    }

                    return new BaoCaoXuatNhapTonModel
                    {
                        IdSanPham = sp.Id,
                        MaSanPham = sp.MaSanPham,
                        TenSanPham = sp.TenSanPham,
                        TonDauKy = tonDauKy,
                        NhapTrongKy = nhapTrongKy,
                        XuatTrongKy = xuatTrongKy,
                        TonCuoiKy = tonDauKy + nhapTrongKy - xuatTrongKy
                    };
                }).ToList();
            }
        }
        #endregion
    }
}

