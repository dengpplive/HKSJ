using HKSJ.WBVV.Common.Language;
using HKSJ.WBVV.Common.Resource;
using System;

namespace HKSJ.WBVV.Entity.ViewModel.Manage
{
    /// <summary>
    /// 视频
    /// </summary>
    [Serializable]
    public class VideoView
    {
        #region Fields

        /// <summary>
        /// Local veriable used to combine Small Pic Path the a video.
        /// </summary>
        string _smallPicPath;

        #endregion

        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 分类编号
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public int Tags { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string About { get; set; }
        /// <summary>
        /// 播放次数
        /// </summary>
        public int PlayCount { get; set; }
        /// <summary>
        /// 评论次数
        /// </summary>
        public int CommentCount { get; set; }
        /// <summary>
        /// 踩的次数
        /// </summary>
        public int BadCount { get; set; }
        /// <summary>
        /// 赞的次数
        /// </summary>
        public int CollectionCount { get; set; }
        /// <summary>
        /// 是否热门
        /// </summary>
        public bool IsHot { get; set; }
        /// <summary>
        /// 是否官方
        /// </summary>
        public bool IsOfficial { get; set; }
        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool IsRecommend { get; set; }
        /// <summary>
        /// 视频来源
        /// </summary>
        public bool VideoSource { get; set; }
        /// <summary>
        /// 视频地址
        /// </summary>
        public string VideoPath { get; set; }
        /// <summary>
        /// 小图片路径
        /// </summary>
        public string SmallPicturePath
        {
            get
            {
                return UrlHelper.QiniuPublicCombine(_smallPicPath);
            }

            set
            {
                _smallPicPath = value;
            }
        }
        /// <summary>
        /// 版权(1:原创2:转载)
        /// </summary>
        /// <returns></returns>
        public Int16 Copyright { get; set; }
        /// <summary>
        /// 是否公开
        /// </summary>
        public bool IsPublic { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        /// <returns></returns>
        public Int32 SortNum { get; set; }
        /// <summary>
        /// 时间长度
        /// </summary>
        /// <returns></returns>
        public Int32? TimeLength { get; set; }
        /// <summary>
        /// 上映时间
        /// </summary>
        /// <returns></returns>
        public DateTime? ReleaseTime { get; set; }
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
        /// 视频状态（0：转码中，1：转码失败，2：审核中，3：审核通过，4：审核不通过）
        /// </summary>
        public short VideoState { get; set; }

        /// <summary>
        /// 视频状态
        /// </summary>
        public string VideoStateStr
        {
            get
            {
                if (VideoState == 0)
                {
                    return LanguageUtil.Translate("entity_Table_Video_VideoState_transcoding");
                }
                else if (VideoState == 1)
                {
                    return LanguageUtil.Translate("entity_Table_Video_VideoState_transcodingFails");
                }
                else if (VideoState == 2)
                {
                    return "待审核";
                }
                else if (VideoState == 3)
                {
                    return LanguageUtil.Translate("entity_Table_Video_VideoState_examinationPassed");
                }
                else if (VideoState == 4)
                {
                    return LanguageUtil.Translate("entity_Table_Video_VideoState_auditNotPassed");
                }
                return null;
            }
        }
       
        /// <summary>
        /// 上传者名
        /// </summary>
        public string CreateManageName { get; set; }
        /// <summary>
        /// 上传者ID
        /// </summary>
        public int CreateManageId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTimeStr {
            get
            {
                return CreateTime.ToShortDateString();
            }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        public string PersistentUrl {
            get { return "http://api.qiniu.com/status/get/prefop?id="+PersistentId; }
        }

        /// <summary>
        /// 预处理ID
        /// </summary>
        public string PersistentId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 审核内容
        /// </summary>
        public string ApproveContent { get; set; }

        /// <summary>
        /// 审核备注信息
        /// </summary>
        public string ApproveRemark { get; set; }

        /// <summary>
        /// 类型低级ID
        /// </summary>
        public long ParentId { get; set; }
    }
}
