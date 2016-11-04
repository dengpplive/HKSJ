using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.ViewModel.Client
{
    /// <summary>
    /// 树分类
    /// </summary>
    [Serializable]
    public class MenuView
    {
        /// <summary>
        /// 父级分类
        /// </summary>
        /// <returns></returns>
        public Menu ParentCategory { get; set; }
        /// <summary>
        /// 子级分类
        /// </summary>
        /// <returns></returns>
        public IList<MenuView> ChildCategorys { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Menu
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        public int CurId { get; set; }

        public int PageSize { get; set; }
    }
}
