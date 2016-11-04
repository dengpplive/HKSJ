
using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using HKSJ.WBVV.Business.Interface.Base;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ApiParaModel;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Common;
using HKSJ.WBVV.Entity.ViewModel.Client;
using Newtonsoft.Json.Linq;
using VideoView = HKSJ.WBVV.Entity.ViewModel.Client.VideoView;
using HKSJ.WBVV.Entity.ViewModel.Manage;
using HKSJ.WBVV.Common.Extender.LinqExtender;

namespace HKSJ.WBVV.Business.Interface
{
    public interface IVideoBusiness : IBaseBusiness
    {
        PageResult GetFilterVideo(string filter, string sortName);
        VideoDetailView GetVideoDetailView(int videoId, int userId = 0);
        int GetPlayCountByUserId(int userId);
        IList<IndexVideoView> GetCategoryVideoLeft(int categoryId);
        IList<IndexVideoView> GetCategoryVideoRight(int categoryId);
        IList<VideoView> GetPlateVideo(int plateId);
        IList<IndexVideoView> GetHotPlateVideo(int plateId);
        IList<IndexVideoView> GetRecommendPlateVideo(int plateId);
        RecommendAndHotPlateVideoView GetRecommendAndHotPlateVideo(int plateId);
        IList<VideoView> GetCategoryVideo(int categoryId);
        IList<IndexVideoView> GetHotCategoryVideo(int categoryId);
        IList<IndexVideoView> GetRecommendCategoryVideo(int categoryId);
        RecommendAndHotCategoryVideoView GetRecommendAndHotCategoryVideo(int categoryId);
        List<VideoView> SearchVideoByPage(string searchKey, out int totalcount, int pageSize = 10, int pageIndex = 1, SortName sortName = 0);
        List<VideoView> SearchVideoByTop(string searchKey);
        List<VideoView> SearchVideoByRecom(string searchKey, int videoId, int recommendationNum = 6);
        void DelAndUpdateAllIndex();
        Video GetAVideoInfoById(int id);
        bool UpdateAVideo(MyVideoPara videoPara);
        PageResult GetVideoPageResult(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions);
        PageResult GetVideoByCategoryIdPageResult(int categoryId, IList<Condtion> condtions, IList<OrderCondtion> orderCondtions);
        bool DeleteVideoInfo(int id);
        string GetUserPicUrl(int uid);
        PageResult GetVideosPageResult(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions);
        VideoView GetVideoById(long id);

        void CheckIndexFile();
        void StartNewThread();
        void UpdateAVideoIndex(Video videoInfo);
        List<VideoView> GetYesRecVideos(int num);
        void AddAVideoIndex(Video videoInfo);
        MyVideoViewResult SearchMyVideoByPage(int userId, string searchKey, int pageIndex, int pageSize, int videoState);

        /// <summary>
        /// 首页和一级分类页的数据
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="isIndexPage"></param>
        /// <returns></returns>
        RecommendAndHotCategoryVideoView GetCategoryVideoData(int categoryId, bool isIndexPage = false);
    }
}