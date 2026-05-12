using BusinessLogic.Model;
using System.Collections.Generic;

namespace QuanLyKho.Models
{
    public class BaoCaoXuatKhoaPhong_DSModel
    {
        public BaoCaoXuatKhoaPhong_DSModel(int pageIndex, int pagecount, int pageSize, int total, IList<BaoCaoXuatKhoaPhongCotSanPham> cotSanPham, IList<BaoCaoXuatKhoaPhongModel> data)
        {
            PageIndex = pageIndex;
            Pagecount = pagecount;
            Total = total;
            PageSize = pageSize;
            CotSanPham = cotSanPham;
            TBL_BaoCao = data;
        }

        public int PageIndex { get; set; }
        public int Pagecount { get; set; }
        public int Total { get; set; }
        public int PageSize { get; private set; }
        public IList<BaoCaoXuatKhoaPhongCotSanPham> CotSanPham { get; set; }
        public IList<BaoCaoXuatKhoaPhongModel> TBL_BaoCao { get; set; }
    }
}
