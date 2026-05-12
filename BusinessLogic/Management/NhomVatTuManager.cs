using BusinessLogic.Model;
using BusinessLogic.Utils;
using SQLDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.Management
{
    public class NhomVatTuManager
    {
        #region Singleton
        private static NhomVatTuManager _instance;
        private NhomVatTuManager() { }
        public static NhomVatTuManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NhomVatTuManager();
                }
                return _instance;
            }
        }
        #endregion

        public List<NhomVatTuModel> SelectAll()
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                return uow.Repository<NhomVatTu>().Query()
                    .OrderBy(q => q.OrderBy(x => x.MaNhom))
                    .Get()
                    .Select(x => x.CopyAs<NhomVatTuModel>())
                    .ToList();
            }
        }

        public NhomVatTuModel SelectById(Guid id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var entity = uow.Repository<NhomVatTu>().Query().Filter(x => x.Id == id).FirstOrDefault();
                if (entity == null)
                    return null;
                return entity.CopyAs<NhomVatTuModel>();
            }
        }

        public List<NhomVatTuModel> SearchByPage(string sSearch, int pageIndex, int pageSize, out int total)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var query = uow.Repository<NhomVatTu>().Query();
                if (sSearch.IsNotNullOrEmpty())
                {
                    var term = sSearch.Trim().ToLower();
                    query = query.Filter(x =>
                        (x.MaNhom != null && x.MaNhom.ToLower().Contains(term))
                        || (x.TenNhom != null && x.TenNhom.ToLower().Contains(term)));
                }

                var list = query.OrderBy(q => q.OrderBy(y => y.MaNhom)).GetPage(pageIndex, pageSize, out total);
                return list.Select(x => x.CopyAs<NhomVatTuModel>()).ToList();
            }
        }

        public string SaveOrUpdate(NhomVatTuModel values)
        {
            if (values == null)
                return "Không có dữ liệu để lưu.";

            var maTrim = (values.MaNhom ?? "").Trim();
            if (string.IsNullOrEmpty(maTrim))
                return "Vui lòng nhập mã nhóm.";

            using (UnitOfWork uow = new UnitOfWork())
            {
                var dup = uow.Repository<NhomVatTu>().Query().Filter(x =>
                    x.MaNhom == maTrim && (values.Id == Guid.Empty || x.Id != values.Id)).FirstOrDefault();
                if (dup != null)
                    return "Mã nhóm \"" + maTrim + "\" đã tồn tại.";

                var entity = values.Id != Guid.Empty
                    ? uow.Repository<NhomVatTu>().Query().Filter(x => x.Id == values.Id).FirstOrDefault()
                    : null;

                if (entity != null)
                {
                    entity.MaNhom = maTrim;
                    entity.TenNhom = values.TenNhom == null ? "" : values.TenNhom.Trim();
                    entity.State = EDataState.Modified;
                    uow.Repository<NhomVatTu>().InsertOrUpdate(entity);
                    uow.Save();
                }
                else
                {
                    var add = values.CopyAs<NhomVatTu>();
                    add.Id = values.Id == Guid.Empty ? Guid.NewGuid() : values.Id;
                    add.MaNhom = maTrim;
                    add.TenNhom = values.TenNhom == null ? "" : values.TenNhom.Trim();
                    add.State = EDataState.Added;
                    uow.Repository<NhomVatTu>().InsertOrUpdate(add);
                    uow.Save();
                }

                return "";
            }
        }

        public string Delete(Guid id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var coSanPham = uow.Repository<KhoSanPham>().Query()
                    .Filter(x => x.IdNhomVatTu != null && x.IdNhomVatTu == id)
                    .FirstOrDefault();
                if (coSanPham != null)
                    return "Không thể xóa: đang có sản phẩm gán vào nhóm này.";

                var repo = uow.Repository<NhomVatTu>();
                if (repo.FindById(id) == null)
                    return "Không tìm thấy nhóm vật tư.";

                repo.Delete(id);
                uow.Save();
                return "";
            }
        }
    }
}
