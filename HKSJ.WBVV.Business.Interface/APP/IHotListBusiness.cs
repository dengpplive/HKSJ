using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Business.Interface.Base;
using HKSJ.WBVV.Entity.Response.App;

namespace HKSJ.WBVV.Business.Interface.APP
{
    public interface IHotListBusiness : IBaseBusiness
    {
        AppUsersView UploadRank(int? loginUserId, int pageSize, int pageIndex);
        AppSerachView Serach(string content, int pageSize, int pageIndex);
        AppVideoView VideoPrereview(int loginUserId);
        bool Bad(int loginUserId, int videoId);
        bool Good(int loginUserId, int videoId);
        bool ReportVideo(int loginUserId, int videoId);
        AppSerachVideoView SerachVideo(string content, int pageSize, int pageIndex);
        AppSerachUserView SerachUser(int? loginUserId, string content, int pageSize, int pageIndex);
    }
}
