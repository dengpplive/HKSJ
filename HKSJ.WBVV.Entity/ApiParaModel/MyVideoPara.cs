using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.ApiParaModel
{
    /// <summary>
    ///我的视频
    /// </summary>
    [Serializable]
    public class MyVideoPara
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 分类编码
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        /// <returns></returns>
        public string Title { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string About { get; set; }

        /// <summary>
        /// 版权(1:原创2:转载)
        /// </summary>
        public short Copyright { get; set; }

        /// <summary>
        /// 是否公开（1.公开0.保密）
        /// </summary>
        public short IsPublic { get; set; }

        /// <summary>
        /// 是否官方（1.官方0.非官方）
        /// </summary>
        public short IsOfficial { get; set; }
        /// <summary>
        /// 封面大图片文件
        /// </summary>
        public string BigPicturePath { get; set; }

        /// <summary>
        /// 封面小图片文件
        /// </summary>
        public string SmallPicturePath { get; set; }

        /// <summary>
        /// 创建人Id
        /// </summary>
        public int CreateManageId { get; set; }

        public string Filter { get; set; }
        
    }
}
