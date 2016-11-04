using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Entity.ViewModel.Client
{

    /// <summary>
    /// 用户评论树
    /// </summary>
    [Serializable]
    public class UserCommentsView
    {
        /// <summary>
        /// 上级评论
        /// </summary>
        public UserCommentView ParentCommentView { get; set; }
        /// <summary>
        /// 下级评论
        /// </summary>
        public IList<UserCommentsView> ChildCommentViews { get; set; }
    }

    /// <summary>
    /// 视频评论
    /// </summary>
    [Serializable]
    public class UserCommentView
    {
        /// <summary>
        /// 评论编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Picture { get; set; }
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
        /// 回复次数
        /// </summary>
        public int ReplyNum { get; set; }
        /// <summary>
        /// 评论时间
        /// </summary>
        public DateTime CreateTime { get; set; }
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
                else if (totalHours >= 24 && totalDays < curMonths[endTime.Month - 1])
                {
                    return message.F(LanguageUtil.Translate("entity_ViewModel_Client_CommentView_Day").F((int)totalDays));
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

