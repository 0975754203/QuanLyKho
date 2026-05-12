using BusinessLogic;
using BusinessLogic.Management;
using BusinessLogic.Model;
using BusinessLogic.Utils;
using SQLDataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuanLyKho.Models
{
    public class TaiKhoanPhongForm : IValidatableObject
    {
        public Guid? Id { get; set; }
        public string UserName { get; set; }
        public string Pass { get; set; }
        public string RePass { get; set; }

        public Guid? IdDonVi { get; set; }
        public bool TrangThai { get; set; }
        public string LoaiTruong { get; set; }
        public string DiaChi { get; set; }
        public string Email { get; set; }
        public string DienThoai { get; set; }
        //Extensions
        public string TenDonVi { get; set; }
        public string MaDonVi { get; set; }
        public string TruongDonVi { get; set; }
        public int Quyen { get; set; }//1: Trường; 2: Phòng; 3: Sở
        public Guid? IdCapTren { get; set; }
        public Guid? IDHuyen { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(MaDonVi))
                yield return new ValidationResult("Vui lòng nhập mã phòng", new[] { nameof(MaDonVi) });

            if (string.IsNullOrEmpty(TenDonVi))
                yield return new ValidationResult("Vui lòng nhập tên phòng giáo dục", new[] { nameof(TenDonVi) });

            if (string.IsNullOrEmpty(TruongDonVi))
                yield return new ValidationResult("Vui lòng nhập tên trưởng phòng", new[] { nameof(TruongDonVi) });

            if (!IDHuyen.IsNotNull())
                yield return new ValidationResult("Vui lòng chọn huyện/ thành phố ", new[] { nameof(IDHuyen) });

            if (string.IsNullOrEmpty(UserName))
                yield return new ValidationResult("Vui lòng nhập tên đăng nhập", new[] { nameof(UserName) });
            else
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    var userCheck = uow.Repository<TaiKhoan>().Query().Filter(x => x.UserName == UserName && (Id == null || x.Id != Id)).FirstOrDefault();
                    if (userCheck != null)
                    {
                        yield return new ValidationResult("Tên đăng nhập đã tồn tại.", new[] { nameof(UserName) });
                    }
                }
            }



            if (string.IsNullOrEmpty(Id.ToString()))
            {
                if (string.IsNullOrEmpty(Pass))
                    yield return new ValidationResult("Vui lòng nhập mật khẩu", new[] { nameof(Pass) });
                else
                {
                    if (!Pass.Equals(RePass))
                    {
                        yield return new ValidationResult("Mật khẩu không trùng nhau", new[] { nameof(RePass) });
                    }
                }

            }

        }
    }


    public class TaiKhoanTruongForm : IValidatableObject
    {
        public Guid? Id { get; set; }
        public string UserName { get; set; }
        public string Pass { get; set; }
        public string RePass { get; set; }

        public Guid? IdDonVi { get; set; }
        public bool TrangThai { get; set; }
        public string LoaiTruong { get; set; }
        public string DonViChuQuan { get; set; }
        public bool IsChuyen { get; set; }
        public string DiaChi { get; set; }
        public string Email { get; set; }
        public string DienThoai { get; set; }
        //Extensions
        public string TenDonVi { get; set; }
        public string MaDonVi { get; set; }
        public string TruongDonVi { get; set; }
        public int Quyen { get; set; }//1: Trường; 2: Phòng; 3: Sở
        public Guid? IdCapTren { get; set; }
        public string IDHuyen { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (string.IsNullOrEmpty(UserName))
                yield return new ValidationResult("Vui lòng nhập tên đăng nhập", new[] { nameof(UserName) });
            else
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    var userCheck = uow.Repository<TaiKhoan>().Query().Filter(x => x.UserName == UserName && (Id == null || x.Id != Id)).FirstOrDefault();
                    if (userCheck != null)
                    {
                        yield return new ValidationResult("Tên đăng nhập đã tồn tại.", new[] { nameof(UserName) });
                    }
                }
            }
            if (string.IsNullOrEmpty(Pass))
                yield return new ValidationResult("Vui lòng nhập mật khẩu", new[] { nameof(Pass) });
            else
            {
                if (!Pass.Equals(RePass))
                {
                    yield return new ValidationResult("Mật khẩu không trùng nhau", new[] { nameof(RePass) });
                }
            }
        }
    }

    public class TaiKhoanForm : IValidatableObject
    {
        public Guid? Id { get; set; }
        public string UserName { get; set; }
        public string Pass { get; set; }
        public string RePass { get; set; }

        public bool IsChuyen { get; set; }
        public string Role { get; set; }//1: Trường; 2: Phòng; 3: Sở
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (string.IsNullOrEmpty(UserName))
                yield return new ValidationResult("Vui lòng nhập tên đăng nhập", new[] { nameof(UserName) });
            else
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    var userCheck = uow.Repository<TaiKhoan>().Query().Filter(x => x.UserName == UserName && (Id == null || x.Id != Id)).FirstOrDefault();
                    if (userCheck != null)
                    {
                        yield return new ValidationResult("Tên đăng nhập đã tồn tại.", new[] { nameof(UserName) });
                    }
                }
            }



            if (string.IsNullOrEmpty(Id.ToString()))
            {
                if (string.IsNullOrEmpty(Pass))
                    yield return new ValidationResult("Vui lòng nhập mật khẩu", new[] { nameof(Pass) });
                else
                {
                    if (!Pass.Equals(RePass))
                    {
                        yield return new ValidationResult("Mật khẩu không trùng nhau", new[] { nameof(RePass) });
                    }
                }

            }

        }
    }
    public class TaiKhoan_DSModel
    {
        public TaiKhoan_DSModel(int pageIndex, int pagecount, int pageSize, int total, IList<TaiKhoanModel> tbl_TaiKhoan)
        {
            PageIndex = pageIndex;
            Pagecount = pagecount;
            Total = total;
            PageSize = pageSize;
            TBL_TaiKhoan = tbl_TaiKhoan;
        }

        public int PageIndex { get; set; }
        public int Pagecount { get; set; }
        public int Total { get; set; }
        public int PageSize { get; private set; }

        public IList<TaiKhoanModel> TBL_TaiKhoan { get; set; }
    }
}

