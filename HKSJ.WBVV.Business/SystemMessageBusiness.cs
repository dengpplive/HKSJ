using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity.Enums;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity.ViewModel.Manage;
using HKSJ.WBVV.Repository;
using HKSJ.WBVV.Repository.Interface;
using HKSJ.WBVV.Entity;
using SystemMessageView = HKSJ.WBVV.Entity.ViewModel.Client.SystemMessageView;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Business
{
    public class SystemMessageBusiness : BaseBusiness, ISystemMessageBusiness
    {
        private readonly IUserRepository _userRepository;
        private readonly IManageRepository _manageRepository;
        private readonly IMessageReadRepository _messageReadRepository;
        private readonly ISysMessageRepository _sysMessageRepository;
        public SystemMessageBusiness(
            IUserRepository userRepository,
            IManageRepository manageRepository,
            IMessageReadRepository messageReadRepository, ISysMessageRepository sysMessageRepository)
        {
            this._userRepository = userRepository;
            this._manageRepository = manageRepository;
            this._messageReadRepository = messageReadRepository;
            _sysMessageRepository = sysMessageRepository;
        }

        #region manage

        private List<string> GetUserByType(string userby)
        {
            var list = new List<string>();
            if (string.IsNullOrEmpty(userby))
            {
                return list;
            }
            if (userby.IndexOf('|') == -1)
            {
                var user = this._userRepository.GetEntity(ConditionEqualId(Convert.ToInt32(userby)));
                if (user != null)
                {
                    var name = user.Account + GetName(user.NickName);
                    list.Add(name);
                    return list;
                }
                return list;
            }
            var arr = userby.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length <= 0)
            {
                return list;
            }
            var users = this._userRepository.GetEntityList().Where(u => arr.Contains(u.Id.ToString()));
            if (!users.Any())
            {
                return list;
            }
            list.AddRange(users.Select(user => user.Account + GetName(user.NickName)));
            return list;
        }

        private string GetName(string nickName)
        {
            if (string.IsNullOrEmpty(nickName))
            {
                return "";
            }
            return "(" + nickName + ")";
        }

        #region 推送消息的用户信息和剩余的用户信息
        /// <summary>
        /// 推送消息的用户信息和剩余的用户信息
        /// </summary>
        /// <param name="userBy">推送用户（1|2|3|4）</param>
        /// <returns></returns>
        public SystemMessageUserView GetUsers(string userBy)
        {
            var systemMessageUser = new SystemMessageUserView();
            if (string.IsNullOrEmpty(userBy))
            {
                systemMessageUser.SelectUser = new List<SystemMessageUser>();
                systemMessageUser.SurplusUser = this._userRepository.GetEntityList(CondtionEqualState()).Select(u => new SystemMessageUser()
                {
                    UserId = u.Id,
                    UserName = u.Account + GetName(u.NickName)
                }).ToList();
            }
            else
            {
                var userIds = SplitUserBy(userBy);
                if (userIds.Count > 0)
                {
                    //选中的用户
                    systemMessageUser.SelectUser =
                        this._userRepository.GetEntityList(CondtionEqualState())
                            .Where(u => userIds.Contains(u.Id))
                            .Select(u => new SystemMessageUser()
                            {
                                UserId = u.Id,
                                UserName = u.Account + GetName(u.NickName)
                            }).ToList();
                    //剩余的用户
                    systemMessageUser.SurplusUser =
                       this._userRepository.GetEntityList(CondtionEqualState())
                            .Where(u => !userIds.Contains(u.Id))
                            .Select(u => new SystemMessageUser()
                            {
                                UserId = u.Id,
                                UserName = u.Account + GetName(u.NickName)
                            }).ToList();
                }
                else
                {
                    systemMessageUser.SelectUser = new List<SystemMessageUser>();
                    systemMessageUser.SurplusUser = this._userRepository.GetEntityList(CondtionEqualState()).Select(u => new SystemMessageUser()
                    {
                        UserId = u.Id,
                        UserName = u.Account + GetName(u.NickName)
                    }).ToList();
                }
            }
            return systemMessageUser;
        }
        #endregion

        #region 查看系统消息
        /// <summary>
        /// 查看系统消息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysMessage GetSystemMessage(int id)
        {
            SysMessage systemMessage = new SysMessage();
            if (id <= 0)
            {
                return systemMessage;
            }
            systemMessage = this._sysMessageRepository.GetEntity(ConditionEqualId(id));
            return systemMessage;
        }
        #endregion

        #region 系统消息分页
        /// <summary>
        /// 系统消息列表
        /// </summary>
        /// <param name="condtions"></param>
        /// <param name="orderCondtions"></param>
        /// <param name="totalCount"></param>
        /// <param name="totalIndex"></param>
        /// <returns></returns>
        public IList<HKSJ.WBVV.Entity.ViewModel.Manage.SystemMessageView> GetSystemMessageList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions, out int totalCount, out int totalIndex)
        {
            var systemmessageview = (from sm in this._sysMessageRepository.GetEntityList(CondtionEqualState())
                                     join m in this._manageRepository.GetEntityList() on sm.CreateManageId equals m.Id
                                     where  sm.EntityType==(int)SysMessageEnum.SystemMessage
                                     orderby sm.CreateTime descending 
                                     select new HKSJ.WBVV.Entity.ViewModel.Manage.SystemMessageView()
                                     {
                                         Id = sm.Id,
                                         MessageDesc = sm.Content,
                                         CreateTime = sm.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                         LoginName = m.LoginName,
                                         UserBy = GetUserByType(sm.ToUserIds),
                                         UserByType = sm.SendType
                                     }).AsQueryable();
            if (condtions != null && condtions.Count > 0)//查询条件
            {
                systemmessageview = systemmessageview.Query(condtions);
            }
            if (orderCondtions != null && orderCondtions.Count > 0)//排序条件
            {
                systemmessageview = systemmessageview.OrderBy(orderCondtions);
            }
            bool isExists = systemmessageview.Any();
            totalCount = isExists ? systemmessageview.Count() : 0;
            if (this.PageSize <= 0)
            {
                totalIndex = 0;
                var queryable = isExists
                    ? systemmessageview.ToList()
                    : new List<Entity.ViewModel.Manage.SystemMessageView>();
                return queryable;
            }
            else
            {
                totalIndex = totalCount % this.PageSize == 0
                    ? (totalCount / this.PageSize)
                    : (totalCount / this.PageSize + 1);
                if (this.PageIndex <= 0)
                {
                    this.PageIndex = 1;
                }
                if (this.PageIndex >= totalIndex)
                {
                    this.PageIndex = totalIndex;
                }

                var queryable = isExists
                    ? systemmessageview.Skip((this.PageIndex - 1) * this.PageSize).Take(this.PageSize).ToList()
                    : new List<Entity.ViewModel.Manage.SystemMessageView>();

                return queryable;
            }
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="condtions"></param>
        /// <param name="orderCondtions"></param>
        /// <returns></returns>
        public PageResult GetSystemMessagePageResult(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            int totalCount = 0;
            int totalIndex = 0;
            IList<HKSJ.WBVV.Entity.ViewModel.Manage.SystemMessageView> plateViews = GetSystemMessageList(condtions, orderCondtions, out totalCount, out totalIndex);
            return new PageResult()
            {
                PageSize = this.PageSize,
                PageIndex = this.PageIndex,
                TotalCount = totalCount,
                TotalIndex = totalIndex,
                Data = plateViews
            };
        }
        #endregion

        #region 添加系统消息
        /// <summary>
        /// 添加系统消息
        /// </summary>
        /// <param name="userByType">消息类型</param>
        /// <param name="userBy">推送给选中用户</param>
        /// <param name="messageDesc">消息内容</param>
        /// <returns></returns>
        public int CreateSystemMessage(short userByType, string userBy, string messageDesc)
        {
            CheckUserId();
            CheckUserByType(userByType);
            if (userByType != 3)
            {
                CheckUserBy(userBy);
            }
            CheckMessageDesc(messageDesc);
            Manage manage;
            CheckUserId(out manage);
            var systemMessage = new SysMessage()
            {
                CreateTime = DateTime.Now,
                CreateManageId = manage.Id,
                Content = messageDesc,
                SendType = userByType,
                EntityType = (short)SysMessageEnum.SystemMessage,
                EntityId = 0,
                ToUserIds = userBy
            };
            this._sysMessageRepository.CreateEntity(systemMessage);
            return systemMessage.Id;
        }

        #endregion

        #endregion


        #region 传入参数检测
        /// <summary>
        /// 检测消息类型不能小于0
        /// </summary>
        /// <param name="userByType"></param>
        private void CheckUserByType(short userByType)
        {
            SystemMessageEnum systemMessageEnum;
            Enum.TryParse<SystemMessageEnum>(userByType.ToString(), out systemMessageEnum);
            int type = (int)systemMessageEnum;
            AssertUtil.AreBigger(type, 0, LanguageUtil.Translate("api_Business_SystemMessage_CheckUserByType"));
        }
        /// <summary>
        /// 检测推送人不能为空
        /// </summary>
        /// <param name="userBy"></param>
        private void CheckUserBy(string userBy)
        {
            AssertUtil.NotNullOrWhiteSpace(userBy, LanguageUtil.Translate("api_Business_SystemMessage_CheckUserBy"));
        }
        /// <summary>
        /// 检测消息不能为空
        /// </summary>
        /// <param name="messageDesc"></param>
        private void CheckMessageDesc(string messageDesc)
        {
            AssertUtil.NotNullOrWhiteSpace(messageDesc, LanguageUtil.Translate("api_Business_SystemMessage_CheckMessageDesc"));
        }
        /// <summary>
        /// 检测用户编号不能小于0
        /// </summary>
        /// <param name="userId"></param>
        private void CheckUserId()
        {
            AssertUtil.AreBigger(UserId, 0, LanguageUtil.Translate("api_Business_SystemMessage_CheckUserId"));
        }
        /// <summary>
        /// 检测用户是否存在
        /// </summary>
        /// <param name="manage"></param>
        private void CheckUserId(out Manage manage)
        {
            IList<Condtion> condtions = new List<Condtion>()
            {
                CondtionEqualState(),
                ConditionEqualId(UserId)
            };
            manage = this._manageRepository.GetEntity(condtions);
            AssertUtil.IsNotNull(manage, LanguageUtil.Translate("api_Business_SystemMessage_CheckUserId_manage"));
        }
        #endregion

        #region 传入参数


        #endregion

        #region 排序参数


        #endregion
    }
}
