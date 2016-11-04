
using System;
using System.Data;
using System.Collections.Generic;
using HKSJ.WBVV.Business.Interface.Base;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.Common.Extender.LinqExtender;

namespace HKSJ.WBVV.Business.Interface
{

    public interface IUserRoomChooseBusiness : IBaseBusiness
    {
        IList<VideoView> GetUserRoomVideoData(int userId, int dataNum);
        PageResult GetUserVideoList(int userId, IList<Condtion> condtions, IList<OrderCondtion> orderCondtions);
        SpecialView GetUserRoomSpecialData(int userId, int dataNum, int videoNum = 0);
        int AddVideoToUserRoom(int userId, string videoIds);
        bool RemoveVideoToUserRoom(int userId, int videoId);
        int AddAlbumToUserRoom(int userId, string albumIds);
        bool RemoveAlbumtToUserRoom(int userId, int albumId);
        MyVideoViewResult GetUserVideoViews(int userId, int pageIndex, int pageSize);
        SpecialView GetUserAlbumsViews(int userId, int pageIndex, int pageSize);

    }
}