using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.ViewModel.Client
{
    public class CategorysView
    {

        public CategoryView ParentCategory;
        public IList<CategoryView> ChildCategory { get; set; }
    }
    /// <summary>
    /// 分类视图
    /// </summary>
    public class CategoryView
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 上级编号
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        ///名称
        /// </summary>
        public string Name { get; set; }
    }
}
