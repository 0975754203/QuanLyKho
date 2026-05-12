using BusinessLogic.Model;
using System.Collections.Generic;

namespace QuanLyKho.Models
{
    public class KhoGiaoDich_DSModel
    {
        public KhoGiaoDich_DSModel(int pageIndex, int pagecount, int pageSize, int total, IList<KhoGiaoDichModel> data)
        {
            PageIndex = pageIndex;
            Pagecount = pagecount;
            Total = total;
            PageSize = pageSize;
            TBL_KhoGiaoDich = data;
        }

        public int PageIndex { get; set; }
        public int Pagecount { get; set; }
        public int Total { get; set; }
        public int PageSize { get; private set; }
        public IList<KhoGiaoDichModel> TBL_KhoGiaoDich { get; set; }
    }
}
