using HKSJ.WBVV.Common.Resource;
using HKSJ.WBVV.Common.Extender;
using System;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Entity.ViewModel.Client
{
    /// <summary>
    /// 专辑视频视图
    /// </summary>
    [Serializable]
    public class SpecialVideoView
    {
        #region Fields

        /// <summary>
        /// Local veriable used to combine Small Pic Path the a video.
        /// </summary>
        string _smallPicPath;

        #endregion

        /// <summary>
        /// 排序号
        /// </summary>
        public int orderId { get; set; }
        /// <summary>
        /// 视频编号
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 视频图片
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
        /// 视频名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 视频简介
        /// </summary>
        public string About { get; set; }

        /// <summary>
        /// 播放次数
        /// </summary>
        public int PlayCount
        {
            get;
            set;
        }
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
        /// 视频时长
        /// </summary>
        public int TimeLength { get; set; }


        public string ShowTimeLength {
            get
            {
                TimeSpan ts = new TimeSpan(0, 0, TimeLength);
                return ts.ToString("g");
            }
        }

        /// <summary>
        /// 评论次数
        /// </summary>
        public int CommentCount { get; set; }
        /// <summary>
        /// 视频创建时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 视频修改时间
        /// </summary>
        public string UpdateTime { get; set; }

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
                if (string.IsNullOrEmpty(UpdateTime) || UpdateTime == DateTime.MinValue.ToString("yyyy-MM-dd"))
                {
                    UpdateTime = CreateTime;
                }
                if (UpdateTime == DateTime.MinValue.ToString("yyyy-MM-dd")) return "";
                DateTime startTime = Convert.ToDateTime(UpdateTime);
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
                    return message.F(LanguageUtil.Translate("entity_ViewModel_Client_CommentView_Second").F(totalseconds));
                }
                else if (totalseconds >= 60 && totalMinutes < 60)
                {
                    return message.F(LanguageUtil.Translate("entity_ViewModel_Client_CommentView_Minute").F(totalMinutes));
                }
                else if (totalMinutes >= 60 && totalHours < 24)
                {
                    return message.F(LanguageUtil.Translate("entity_ViewModel_Client_CommentView_Hour").F(totalHours));
                }
                else if (totalHours >= 24 && totalDays < curMonths[endTime.Month - 1])
                {
                    return message.F(LanguageUtil.Translate("entity_ViewModel_Client_CommentView_Day").F(totalDays));
                }
                else
                {
                    int month = (int)totalDays / curMonths[endTime.Month - 1];
                    if (month < 12)
                    {
                        return message.F(LanguageUtil.Translate("entity_ViewModel_Client_SpecialVideoView_Month").F(month));
                    }
                    else
                    {
                        int year = month / 12;
                        return message.F(LanguageUtil.Translate("entity_ViewModel_Client_SpecialVideoView_year").F(year));
                    }
                }
            }
        }
    }
}