
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
    /// 用户专辑
    /// </summary>
    public interface IUserSpecialBusiness : IBaseBusiness
    {
        //----------------------------------------------------个人专辑管理 begin------------------------------------------------//
        SpecialView GetUserAlbumsViews();
        SpecialView GetUserAlbumsViews(int vid);
        int AddUserAlbum(string title, string profile, string label,string image);
        UserSpecial GetEditUserAlbum(int albumsId);
        bool EditUserAlbum(int albumId, string title, string remark, string tag);
        bool DeleteUserAlbum(int albumId);
        SpecialDetailView GetUserAlbumVideoViews(int albumId);
        bool DeleteAlbumVideos(int albumId, string videoIds);
        bool AddAlbumVideos(int albumId, string videoIds);
        MyVideoViewResult GetUserVideoViews(int albumId);
        MyVideoViewResult GetUserCollectVideoViews(int albumId);
        bool SetCover(int albumId, int videoId);
        bool UpdateAlbumPic(int albumId, string key);
        bool AddVideo2Albums(int vid, string albumsId);

        //----------------------------------------------------首页-专辑页面 end------------------------------------------------//

        //----------------------------------------------------首页-专辑页面 begin------------------------------------------------//

        SpecialView GetAllAlbumsViews();
        SpecialView GetRecommendAlbumsViews();
        SpecialDetailView GetAlbumVideoViews(int albumId, string isHot);

        //----------------------------------------------------首页-专辑页面 end------------------------------------------------//



        //----------------------------------------------------后台-获取推荐专辑 begin------------------------------------------------//

        PageResult GetRecommendAlbumsPageResult(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions);
        bool AddRecommendAlbums(string albumIds, int limitCount = 3);
        bool RemoveRecommendAlbums(string albumIds);
        bool SavaRecommendAlbumsSort(string albumIds, string sortNums);

        //----------------------------------------------------后台-获取推荐专辑 end------------------------------------------------//


        //----------------------------------------------------个人空间 专辑 begin------------------------------------------------//
        PageResult GetUserAlbumsViewsByOrder(int userId, IList<Condtion> condtions, IList<OrderCondtion> orderCondtions);
        PageResult GetUserAlbumVideosById(int userId, int userSpecialId, IList<Condtion> condtions, IList<OrderCondtion> orderCondtions);

        //----------------------------------------------------个人空间 专辑  end------------------------------------------------//

    }
}