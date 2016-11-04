using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Common.Extender;

namespace HKSJ.WBVV.Entity.ViewModel.Client
{
    /// <summary>
    /// 专辑视图
    /// </summary>
    [Serializable]
    public class SpecialView
    {
        /// <summary>
        /// 专辑总数
        /// </summary>
        public int SpecialCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 专辑集合
        /// </summary>
        public List<SpecialDetailView> SpecialVideoList { get; set; }


    }
}