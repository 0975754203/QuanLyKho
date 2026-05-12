using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Model
{
    public class TaiKhoanModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Pass { get; set; }
        public string RePass { get; set; }
        public string Role { get; set; }

    }
}
