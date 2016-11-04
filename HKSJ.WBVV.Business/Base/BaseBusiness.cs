using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity.Response.App;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.Enums;

namespace HKSJ.WBVV.Business.Base
{
    public class BaseBusiness
    {
        /// <summary>
        /// 用户ip地址
        /// </summary>
        public string IpAddress { get; set; }
        /// <summary>
        /// 登录的用户编号
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 显示多少行数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 显示第几页
        /// </summary>
        public int PageIndex { get; set; }
        public string Token { get; set; }

        #region 获取localhostPath
        /// <summary>
        /// 获取分类的localhostPath,最大长度为6位
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        protected string GetLocalhostPath(int categoryId)
        {
            if (categoryId < 10)
            {
                return "00000" + categoryId;
            }
            else if (categoryId >= 10 && categoryId < 100)
            {
                return "0000" + categoryId;
            }
            else if (categoryId >= 100 & categoryId < 1000)
            {
                return "000" + categoryId;
            }
            else if (categoryId >= 1000 & categoryId < 10000)
            {
                return "00" + categoryId;
            }
            else if (categoryId >= 10000 & categoryId < 100000)
            {
                return "0" + categoryId;
            }
            else
            {
                return categoryId.ToString();
            }
        }
        #endregion

        #region 拆分过滤属性

        /// <summary>
        /// 拆分一个字符串(不可重复)(2c 3r 3c 4r)
        /// </summary>
        /// <param name="two">c</param>
        /// <param name="filter">过滤条件</param>
        /// <param name="one">r</param>
        /// <returns>[{2,3},{3,4}]</returns>
        protected IDictionary<int, int> GetDictionarys(char one, char two, string filter)
        {
            IDictionary<int, int> dictionary = new Dictionary<int, int>();
            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToLower();
                if (filter.IndexOf(one) != -1)//先按行分割
                {
                    var rows = filter.Split(new char[] { one }, StringSplitOptions.RemoveEmptyEntries);
                    if (rows.Length > 0)
                    {
                        for (int i = 0; i < rows.Length; i++)
                        {
                            if (rows[i].IndexOf(two) != -1)//在按列分割
                            {
                                var clo = rows[i].Split(two);
                                if (clo.Length == 2)
                                {
                                    int key = 0;
                                    int value = 0;
                                    int.TryParse(clo[0], out key);
                                    int.TryParse(clo[1], out value);
                                    if (key > 0)
                                    {
                                        if (dictionary.ContainsKey(key))
                                        {
                                            dictionary[key] = value;
                                        }
                                        else
                                        {
                                            dictionary.Add(key, value);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return dictionary;
        }
        /// <summary>
        /// 拆分一个字符串(g c 2c 3r g d 2c 4r)
        /// </summary>
        /// <param name="three">c</param>
        /// <param name="filter"></param>
        /// <param name="one">g</param>
        /// <param name="two">r</param>
        /// <returns>[{"c",[{2,3}]},{"d",[{2,4}]}]</returns>
        protected IDictionary<string, IDictionary<int, int>> GetDictionarys(char one, char two, char three, string filter)
        {
            IDictionary<string, IDictionary<int, int>> dictionary = new Dictionary<string, IDictionary<int, int>>();
            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToLower();
                if (filter.IndexOf(one) != -1) //先按行分割
                {
                    var gArr = filter.Split(new char[] { one }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var s in gArr)
                    {
                        //截取最后一个字符
                        var key = s.Substring(0, 1);
                        //剩余字符串
                        var value = s.Substring(1, s.Length - 1);
                        IDictionary<int, int> dict = new Dictionary<int, int>();
                        if (!string.IsNullOrEmpty(value))
                        {
                            dict = GetDictionarys(two, three, value);
                        }
                        if (dictionary.ContainsKey(key))
                        {
                            dictionary[key] = dict;
                        }
                        else
                        {
                            dictionary.Add(key, dict);
                        }
                    }
                }
            }
            return dictionary;
        }
        /// <summary>
        /// 拆分一个字符串(g c 2c3r g d 3c4r)
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="one">g</param>
        /// <returns>[{c,["2c3r"]},{d,["3c4r"]}]</returns>
        protected IDictionary<string, string> GetDictionarys(char one, string filter)
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToLower();
                if (filter.IndexOf(one) != -1) //先按行分割
                {
                    var gArr = filter.Split(new char[] { one }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var s in gArr)
                    {
                        //截取第一个字符
                        var key = s.Substring(0, 1);
                        //剩余字符串
                        var value = s.Substring(1, s.Length - 1);
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (dictionary.ContainsKey(key))
                            {
                                dictionary[key] = value;
                            }
                            else
                            {
                                dictionary.Add(key, value);
                            }
                        }

                    }
                }
            }
            return dictionary;
        }
        /// <summary>
        /// 拆分一个字符串(2c3 r 2c4 r)
        /// </summary>
        /// <param name="one">r</param>
        /// <param name="filter"></param>
        /// <returns>["2c3","2c4"]</returns>
        protected IList<string> GetLists(char one, string filter)
        {
            IList<string> dictionary = new List<string>();
            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToLower();
                if (filter.IndexOf(one) != -1)//先按行分割
                {
                    var rows = filter.Split(new char[] { one }, StringSplitOptions.RemoveEmptyEntries);
                    dictionary = rows.ToList();
                }
            }
            return dictionary;
        }

        #endregion

        #region 拆分用户
        /// <summary>
        /// 拆分用户
        /// </summary>
        /// <param name="userBy">1|2|3|4</param>
        /// <returns></returns>
        protected IList<int> SplitUserBy(string userBy)
        {
            IList<int> list = new List<int>();
            if (string.IsNullOrEmpty(userBy))
            {
                return list;
            }
            if (userBy.IndexOf('|') == -1)
            {
                int userId = 0;
                int.TryParse(userBy, out userId);
                list.Add(userId);
                return list;
            }
            var arr = userBy.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length <= 0)
            {
                return list;
            }
            foreach (var s in arr)
            {
                int userId = 0;
                int.TryParse(s, out userId);
                if (userId > 0)
                {
                    list.Add(userId);
                }
            }
            return list;
        }
        #endregion

        #region 拆分标签

        /// <summary>
        /// 拆分标签
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="splitChar"></param>
        /// <returns></returns>
        protected IList<string> SplitByTags(string tags, char splitChar)
        {
            IList<string> tagsList = new List<string>();
            if (string.IsNullOrEmpty(tags))
            {
                return tagsList;
            }
            if (tags.IndexOf(splitChar) == -1)
            {
                tagsList.Add(tags);
                return tagsList;
            }
            var arr = tags.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length <= 0)
            {
                return tagsList;
            }
            foreach (var s in arr)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    tagsList.Add(s.Trim());
                }
            }
            return tagsList;
        }
        #endregion

        #region 公共传入参数
        /// <summary>
        /// 比较状态相等
        /// </summary>
        /// <param name="state">默认false（true表示删除或者禁用）</param>
        /// <returns></returns>
        protected Condtion CondtionEqualState(bool state = false)
        {
            var condtion = new Condtion()
            {
                FiledName = "State",
                FiledValue = state,
                ExpressionType = ExpressionType.Equal,
                ExpressionLogic = ExpressionLogic.And
            };
            return condtion;
        }
        /// <summary>
        /// 比较分类编号相等
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected Condtion ConditionEqualId(int id)
        {
            var condtion = new Condtion()
            {
                FiledName = "Id",
                FiledValue = id,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }
        /// <summary>
        /// 比较分类编号不相等
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected Condtion ConditionNotEqualId(int id)
        {
            var condtion = new Condtion()
            {
                FiledName = "Id",
                FiledValue = id,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.NotEqual
            };
            return condtion;
        }
        #endregion

        #region 公共排序参数
        /// <summary>
        /// 按SortNum排序
        /// </summary>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        protected OrderCondtion OrderCondtionSortNum(bool isDesc)
        {
            var orderCodtion = new OrderCondtion()
            {
                FiledName = "SortNum",
                IsDesc = isDesc
            };
            return orderCodtion;
        }
        /// <summary>
        /// 按CreateTime排序
        /// </summary>
        /// <param name="isDesc">是否降序</param>
        /// <returns></returns>
        protected OrderCondtion OrderCondtionCreateTime(bool isDesc)
        {
            var orderCodtion = new OrderCondtion()
            {
                FiledName = "CreateTime",
                IsDesc = isDesc
            };
            return orderCodtion;
        }

        #endregion

        #region TimeSpan
        /// <summary>
        /// TimeSpan
        /// </summary>
        public string TimeSpan(DateTime time)
        {
            DateTime startTime = time;
            DateTime endTime = DateTime.Now;
            TimeSpan ts = endTime - startTime;
            var rMonths = new int[] { 30, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            var pMonths = new int[] { 30, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            var curMonths = endTime.Year % 4 == 0 ? rMonths : pMonths;
            double totalseconds = ts.TotalSeconds;
            double totalMinutes = ts.TotalMinutes;
            double totalHours = ts.TotalHours;
            double totalDays = ts.TotalDays;
            const string message = "发表于{0}前";
            if (totalseconds < 60)
            {
                return message.F((int)totalseconds + "秒");
            }
            else if (totalseconds >= 60 && totalMinutes < 60)
            {
                return message.F((int)totalMinutes + "分");
            }
            else if (totalMinutes >= 60 && totalHours < 24)
            {
                return message.F((int)totalHours + "小时");
            }
            else if (totalHours >= 24 && totalDays < 7)
            {
                return message.F((int)totalDays + "天");
            }
            else if (totalDays >= 7 && totalDays < 14)
            {
                return message.F("一周");
            }
            else if (totalDays >= 14 && totalDays < 21)
            {
                return message.F("两周");
            }
            else if (totalDays >= 21 && totalDays < curMonths[endTime.Month - 1])
            {
                return message.F("三周");
            }
            else if (totalDays >= curMonths[endTime.Month - 1] && totalDays < curMonths[endTime.Month - 1] + curMonths[endTime.Month - 2])
            {
                return message.F("一个月");
            }
            else
            {
                return startTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        #endregion

        #region 分页数据逻辑处理
        /// <summary>
        /// 分页数据逻辑处理,PageSize无值时默认10
        /// </summary>
        /// <param name="dataCount">总页数</param>
        protected int GetPageCountToDataCount(int dataCount)
        {
            PageSize = PageSize <= 0 ? 10 : PageSize;
            int PageCount = dataCount % PageSize == 0 ? dataCount / PageSize : (dataCount / PageSize + 1);
            PageIndex = PageIndex <= 0 ? 1 : PageIndex;
            PageIndex = PageCount <= PageIndex ? PageCount : PageIndex;
            return PageCount;
        }
        #endregion

        #region 是否登录

        /// <summary>
        /// 是否登录
        /// </summary>
        /// <param name="users">用户列表</param>
        /// <param name="loginUserId">登录用户编号</param>
        /// <returns></returns>
        protected bool IsLogin(IQueryable<User> users, int loginUserId)
        {
            if (loginUserId <= 0)
            {
                return false;
            }
            var isLogin = (from u in users
                        where u.Id == loginUserId && u.State == false
                        select u).Any();
            return isLogin;
        }

        /// <summary>
        /// 是否登录
        /// </summary>
        /// <param name="users">用户列表</param>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="user"></param>
        /// <returns></returns>
        protected bool IsLogin(IQueryable<User> users, int loginUserId, out User user)
        {
            if (loginUserId <= 0)
            {
                user = new User();
                return false;
            }
            user = (from u in users
                    where u.Id == loginUserId && u.State == false
                    select u).FirstOrDefault();
            return user != null;
        }
        #endregion

        #region 是否关注

        /// <summary>
        /// 是否关注
        /// </summary>
        /// <param name="userFanses">关注列表</param>
        /// <param name="userId">上传者编号</param>
        /// <param name="loginUserId">登录用户编号</param>
        /// <returns>已关注（true）,未关注（false）</returns>
        protected bool IsSubed(IQueryable<UserFans> userFanses, int userId, int loginUserId)
        {
            var isSubed = (from uf in userFanses
                            where uf.SubscribeUserId == userId 
                               && uf.CreateUserId == loginUserId
                               &&uf.State==false
                            select uf).Any();
            return isSubed;
        }
        #endregion

        #region 是否收藏

        /// <summary>
        /// 是否收藏
        /// </summary>
        /// <param name="userCollects">收藏列表</param>
        /// <param name="videoId">收藏视频编号</param>
        /// <param name="loginUserId">登录用户编号</param>
        /// <returns>未收藏（FALSE）,已收藏（TRUE）</returns>
        protected bool IsCollect(IQueryable<UserCollect> userCollects, int videoId, int loginUserId)
        {
            var isCollect = (from r in userCollects
                          where r.VideoId == videoId 
                                && r.CreateUserId == loginUserId
                                && r.State == false
                          select r).Any();
            return isCollect;
        }
        #endregion

        #region 评论是否点赞

        /// <summary>
        /// 评论是否点赞
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="comment">评论信息</param>
        /// <param name="praiseses">点赞列表</param>
        /// <returns></returns>
        protected bool IsPraises(int loginUserId, Comments comment, IQueryable<Praises> praiseses)
        {
            var isPraises = (from p in praiseses
                             where comment.Id == p.ThemeId 
                                 && comment.EntityType == p.ThemeTypeId 
                                 && p.CreateUserId == loginUserId 
                                 && p.State == true
                           select p).Any();
            return isPraises;
        }
        #endregion

        #region 消息是否已读

        /// <summary>
        /// 是否已读
        /// </summary>
        /// <param name="messageReads">读消息列表</param>
        /// <param name="loginUserid">登录用户编号</param>
        /// <param name="messageId">消息编号</param>
        /// <returns>已读（true）,未读（false）</returns>
        protected bool IsRead(IQueryable<MessageRead> messageReads,int loginUserid, int messageId)
        {
            var isRead = (from mr in messageReads
                          where mr.MessageId == messageId
                                && mr.UserId == loginUserid
                          select mr).Any();
            return isRead;
        }
        #endregion

        #region 消息是否删除

        /// <summary>
        /// 是否已删
        /// </summary>
        /// <param name="messageReads">读消息列表</param>
        /// <param name="loginUserid">登录用户编号</param>
        /// <param name="systemMessageId">消息编号</param>
        /// <returns>已删（true）,未删（false）</returns>
        protected bool IsDelete(IQueryable<MessageRead> messageReads, int loginUserid, int systemMessageId)
        {
            var isDelete = (from mr in messageReads
                            where mr.MessageId == systemMessageId
                                  && mr.UserId == loginUserid
                                  && mr.State == (int)SystemMessageStateEnum.Deleted
                            select mr).Any();
            return isDelete;
        }
        #endregion

        #region 获取用户视图
        /// <summary>
        /// 获取用户视图(不含是否关注和上传视频数量)
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns></returns>
        protected AppUserView UserView(User user)
        {
            return UserView(0, 0, user, null, null);
        }

        /// <summary>
        /// 获取用户视图(用户信息包含是否关注)
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="userFans">关注列表</param>
        /// <param name="loginUserId">登录用户</param>
        /// <returns></returns>
        protected AppUserView UserView(int loginUserId, User user, IQueryable<UserFans> userFans)
        {
            return UserView(loginUserId, 0, user, null, userFans);
        }

        /// <summary>
        /// 获取用户视图(用户信息包含上传视频数量和空间留言数量)
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="videos">视频信息</param>
        /// <param name="unreadCommentCount">空间未读留言数量</param>
        /// <returns></returns>
        protected AppUserView UserView(User user, int unreadCommentCount, IQueryable<Video> videos)
        {
            return UserView(0, unreadCommentCount, user, videos, null);
        }

        /// <summary>
        /// 获取用户视图(用户信息包含是否关注和上传视频数量)
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="videos">视频信息</param>
        /// <param name="userFans">关注列表</param>
        /// <param name="loginUserId">登录用户</param>
        /// <param name="unreadCommentCount">空间未读留言数量</param>
        /// <returns></returns>
        protected AppUserView UserView(int loginUserId, int unreadCommentCount, User user, IQueryable<Video> videos, IQueryable<UserFans> userFans)
        {
            return new AppUserView()
            {
                Id = user.Id,
                Account = user.Account,
                Pwd = user.Password,
                NickName = user.NickName,
                PlayCount = user.PlayCount,
                FansCount = user.FansCount,
                UploadCount = videos == null ? 0 : videos.Count(v => v.State == false && v.VideoSource && v.VideoState == 3 && v.CreateManageId == user.Id),
                CommentCount = unreadCommentCount,
                SkinId = user.SkinId,
                Picture = user.Picture,
                BannerImage = user.BannerImage,
                Bardian = user.Bardian,
                Phone = user.Phone,
                SubscribeNum = user.SubscribeNum,
                Level = user.Level,
                IsSubed = userFans != null && loginUserId > 0 && IsSubed(userFans,user.Id,loginUserId),
                State = user.State
            };
        }
        /// <summary>
        /// 获取用户视图（登录）
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="userFans">关注列表</param>
        /// <param name="loginUserId">登录用户</param>
        /// <returns></returns>
        protected AppUserView UserSimpleView(int loginUserId, User user,  IQueryable<UserFans> userFans)
        {
            return new AppUserView()
            {
                Id = user.Id,
                NickName = user.NickName,
                Picture = user.Picture,
                Bardian = user.Bardian,
                IsSubed = userFans != null && loginUserId > 0 && IsSubed(userFans, user.Id, loginUserId)
            };
        }

        /// <summary>
        /// 获取用户视图(未登录)
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns></returns>
        protected AppUserView UserSimpleView(User user)
        {
            return new AppUserView()
            {
                Id = user.Id,
                NickName = user.NickName,
                Picture = user.Picture,
                Bardian = user.Bardian,
                IsSubed = false
            };
        }
        /// <summary>
        /// 获取用户简单视图
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns></returns>
        protected AppUserSimpleView UserEasyView(User user)
        {
            return new AppUserSimpleView()
            {
                Id = user.Id,
                NickName = user.NickName,
                Picture = user.Picture
            };
        }
        #endregion

        #region 获取视频视图

        /// <summary>
        /// 获取视频视图
        /// </summary>
        /// <param name="video">视频信息</param>
        /// <param name="user">上传者</param>
        /// <returns></returns>
        protected AppVideoView VideoView(Video video,User user)
        {
            return new AppVideoView
            {
                Id = (int)video.Id,
                Title = video.Title,
                About = video.Content,
                SmallPicturePath = video.SmallPicturePath,
                BigPicturePath = video.BigPicturePath,
                VideoPath = video.VideoPath,
                PlayCount = video.PlayCount,
                CollectionCount = video.CollectionCount,
                CommentCount = video.CommentCount,
                CreateTime = video.CreateTime,
                UserInfo =user==null?new AppUserSimpleView() : UserEasyView(user),
                UpdateTime = video.UpdateTime.HasValue ? Convert.ToDateTime(video.UpdateTime) : video.CreateTime,
                TimeLength = video.TimeLength.HasValue ? Convert.ToInt16(video.TimeLength) : 0,
                DownloadPath = ((Autofac.IContainer)StaticObj.Container).Resolve<IQiniuUploadBusiness>().GetDownloadUrl(video.DownloadPath, "video")
            };
        }

        /// <summary>
        /// 获取视频视图
        /// </summary>
        /// <param name="video">视频信息</param>
        /// <returns></returns>
        protected AppVideoView VideoView(Video video)
        {
            return VideoView(video,null);
        }
        #endregion

        #region 获取评论视图

        /// <summary>
        /// 获取评论视图
        /// </summary>
        /// <param name="comment">评论信息</param>
        /// <param name="fromUser">发送人</param>
        /// <param name="toUser">接收人</param>
        /// <returns></returns>
        protected AppCommentView CommentView(Comments comment, User fromUser, User toUser)
        {
            return new AppCommentView()
            {
                Id = comment.Id,
                Content = comment.Content,
                LocalPath = comment.LocalPath,
                CreateTime =TimeSpan(comment.CreateTime),
                FromUser = UserEasyView(fromUser),
                ToUser = toUser == null ? new AppUserSimpleView() : UserEasyView(toUser)
            };
        }
        #endregion

        #region 获取用户喜欢视图
        /// <summary>
        /// 获取用户喜欢视图
        /// </summary>
        /// <param name="userCollect">喜欢信息</param>
        /// <param name="user">喜欢用户信息</param>
        /// <param name="video">喜欢视频信息</param>
        /// <returns></returns>
        protected AppUserCollectionView UserCollectionView(UserCollect userCollect, User user, Video video)
        {
            return new AppUserCollectionView()
            {
                Id = userCollect.Id,
                CreateTime = Convert.ToDateTime(userCollect.CreateTime),
                UserInfo = UserEasyView(user),
                VideoInfo = VideoView(video)
            };
        }
        #endregion

        #region 获取用户粉丝视图

        /// <summary>
        /// 获取用户粉丝视图
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="uploadCount">最近上传数量</param>
        /// <param name="userFans">粉丝信息</param>
        /// <param name="user">关注的用户</param>
        /// <param name="subscribeUser">被关注的用户</param>
        /// <param name="userFanses">粉丝列表</param>
        /// <returns></returns>
        protected AppUserFansView UserFansView(int loginUserId, int uploadCount, UserFans userFans, User user, User subscribeUser, IQueryable<UserFans> userFanses)
        {
            return new AppUserFansView()
            {
                Id = userFans.Id,
                UploadCount = uploadCount,
                IsSubed = userFanses != null && loginUserId>0 && IsSubed(userFanses, subscribeUser.Id, loginUserId),
                SubscribeUser = UserEasyView(subscribeUser),
                UserInfo = UserEasyView(user)
            };
        }
        /// <summary>
        /// 获取用户粉丝视图
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="userFans">粉丝信息</param>
        /// <param name="user">关注的用户</param>
        /// <param name="subscribeUser">被关注的用户</param>
        /// <param name="userFanses">粉丝列表</param>
        /// <returns></returns>
        protected AppUserFansView UserFansView(int loginUserId, UserFans userFans, User user, User subscribeUser, IQueryable<UserFans> userFanses)
        {
            return UserFansView(loginUserId, 0, userFans, user, subscribeUser, userFanses);
        }
        /// <summary>
        /// 获取用户粉丝视图
        /// </summary>
        /// <param name="uploadCount">上传数量</param>
        /// <param name="userFans">粉丝信息</param>
        /// <param name="user">关注的用户</param>
        /// <param name="subscribeUser">被关注的用户</param>
        /// <returns></returns>
        protected AppUserFansView UserFansView(int uploadCount, UserFans userFans, User user, User subscribeUser)
        {
            return UserFansView(0, uploadCount, userFans, user, subscribeUser, null);
        }
        #endregion

        #region 获取分页

        /// <summary>
        /// 获取分页视频
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="pageSize">显示多少条</param>
        /// <param name="pageIndex">显示第几页</param>
        /// <returns></returns>
        protected IList<T> PageList<T>(IQueryable<T> source, int pageSize, int pageIndex)
        {
            IList<T> list = new List<T>();
            if (source.Any())
            {
                pageSize = pageSize <= 0 ? 5 : pageSize;
                int totalCount = source.Any() ? source.Count() : 0;
                int totalIndex = totalCount % pageSize == 0 ? (totalCount / pageSize) : (totalCount / pageSize + 1);
                pageIndex = pageIndex <= 0 ? 1 : (pageIndex >= totalIndex ? totalIndex : pageIndex);
                list = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            return list;
        }
        /// <summary>
        /// 获取分页视频
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="pageSize">显示多少条</param>
        /// <param name="pageIndex">显示第几页</param>
        /// <returns></returns>
        protected PageResult PageResult<T>(IQueryable<T> source, int pageSize, int pageIndex)
        {
            PageResult pageResult = new PageResult();
            if (source.Any())
            {
                pageSize = pageSize <= 0 ? 5 : pageSize;
                int totalCount = source.Any() ? source.Count() : 0;
                int totalIndex = totalCount % pageSize == 0 ? (totalCount / pageSize) : (totalCount / pageSize + 1);
                pageIndex = pageIndex <= 0 ? 1 : (pageIndex >= totalIndex ? totalIndex : pageIndex);
                pageResult.PageIndex = pageIndex;
                pageResult.PageSize = pageSize;
                pageResult.TotalCount = totalCount;
                pageResult.TotalIndex = totalIndex;
                pageResult.Data = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            return pageResult;
        }
        #endregion

    }
}
