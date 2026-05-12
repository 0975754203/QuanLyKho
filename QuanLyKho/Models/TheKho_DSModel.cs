using BusinessLogic.Model;
using System.Collections.Generic;

namespace QuanLyKho.Models
{
    public class TheKho_DSModel
    {
        public TheKho_DSModel(int pageIndex, int pagecount, int pageSize, int total, IList<TheKhoModel> data, decimal tongSoLuong = 0)
        {
            PageIndex = pageIndex;
            Pagecount = pagecount;
            Total = total;
            PageSize = pageSize;
            TBL_TheKho = data;
            TongSoLuong = tongSoLuong;
        }

        public int PageIndex { get; set; }
        public int Pagecount { get; set; }
        public int Total { get; set; }
        public int PageSize { get; private set; }

        /// <summary>
        /// Tổng số lượng có dấu (nhập dương, xuất âm) theo toàn bộ bản ghi khớp bộ lọc, không chỉ trang hiện tại.
        /// </summary>
        public decimal TongSoLuong { get; set; }

        public IList<TheKhoModel> TBL_TheKho { get; set; }
    }
}
