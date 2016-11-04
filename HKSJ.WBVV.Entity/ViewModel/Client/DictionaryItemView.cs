using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.ViewModel.Client
{
    public class DictionaryItemView
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 字典类型
        /// </summary>
        public int DictionaryId { get; set; }
        /// <summary>
        /// 所属分组类型
        /// </summary>
        public string GroupType { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsCheck { get; set; }
    }
}
