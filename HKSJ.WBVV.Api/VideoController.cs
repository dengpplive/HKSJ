
using System;
using System.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using HKSJ.WBVV.Api.Base;
using HKSJ.WBVV.Common;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Logger;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.WBVV.Entity.ApiParaModel;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Entity.ViewModel.Client;
using Microsoft.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;
using HKSJ.WBVV.Api.Filters;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Api
{
    public class VideoController : ApiControllerBase
    {
        private readonly IVideoBusiness _videoBusiness;
        private readonly IBannerVideoBusiness _bannerVideoBusiness;
        private readonly ICategoryBusiness _categoryBusiness;


        public VideoController(IVideoBusiness videoBusiness, IBannerVideoBusiness bannerVideoBusiness, ICategoryBusiness categoryBusiness)
        {
            this._videoBusiness = videoBusiness;
            this._bannerVideoBusiness = bannerVideoBusiness;
            this._categoryBusiness = categoryBusiness;
        }
        /// <summary>
        /// 首页和一级分类页的的数据
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="isIndexPage"></param>
        /// <returns></returns>
        [HttpGet]
        public RecommendAndHotCategoryVideoView GetCategoryVideoData(int categoryId, bool isIndexPage = false)
        {
            this._videoBusiness.PageSize = PageSize;
            return this._videoBusiness.GetCategoryVideoData(categoryId, isIndexPage);
        }

        /// <summary>
        /// 过滤视频列表
        /// </summary>
        /// <param name="categoryId">分类编号</param>
        /// <param name="filter">过滤条件3c12r 4c19r 5c27r</param>
        /// <param name="sortName">排序类型默认热门</param>
        /// <returns></returns>
        public PageResult GetFilterVideo(string filter, string sortName)
        {
            this._videoBusiness.PageIndex = PageIndex;
            this._videoBusiness.PageSize = PageSize;
            return this._videoBusiness.GetFilterVideo(filter, sortName);
        }

        /// <summary>
        /// 一级分类下的Banner视频
        /// </summary>
        /// <param name="categoryId">分类编号</param>
        /// <returns></returns>
        [HttpPost]
        public IList<VideoView> GetBannerVideoList(int categoryId)
        {
            this._bannerVideoBusiness.PageSize = 100;
            return this._bannerVideoBusiness.GetBannerVideoList(categoryId, this.OrderCondtions);
        }

        /// <summary>
        /// 获取板块分页集合
        /// </summary>
        [HttpPost]
        public PageResult GetVideoPageResult()
        {
            this._videoBusiness.PageIndex = this.PageIndex;
            this._videoBusiness.PageSize = this.PageSize;
            return this._videoBusiness.GetVideoPageResult(this.Condtions, this.OrderCondtions);
        }

        /// <summary>
        /// 获取板块分页集合
        /// </summary>
        [HttpPost]
        public PageResult GetVideoByCategoryIdPageResult(int categoryId)
        {
            this._videoBusiness.PageIndex = this.PageIndex;
            this._videoBusiness.PageSize = this.PageSize;
            return this._videoBusiness.GetVideoByCategoryIdPageResult(categoryId, this.Condtions, this.OrderCondtions);
        }

        /// <summary>
        /// 播放页面视频信息
        /// </summary>
        /// <param name="videoId">视频编号</param>
        /// <returns></returns>
        [HttpGet]
        public VideoDetailView GetVideoDetailView(int videoId)
        {
            return _videoBusiness.GetVideoDetailView(videoId);
        }

        /// <summary>
        /// 播放页面视频信息,并获取登录的用户是否收藏该视频
        /// </summary>
        /// <param name="videoId">视频编号</param>
        /// <param name="userId">当前登录用户</param>
        /// <returns></returns>
        [HttpGet]
        public VideoDetailView GetVideoDetailView(int videoId, int userId)
        {
            return _videoBusiness.GetVideoDetailView(videoId, userId);
        }

        /// <summary>
        /// 根据Id获取一个视频
        /// </summary>
        /// <param name="id">视频编号</param>
        /// <returns></returns>
        [HttpGet]
        public Video GetAVideoById(int id)
        {
            var video = _videoBusiness.GetAVideoInfoById(id);
            return video;
        }

        /// <summary>
        /// 板块视频
        /// </summary>
        /// <param name="plateId"></param>
        /// <returns></returns>
        [HttpGet]
        public IList<VideoView> GetPlateVideo(int plateId)
        {
            this._videoBusiness.PageSize = PageSize;
            return _videoBusiness.GetPlateVideo(plateId);
        }
        /// <summary>
        /// 板块今日推荐
        /// </summary>
        /// <param name="plateId"></param>
        /// <returns></returns>
        [HttpGet]
        public IList<IndexVideoView> GetRecommendPlateVideo(int plateId)
        {
            this._videoBusiness.PageSize = PageSize;
            return _videoBusiness.GetRecommendPlateVideo(plateId);
        }
        /// <summary>
        /// 板块热门精选
        /// </summary>
        /// <param name="plateId"></param>
        /// <returns></returns>
        [HttpGet]
        public IList<IndexVideoView> GetHotPlateVideo(int plateId)
        {
            this._videoBusiness.PageSize = PageSize;
            return _videoBusiness.GetHotPlateVideo(plateId);
        }
        /// <summary>
        /// 推荐和热门板块视频
        /// </summary>
        /// <param name="plateId"></param>
        /// <returns></returns>
        [HttpGet]
        public RecommendAndHotPlateVideoView GetRecommendAndHotPlateVideo(int plateId)
        {
            this._videoBusiness.PageSize = PageSize;
            return _videoBusiness.GetRecommendAndHotPlateVideo(plateId);
        }
        /// <summary>
        /// 一级分类左边
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet]
        public IList<IndexVideoView> GetCategoryVideoLeft(int categoryId)
        {
            this._videoBusiness.PageSize = PageSize;
            return _videoBusiness.GetCategoryVideoLeft(categoryId);
        }
        /// <summary>
        /// 一级分类右边
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet]
        public IList<IndexVideoView> GetCategoryVideoRight(int categoryId)
        {
            this._videoBusiness.PageSize = PageSize;
            return _videoBusiness.GetCategoryVideoRight(categoryId);
        }

        /// <summary>
        /// 分类视频
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet]
        public IList<VideoView> GetCategoryVideo(int categoryId)
        {
            this._videoBusiness.PageSize = PageSize;
            return _videoBusiness.GetCategoryVideo(categoryId);
        }
        /// <summary>
        /// 分类今日推荐
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet]
        public IList<IndexVideoView> GetRecommendCategoryVideo(int categoryId)
        {
            this._videoBusiness.PageSize = PageSize;
            return _videoBusiness.GetRecommendCategoryVideo(categoryId);
        }
        /// <summary>
        /// 分类热门精选
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet]
        public IList<IndexVideoView> GetHotCategoryVideo(int categoryId)
        {
            this._videoBusiness.PageSize = PageSize;
            return _videoBusiness.GetHotCategoryVideo(categoryId);
        }
        /// <summary>
        /// 推荐和热门分类视频
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet]
        public RecommendAndHotCategoryVideoView GetRecommendAndHotCategoryVideo(int categoryId)
        {
            this._videoBusiness.PageSize = PageSize;
            return _videoBusiness.GetRecommendAndHotCategoryVideo(categoryId);
        }

        /// <summary>
        /// 获取板块下的视频
        /// </summary>
        /// <param name="plateId"></param>
        /// <returns></returns>
        [HttpGet]
        public IList<VideoView> GetPlateVideoList(int plateId)
        {
            return _videoBusiness.GetPlateVideo(plateId);
        }

        #region 视频分页搜索
        /// <summary>
        /// 搜索视频分页
        /// </summary>
        /// <param name="searchKey">搜索关键字</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpGet]
        public dynamic GetSearchListByPage(string searchKey, int pageSize = 10, int pageIndex = 1, SortName sortName = SortName.CreateTime)
        {

            List<VideoView> videolist = new List<VideoView>();
            int _totalcount = 0;
            if (string.IsNullOrEmpty(searchKey))
            {
                return new { msg = LanguageUtil.Translate("api_Controller_Video_GetSearchListByPage_msg") };
            }
            try
            {
                videolist = _videoBusiness.SearchVideoByPage(searchKey, out _totalcount, pageSize, pageIndex, sortName);
                int _totalPage = (int)Math.Ceiling(_totalcount / (pageSize * 1.0));
                return new { msg = "success", page = new { totalNum = _totalcount, totalPage = _totalPage }, listdata = videolist };

            }
            catch (Exception ex)
            {
                return new { msg = ex.Message };
            }

        }
        #endregion

        #region 搜索页头部官方视频搜索
        [HttpGet]
        public dynamic SearchVideoByTop(string searchKey)
        {
            List<VideoView> videoList = new List<VideoView>();
            if (string.IsNullOrEmpty(searchKey))
            {
                return new { msg = LanguageUtil.Translate("api_Controller_Video_SearchVideoByTop_msg") };
            }
            try
            {
                videoList = _videoBusiness.SearchVideoByTop(searchKey);

                return new { msg = "success", topdata = videoList };
            }
            catch (Exception ex)
            {
                return new { msg = ex.Message };
            }

        }
        #endregion

        #region 播放页相关视频推荐
        [HttpGet]
        public dynamic SearchVideoByRecom(string searchKey, int videoId, int recommendationNum = 6)
        {
            List<VideoView> videoList = new List<VideoView>();
            if (string.IsNullOrEmpty(searchKey))
            {
                return new { msg = LanguageUtil.Translate("api_Controller_Video_SearchVideoByRecom_msg") };
            }
            try
            {
                videoList = _videoBusiness.SearchVideoByRecom(searchKey, videoId, recommendationNum);

                return new { msg = "success", topdata = videoList };
            }
            catch (Exception ex)
            {
                return new { msg = ex.Message };
            }

        }
        #endregion


        [HttpGet]
        public string DelAndUpdateAllIndex()
        {
            _videoBusiness.DelAndUpdateAllIndex();
            return "OK";
        }


        #region 删除一个视频信息
        /// <summary>
        /// 删除一个视频信息
        /// </summary>
        /// <param name="vid"></param>
        /// <returns></returns>
        [HttpPost]
        public Result DeleteAVideo()
        {
            return CommonResult(() => this._videoBusiness.DeleteVideoInfo(JObject.Value<int>("vid")), (r) => Console.WriteLine(r.ToJSON()));
        }
        #endregion


        #region 更新一个视频信息
        /// <summary>
        /// 更新一个视频信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result UpdateAVideo()
        {
            var video = JObject.Value<string>("dataModel");
            var myVideoPara = video.FromJSON<MyVideoPara>();
            return CommonResult(() => this._videoBusiness.UpdateAVideo(myVideoPara), (r) => Console.WriteLine((r.ToJSON())));
        }

        #endregion

        [HttpGet]
        public Result GetVideoById(int id)
        {
            return CommonResult(() => this._videoBusiness.GetVideoById(id), (r) => Console.WriteLine((r.ToJSON())));
        }

        #region 获取视频分页集合
        /// <summary>
        /// 获取视频分页集合
        /// </summary>
        [HttpPost]
        public PageResult GetVideosPageResult()
        {
            this._videoBusiness.PageIndex = this.PageIndex;
            this._videoBusiness.PageSize = this.PageSize;
            return this._videoBusiness.GetVideosPageResult(this.Condtions, this.OrderCondtions);
        }
        #endregion

        [HttpGet]
        public string CheckIndexFile()
        {
            try
            {
                _videoBusiness.CheckIndexFile();
            }
            catch (Exception ex)
            {
#if !DEBUG
                     LogBuilder.Log4Net.Error(LanguageUtil.Translate("api_Controller_Video_CheckIndexFile_Error"),ex.MostInnerException());
#else
                Console.WriteLine("索引检测失败" + ex.MostInnerException().Message);
#endif

                return ex.InnerException.Message;
            }
            return "OK";
        }

        [HttpGet]
        public string StartNewThread()
        {
            try
            {
                _videoBusiness.StartNewThread();
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
            return "OK";
        }

        [HttpGet]
        public Result GetYesRecVideos(int num)
        {
            return CommonResult(() => this._videoBusiness.GetYesRecVideos(num), (r) => Console.WriteLine((r.ToJSON())));
        }

        [HttpGet]
        public Result SearchMyVideoByPage(int userId, string mysearchKey, int pageIndex = 1, int pageSize = 5, int videoState=-1)
        {
            return CommonResult(() => this._videoBusiness.SearchMyVideoByPage(userId, mysearchKey, pageIndex, PageSize,videoState),
                (r) => Console.WriteLine(r.ToJSON()));
        }
    }
}