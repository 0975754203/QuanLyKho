using BusinessLogic.Model;
using SQLDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.Management
{
    public class KhoManager
    {
        #region Singleton
        private static KhoManager _instance;
        private KhoManager() { }
        public static KhoManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new KhoManager();
                }
                return _instance;
            }
        }
        #endregion

        #region Public Interface
        public KhoModel SelectById(Guid id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var entity = uow.Repository<Kho>().Query().Filter(x => x.Id == id).FirstOrDefault();
                if (entity == null)
                    return null;
                return entity.CopyAs<KhoModel>();
            }
        }

        public List<KhoModel> SearchByPage(string sSearch, int pageIndex, int pageSize, out int total)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var query = uow.Repository<Kho>().Query();
                if (!string.IsNullOrWhiteSpace(sSearch))
                {
                    var term = sSearch.Trim();
                    query = query.Filter(x =>
                        (x.MaKho != null && x.MaKho.Contains(term))
                        || (x.TenKho != null && x.TenKho.Contains(term)));
                }

                var list = query.OrderBy(q => q.OrderBy(y => y.MaKho)).GetPage(pageIndex, pageSize, out total);
                return list.Select(x => x.CopyAs<KhoModel>()).ToList();
            }
        }

        public List<KhoModel> SelectAll()
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                return uow.Repository<Kho>().Query()
                    .OrderBy(q => q.OrderBy(x => x.MaKho))
                    .Get()
                    .Select(x => x.CopyAs<KhoModel>())
                    .ToList();
            }
        }

        public string SaveOrUpdate(KhoModel values)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var entity = values.Id != Guid.Empty
                    ? uow.Repository<Kho>().Query().Filter(x => x.Id == values.Id).FirstOrDefault()
                    : null;

                if (entity != null)
                {
                    entity.MaKho = values.MaKho;
                    entity.TenKho = values.TenKho;
                    entity.DiaChi = values.DiaChi;
                    entity.State = EDataState.Modified;
                    uow.Repository<Kho>().InsertOrUpdate(entity);
                    uow.Save();
                }
                else
                {
                    var add = values.CopyAs<Kho>();
                    add.Id = values.Id == Guid.Empty ? Guid.NewGuid() : values.Id;
                    add.State = EDataState.Added;
                    uow.Repository<Kho>().InsertOrUpdate(add);
                    uow.Save();
                }

                return "";
            }
        }

        public bool Delete(Guid id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var repo = uow.Repository<Kho>();
                if (repo.FindById(id) == null)
                    return false;
                repo.Delete(id);
                uow.Save();
                return true;
            }
        }
        #endregion
    }
}
