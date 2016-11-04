
using System;
using System.Data;
using System.Collections.Generic;
using HKSJ.WBVV.Business.Interface.Base;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.Entity.ViewModel.Manage;
using HKSJ.WBVV.Common.Extender.LinqExtender;

namespace HKSJ.WBVV.Business.Interface
{
    /// <summary>
    /// ”√ªß¡Ù—‘
    /// </summary>
    public interface IUserMessageBusiness : IBaseBusiness
    {
        bool ReadUserMessage(int messageId);
        PushMessage PushMessage();
        int Comment(int userId, string messageContent);
        int Reply(int messageId, int userId, string messageContent);
        bool DeleteUserMessage(int id);
        UserMessageDetialsView GetComment(int messageId);
        IList<UserMessageDetialView> GetUserMessageDetialsList(int userId);
        PageResult GetUserMessageViewPageResult(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions);
        PageResult GetUserMessageDetialsPageResult(int userId);
        IList<UserMessageDetialsView> GetParentUserMessageDetials(int userId);

    }
}