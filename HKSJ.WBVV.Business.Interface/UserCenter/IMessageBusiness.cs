using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Business.Interface.Base;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity;

namespace HKSJ.WBVV.Business.Interface.UserCenter
{
    public interface IMessageBusiness : IBaseBusiness
    {
        MessageType GetHeaderMessage();
        IList<MessageType> GetMessages();
        PageResult GetPageSystemMessages();
        MessageView GetSystemMessages();
        bool DeleteSystemMessage(int messageId);
        bool ClearSystemMessage();
        PageResult GetPageComments(int parentSize, int childSize);
        MessageView GetComments(int parentSize, int childSize);
        bool DeleteVideoComment(int messageId);
        PageResult GetPageUserMessages(int parentSize, int childSize);
        MessageView GetUserMessages(int parentSize, int childSize);
        bool DeleteSpaceComment(int messageId);
    }
}
