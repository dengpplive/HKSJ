using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Resource;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Entity.ViewModel.Client
{
    /// <summary>
    /// 视频评论树
    /// </summary>
    [Serializable]
    public class VideoCommentsView
    {
        /// <summary>
        /// 上级评论
        /// </summary>
        public VideoCommentView ParentCommentView { get; set; }
        /// <summary>
        /// 下级评论
        /// </summary>
        public IList<VideoCommentsView> ChildCommentViews { get; set; }
    }

    /// <summary>
    /// 视频评论
    /// </summary>
    [Serializable]
    public class VideoCommentView
    {
        private string _picture;

        /// <summary>
        /// 评论编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 创建评论用户编号
        /// </summary>
        public int CreateUserId { get; set; }

        /// <summary>
        /// 视频编号
        /// </summary>
        public int VedioId { get; set; }

        /// <summary>
        /// 视频名称
        /// </summary>
        public string VideoName { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Picture
        {
            get
            {
                if (_picture == null || _picture.IndexOf("Content/images", System.StringComparison.Ordinal) > -1)
                    return _picture;
                return UrlHelper.QiniuPublicCombine(_picture);
            }
            set { _picture = value; }

        }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string CommentContent { get; set; }
        /// <summary>
        /// 被评论编号
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 点赞次数
        /// </summary>
        public int PraisesNum { get; set; }
        /// <summary>
        /// 是否已被点赞，当前登录用户对该评论的点赞状态
        /// </summary>
        public bool IsPraised { get; set; }
        /// <summary>
        /// 回复次数
        /// </summary>
        public int ReplyNum { get; set; }
        /// <summary>
        /// 评论时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 评论时间字符串
        /// </summary>
        public string CreateTimeStr
        {
            get
            {
                return CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        /// <summary>
        /// 发表于14小时前
        /// </summary>
        public string TimeSpan
        {
            get
            {
                DateTime startTime = CreateTime;
                DateTime endTime = DateTime.Now;
                TimeSpan ts = endTime - startTime;
                var rMonths = new int[] { 30, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
                var pMonths = new int[] { 30, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
                var curMonths = endTime.Year % 4 == 0 ? rMonths : pMonths;
                double totalseconds = ts.TotalSeconds;
                double totalMinutes = ts.TotalMinutes;
                double totalHours = ts.TotalHours;
                double totalDays = ts.TotalDays;
                string message = LanguageUtil.Translate("entity_ViewModel_Client_CommentView_Issue");
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
        /// 评论定位在哪一页
        /// </summary>
        public int PageIndex { get; set; }

        public bool State { get; set; }
    }
}
