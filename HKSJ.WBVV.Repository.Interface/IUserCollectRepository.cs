using System;
using System.Data;
using System.Collections.Generic;
using HKSJ.WBVV.Repository.Interface.IBase;
using HKSJ.WBVV.Entity;

namespace HKSJ.WBVV.Repository.Interface
{
    public interface IUserCollectRepository : IBaseAccess<UserCollect>
    {
        bool UnCollectVideoTransaction(UserCollect userCollect, Video video);
        bool CollectVideo(int videoId, int userId);
        bool UnCollectVideo(int id, int userId, int videoId);
        bool UnCollectVideo(int userId, int videoId);
        bool DelAllCollectVideo(int userId);
    }
}