using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKho.Utility
{
    public static class Paging
    {

        public static HtmlPaging<TModel> GenPaging<TModel>(this HtmlHelper<TModel> html)
        {
            return new HtmlPaging<TModel>(html);
        }

        public class HtmlPaging<TModel>
        {
            private readonly HtmlHelper<TModel> html;

            internal HtmlPaging(HtmlHelper<TModel> helper)
            {
                html = helper;
            }

            public HtmlHelper<TModel> Html
            {
                get { return html; }
            }

            public MvcHtmlString PagingHTML(int pageIndex, int pageCount, int totals)
            {
                if (pageCount < 1)
                    return MvcHtmlString.Empty;

                var sb = new StringBuilder();
                sb.Append("<nav><ul class=\"pagination pagination-md justify-content-center\">");
                if (pageIndex > 1)
                {
                    sb.AppendFormat("<li class=\"page-item\"><a class=\"page-link\" href=\"javascript:pagingData('1')\"><i class=\"ti-angle-double-left\"></i></a></li>");
                    sb.AppendFormat("<li class=\"page-item\"><a class=\"page-link\" href=\"{0}\"><i class=\"ti-angle-left\"></i></a></li>", "javascript:pagingData('" + (pageIndex - 1) + "')");
                }
                else
                {
                    sb.AppendFormat("<li class=\"page-item disabled\"><a class=\"page-link\"><i class=\"ti-angle-double-left\"></i></a></li>");
                    sb.AppendFormat("<li class=\"page-item disabled\"><a class=\"page-link\" ><i class=\"ti-angle-left\"></i></a></li>");
                }
                for (int i = (pageIndex - 2) > 0 ? pageIndex - 2 : 1; i <= ((pageIndex + 4 <= pageCount) ? pageIndex + 2 : pageCount); i++)
                {
                    if (pageIndex == i)
                        sb.AppendFormat("<li class=\"page-item active\"><a class=\"page-link\">{0}</a></li>", i);
                    else
                        sb.AppendFormat("<li class=\"page-item\"><a class=\"page-link\" href=\"{1}\">{0}</a></li>", i, "javascript:pagingData('" + i + "')");
                }
                if (pageIndex < pageCount)
                {
                    sb.AppendFormat("<li class=\"page-item\"><a class=\"page-link\" href=\"{0}\"><i class=\"ti-angle-right\"></i></a></li>", "javascript:pagingData('" + (pageIndex + 1) + "')");
                    sb.AppendFormat(" <li class=\"page-item\"><a class=\"page-link\" href=\"{0}\"><i class=\"ti-angle-double-right\"></i></a></li>", "javascript:pagingData('" + pageCount + "')");
                }
                else
                {
                    sb.AppendFormat("<li class=\"page-item disabled\"><a class=\"page-link\" ><i class=\"ti-angle-right\"></i></a></li>");
                    sb.AppendFormat(" <li class=\"page-item disabled\"><a class=\"page-link\" ><i class=\"ti-angle-double-right\"></i></a></li>");
                }
                sb.Append("</ul></nav>");
                return new MvcHtmlString(sb.ToString());
            }

            public MvcHtmlString PagingHTML_Modal(int pageIndex, int pageCount, int totals)
            {
                if (pageCount < 1)
                    return MvcHtmlString.Empty;

                var sb = new StringBuilder();
                sb.Append("<nav><ul class=\"pagination pagination-md justify-content-center\">");
                if (pageIndex > 1)
                {
                    sb.AppendFormat("<li class=\"page-item\"><a class=\"page-link\" href=\"javascript:pagingData_Modal('1')\"><i class=\"ti-angle-double-left\"></i></a></li>");
                    sb.AppendFormat("<li class=\"page-item\"><a class=\"page-link\" onclick=\"pagingData_Modal('" + (pageIndex - 1) + "')\" href=\"javascript:;\"><i class=\"ti-angle-left\"></i></a></li>");
                }
                else
                {
                    sb.AppendFormat("<li class=\"page-item disabled\"><a class=\"page-link\"><i class=\"ti-angle-double-left\"></i></a></li>");
                    sb.AppendFormat("<li class=\"page-item disabled\"><a class=\"page-link\" ><i class=\"ti-angle-left\"></i></a></li>");
                }
                for (int i = (pageIndex - 2) > 0 ? pageIndex - 2 : 1; i <= ((pageIndex + 4 <= pageCount) ? pageIndex + 2 : pageCount); i++)
                {
                    if (pageIndex == i)
                        sb.AppendFormat("<li class=\"page-item active\"><a class=\"page-link\">{0}</a></li>", i);
                    else
                        sb.AppendFormat("<li class=\"page-item\"><a class=\"page-link\" onclick=\"pagingData_Modal('" + (i) + "')\" href=\"javascript:;\">" + i + "</a></li>");
                }
                if (pageIndex < pageCount)
                {
                    sb.AppendFormat("<li class=\"page-item\"><a class=\"page-link\" onclick=\"pagingData_Modal('" + (pageIndex + 1) + "')\" href=\"javascript:;\"><i class=\"ti-angle-right\"></i></a></li>");
                    sb.AppendFormat(" <li class=\"page-item\"><a class=\"page-link\" onclick=\"pagingData_Modal('" + (pageCount) + "')\" href=\"javascript:;\"><i class=\"ti-angle-double-right\"></i></a></li>");
                }
                else
                {
                    sb.AppendFormat("<li class=\"page-item disabled\"><a class=\"page-link\" ><i class=\"ti-angle-right\"></i></a></li>");
                    sb.AppendFormat(" <li class=\"page-item disabled\"><a class=\"page-link\" ><i class=\"ti-angle-double-right\"></i></a></li>");
                }
                sb.Append("</ul></nav>");
                return new MvcHtmlString(sb.ToString());
            }
            public MvcHtmlString PagingHTML_Modal(int pageIndex, int pageCount, int totals, string menthod)
            {
                if (pageCount < 1)
                    return MvcHtmlString.Empty;

                var sb = new StringBuilder();
                sb.Append("<nav><ul class=\"pagination pagination-md justify-content-center\">");
                if (pageIndex > 1)
                {
                    sb.AppendFormat($"<li class=\"page-item\"><a class=\"page-link\" href=\"javascript:{menthod}('1')\"><i class=\"ti-angle-double-left\"></i></a></li>");
                    sb.AppendFormat($"<li class=\"page-item\"><a class=\"page-link\" onclick=\"{menthod}('" + (pageIndex - 1) + "')\" href=\"javascript:;\"><i class=\"ti-angle-left\"></i></a></li>");
                }
                else
                {
                    sb.AppendFormat("<li class=\"page-item disabled\"><a class=\"page-link\"><i class=\"ti-angle-double-left\"></i></a></li>");
                    sb.AppendFormat("<li class=\"page-item disabled\"><a class=\"page-link\" ><i class=\"ti-angle-left\"></i></a></li>");
                }
                for (int i = (pageIndex - 2) > 0 ? pageIndex - 2 : 1; i <= ((pageIndex + 4 <= pageCount) ? pageIndex + 2 : pageCount); i++)
                {
                    if (pageIndex == i)
                        sb.AppendFormat("<li class=\"page-item active\"><a class=\"page-link\">{0}</a></li>", i);
                    else
                        sb.AppendFormat($"<li class=\"page-item\"><a class=\"page-link\" onclick=\"{menthod}('" + (i) + "')\" href=\"javascript:;\">" + i + "</a></li>");
                }
                if (pageIndex < pageCount)
                {
                    sb.AppendFormat($"<li class=\"page-item\"><a class=\"page-link\" onclick=\"{menthod}('" + (pageIndex + 1) + "')\" href=\"javascript:;\"><i class=\"ti-angle-right\"></i></a></li>");
                    sb.AppendFormat($" <li class=\"page-item\"><a class=\"page-link\" onclick=\"{menthod}('" + (pageCount) + "')\" href=\"javascript:;\"><i class=\"ti-angle-double-right\"></i></a></li>");
                }
                else
                {
                    sb.AppendFormat("<li class=\"page-item disabled\"><a class=\"page-link\" ><i class=\"ti-angle-right\"></i></a></li>");
                    sb.AppendFormat(" <li class=\"page-item disabled\"><a class=\"page-link\" ><i class=\"ti-angle-double-right\"></i></a></li>");
                }
                sb.Append("</ul></nav>");
                return new MvcHtmlString(sb.ToString());
            }
        }

    }
}