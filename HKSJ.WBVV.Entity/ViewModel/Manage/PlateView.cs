using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.ViewModel.Manage
{
    /// <summary>
    /// 板块视图
    /// </summary>
    [Serializable]
    public class PlateView
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 一级分类ID
        /// </summary>
        /// <returns></returns>
        public int CategoryId { get; set; }
        /// <summary>
        /// 一级分类名称
        /// </summary>
        /// <returns></returns>
        public string CategoryName { get; set; }
        /// <summary>
        /// 板块名称
        /// </summary>
        /// <returns></returns>
        public string Name { get; set; }
        /// <summary>
        /// 显示条数
        /// </summary>
        /// <returns></returns>
        public Int32 PageSize { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        /// <returns></returns>
        public string KeyWord { get; set; }
        /// <summary>
        /// 排序数量（越大越靠前）
        /// </summary>
        /// <returns></returns>
        public Int32 SortNum { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        public string CreateTime { get; set; }
        /// <summary>
        /// 状态（1.删除0.可用（默认））
        /// </summary>
        /// <returns></returns>
        public string State { get; set; }
        /// <summary>
        /// 创建者名称
        /// </summary>
        public string CreateManageName { get; set; }
        /// <summary>
        /// 修改者
        /// </summary>
        /// <returns></returns>
        public string UpdateManageName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        public string UpdateTime { get; set; }

    }
}
