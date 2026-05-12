using BusinessLogic.Model;
using System.Collections.Generic;

namespace QuanLyKho.Models
{
    public class KhoTon_DSModel
    {
        public KhoTon_DSModel(int pageIndex, int pagecount, int pageSize, int total, IList<KhoTonModel> tbl_TonKho)
        {
            PageIndex = pageIndex;
            Pagecount = pagecount;
            Total = total;
            PageSize = pageSize;
            TBL_TonKho = tbl_TonKho;
        }

        public int PageIndex { get; set; }
        public int Pagecount { get; set; }
        public int Total { get; set; }
        public int PageSize { get; private set; }

        public IList<KhoTonModel> TBL_TonKho { get; set; }
    }
}

