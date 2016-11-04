using System;
using System.Data;
using System.Collections.Generic;
using System.Web.Http;
using HKSJ.WBVV.Common;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Api.Base;
using HKSJ.WBVV.Api.Filters;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.WBVV.Entity.ApiParaModel;
using HKSJ.WBVV.Repository.Interface;
using HKSJ.WBVV.Entity.ViewModel.Client;
using Newtonsoft.Json;

namespace HKSJ.WBVV.Api
{
    public class UserSpecialController : ApiControllerBase
    {

        private readonly IUserSpecialBusiness _userspecialbusiness;
        private readonly IVideoBusiness _videoBusiness;
        public UserSpecialController(IUserSpecialBusiness userspecialbusiness, IVideoBusiness videoBusiness)
        {
            this._userspecialbusiness = userspecialbusiness;
            this._videoBusiness = videoBusiness;
        }

        #region client
        #region 个人专辑管理
        /// <summary>
        /// 获取用户专辑视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public SpecialView GetUserAlbumsViews(int userId)
        {
            this._userspecialbusiness.UserId = userId;
            this._userspecialbusiness.PageIndex = PageIndex;
            this._userspecialbusiness.PageSize = PageSize;
            return this._userspecialbusiness.GetUserAlbumsViews();
        }

        /// <summary>
        /// 获取用户专辑视图 带有分页 排序 条件刷选
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PageResult GetUserAlbumsViewsByOrder()
        {
            int userId = JObject.Value<int>("userId");
            this._userspecialbusiness.PageSize = this.PageSize;
            this._userspecialbusiness.PageIndex = this.PageIndex;
            return this._userspecialbusiness.GetUserAlbumsViewsByOrder(userId, this.Condtions, this.OrderCondtions);
        }

        /// <summary>
        /// 获取用户专辑视图下面的视频  带有分页 排序 条件刷选
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PageResult GetUserAlbumVideosById()
        {
            int userId = JObject.Value<int>("userId");
            int userSpecialId = JObject.Value<int>("userSpecialId");
            this._userspecialbusiness.PageSize = this.PageSize;
            this._userspecialbusiness.PageIndex = this.PageIndex;
            return this._userspecialbusiness.GetUserAlbumVideosById(userId, userSpecialId, this.Condtions, this.OrderCondtions);
        }



        /// <summary>
        /// 创建一个新专辑
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <param name="profile"></param>
        /// <param name="label"></param>
        /// <param name="classify"></param>
        /// <returns></returns>
        [HttpPost]
        public Result AddUserAlbum()
        {
            this._userspecialbusiness.UserId = JObject.Value<int>("userId");
            return CommonResult(() => this._userspecialbusiness.AddUserAlbum(JObject.Value<string>("Title"), JObject.Value<string>("Remark"), JObject.Value<string>("Tag"), JObject.Value<string>("Image")),
                (r) => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 获取编辑时需要的专辑信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="albumsId"></param>
        /// <returns></returns>
        [HttpGet]
        public UserSpecial GetEditUserAlbum(int userId, int albumsId)
        {
            this._userspecialbusiness.UserId = userId;
            return this._userspecialbusiness.GetEditUserAlbum(albumsId);
        }

        /// <summary>
        /// 编辑专辑
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result EditUserAlbum()
        {
            this._userspecialbusiness.UserId = JObject.Value<int>("userId");
            int id = JObject.Value<int>("Id");
            string title = JObject.Value<string>("Title");
            string remark = JObject.Value<string>("Remark");
            string tag = JObject.Value<string>("Tag");
            string image = JObject.Value<string>("Image");

            return CommonResult(() => this._userspecialbusiness.EditUserAlbum(id,title,remark,tag),
                (r) => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 删除专辑
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result DeleteUserAlbum()
        {
            this._userspecialbusiness.UserId = JObject.Value<int>("userId");
            return CommonResult(() => this._userspecialbusiness.DeleteUserAlbum(JObject.Value<int>("albumId")), (r) => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 获取用户专辑视频视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public SpecialDetailView GetUserAlbumVideoViews(int userId, int albumId)
        {
            this._userspecialbusiness.UserId = userId;
            this._userspecialbusiness.PageIndex = PageIndex;
            this._userspecialbusiness.PageSize = PageSize;
            return this._userspecialbusiness.GetUserAlbumVideoViews(albumId);
        }

        /// <summary>
        /// 删除专辑下视频,可批量
        /// </summary>
        /// <returns></returns>
        [HttpPost] //TODO 需要客户端权限认证
        public Result DeleteAlbumVideos()
        {
            this._userspecialbusiness.UserId = JObject.Value<int>("userId");
            return CommonResult(() => this._userspecialbusiness.DeleteAlbumVideos(JObject.Value<int>("albumId"), JObject.Value<string>("videoIds")), (r) => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 添加专辑下视频,可批量
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result AddAlbumVideos()
        {
            this._userspecialbusiness.UserId = JObject.Value<int>("userId");
            return CommonResult(() => this._userspecialbusiness.AddAlbumVideos(JObject.Value<int>("albumId"), JObject.Value<string>("videoIds")), (r) => Console.WriteLine(r.ToJSON()));
        }


        /// <summary>
        /// 获取用户上传视频,过滤已添加专辑的视频
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="escapeStatestate"></param>
        /// <returns></returns>
        [HttpGet]
        public MyVideoViewResult GetUserVideoViews(int userId, int albumId)
        {
            this._userspecialbusiness.UserId = userId;
            this._userspecialbusiness.PageIndex = PageIndex;
            this._userspecialbusiness.PageSize = PageSize;
            return this._userspecialbusiness.GetUserVideoViews(albumId);

        }


        /// <summary>
        /// 获取收藏视频,过滤已添加专辑的视频
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="escapeStatestate"></param>
        /// <returns></returns>
        [HttpGet]
        public MyVideoViewResult GetUserCollectVideoViews(int userId, int albumId)
        {
            this._userspecialbusiness.UserId = userId;
            this._userspecialbusiness.PageIndex = PageIndex;
            this._userspecialbusiness.PageSize = PageSize;
            return this._userspecialbusiness.GetUserCollectVideoViews(albumId);
        }

        /// <summary>
        /// 获取用户专辑视图过滤已添加视频的专辑
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public SpecialView GetUserAlbumsView(int userId,int vid)
        {
            this._userspecialbusiness.UserId = userId;
            this._userspecialbusiness.PageIndex = PageIndex;
            this._userspecialbusiness.PageSize = PageSize;
            return this._userspecialbusiness.GetUserAlbumsViews(vid);
        }

        /// <summary>
        /// 添加视频到专辑，可多选
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result AddVideo2Albums()
        {
            return CommonResult(() => this._userspecialbusiness.AddVideo2Albums(JObject.Value<int>("vid"),JObject.Value<string>("albumIds")), (r) => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 设为封面
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result SetCover()
        {
            this._userspecialbusiness.UserId = JObject.Value<int>("userId");
            return CommonResult(() => this._userspecialbusiness.SetCover(JObject.Value<int>("albumId"), JObject.Value<int>("videoId")), (r) => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 更新专辑封面
        /// </summary>
        /// <returns></returns>
        public Result UpdateAlbumPic()
        {
            this._userspecialbusiness.UserId = JObject.Value<int>("userId");
            return CommonResult(() => this._userspecialbusiness.UpdateAlbumPic(JObject.Value<int>("albumId"), JObject.Value<string>("key")), (r) => Console.WriteLine(r.ToJSON()));
        }

        #endregion


        #region 首页-专辑页面

        /// <summary>
        /// 获取(今日)热门专辑（所有专辑），根据分页返回
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public SpecialView GetAllAlbumsViews()
        {
            this._userspecialbusiness.PageIndex = PageIndex;
            this._userspecialbusiness.PageSize = PageSize;
            return this._userspecialbusiness.GetAllAlbumsViews();
        }

        /// <summary>
        /// 获取推荐专辑列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public SpecialView GetRecommendAlbumsViews()
        {
            this._userspecialbusiness.PageIndex = PageIndex;
            this._userspecialbusiness.PageSize = PageSize;
            return this._userspecialbusiness.GetRecommendAlbumsViews();
        }

        /// <summary>
        /// 获取专辑视频视图
        /// </summary>
        /// <param name="albumId"></param>
        /// <returns></returns>
        [HttpGet]
        public SpecialDetailView GetAlbumVideoViews(int albumId, string isHot)
        {
            this._userspecialbusiness.PageIndex = PageIndex;
            this._userspecialbusiness.PageSize = PageSize;
            return this._userspecialbusiness.GetAlbumVideoViews(albumId, isHot);
        }

        #endregion

        #endregion


        #region manage
        /// <summary>
        /// 获取所有专辑
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PageResult GetRecommendAlbumsPageResult()
        {
            this._userspecialbusiness.PageIndex = this.PageIndex;
            this._userspecialbusiness.PageSize = this.PageSize;
            return this._userspecialbusiness.GetRecommendAlbumsPageResult(this.Condtions, this.OrderCondtions);
        }
        /// <summary>
        /// 添加推荐专辑
        /// </summary>
        /// <returns></returns>
        [HttpPost, CheckApp] //TODO 需要客户端权限认证
        public Result AddRecommendAlbums()
        {
            return CommonResult(() => this._userspecialbusiness.AddRecommendAlbums(JObject.Value<string>("albumIds")), (r) => Console.WriteLine(r.ToJSON()));
        }

        /// <summary>
        /// 移除推荐专辑
        /// Author : AxOne
        /// </summary>
        /// <returns></returns>
        [HttpPost, CheckApp] //TODO 需要客户端权限认证
        public Result RemoveRecommendAlbums()
        {
            return CommonResult(() => this._userspecialbusiness.RemoveRecommendAlbums(JObject.Value<string>("albumIds")), (r) => Console.WriteLine(r.ToJSON()));
        }


        /// <summary>
        /// 保存推荐专辑排序
        /// </summary>
        /// <returns></returns>
        [HttpPost, CheckApp] //TODO 需要客户端权限认证
        public Result SavaRecommendAlbumsSort()
        {
            return CommonResult(() => this._userspecialbusiness.SavaRecommendAlbumsSort(JObject.Value<string>("albumIds"), JObject.Value<string>("sortNums")), (r) => Console.WriteLine(r.ToJSON()));
        }

        #endregion

    }
}
