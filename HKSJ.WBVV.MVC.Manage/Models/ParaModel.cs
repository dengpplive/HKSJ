using System.Collections.Generic;
using HKSJ.WBVV.Common.Extender.LinqExtender;

namespace HKSJ.WBVV.MVC.Manage.Models
{
    public class ParaModel
    {
        /// <summary>
        /// ip地址
        /// </summary>
        public string IpAddress { get; set; }
        /// <summary>
        /// 显示行数
        /// </summary>
        public int pagesize { get; set; }
        /// <summary>
        /// 当前页数
        /// </summary>
        public int pageindex { get; set; }
        /// <summary>
        /// 查询参数
        /// </summary>
        public IList<Condtion> condtions { get; set; }
        /// <summary>
        /// 排序参数
        /// </summary>
        public IList<OrderCondtion> ordercondtions { get; set; }
    }

    public class Condtion
    {
        /// <summary>
        /// 条件字段名称
        /// </summary>
        public string FiledName { get; set; }
        /// <summary>
        /// 条件字段值
        /// </summary>
        public string FiledValue { get; set; }
        /// <summary>
        /// 条件中的比较类型
        /// </summary>
        public ExpressionType ExpressionType { get; set; }
        /// <summary>
        /// 条件中逻辑类型
        /// </summary>
        public ExpressionLogic ExpressionLogic { get; set; }
    }

    /// <summary>
    /// 分页响应数据视图
    /// </summary>
    public class PageResult<T>
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
        public T Data { get; set; }
    }

}