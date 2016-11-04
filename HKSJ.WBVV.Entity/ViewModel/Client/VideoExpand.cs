


using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HKSJ.WBVV.Entity.ViewModel.Client
{
    /// <summary>
    /// 视频
    /// </summary>
    [Serializable]
    public class VideoExpand
    {

        /// <summary>
        /// 视频编号
        /// </summary>
        /// <returns></returns>
        public long Id { get; set; }

        /// <summary>
        /// 分类编号
        /// </summary>
        /// <returns></returns>
        public int CategoryId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        /// <returns></returns>
        public string Title { get; set; }

        /// <summary>
        /// 过滤条件（存放方式【字典编号:字典节点编号;字典编号:字典节点编号;】）
        /// </summary>
        /// <returns></returns>
        public string Filter { get; set; }

        /// <summary>
        /// 标签(标签1|标签2|标签3|标签4|标签5)
        /// </summary>
        /// <returns></returns>
        public string Tags { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        /// <returns></returns>
        public string About { get; set; }

        /// <summary>
        /// 播放次数
        /// </summary>
        /// <returns></returns>
        public int PlayCount { get; set; }

        /// <summary>
        /// 评论次数
        /// </summary>
        /// <returns></returns>
        public int CommentCount { get; set; }

        /// <summary>
        /// 赞的次数
        /// </summary>
        /// <returns></returns>
        public int PraiseCount { get; set; }

        /// <summary>
        /// 视频打赏播币数量
        /// </summary>
        public long RewardCount { get; set; }

        /// <summary>
        /// 踩的次数
        /// </summary>
        /// <returns></returns>
        public int BadCount { get; set; }

        /// <summary>
        /// 收藏次数
        /// </summary>
        /// <returns></returns>
        public int CollectionCount { get; set; }

        /// <summary>
        /// 是否热门(0.否1.是)
        /// </summary>
        /// <returns></returns>
        public bool IsHot { get; set; }

        /// <summary>
        /// 是否是官方(0:不是,1:是)
        /// </summary>
        /// <returns></returns>
        public bool IsOfficial { get; set; }

        /// <summary>
        /// 是否推荐(0.否1.是)
        /// </summary>
        /// <returns></returns>
        public bool IsRecommend { get; set; }

        /// <summary>
        /// 视频来源（0：后台管理员上传(CreateManageId为后台管理员编号)1:前台用户上传(CreateManageId为前台用户编号)）
        /// </summary>
        /// <returns></returns>
        public bool VideoSource { get; set; }

        /// <summary>
        /// 视频路径
        /// </summary>
        /// <returns></returns>
        public string VideoPath { get; set; }

        /// <summary>
        /// 小图片路径
        /// </summary>
        /// <returns></returns>
        public string SmallPicturePath { get; set; }

        /// <summary>
        /// 大图片路径
        /// </summary>
        /// <returns></returns>
        public string BigPicturePath { get; set; }

        /// <summary>
        /// 版权(1:原创2:转载)
        /// </summary>
        /// <returns></returns>
        public int Copyright { get; set; }

        /// <summary>
        /// 是否公开（1.公开0.保密）
        /// </summary>
        /// <returns></returns>
        public bool IsPublic { get; set; }

        /// <summary>
        /// 视频状态（0：转码中，1：转码失败，2：审核中，3：审核通过，4：审核不通过）
        /// </summary>
        /// <returns></returns>
        public int VideoState { get; set; }
        
        /// <summary>
        /// 视频原名
        /// </summary>
        /// <returns></returns>
        public string VideoRosella { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        /// <returns></returns>
        public int SortNum { get; set; }

        /// <summary>
        /// 详细介绍
        /// </summary>
        /// <returns></returns>
        public string Content { get; set; }

        /// <summary>
        /// 时间长度
        /// </summary>
        /// <returns></returns>
        public int TimeLength { get; set; }

        /// <summary>
        /// 上映时间
        /// </summary>
        /// <returns></returns>
        public DateTime ReleaseTime { get; set; }

        /// <summary>
        /// 主演
        /// </summary>
        /// <returns></returns>
        public string Starring { get; set; }

        /// <summary>
        /// 导演
        /// </summary>
        /// <returns></returns>
        public string Director { get; set; }

        /// <summary>
        /// 创建者编号
        /// </summary>
        /// <returns></returns>
        public int CreateManageId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改管理员编号
        /// </summary>
        /// <returns></returns>
        public int UpdateManageId { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 状态（1.删除0.可用（默认））
        /// </summary>
        /// <returns></returns>
        public bool State { get; set; }

        /// <summary>
        /// 视频每日播放次数
        /// </summary>
        public int TheDayPlayCount { get; set; }

    }
}