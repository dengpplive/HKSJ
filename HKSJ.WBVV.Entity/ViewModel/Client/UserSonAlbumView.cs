using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.ViewModel.Client
{
    /// <summary>
    /// 个人空间专辑下面的视频数据
    /// </summary>
    [Serializable]
    public class UserSonAlbumView
    {
        /// <summary>
        /// 专辑的标题
        /// </summary>
        public string SpecialTitle { get; set; }
        /// <summary>
        /// 视频数量
        /// </summary>
        public int VideoCount { get; set; }
        /// <summary>
        /// 专辑中的视频
        /// </summary>
        public List<VideoView> VideoViewList { get; set; }
    }
}
