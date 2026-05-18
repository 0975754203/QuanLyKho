using BusinessLogic.Model;
using System.Collections.Generic;

namespace QuanLyKho.Models
{
    public class KhoTon_DSModel
    {
        public KhoTon_DSModel(int pageIndex, int pagecount, int pageSize, int total, IList<KhoTonModel> tbl_TonKho, decimal tongTonKho = 0, decimal tongGiaTri = 0)
        {
            PageIndex = pageIndex;
            Pagecount = pagecount;
            Total = total;
            PageSize = pageSize;
            TBL_TonKho = tbl_TonKho;
            TongTonKho = tongTonKho;
            TongGiaTri = tongGiaTri;
        }

        public int PageIndex { get; set; }
        public int Pagecount { get; set; }
        public int Total { get; set; }
        public int PageSize { get; private set; }
        public decimal TongTonKho { get; set; }
        public decimal TongGiaTri { get; set; }

        public IList<KhoTonModel> TBL_TonKho { get; set; }
    }
}

