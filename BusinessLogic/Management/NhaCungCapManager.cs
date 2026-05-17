using BusinessLogic.Model;
using SQLDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.Management
{
    public class NhaCungCapManager
    {
        #region Singleton
        private static NhaCungCapManager _instance;
        private NhaCungCapManager() { }
        public static NhaCungCapManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NhaCungCapManager();
                }
                return _instance;
            }
        }
        #endregion

        public NhaCungCapModel SelectById(Guid id)
        {
            using (var uow = new UnitOfWork())
            {
                var entity = uow.Repository<NhaThauCungCap>().Query()
                    .Filter(x => x.idNhathaucc == id)
                    .FirstOrDefault();

                if (entity == null)
                    return null;

                return new NhaCungCapModel
                {
                    Id = entity.idNhathaucc,
                    TenNhaCungCap = entity.TenNhaThau
                };
            }
        }

        public List<NhaCungCapModel> SearchByPage(string sSearch, int pageIndex, int pageSize, out int total)
        {
            using (var uow = new UnitOfWork())
            {
                var query = uow.Repository<NhaThauCungCap>().Query();
                if (!string.IsNullOrWhiteSpace(sSearch))
                {
                    var term = sSearch.Trim().ToLower();
                    query = query.Filter(x => x.TenNhaThau != null && x.TenNhaThau.ToLower().Contains(term));
                }

                var list = query.OrderBy(q => q.OrderBy(y => y.TenNhaThau))
                    .GetPage(pageIndex, pageSize, out total);

                return list.Select(x => new NhaCungCapModel
                {
                    Id = x.idNhathaucc,
                    TenNhaCungCap = x.TenNhaThau
                }).ToList();
            }
        }

        public string SaveOrUpdate(NhaCungCapModel values)
        {
            if (values == null)
                return "Khong co du lieu de luu.";

            var tenTrim = (values.TenNhaCungCap ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(tenTrim))
                return "Vui long nhap ten nha cung cap.";

            using (var uow = new UnitOfWork())
            {
                var dup = uow.Repository<NhaThauCungCap>().Query().Filter(x =>
                    x.TenNhaThau == tenTrim && (values.Id == Guid.Empty || x.idNhathaucc != values.Id)).FirstOrDefault();
                if (dup != null)
                    return "Ten nha cung cap \"" + tenTrim + "\" da ton tai.";

                var entity = values.Id != Guid.Empty
                    ? uow.Repository<NhaThauCungCap>().Query().Filter(x => x.idNhathaucc == values.Id).FirstOrDefault()
                    : null;

                if (entity != null)
                {
                    entity.TenNhaThau = tenTrim;
                    entity.State = EDataState.Modified;
                    uow.Repository<NhaThauCungCap>().InsertOrUpdate(entity);
                }
                else
                {
                    entity = new NhaThauCungCap
                    {
                        idNhathaucc = Guid.NewGuid(),
                        TenNhaThau = tenTrim,
                        State = EDataState.Added
                    };
                    uow.Repository<NhaThauCungCap>().InsertOrUpdate(entity);
                }

                uow.Save();
                return string.Empty;
            }
        }

        public string Delete(Guid id)
        {
            using (var uow = new UnitOfWork())
            {
                var coGiaoDich = uow.Repository<KhoGiaoDich>().Query()
                    .Filter(x => x.idNhaCungCap != null && x.idNhaCungCap == id && !x.DaXoa)
                    .FirstOrDefault();
                if (coGiaoDich != null)
                    return "Khong the xoa: nha cung cap dang duoc phieu nhap kho su dung.";

                var entity = uow.Repository<NhaThauCungCap>().Query()
                    .Filter(x => x.idNhathaucc == id)
                    .FirstOrDefault();
                if (entity == null)
                    return "Khong tim thay nha cung cap.";

                uow.Repository<NhaThauCungCap>().Delete(entity);
                uow.Save();
                return string.Empty;
            }
        }
    }
}
