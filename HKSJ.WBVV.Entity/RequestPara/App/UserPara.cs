using System;
using HKSJ.WBVV.Entity.RequestPara.Base;
using System.ComponentModel.DataAnnotations;
using HKSJ.WBVV.Common.Validation.Attributes;

namespace HKSJ.WBVV.Entity.RequestPara
{
    /// <summary>
    /// 用户参数
    /// </summary>
    public class UserPara
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [Display(Name = "用户id"), ParaRequired]
        public int UserId { get; set; }
    }

    /// <summary>
    /// 用户收藏参数
    /// </summary>
    public class CollectPara
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [Display(Name = "用户id"), ParaRequired]
        public int UserId { get; set; }

        /// <summary>
        /// 视频id
        /// </summary>
        [Display(Name = "视频id"), ParaRequired]
        public int VideoId { get; set; }

        /// <summary>
        /// 收藏id
        /// </summary>
        [Display(Name = "收藏id"), ParaRequired]
        public int CollectId { get; set; }
    }

    /// <summary>
    /// 获取用户收藏列表参数
    /// </summary>
    public class UserCollectsPara : ParaBase
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [Display(Name = "用户id"), ParaRequired]
        public int UserId { get; set; }
    }

    /// <summary>
    /// 修改用户信息参数
    /// </summary>
    public class UpdateUserPara
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [Display(Name = "用户id"), ParaRequired]
        public int UserId { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        [Display(Name = "用户头像")]
        public string Picture { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        [Display(Name = "用户昵称")]
        public string NickName { get; set; }

        /// <summary>
        /// 性别(1:男0:女)
        /// </summary>
        /// <returns></returns>
        [Display(Name = "性别(1:男 0:女)")]
        public bool Gender { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        /// <returns></returns>
        [Display(Name = "生日")]
        public DateTime? Birthdate { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        /// <returns></returns>
        [Display(Name = "城市")]
        public string City { get; set; }

        /// <summary>
        /// 个性签名
        /// </summary>
        [Display(Name = "个性签名")]
        public string Bardian { get; set; }

        /// <summary>
        /// 空间背景图片
        /// </summary>
        /// <returns></returns>
        [Display(Name = "空间背景图片")]
        public string BannerImage { get; set; }

    }

    /// <summary>
    /// 获取视频下载地址
    /// </summary>
    public class VideoUrlPara
    {
        /// <summary>
        /// 视频key
        /// </summary>
        /// <returns></returns>
        [Display(Name = "视频key"), ParaRequired]
        public string key { get; set; }
    }

}
