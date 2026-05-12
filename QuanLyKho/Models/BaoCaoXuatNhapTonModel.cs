using BusinessLogic.Model;
using System.Collections.Generic;

namespace QuanLyKho.Models
{
    public class BaoCaoXuatNhapTon_DSModel
    {
        public BaoCaoXuatNhapTon_DSModel(int pageIndex, int pagecount, int pageSize, int total, IList<BaoCaoXuatNhapTonModel> data)
        {
            PageIndex = pageIndex;
            Pagecount = pagecount;
            Total = total;
            PageSize = pageSize;
            TBL_BaoCaoXuatNhapTon = data;
        }

        public int PageIndex { get; set; }
        public int Pagecount { get; set; }
        public int Total { get; set; }
        public int PageSize { get; private set; }
        public IList<BaoCaoXuatNhapTonModel> TBL_BaoCaoXuatNhapTon { get; set; }
    }
}
