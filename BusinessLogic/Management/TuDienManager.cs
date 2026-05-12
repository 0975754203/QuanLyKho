using BusinessLogic.Model;
using SQLDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Management
{
    public class TuDienManager
    {
        #region Singleton
        private static TuDienManager _instance;
        private TuDienManager() { }
        public static TuDienManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TuDienManager();
                }
                return _instance;
            }
        }
        #endregion

        #region Public Interface
        public TuDienModel SelectById(Guid id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var tudien = uow.Repository<TuDien>().Query().Filter(x => x.Id == id).FirstOrDefault();
                var result = tudien.CopyAs<TuDienModel>();
                return result;
            }
        }
        public List<TuDienModel> SelectByLoai(string sLoaiTuDien)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var lstTuDien = uow.Repository<TuDien>().Query()
                    .Filter(x => x.TuDienLoai.MaLoai == sLoaiTuDien) 
                    .OrderBy(x => x.OrderBy(y => y.TenTuDien))
                    .Get();
                return lstTuDien.Select(x =>
                {
                    return x.CopyAs<TuDienModel>();
                }).ToList();
            }
        }
        public void SaveOrUpdate(TuDienModel values)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var checkTrung = uow.Repository<TuDien>().Query().Filter(x => x.MaTuDien == values.MaTuDien && x.IdLoaiTuDien == values.IdLoaiTuDien && x.Id != values.Id).FirstOrDefault();
                if (checkTrung != null)
                {
                    throw new Exception("Mã từ điển đã tồn tại.");
                }
                var td = uow.Repository<TuDien>().Query().Filter(x => x.Id == values.Id).FirstOrDefault();
                if (td != null)
                {
                    td.MaTuDien = values.MaTuDien;
                    td.TenTuDien = values.TenTuDien;
                    td.GhiChu = values.GhiChu;
                    td.State = EDataState.Modified;
                    uow.Repository<TuDien>().InsertOrUpdate(td);
                    uow.Save();
                }
                else
                {
                    td = new TuDien();
                    td.Id = Guid.NewGuid();
                    td.IdLoaiTuDien = values.IdLoaiTuDien;
                    td.MaTuDien = values.MaTuDien;
                    td.TenTuDien = values.TenTuDien;
                    td.GhiChu = values.GhiChu;
                    td.State = EDataState.Added;
                    uow.Repository<TuDien>().InsertOrUpdate(td);
                    uow.Save();
                }
            }
        }
        public void Delete(Guid id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                uow.Repository<TuDien>().Delete(id);
                uow.Save();
            }
        }
        #endregion
        #region LoaiTuDien
        public TuDienLoaiModel SelectLoaiTuDienById(Guid id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var tudien = uow.Repository<TuDienLoai>().Query().Filter(x => x.Id == id).FirstOrDefault();
                var result = tudien.CopyAs<TuDienLoaiModel>();
                return result;
            }
        }
        public TuDienLoaiModel SelectLoaiTuDienByMa(string sMa)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var tudien = uow.Repository<TuDienLoai>().Query().Filter(x => x.MaLoai == sMa).FirstOrDefault();
                var result = tudien.CopyAs<TuDienLoaiModel>();
                return result;
            }
        }
        #endregion
    }
}
