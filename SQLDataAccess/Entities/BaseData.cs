using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataAccess
{
    public abstract class BaseData
    {
        public EDataState State { get; set; }
    }

    public partial class KhoGiaoDich : BaseData
    {
    }
    public partial class KhoGiaoDichChiTiet : BaseData
    {
    }
    public partial class KhoSanPham : BaseData
    {
    }
    public partial class KhoTon : BaseData
    {
    }
    public partial class TaiKhoan : BaseData
    {
    }
    public partial class TuDien : BaseData
    {
    }
    public partial class TuDienLoai : BaseData
    {
    }
    public partial class Kho : BaseData
    {
    }
    public partial class NhomVatTu : BaseData
    {
    }
    public partial class NhaThauCungCap : BaseData
    {
    }
}
