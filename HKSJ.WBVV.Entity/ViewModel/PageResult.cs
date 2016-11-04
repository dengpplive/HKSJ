using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.ViewModel
{
    /// <summary>
    /// 分页响应数据视图
    /// </summary>
    public class PageResult
    {
        /// <summary>
        /// 返回显示的条数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 返回当前页
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 返回总的页数
        /// </summary>
        public int TotalIndex { get; set; }
        /// <summary>
        /// 返回总的条数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 返回前几条记录
        /// </summary>
        public Object Data { get; set; }
    }
}
