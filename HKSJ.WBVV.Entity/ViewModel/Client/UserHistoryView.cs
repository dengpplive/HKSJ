using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Language;
using HKSJ.WBVV.Common.Resource;
using System;
using System.Collections.Generic;

namespace HKSJ.WBVV.Entity.ViewModel.Client
{
    [Serializable]
    public class UserHistoryViews
    {
        public List<UserHistoryView> TodayHistories { get; set; }
        public List<UserHistoryView> YesterdayHistories { get; set; }

        public List<UserHistoryView> OneWeekHistories { get; set; }
        public List<UserHistoryView> MoreHistories { get; set; }

        public int TotalMoreCount { get; set; }
    }


    [Serializable]
    public class UserHistoryView
    {
        #region Fields

        /// <summary>
        /// Local veriable used to combine Small Pic Path the a video.
        /// </summary>
        string _smallPicPath;

        #endregion
        public int Id { get; set; }
        public int UserId { get; set; }
        public int VideoId { get; set; }
        public int WatchTime { get; set; }
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
        public string Title { get; set; }
        public DateTime CreateTime { get; set; }

        public int SpanDay
        {
            get
            {
                return (DateTime.Now.Date - CreateTime.Date).Days;

            }

        }

        /// <summary>
        /// 已看到多少分多少秒
        /// </summary>
        public string WatchTimeString
        {
            get
            {
                var m = WatchTime / 60;
                var s = WatchTime % 60;
                string message = LanguageUtil.Translate("entity_ViewModel_Client_UserHistoryView_WatchTimeMessage");
                return message.F(LanguageUtil.Translate("entity_ViewModel_Client_UserHistoryView_MS").F(m, s));
            }
        }



    }
}
