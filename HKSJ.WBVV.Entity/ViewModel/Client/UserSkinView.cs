using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.ViewModel.Client
{
    /// <summary>
    /// 皮肤视图
    /// </summary>
    [Serializable]
    public class UserSkinView
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 背景小图片
        /// </summary>
        public string SmallImage { get; set; }

        /// <summary>
        /// 皮肤名称
        /// </summary>
        public string SkinName { get; set; }

        /// <summary>
        /// css路径
        /// </summary>
        public string CssPath { get; set; }
        /// <summary>
        /// 皮肤类型
        /// </summary>
        public int SkinType { get; set; }

        /// <summary>
        /// 是否为默认皮肤
        /// </summary>
        public bool IsDefaultSkin { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
