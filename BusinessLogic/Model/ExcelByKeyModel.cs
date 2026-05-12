using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Model
{
    public class ExcelByKeyModel
    {
        public string Key { get; set; }
        public IEnumerable<object> lstData{ get; set; }
    }
}
