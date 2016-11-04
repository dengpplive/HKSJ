using System;
using System.Data;
using System.Collections.Generic;
using HKSJ.WBVV.Repository.Interface.IBase;
using HKSJ.WBVV.Entity;

namespace HKSJ.WBVV.Repository.Interface
{
    public interface IUserReportRepository : IBaseAccess<UserReport>
    {
        bool ReportVideo(int loginUserId, int videoId);
        bool ReportComment(int loginUserId, int commentId);
    }
}