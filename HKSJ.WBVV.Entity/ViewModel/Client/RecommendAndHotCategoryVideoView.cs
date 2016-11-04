using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.ViewModel.Client
{
    public class RecommendAndHotCategoryVideoView
    {
        /// <summary>
        /// 分类
        /// </summary>
        public CategorysView Category { get; set; }
        /// <summary>
        /// 热门
        /// </summary>
        public IList<IndexVideoView> HotCategoryVideo { get; set; }
        /// <summary>
        /// 推荐
        /// </summary>
        public IList<IndexVideoView> RecommendCategoryVideo { get; set; }
    }
}
