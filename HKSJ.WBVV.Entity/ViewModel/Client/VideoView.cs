using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Language;
using HKSJ.WBVV.Common.Resource;
using System;
using System.Collections.Generic;

namespace HKSJ.WBVV.Entity.ViewModel.Client
{
    /// <summary>
    /// 搜索视频视图
    /// </summary>
    [Serializable]
    public class VideoView
    {
        #region Fields

        /// <summary>
        /// Local veriable used to combine Small Pic Path the a video.
        /// </summary>
        string _smallPicPath;

        /// <summary>
        /// Local veriable used to combine Big Pic Path the a video.
        /// </summary>
        string _bigPicPath;

        /// <summary>
        /// Local veriable used to combine Banner Image Path the a video.
        /// </summary>
        string _bannerImgPath;
        #endregion

        /// <summary>
        /// 视频编号
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 类型分类
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 类型ID
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// <字典编号,字典节点编号>
        /// </summary>
        public IDictionary<int, int> DictionaryId { get; set; }
        /// <summary>
        /// <类型,<字典名称,字典节点名称>>
        /// </summary>
        public IDictionary<string, string> DictionaryViews { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        /// <returns></returns>
        public string Title { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Tags { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        /// <returns></returns>
        public string About { get; set; }
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
        /// 视频大图片
        /// </summary>
        public string BigPicturePath
        {
            get
            {
                return UrlHelper.QiniuPublicCombine(_bigPicPath);
            }

            set
            {
                _bigPicPath = value;
            }
        }
        /// <summary>
        /// Banner图片
        /// </summary>
        public string BannerImagePath
        {
            get
            {
                return UrlHelper.QiniuPublicCombine(_bannerImgPath);
            }

            set
            {
                _bannerImgPath = value;
            }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建时间字符串
        /// </summary>
        public String CreateTimeStr
        {
            get
            {
                return CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 是否是官方
        /// </summary>
        public bool IsOfficial { get; set; }
        /// <summary>
        ///是否热门(0.否1.是)
        /// </summary>
        public bool IsHot { get; set; }
        /// <summary>
        /// 是否推荐(0.否1.是)
        /// </summary>
        public bool IsRecommend { get; set; }

        /// <summary>
        /// 播放次数
        /// </summary>
        /// <returns></returns>
        public Int32 PlayCount { get; set; }

        /// <summary>
        /// 视频时间长度(秒)
        /// </summary>
        public int TimeLength { get; set; }

        /// <summary>
        /// 播放次数：1,234,567
        /// </summary>
        public string FormatPlayCount
        {
            get
            {
                string formatNum = string.Format("{0:n}", PlayCount);
                return formatNum.Substring(0, formatNum.Length - 3);
            }
        }

        /// <summary>
        /// 评论次数
        /// </summary>
        /// <returns></returns>
        public Int32 CommentCount { get; set; }

        /// <summary>
        /// 用户打赏播币总数量
        /// </summary>
        public Int32 RewardCount { get; set; }

        /// <summary>
        /// 用户打赏播币总数量
        /// </summary>
        public short VideoState { get; set; }

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
        /// 上传者用户的昵称
        /// </summary>
        public string UserNickName { get; set; }
        /// <summary>
        /// 视频所属专辑标题 没有为空
        /// </summary>
        /// <returns></returns>
        public string SpecialTitle { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        /// <returns></returns>
        public string Filter { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        /// <returns></returns>
        public Int32 SortNum { get; set; }
        /// <summary>
        /// 显示时间
        /// </summary>
        public string ShowTimeSpan
        {
            get
            {
                return TimeSpan;
            }
        }

        /// <summary>
        /// 发表于14小时前
        /// </summary>
        public string TimeSpan
        {
            get
            {
                if (UpdateTime.IsDbNull() || UpdateTime == DateTime.MinValue)
                {
                    UpdateTime = CreateTime;
                }
                DateTime startTime = UpdateTime;
                DateTime endTime = DateTime.Now;
                TimeSpan ts = endTime - startTime;
                var rMonths = new int[] { 30, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
                var pMonths = new int[] { 30, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
                var curMonths = endTime.Year % 4 == 0 ? rMonths : pMonths;
                int totalseconds = (int)ts.TotalSeconds;
                int totalMinutes = (int)ts.TotalMinutes;
                int totalHours = (int)ts.TotalHours;
                int totalDays = (int)ts.TotalDays;

                string message = LanguageUtil.Translate("entity_ViewModel_Client_SpecialVideoView_Before");
                if (totalseconds < 60)
                {
                    return message.F(LanguageUtil.Translate("entity_ViewModel_Client_CommentView_Second").F((int)totalseconds));
                }
                else if (totalseconds >= 60 && totalMinutes < 60)
                {
                    return message.F(LanguageUtil.Translate("entity_ViewModel_Client_CommentView_Minute").F((int)totalMinutes));
                }
                else if (totalMinutes >= 60 && totalHours < 24)
                {
                    return message.F(LanguageUtil.Translate("entity_ViewModel_Client_CommentView_Hour").F((int)totalHours));
                }
                else if (totalHours >= 24 && totalDays < 7)
                {
                    return message.F(LanguageUtil.Translate("entity_ViewModel_Client_CommentView_Day").F((int)totalDays));
                }
                else if (totalDays >= 7 && totalDays < 14)
                {
                    return message.F(LanguageUtil.Translate("entity_ViewModel_Client_CommentView_OneWeek"));
                }
                else if (totalDays >= 14 && totalDays < 21)
                {
                    return message.F(LanguageUtil.Translate("entity_ViewModel_Client_CommentView_TwoWeek"));
                }
                else if (totalDays >= 21 && totalDays < curMonths[endTime.Month - 1])
                {
                    return message.F(LanguageUtil.Translate("entity_ViewModel_Client_CommentView_ThreeWeek"));
                }
                else if (totalDays >= curMonths[endTime.Month - 1] && totalDays < curMonths[endTime.Month - 1] + curMonths[endTime.Month - 2])
                {
                    return message.F(LanguageUtil.Translate("entity_ViewModel_Client_CommentView_Month"));
                }
                else
                {
                    return startTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
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
        /// 视频状态
        /// </summary>
        public string VideoStateStr
        {
            get
            {
                if (VideoState == 0)
                {
                    return LanguageUtil.Translate("entity_Table_Video_VideoState_transcoding");// "转码中";
                }
                else if (VideoState == 1)
                {
                    return LanguageUtil.Translate("entity_Table_Video_VideoState_transcodingFails");//"转码失败";
                }
                else if (VideoState == 2)
                {
                    return LanguageUtil.Translate("entity_Table_Video_VideoState_underReview");//"审核中";
                }
                else if (VideoState == 3)
                {
                    return LanguageUtil.Translate("entity_Table_Video_VideoState_examinationPassed");//"审核通过";
                }
                else if (VideoState == 4)
                {
                    return LanguageUtil.Translate("entity_Table_Video_VideoState_auditNotPassed");//"审核不通过";
                }
                return null;
            }
        }
    }

    /// <summary>
    /// 简单视频视图
    /// </summary>
    public class VideoSimpleView
    {
        /// <summary>
        /// 视频编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 视频名称
        /// </summary>
        public string Title { get; set; }

    }
}