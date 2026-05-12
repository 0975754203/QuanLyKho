using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuanLyKho.Models
{
    public class DangNhapModel : IValidatableObject
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string PasswordNew { get; set; }
        public string PasswordNewComfirm { get; set; }



        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(UserName))
                yield return new ValidationResult("Vui lòng nhập tên đăng nhập", new[] { nameof(UserName) });
            if (string.IsNullOrEmpty(Password))
                yield return new ValidationResult("Vui lòng nhập mật khẩu", new[] { nameof(Password) });

        }
    }
}