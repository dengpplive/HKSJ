using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.ViewModel.Client
{
    /// <summary>
    /// 二级分类下的筛选视图
    /// </summary>
    [Serializable]
    public class DictionaryView
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// key值
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 分组类型
        /// </summary>
        public string GroupType { get; set; }
        /// <summary>
        /// 对应的value
        /// </summary>
        public List<DictionaryItemView> DictionaryItems { get; set; }
    }
}
