using System;
using System.Data;
using System.Collections.Generic;
using HKSJ.WBVV.Repository.Interface.IBase;
using HKSJ.WBVV.Entity;

namespace HKSJ.WBVV.Repository.Interface
{
    public interface  IVideoPrereviewRepository:IBaseAccess<VideoPrereview>
    {
        bool Bad(int loginUserId, int videoId);
        bool Good(int loginUserId, int videoId);
    }
}