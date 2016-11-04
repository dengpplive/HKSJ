using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.ViewModel.Manage
{
    /// <summary>
    /// 用户
    /// </summary>
    [Serializable]
    public class CategoryTagsView
    {
        /// <summary>
        /// 分类id
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 标签id数组
        /// </summary>
        public int[] TagIdsArray{ get; set; }
        /// <summary>
        /// 标签排序数组
        /// </summary>
        public int[] TagSortsArray { get; set; }
        /// <summary>
        /// 标签名称数组
        /// </summary>
        public string[] TagNamesArray { get; set; }
    }
}
