using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Resource;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Entity.ViewModel.Client
{
    /// <summary>
    /// 留言树
    /// </summary>
    public class UserMessageDetialsView
    {
        /// <summary>
        /// 上级留言
        /// </summary>
        public UserMessageDetialView ParentUserMessageDetialView { get; set; }
        /// <summary>
        /// 下级留言
        /// </summary>
        public IList<UserMessageDetialsView> ChildUserMessageDetialViews { get; set; }
    }

    /// <summary>
    /// 我的粉丝视图数据
    /// </summary>
    public class UserMessageDetialView
    {
        /// <summary>
        /// 评论编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 创建评论用户编号
        /// </summary>
        public int CreateUserId { get; set; }
        /// <summary>
        /// 接受消息用户编号
        /// </summary>
        public int UserToId { get; set; }

        /// <summary>
        /// 留言所属者用户ID 根用户ID 
        /// </summary>
        /// <returns></returns>
        public Int32 OwnerUserId { get; set; }

        private string _picture = string.Empty;

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
            set
            {
                _picture = value;
            }
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

        /// <summary>
        /// 是否推送
        /// </summary>
        public bool IsPush { get; set; }
        /// <summary>
        /// 是否查看
        /// </summary>
        public bool IsSee { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public bool State { get; set; }
    }

    /// <summary>
    /// 用户中心-留言
    /// </summary>
    public class UserMessageView
    {
        /// <summary>
        /// 留言编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 接受消息用户编号
        /// </summary>
        public int UserToId { get; set; }
        /// <summary>
        /// 发送消息用户编号
        /// </summary>
        public int UserFromId { get; set; }
        /// <summary>
        /// 留言所属者ID 顶级根用户ID 
        /// </summary>
        /// <returns></returns>
        public Int32 OwnerUserId { get; set; }
        /// <summary>
        /// 发送消息用户昵称
        /// </summary>
        public string SendNickName { get; set; }
        /// <summary>
        /// 接收消息的用户昵称
        /// </summary>
        public string ReceiveNickName { get; set; }


        private string _userFromPicture = string.Empty;

        /// <summary>
        /// 发送消息用户头像
        /// </summary>
        public string UserFromPicture
        {
            get
            {
                if (_userFromPicture == null || _userFromPicture.IndexOf("Content/images", System.StringComparison.Ordinal) > -1)
                    return _userFromPicture;
                return UrlHelper.QiniuPublicCombine(_userFromPicture);
            }
            set
            {
                _userFromPicture = value;
            }
        }
        private string _userToPicture = string.Empty;

        /// <summary>
        /// 接收消息用户头像
        /// </summary>
        public string UserToPicture
        {
            get
            {
                if (_userToPicture == null || _userToPicture.IndexOf("Content/images", System.StringComparison.Ordinal) > -1)
                    return _userToPicture;
                return UrlHelper.QiniuPublicCombine(_userToPicture);
            }
            set
            {
                _userToPicture = value;
            }
        }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTimeStr
        {
            get
            {
                return CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        /// <summary>
        /// 已读和未读
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        /// <returns></returns>
        public string MessageContent { get; set; }
        /// <summary>
        /// 留言定位在哪一页
        /// </summary>
        public int PageIndex { get; set; }
    }

    /// <summary>
    /// 推送消息类
    /// </summary>
    public class PushMessage
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public int MessageType { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTimeStr
        {
            get
            {
                return CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        /// <summary>
        /// 发送用户
        /// </summary>
        public int SendUserId { get; set; }
        /// <summary>
        /// 发送用户名称
        /// </summary>
        public string SendUserName { get; set; }
        /// <summary>
        /// 接受用户编号
        /// </summary>
        public int ReceiveUserId { get; set; }
        /// <summary>
        /// 接受用户名称
        /// </summary>
        public string ReceiveUserName { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string MessageContent { get; set; }
    }
}
