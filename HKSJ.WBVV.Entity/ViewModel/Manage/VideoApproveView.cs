using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.ViewModel.Manage
{
    /// <summary>
    /// 视频审核视图
    /// </summary>
    [Serializable]
    public class VideoApproveView
    {
        /// <summary>
        /// 
        /// </summary>
        public string ApproveContent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ApproveRemark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Copyright { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int VideoId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int CreateAdminId { get; set; }
        /// <summary>
        /// 分类ID
        /// </summary>
        public int CategoryId { get; set; }
    }


}
