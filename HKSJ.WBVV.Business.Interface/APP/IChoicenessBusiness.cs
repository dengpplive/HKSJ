using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Business.Interface.Base;
using HKSJ.WBVV.Entity.Response.App;

namespace HKSJ.WBVV.Business.Interface.APP
{
    public interface IChoicenessBusiness : IBaseBusiness
    {
        AppChoicenesssView ChoicenessVideos(int? loginUserId, int pageSize, int pageIndex);
        AppChoicenesssView FriendVideos(int loginUserId, int pageSize, int pageIndex);
        AppComments VideoComments(int videoId, int pageSize, int pageIndex);
        AppCommentsView VideoComments(int videoId, int pid, int pageSize, int pageIndex);
        AppUserCollectionsView VideoCollections(int videoId, int pageSize, int pageIndex);
        AppChoicenesssView Videos(int? loginUserId, int cateId, int pageSize, int pageIndex);
        bool ReportComment(int loginUserId, int commentId);
        bool UserSubscribe(int loginUserId, int subscribeUserId);
        bool UserCancelSubscribe(int loginUserId, int subscribeUserId);
        int CreateVideoComment(int loginUserId, int videoId, string content);
        int ReplyVideoComment(int loginUserId, int videoId, int commentId, string content);
        bool CollectVideo(int loginUserId, int videoId);
        bool UnCollectVideo(int loginUserId, int videoId);
    }
}
