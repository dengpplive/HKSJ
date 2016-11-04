using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.ViewModel.Manage
{
    /// <summary>
    /// 后台管理树菜单
    /// </summary>
   [Serializable]
    public class CategoryView
    {
        /// <summary>
        ///  编号
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 父级编号
        /// </summary>
        public int pId { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 默认开启
        /// </summary>
        public bool open { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        //public bool isParent = true;
    }
}
