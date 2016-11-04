using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.Utilities.Base.Common
{
    public class PageEntity
    {
        public string CurrentPageCss { get; set; }
        public int CurrentPageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public string UrlPre { get; set; }

        private int _displayPageCount = 10;

        public int DisplayPageCount
        {
            get { return _displayPageCount; }
            set { _displayPageCount = value; }
        }

    }
    public static class PageHelper
    {
        /// <summary>
        /// 扩展UrlHelper，实现输出分页HTML
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="controllerName">控制器名</param>
        /// <param name="actionName">行为名</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageModel.CurrentPageIndex">当前页码</param>
        /// <param name="pageModel.TotalCount">总记录数</param>
        /// <returns></returns>
        public static string Pager(PageEntity pageModel)//string pageModel.UrlPre, int pageSize, int pageModel.CurrentPageIndex, int pageModel.TotalCount, string currentPageCss
        {
            //<ul class="next_ul fr">
            //        <span class="next"><a href="#">首页</a></span>
            //        <span class="next"><a href="#"> < &nbsp;上一页</a></span>
            //        <li><a href="#" class="next_ul_a next_a_active">1</a></li>
            //        <li><a href="#" class="next_ul_a">2</a></li>
            //        <li><a href="#" class="next_ul_a">3</a></li>
            //        <li><a href="#" class="next_ul_a">4</a></li>
            //        <li><a href="#" class="next_ul_a">5</a></li>
            //        <span class="next ml"><a href="#">下一页 &nbsp; ></a></span>
            //        <span class="next"><a href="#">尾页</a></span>
            //    </ul>


            // 如果分页大小等于0，则返回空字符串
            if (pageModel.PageSize == 0 || pageModel.TotalCount == 0)
            {
                return string.Empty;
            }

            // 根据总记录数和分页大小计算出分页数量
            int pageCount = (int)decimal.Ceiling((decimal)pageModel.TotalCount / (decimal)pageModel.PageSize);

            // 首页、末页
            //string firstStr = string.Empty;
            //string lastStr = string.Empty;
            //if (pageModel.TotalCount > 0)
            //{
            //    string firstUrl = pageModel.UrlPre + 1;
            //    if (pageModel.CurrentPageIndex != 1)
            //        firstStr = "<a href=\"" + firstUrl + "\">First</a>";
            //    else
            //        firstStr = "<a disabled=\"disabled\">First</a>";

            //    string lastUrl = pageModel.UrlPre + pageCount;
            //    if (pageModel.CurrentPageIndex != pageCount)
            //        lastStr = "<a href=\"" + lastUrl + "\">Last</a>";
            //    else
            //        lastStr = "<a disabled=\"disabled\">Last</a>";
            //}
            //else
            //{
            //    firstStr = "<a disabled=\"disabled\">First</a>";
            //    lastStr = "<a disabled=\"disabled\">Last</a>";
            //}

            // 上一页
            string preStr = string.Empty;
            if (pageModel.CurrentPageIndex > 1 && pageModel.CurrentPageIndex <= pageCount)
            {
                string prevUrl = pageModel.UrlPre + (pageModel.CurrentPageIndex - 1);
                preStr = "<div class=\"last fl\"><a href=\"" + prevUrl + "\"> < </a></div>";
            }
            else
            {
                preStr = "<div style=\"background:#e3e3e3;\" class=\"last fl\"><a disabled=\"disabled\"> < </a></div>";
            }

            // 下一页
            string nextStr = string.Empty;
            if (pageModel.CurrentPageIndex > 0 && pageModel.CurrentPageIndex < pageCount)
            {
                string nextUrl = pageModel.UrlPre + (pageModel.CurrentPageIndex + 1);
                nextStr = "<div class=\"next fl\"><a href='" + nextUrl + "'> > </a></div>";
            }
            else
            {
                nextStr = "<div style=\"background:#e3e3e3;\"  class=\"next fl\"><a disabled=\"disabled\"> > </a></div>";
            }

            // 页码
            string numStr = string.Empty;
            if (pageCount > 0)
            {
                numStr = "<ul>";
                int pageListIndex = (int)Math.Ceiling((decimal)pageModel.CurrentPageIndex / (decimal)pageModel.DisplayPageCount);
                int startPage = (pageListIndex - 1) * pageModel.DisplayPageCount + 1;
                int _displayPage = pageCount > (startPage - 1) + pageModel.DisplayPageCount ? pageModel.DisplayPageCount : pageCount - (startPage - 1);
                if (startPage > 1)
                {
                    string numUrl = pageModel.UrlPre + (startPage - 1);
                    numStr += "<li><a href=\"" + numUrl + "\">...</a></li>";
                }
                // 遍历输出全部的页码
                for (int i = 1; i <= _displayPage; i++)
                {
                    string numUrl = pageModel.UrlPre + startPage;
                    // 当前页码加粗
                    if (startPage == pageModel.CurrentPageIndex)
                    {
                        numStr += "<li class=\"" + pageModel.CurrentPageCss + "\"><a>" + startPage + "</a></li>";
                    }
                    else
                    {
                        numStr += "<li><a href=\"" + numUrl + "\">" + startPage + "</a></li>";
                    }
                    startPage++;
                }
                if (startPage <= pageCount)
                {
                    string numUrl = pageModel.UrlPre + startPage;
                    numStr += "<li><a href=\"" + numUrl + "\">...</a></li>";
                }
                numStr += "</ul>";
            }

            string pageStr = "<div id=\"mvcPage\"  class=\"paging\">" + preStr + numStr + nextStr + "</div>";

            return pageStr;
        }


        public static string PageNavigate(PageEntity pageModel)
        {
            string redirectTo = pageModel.UrlPre;
            int currentPage = pageModel.CurrentPageIndex;

            int pageSize = pageModel.PageSize;

            int totalCount = pageModel.TotalCount;
            int currint = pageModel.DisplayPageCount;
            // var redirectTo = htmlHelper.ViewContext.RequestContext.HttpContext.Request.Url.AbsolutePath;
            pageSize = pageSize == 0 ? 5 : pageSize;
            var totalPages = Math.Max((totalCount + pageSize - 1) / pageSize, 1); //总页数
            var output = new StringBuilder("<ul class='next_ul fr'>");
            if (totalPages > 1)
            {
                output.AppendFormat("<span class='next'><a href='{0}{1}'>首页</a></span>", redirectTo, 1);
                if (currentPage > 1)
                {
                    //处理上一页的连接
                    output.AppendFormat("<li class='next'><a href='{0}{1}'>&nbsp;上一页</a> </li>", redirectTo, currentPage - 1);
                }

                output.Append(" ");

                for (int i = 0; i <= 10; i++)
                {//一共最多显示10个页码，前面5个，后面5个
                    if ((currentPage + i - currint) >= 1 && (currentPage + i - currint) <= totalPages)
                    {
                        if (currint == i)
                        {//当前页处理                           
                            output.AppendFormat("<li class='next_ul_a '><a  href='{0}{1}' class='next_a_active'>{2}</a></li> ", redirectTo, currentPage, currentPage);
                        }
                        else
                        {//一般页处理
                            output.AppendFormat("<li class='next_ul_a'><a  href='{0}{1}'>{2}</a></li> ", redirectTo, currentPage + i - currint, currentPage + i - currint);
                        }
                    }
                    output.Append(" ");
                }
                if (currentPage < totalPages)
                {//处理下一页的链接
                    output.AppendFormat("<span class='next ml'><a  href='{0}{1}'>下一页 &nbsp; </a></span> ", redirectTo, currentPage + 1);
                }

                output.Append(" ");
                if (currentPage != totalPages)
                {
                    output.AppendFormat("<span class='next'><a href='{0}{1}'>末页</a></span> ", redirectTo, totalPages);
                }
                output.Append("</ul> ");
            }
            output.Append("</ul> ");
            //output.AppendFormat("<div class='jump fr'><span>第{0}页 / 共{1}页</span></div>", currentPage, totalPages);//这个统计加不加都行
            //output.Append("</div>");
            return output.ToString();
        }
    }
}
