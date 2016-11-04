


using System;
using System.Data;
using System.Collections.Generic;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Repository.Interface.IBase;

namespace HKSJ.WBVV.Repository.Interface
{
    public interface IVideoRepository : IBaseAccess<Video>
    {
        void IncomeUpload(string approveContent, string approveRemark, long videoId, bool status, int createAdminId);
        void IncomeWatch(int loginUserId, long videoId, string ipAddress);
        void IncomeShare(int videoId, int demandUserId, string ipAddress, int shareUserId, string shareUserIp);
    }
}