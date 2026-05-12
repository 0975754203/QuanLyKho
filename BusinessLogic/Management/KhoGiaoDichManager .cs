using BusinessLogic.Model;
using BusinessLogic.Utils;
using DocumentFormat.OpenXml.VariantTypes;
using Spire.Xls;
using SQLDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;

namespace BusinessLogic.Management
{
    public class KhoGiaoDichManager
    {
        #region Singleton
        private static KhoGiaoDichManager _instance;
        private KhoGiaoDichManager() { }
        public static KhoGiaoDichManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new KhoGiaoDichManager();
                }
                return _instance;
            }
        }
        #endregion

        #region Public Interface
        public KhoGiaoDichModel SelectById(Guid id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var data = uow.Repository<KhoGiaoDich>().Query().Filter(x => x.Id == id).Include(x => x.KhoGiaoDichChiTiets).Include(x => x.Kho).Include(x => x.TuDien).FirstOrDefault();
                if (data == null)
                {
                    return null;
                }
                var result = data.CopyAs<KhoGiaoDichModel>();
                if (data.Kho != null)
                {
                    result.sTenKho = data.Kho.TenKho;
                }
                if (data.TuDien != null)
                {
                    result.sTenKhoaPhong = data.TuDien.TenTuDien;
                }
                result.lstChiTiet = data.KhoGiaoDichChiTiets.Select(x =>
                {
                    var chitiet = new KhoGiaoDichChiTietModel();
                    chitiet.IdSanPham = x.IdSanPham;
                    chitiet.IdGiaoDich = x.IdGiaoDich;
                    chitiet.SoLuong = x.SoLuong;
                    chitiet.MaSanPham = x.KhoSanPham != null ? x.KhoSanPham.MaSanPham : null;
                    chitiet.TenSanPham = x.KhoSanPham != null ? x.KhoSanPham.TenSanPham : null;
                    chitiet.sDonVi = (x.KhoSanPham != null && x.KhoSanPham.TuDien != null) ? x.KhoSanPham.TuDien.TenTuDien : null;
                    return chitiet;
                }).ToList();
                return result;
            }
        }

        public string SaveOrUpdate(KhoGiaoDichModel values)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                if (values.LoaiGiaoDich == "XUAT" && values.IdKho.IsNotNull())
                {
                    if (values.IdKhoNhan.HasValue && values.IdKhoNhan.Value != Guid.Empty
                        && values.IdKhoNhan.Value == values.IdKho.Value)
                    {
                        return "Kho nhận phải khác kho xuất.";
                    }

                    var idKho = values.IdKho.Value;
                    foreach (var item in values.lstChiTiet)
                    {
                        var tonChk = uow.Repository<KhoTon>().Query().Filter(x => x.IdSanPham == item.IdSanPham && x.IdKho == idKho).FirstOrDefault();
                        if (tonChk == null)
                        {
                            return "Sản phẩm không có tồn trong kho được chọn.";
                        }
                        if (tonChk.SoLuong < item.SoLuong)
                        {
                            return string.Format("Không đủ hàng trong kho để xuất. Tồn hiện có: {0:n2}, số lượng xuất: {1:n2}.", tonChk.SoLuong, item.SoLuong);
                        }
                    }
                }

                var objUserAdd = values.CopyAs<KhoGiaoDich>();
                objUserAdd.Id = Guid.NewGuid();
                objUserAdd.State = EDataState.Added;
                objUserAdd.LoaiGiaoDich = values.LoaiGiaoDich;
                objUserAdd.MaGiaoDich = GenCodeGiaoDich(uow, values.LoaiGiaoDich);
                objUserAdd.NgayTao = DateTime.Now;
                objUserAdd.IdNguoiTao = values.IdNguoiTao;
                objUserAdd.IdKhoaPhong = values.IdKhoaPhong.IsNotNull() ? values.IdKhoaPhong : null;
                objUserAdd.IdKho = values.IdKho.IsNotNull() ? values.IdKho : null;
                objUserAdd.IdGiaoDichCha = null;
                objUserAdd.GhiChu = values.GhiChu;
                objUserAdd.NguoiGiaoNhan = values.NguoiGiaoNhan;
                objUserAdd.DaXoa = false;
                uow.Repository<KhoGiaoDich>().InsertOrUpdate(objUserAdd);
                foreach (var item in values.lstChiTiet)
                {
                    var itemChiTiet = item.CopyAs<KhoGiaoDichChiTiet>();
                    itemChiTiet.Id = Guid.NewGuid();
                    itemChiTiet.IdGiaoDich = objUserAdd.Id;
                    itemChiTiet.State = EDataState.Added;
                    uow.Repository<KhoGiaoDichChiTiet>().InsertOrUpdate(itemChiTiet);
                    KhoTon tonkho;
                    if (values.IdKho.IsNotNull())
                    {
                        var idKho = values.IdKho.Value;
                        tonkho = uow.Repository<KhoTon>().Query().Filter(x => x.IdSanPham == item.IdSanPham && x.IdKho == idKho).FirstOrDefault();
                    }
                    else
                    {
                        tonkho = uow.Repository<KhoTon>().Query().Filter(x => x.IdSanPham == item.IdSanPham).FirstOrDefault();
                    }
                    if (tonkho != null)
                    {
                        tonkho.SoLuong += values.LoaiGiaoDich == "NHAP" ? item.SoLuong : -item.SoLuong;
                        tonkho.State = EDataState.Modified;
                        if (tonkho.SoLuong < 0)
                        {
                            return "Không đủ hàng trong kho để xuất.";
                        }
                        uow.Repository<KhoTon>().InsertOrUpdate(tonkho);
                    }
                    else
                    {
                        if (values.LoaiGiaoDich == "XUAT")
                        {
                            return "Sản phẩm không có tồn trong kho được chọn.";
                        }
                        if (!values.IdKho.IsNotNull())
                        {
                            return "Vui lòng chọn kho để cập nhật tồn.";
                        }
                        tonkho = new KhoTon();
                        tonkho.Id = Guid.NewGuid();
                        tonkho.IdSanPham = item.IdSanPham;
                        tonkho.IdKho = values.IdKho.Value;
                        tonkho.SoLuong = item.SoLuong;
                        tonkho.State = EDataState.Added;
                        uow.Repository<KhoTon>().InsertOrUpdate(tonkho);
                    }
                }

                // Tạo giao dịch nhập kho đi kèm khi xuất kho có chọn kho nhận
                if (values.LoaiGiaoDich == "XUAT" && values.IdKhoNhan.HasValue && values.IdKhoNhan.Value != Guid.Empty)
                {
                    var idKhoNhan = values.IdKhoNhan.Value;
                    var nhap = new KhoGiaoDich
                    {
                        Id = Guid.NewGuid(),
                        State = EDataState.Added,
                        LoaiGiaoDich = "NHAP",
                        MaGiaoDich = GenCodeGiaoDich(uow, "NHAP"),
                        NgayTao = DateTime.Now,
                        IdNguoiTao = values.IdNguoiTao,
                        IdKhoaPhong = values.IdKhoaPhong.IsNotNull() ? values.IdKhoaPhong : null,
                        IdKho = idKhoNhan,
                        IdGiaoDichCha = objUserAdd.Id,
                        GhiChu = values.GhiChu,
                        NguoiGiaoNhan = values.NguoiGiaoNhan,
                        DaXoa = false
                    };
                    uow.Repository<KhoGiaoDich>().InsertOrUpdate(nhap);

                    foreach (var item in values.lstChiTiet)
                    {
                        var ct = new KhoGiaoDichChiTiet
                        {
                            Id = Guid.NewGuid(),
                            State = EDataState.Added,
                            IdGiaoDich = nhap.Id,
                            IdSanPham = item.IdSanPham,
                            SoLuong = item.SoLuong
                        };
                        uow.Repository<KhoGiaoDichChiTiet>().InsertOrUpdate(ct);

                        var tonNhan = uow.Repository<KhoTon>().Query()
                            .Filter(x => x.IdSanPham == item.IdSanPham && x.IdKho == idKhoNhan)
                            .FirstOrDefault();
                        if (tonNhan != null)
                        {
                            tonNhan.SoLuong += item.SoLuong;
                            tonNhan.State = EDataState.Modified;
                            uow.Repository<KhoTon>().InsertOrUpdate(tonNhan);
                        }
                        else
                        {
                            var tonNew = new KhoTon
                            {
                                Id = Guid.NewGuid(),
                                IdSanPham = item.IdSanPham,
                                IdKho = idKhoNhan,
                                SoLuong = item.SoLuong,
                                State = EDataState.Added
                            };
                            uow.Repository<KhoTon>().InsertOrUpdate(tonNew);
                        }
                    }
                }

                uow.Save();

                return "";
            }
        }
        public string XoaGiaoDich(Guid idGiaoDich)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var gd = uow.Repository<KhoGiaoDich>().Query().Filter(x => x.Id == idGiaoDich).FirstOrDefault();
                if (gd == null)
                {
                    return "Không tìm thấy giao dịch.";
                }
                if (gd.DaXoa)
                {
                    return "Giao dịch đã được xóa trước đó.";
                }

                // Không cho xóa trực tiếp giao dịch nhập kho đi kèm với giao dịch xuất
                if (gd.LoaiGiaoDich == "NHAP" && gd.IdGiaoDichCha.HasValue)
                {
                    return "Giao dịch nhập này được tạo từ phiếu xuất, không thể xóa trực tiếp. Vui lòng xóa phiếu xuất tương ứng.";
                }

                // Nếu là giao dịch xuất, kiểm tra & gom các giao dịch nhập đi kèm
                var dsNhapCon = new List<KhoGiaoDich>();
                if (gd.LoaiGiaoDich == "XUAT")
                {
                    dsNhapCon = uow.Repository<KhoGiaoDich>().Query()
                        .Filter(x => x.IdGiaoDichCha == idGiaoDich && !x.DaXoa && x.LoaiGiaoDich == "NHAP")
                        .Get()
                        .ToList();

                    foreach (var nhap in dsNhapCon)
                    {
                        if (!nhap.IdKho.HasValue)
                        {
                            continue;
                        }
                        var idKhoNhan = nhap.IdKho.Value;
                        foreach (var item in nhap.KhoGiaoDichChiTiets)
                        {
                            var ton = uow.Repository<KhoTon>().Query()
                                .Filter(x => x.IdSanPham == item.IdSanPham && x.IdKho == idKhoNhan)
                                .FirstOrDefault();
                            var tonHienCo = ton == null ? 0m : ton.SoLuong;
                            if (tonHienCo < item.SoLuong)
                            {
                                var tenSp = item.KhoSanPham != null ? item.KhoSanPham.TenSanPham : item.IdSanPham.ToString();
                                return string.Format(
                                    "Không đủ hàng tại kho nhận để hủy giao dịch. Sản phẩm: {0}. Tồn hiện có: {1:n2}, cần trừ: {2:n2}.",
                                    tenSp, tonHienCo, item.SoLuong);
                            }
                        }
                    }
                }

                // Xử lý xóa các giao dịch nhập kho đi kèm trước
                foreach (var nhap in dsNhapCon)
                {
                    nhap.DaXoa = true;
                    nhap.State = EDataState.Modified;
                    if (nhap.IdKho.HasValue)
                    {
                        var idKhoNhan = nhap.IdKho.Value;
                        foreach (var item in nhap.KhoGiaoDichChiTiets)
                        {
                            var ton = uow.Repository<KhoTon>().Query()
                                .Filter(x => x.IdSanPham == item.IdSanPham && x.IdKho == idKhoNhan)
                                .FirstOrDefault();
                            if (ton != null)
                            {
                                ton.SoLuong -= item.SoLuong;
                                ton.State = EDataState.Modified;
                                if (ton.SoLuong < 0)
                                {
                                    return "Không đủ hàng tại kho nhận để hủy giao dịch.";
                                }
                                uow.Repository<KhoTon>().InsertOrUpdate(ton);
                            }
                        }
                    }
                    uow.Repository<KhoGiaoDich>().InsertOrUpdate(nhap);
                }

                // Sau đó xử lý xóa giao dịch chính
                gd.DaXoa = true;
                gd.State = EDataState.Modified;
                foreach (var item in gd.KhoGiaoDichChiTiets)
                {
                    KhoTon tonkho;
                    if (gd.IdKho.HasValue)
                    {
                        tonkho = uow.Repository<KhoTon>().Query().Filter(x => x.IdSanPham == item.IdSanPham && x.IdKho == gd.IdKho.Value).FirstOrDefault();
                    }
                    else
                    {
                        tonkho = uow.Repository<KhoTon>().Query().Filter(x => x.IdSanPham == item.IdSanPham).FirstOrDefault();
                    }
                    if (tonkho != null)
                    {
                        tonkho.SoLuong += gd.LoaiGiaoDich == "NHAP" ? -item.SoLuong : item.SoLuong;
                        tonkho.State = EDataState.Modified;
                        if (tonkho.SoLuong < 0)
                        {
                            return "Không hủy được giao dịch do không đủ số lượng tồn kho.";
                        }
                        uow.Repository<KhoTon>().InsertOrUpdate(tonkho);
                    }
                }
                uow.Repository<KhoGiaoDich>().InsertOrUpdate(gd);
                uow.Save();

                return "";
            }
        }
        public string GenCodeGiaoDich(UnitOfWork uow, string sLoaiGiaoDich)
        {
            string date = DateTime.Now.ToString("yyyyMMdd");
            var str = sLoaiGiaoDich.ToUpperInvariant() + date;
            var spLast = uow.Repository<KhoGiaoDich>().Query().Filter(x => x.MaGiaoDich.Contains(str))
                .OrderBy(x => x.OrderByDescending(y => y.MaGiaoDich)).FirstOrDefault();
            int number = 0;
            if (spLast != null)
            {
                var code = spLast.MaGiaoDich;
                int.TryParse(code.Substring(sLoaiGiaoDich.Length + 8), out number);
                string numberCode = (number + 1).ToString("000000");
                return str + numberCode;
            }
            else
            {
                string numberCode = "000001";
                return str + numberCode;
            }
        }
        public List<KhoGiaoDichModel> Search(string sSearch, DateTime TuNgay, DateTime DenNgay, int pageIndex, int pageSize, out int pageTotal)
        {
            DenNgay = DenNgay.AddDays(1);
            using (UnitOfWork uow = new UnitOfWork())
            {
                var query = uow.Repository<KhoGiaoDich>().Query().Filter(x => (sSearch == null || sSearch == ""
                    || (x.MaGiaoDich.Contains(sSearch))) && x.NgayTao >= TuNgay && x.NgayTao < DenNgay).Include(x => x.TuDien);
                IEnumerable<KhoGiaoDich> lstSP = null;
                if (pageIndex != -1)
                {
                    lstSP = query.OrderBy(x => x.OrderBy(y => y.MaGiaoDich)).GetPage(pageIndex, pageSize, out pageTotal);
                }
                else
                {
                    lstSP = query.OrderBy(x => x.OrderBy(y => y.MaGiaoDich)).Get();
                    pageTotal = lstSP.Count();
                }
                return lstSP.Select(x =>
                {
                    var item = x.CopyAs<KhoGiaoDichModel>();
                    item.sTenKhoaPhong = x.TuDien.TenTuDien;
                    return item;
                }).ToList();
            }
        }

        public List<KhoGiaoDichModel> SearchNhapKho(string sSearch, Guid? idKho, DateTime? tuNgay, DateTime? denNgay, int pageIndex, int pageSize, out int pageTotal)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var q = uow.Repository<KhoGiaoDich>()
                    .Query()
                    .Filter(x => !x.DaXoa
                        && x.LoaiGiaoDich == "NHAP"
                        && (string.IsNullOrEmpty(sSearch) || x.MaGiaoDich.Contains(sSearch)));
                if (idKho.HasValue && idKho.Value != Guid.Empty)
                {
                    var idKhoValue = idKho.Value;
                    q = q.Filter(x => x.IdKho.HasValue && x.IdKho.Value == idKhoValue);
                }
                if (tuNgay.HasValue)
                {
                    var t = tuNgay.Value.Date;
                    q = q.Filter(x => x.NgayTao >= t);
                }
                if (denNgay.HasValue)
                {
                    var dEnd = denNgay.Value.Date.AddDays(1);
                    q = q.Filter(x => x.NgayTao < dEnd);
                }
                var query = q.Include(x => x.TuDien).Include(x => x.Kho);

                IEnumerable<KhoGiaoDich> list = query
                    .OrderBy(x => x.OrderByDescending(y => y.NgayTao))
                    .GetPage(pageIndex, pageSize, out pageTotal);

                return list.Select(x => new KhoGiaoDichModel
                {
                    Id = x.Id,
                    MaGiaoDich = x.MaGiaoDich,
                    NgayTao = x.NgayTao,
                    IdKhoaPhong = x.IdKhoaPhong,
                    IdGiaoDichCha = x.IdGiaoDichCha,
                    sTenKho = x.Kho != null ? x.Kho.TenKho : string.Empty,
                    sTenKhoaPhong = x.TuDien != null ? x.TuDien.TenTuDien : string.Empty
                }).ToList();
            }
        }

        public List<KhoGiaoDichModel> SearchXuatKho(string sSearch, Guid? idKho, DateTime? tuNgay, DateTime? denNgay, int pageIndex, int pageSize, out int pageTotal)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var q = uow.Repository<KhoGiaoDich>()
                    .Query()
                    .Filter(x => !x.DaXoa
                        && x.LoaiGiaoDich == "XUAT"
                        && (string.IsNullOrEmpty(sSearch) || x.MaGiaoDich.Contains(sSearch)));
                if (idKho.HasValue && idKho.Value != Guid.Empty)
                {
                    var idKhoValue = idKho.Value;
                    q = q.Filter(x => x.IdKho.HasValue && x.IdKho.Value == idKhoValue);
                }
                if (tuNgay.HasValue)
                {
                    var t = tuNgay.Value.Date;
                    q = q.Filter(x => x.NgayTao >= t);
                }
                if (denNgay.HasValue)
                {
                    var dEnd = denNgay.Value.Date.AddDays(1);
                    q = q.Filter(x => x.NgayTao < dEnd);
                }
                var query = q.Include(x => x.TuDien).Include(x => x.Kho);

                IEnumerable<KhoGiaoDich> list = query
                    .OrderBy(x => x.OrderByDescending(y => y.NgayTao))
                    .GetPage(pageIndex, pageSize, out pageTotal);

                return list.Select(x => new KhoGiaoDichModel
                {
                    Id = x.Id,
                    MaGiaoDich = x.MaGiaoDich,
                    NgayTao = x.NgayTao,
                    IdKhoaPhong = x.IdKhoaPhong,
                    sTenKho = x.Kho != null ? x.Kho.TenKho : string.Empty,
                    sTenKhoaPhong = x.TuDien != null ? x.TuDien.TenTuDien : string.Empty
                }).ToList();
            }
        }

        /// <summary>
        /// Báo cáo xuất kho cho khoa/phòng: các phiếu XUAT có IdKhoaPhong, gom theo khoa/phòng;
        /// mỗi sản phẩm xuất hiện trong kỳ là một cột, ô là số lượng xuất cho khoa đó.
        /// </summary>
        public BaoCaoXuatKhoaPhongKetQua BaoCaoXuatChoKhoaPhong(Guid? idKho, DateTime? tuNgay, DateTime? denNgay, int pageIndex, int pageSize, out int pageTotal)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var q = uow.Repository<KhoGiaoDich>()
                    .Query()
                    .Filter(x => !x.DaXoa
                        && x.LoaiGiaoDich == "XUAT"
                        && x.IdKhoaPhong.HasValue
                        && x.IdKhoaPhong.Value != Guid.Empty);

                if (idKho.HasValue && idKho.Value != Guid.Empty)
                {
                    var idKhoVal = idKho.Value;
                    q = q.Filter(x => x.IdKho.HasValue && x.IdKho.Value == idKhoVal);
                }

                if (tuNgay.HasValue)
                {
                    var t = tuNgay.Value.Date;
                    q = q.Filter(x => x.NgayTao >= t);
                }
                if (denNgay.HasValue)
                {
                    var dEnd = denNgay.Value.Date.AddDays(1);
                    q = q.Filter(x => x.NgayTao < dEnd);
                }

                var query = q
                    .Include(x => x.TuDien)
                    .Include(x => x.Kho)
                    .Include(x => x.KhoGiaoDichChiTiets.Select(ct => ct.KhoSanPham));

                var dsGd = query
                    .OrderBy(x => x.OrderByDescending(y => y.NgayTao))
                    .Get()
                    .ToList();

                var dongGom = dsGd
                    .GroupBy(x => x.IdKhoaPhong.Value)
                    .Select(g =>
                    {
                        var tenKp = g.Select(z => z.TuDien != null ? z.TuDien.TenTuDien : null)
                            .FirstOrDefault(s => !string.IsNullOrWhiteSpace(s));
                        if (string.IsNullOrWhiteSpace(tenKp))
                        {
                            tenKp = "(Chưa có tên khoa phòng)";
                        }

                        var tongSp = new Dictionary<Guid, decimal>();
                        var tenSp = new Dictionary<Guid, string>();

                        foreach (var gd in g)
                        {
                            if (gd.KhoGiaoDichChiTiets == null)
                            {
                                continue;
                            }
                            foreach (var ct in gd.KhoGiaoDichChiTiets)
                            {
                                if (!tongSp.ContainsKey(ct.IdSanPham))
                                {
                                    tongSp[ct.IdSanPham] = 0m;
                                }
                                tongSp[ct.IdSanPham] += ct.SoLuong;

                                var sp = ct.KhoSanPham;
                                if (sp != null && !string.IsNullOrWhiteSpace(sp.TenSanPham))
                                {
                                    tenSp[ct.IdSanPham] = sp.TenSanPham;
                                }
                            }
                        }

                        return new
                        {
                            IdKhoaPhong = g.Key,
                            TenKhoaPhong = tenKp,
                            TongSp = tongSp,
                            TenSp = tenSp
                        };
                    })
                    .OrderBy(x => x.TenKhoaPhong)
                    .ToList();

                var allProductIds = dongGom.SelectMany(x => x.TongSp.Keys).Distinct().ToList();
                var tenCot = new Dictionary<Guid, string>();
                foreach (var idSp in allProductIds)
                {
                    var ten = dongGom
                        .Select(d => d.TenSp.ContainsKey(idSp) ? d.TenSp[idSp] : null)
                        .FirstOrDefault(s => !string.IsNullOrWhiteSpace(s));
                    tenCot[idSp] = string.IsNullOrWhiteSpace(ten) ? idSp.ToString() : ten;
                }

                var cotSanPham = tenCot
                    .OrderBy(kv => kv.Value)
                    .Select(kv => new BaoCaoXuatKhoaPhongCotSanPham { IdSanPham = kv.Key, TenCot = kv.Value })
                    .ToList();

                var fullRows = dongGom.Select(d => new BaoCaoXuatKhoaPhongModel
                {
                    IdKhoaPhong = d.IdKhoaPhong,
                    TenKhoaPhong = d.TenKhoaPhong,
                    SoLuongTheoSanPham = cotSanPham.ToDictionary(c => c.IdSanPham, c => d.TongSp.ContainsKey(c.IdSanPham) ? d.TongSp[c.IdSanPham] : 0m),
                    TongSoLuong = d.TongSp.Values.Sum()
                }).ToList();

                pageTotal = fullRows.Count;
                List<BaoCaoXuatKhoaPhongModel> hang;
                if (pageSize <= 0)
                {
                    hang = fullRows;
                }
                else
                {
                    hang = fullRows
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
                }

                return new BaoCaoXuatKhoaPhongKetQua
                {
                    CotSanPham = cotSanPham,
                    Hang = hang
                };
            }
        }

        #endregion
    }
}
