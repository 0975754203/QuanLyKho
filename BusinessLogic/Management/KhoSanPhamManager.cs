using BusinessLogic.Model;
using BusinessLogic.Utils;
using SQLDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;

namespace BusinessLogic.Management
{
    public class KhoSanPhamManager
    {
        #region Singleton
        private static KhoSanPhamManager _instance;
        private KhoSanPhamManager() { }
        public static KhoSanPhamManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new KhoSanPhamManager();
                }
                return _instance;
            }
        }
        #endregion

        #region Public Interface
        public KhoSanPhamModel SelectById(Guid id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var taikhoan = uow.Repository<KhoSanPham>().Query().Filter(x => x.Id == id).FirstOrDefault();
                var result = taikhoan.CopyAs<KhoSanPhamModel>();
                result.sDonViTinh = taikhoan.TuDien.TenTuDien;
                if (taikhoan.NhomVatTu != null)
                    result.TenNhomVatTu = taikhoan.NhomVatTu.TenNhom;
                return result;
            }
        }
        public KhoSanPhamModel SelectByCode(string MaSanPham)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var taikhoan = uow.Repository<KhoSanPham>().Query().Filter(x => x.MaSanPham == MaSanPham).FirstOrDefault();
                if (taikhoan != null)
                {
                    var result = taikhoan.CopyAs<KhoSanPhamModel>();
                    result.sDonViTinh = taikhoan.TuDien.TenTuDien;
                    return result;
                }
                return null;
            }
        }

        public string SaveOrUpdate(KhoSanPhamModel values)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var taikhoan = uow.Repository<KhoSanPham>().Query().Filter(x => x.Id == values.Id).FirstOrDefault();
                if (taikhoan != null)
                {
                    taikhoan.TenSanPham = values.TenSanPham;
                    //taikhoan.MaSanPham = values.MaSanPham;
                    taikhoan.GhiChu = values.GhiChu;
                    taikhoan.IdDonViTinh = values.IdDonViTinh;
                    taikhoan.IdNhomVatTu = values.IdNhomVatTu;
                    taikhoan.XuatXu = values.XuatXu == null ? "" : values.XuatXu.Trim();
                    taikhoan.State = EDataState.Modified;
                    uow.Repository<KhoSanPham>().InsertOrUpdate(taikhoan);
                    uow.Save();
                }
                else
                {
                    var objUserAdd = values.CopyAs<KhoSanPham>();
                    objUserAdd.Id = Guid.NewGuid();
                    objUserAdd.State = EDataState.Added;
                    objUserAdd.TenSanPham = values.TenSanPham;
                    objUserAdd.MaSanPham = GenCodeSaPham(uow);
                    objUserAdd.IdDonViTinh = values.IdDonViTinh;
                    objUserAdd.IdNhomVatTu = values.IdNhomVatTu;
                    objUserAdd.XuatXu = values.XuatXu == null ? "" : values.XuatXu.Trim();
                    objUserAdd.GhiChu = values.GhiChu;
                    uow.Repository<KhoSanPham>().InsertOrUpdate(objUserAdd);
                    uow.Save();
                }
                return "";
            }
        }
        public string GenCodeSaPham(UnitOfWork uow)
        {
            var spLast = uow.Repository<KhoSanPham>().Query().OrderBy(x => x.OrderByDescending(y => y.MaSanPham)).FirstOrDefault();
            int number = 0;
            if (spLast != null)
            {
                var code = spLast.MaSanPham;
                int.TryParse(code.Substring(2), out number);
                string numberCode = (number + 1).ToString("000000");
                return "SP" + numberCode;
            }
            else
            {
                string numberCode = "000001";
                return "SP" + numberCode;
            }
        }

        public List<KhoSanPhamModel> Search(string sSearch, Guid? idNhomVatTu, int pageIndex, int pageSize, out int pageTotal)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var query = uow.Repository<KhoSanPham>().Query().Filter(x => sSearch == null || sSearch == ""
                    || (x.TenSanPham.Contains(sSearch)) || x.MaSanPham.Contains(sSearch));
                if (idNhomVatTu.HasValue)
                    query = query.Filter(x => x.IdNhomVatTu != null && x.IdNhomVatTu == idNhomVatTu.Value);
                IEnumerable<KhoSanPham> lstSP = null;
                if (pageIndex != -1)
                {
                    lstSP = query.OrderBy(x => x.OrderBy(y => y.MaSanPham)).GetPage(pageIndex, pageSize, out pageTotal);
                }
                else
                {
                    lstSP = query.OrderBy(x => x.OrderBy(y => y.MaSanPham)).Get();
                    pageTotal = lstSP.Count();
                }
                return lstSP.Select(x =>
                {
                    var item = x.CopyAs<KhoSanPhamModel>();
                    item.sDonViTinh = x.TuDien.TenTuDien;
                    if (x.NhomVatTu != null)
                        item.TenNhomVatTu = x.NhomVatTu.TenNhom;
                    return item;
                }).ToList();
            }
        }

        #endregion
    }
}
