using BusinessLogic;
using BusinessLogic.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.Models
{
    public class BaoCaoKetQuaXetTNPage
    {
        public BaoCaoKetQuaXetTNPage(int iPageIndex, int iPageCount, int iPageSize, int iTotal, DataTable lstData)
        {
            PageIndex = iPageIndex;
            Pagecount = iPageCount;
            Total = iTotal;
            PageSize = iPageSize;
            TBL_Data = lstData;
        }

        public int PageIndex { get; set; }
        public int Pagecount { get; set; }
        public int Total { get; set; }
        public int PageSize { get; private set; }

        public DataTable TBL_Data { get; set; }
    }
}
