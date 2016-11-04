using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity.ViewModel.Manage;
using HKSJ.WBVV.Repository.Interface;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Business
{
    public class PlateVideoBusiness : BaseBusiness, IPlateVideoBusiness
    {
        private readonly IPlateVideoRepository _plateVideoRepository;
        private readonly IPlateRepository _plateRepository;
        private readonly IVideoRepository _videoRepository;
        private readonly ICategoryRepository _categoryRepository;
        public PlateVideoBusiness(
            IPlateVideoRepository plateVideoRepository,
            IPlateRepository plateRepository,
            IVideoRepository videoRepository,
            ICategoryRepository categoryRepository
            )
        {
            this._plateVideoRepository = plateVideoRepository;
            this._plateRepository = plateRepository;
            this._videoRepository = videoRepository;
            this._categoryRepository = categoryRepository;
        }

        #region manage

        private IQueryable<PlateVideo> GetPlateVideoList()
        {
            return this._plateVideoRepository.GetEntityList();
        }
        private IQueryable<Plate> GetPlateList()
        {
            return this._plateRepository.GetEntityList();
        }
        private IQueryable<Video> GetVideoList()
        {
            return this._videoRepository.GetEntityList();
        }
        private IQueryable<Category> GetCategoryList()
        {
            return this._categoryRepository.GetEntityList();
        }

        #region 板块视频列表
        /// <summary>
        /// 板块分页显示
        /// </summary>
        /// <param name="condtions"></param>
        /// <param name="orderCondtions"></param>
        /// <returns></returns>
        public PageResult GetPlateVideoPageResult(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            int totalCount = 0;
            int totalIndex = 0;
            IList<PlateVideoView> plateViews = GetPlateVideoList(condtions, orderCondtions, out totalCount, out totalIndex);
            return new PageResult()
            {
                PageSize = this.PageSize,
                PageIndex = this.PageIndex,
                TotalCount = totalCount,
                Data = plateViews
            };
        }
        /// <summary>
        /// 板块视频列表
        /// </summary>
        /// <param name="condtions"></param>
        /// <param name="orderCondtions"></param>
        /// <param name="totalCount"></param>
        /// <param name="totalIndex"></param>
        /// <returns></returns>
        public IList<PlateVideoView> GetPlateVideoList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions,
            out int totalCount, out int totalIndex)
        {
            var plateVideo = (from pv in GetPlateVideoList()
                              join p in GetPlateList() on pv.PlateId equals p.Id
                              into pJoin
                              from plate in pJoin.DefaultIfEmpty()
                              join v in GetVideoList() on pv.VideoId equals v.Id
                              into vJoin
                              from video in vJoin.DefaultIfEmpty()
                              join c in GetCategoryList() on pv.CategoryId equals c.Id
                              into mmJoin
                              from cate in mmJoin.DefaultIfEmpty()
                              select new PlateVideoView()
                              {
                                  Id = pv.Id,
                                  PlateName = plate == null ? (cate != null ? cate.Name : "") + LanguageUtil.Translate("api_Business_PlateVideo_PlateName") : plate.Name,
                                  PlateId = plate == null ? 0 : plate.Id,
                                  VideoId = video != null ? video.Id : 0,
                                  VideoImage = video != null ? video.SmallPicturePath : "",
                                  VideoTitle = video != null ? video.Title : "",
                                  SortNum = pv.SortNum,
                                  IsHot = pv.IsHot ? LanguageUtil.Translate("api_Business_PlateVideo_IsHot_yes") : LanguageUtil.Translate("api_Business_PlateVideo_IsHot_no"),
                                  IsRecommend = pv.IsRecommend ? LanguageUtil.Translate("api_Business_PlateVideo_IsRecommend_yes") : LanguageUtil.Translate("api_Business_PlateVideo_IsRecommend_no"),
                                  CategoryName = plate == null ? (cate != null ? cate.Name : "") : (plate.CategoryId == 0 ? LanguageUtil.Translate("api_Business_PlateVideo_CategoryName") : GetCategory(plate.CategoryId).Name),
                                  CreateTime = pv.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")
                              }
                             );
            if (condtions != null && condtions.Count > 0)//查询条件
            {
                plateVideo = plateVideo.Query(condtions);
            }
            if (orderCondtions != null && orderCondtions.Count > 0)//排序条件
            {
                plateVideo = plateVideo.OrderBy(orderCondtions);
            }
            bool isExists = plateVideo.Any();
            totalCount = isExists ? plateVideo.Count() : 0;
            if (this.PageSize <= 0)
            {
                totalIndex = 0;
                var queryable = isExists
                    ? plateVideo.ToList()
                    : new List<PlateVideoView>();
                return queryable;
            }
            else
            {
                totalIndex = totalCount % this.PageSize == 0
                    ? (totalCount / this.PageSize)
                    : (totalCount / this.PageSize + 1);
                if (this.PageIndex <= 0)
                {
                    this.PageIndex = 1;
                }
                if (this.PageIndex >= totalIndex)
                {
                    this.PageIndex = totalIndex;
                }

                var queryable = isExists
                    ? plateVideo.Skip((this.PageIndex - 1) * this.PageSize).Take(this.PageSize).ToList()
                    : new List<PlateVideoView>();

                return queryable;
            }
        }


        #endregion

        #region 查询分类信息
        /// <summary>
        /// 单个分类信息
        /// </summary>
        /// <returns></returns>
        public Category GetCategory(int id)
        {
            return id <= 0 ? null : this._categoryRepository.GetEntity(ConditionEqualId(id));
        }

        #endregion

        #region 单个板块视频信息
        /// <summary>
        /// 单个板块视频信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PlateVideo GetPlateVideo(int id)
        {
            return id <= 0 ? new PlateVideo() : this._plateVideoRepository.GetEntity(ConditionEqualId(id));
        }

        #endregion

        #region 添加自定义板块视频
        /// <summary>
        /// 添加自定义板块视频
        /// </summary>
        /// <param name="plateId">板块ID</param>
        /// <param name="videoId">视频ID</param>
        /// <returns></returns>
        public int CreatePlateVideo(int plateId, int sortNum, int isHot, int isRecommend, int videoId)
        {
            CheckPlateId(plateId);
            CheckVideoId(videoId);
            Plate plate;
            CheckPlateId(plateId, out plate);
            Video video;
            CheckVideoId(videoId, out video);
            PlateVideo pv;
            CheckPlateVideoId(plateId, videoId, out pv);//检测板块视频信息是否已经存在
            var plateVideo = new PlateVideo()
            {
                PlateId = plate.Id,
                VideoId = video.Id,
                SortNum = sortNum,
                IsHot = isHot == 1,
                IsRecommend = isRecommend == 1,
                CreateTime = DateTime.Now,
                CreateManageId = UserId
            };
            this._plateVideoRepository.CreateEntity(plateVideo);
            return plateVideo.Id;
        }
        #endregion

        #region 添加系统板块视频
        /// <summary>
        /// 添加系统板块视频
        /// </summary>
        /// <param name="categoryId">一级分类ID</param>
        /// <param name="videoId">视频ID</param>
        /// <returns></returns>
        public int CreatePlateCategoryVideo(int categoryId, int sortNum, int isHot, int isRecommend, int videoId)
        {
            CheckCategoryId(categoryId);
            CheckVideoId(videoId);
            Video video;
            CheckVideoId(videoId, out video);
            PlateVideo pv;
            CheckPlateCategoryVideoId(categoryId, videoId, out pv);//检测板块一级分类视频信息是否已经存在
            var plateVideo = new PlateVideo()
            {
                PlateId = 0,
                CategoryId = categoryId,
                VideoId = video.Id,
                SortNum = sortNum,
                IsHot = isHot == 1,
                IsRecommend = isRecommend == 1,
                CreateTime = DateTime.Now,
                CreateManageId = UserId
            };
            this._plateVideoRepository.CreateEntity(plateVideo);
            return plateVideo.Id;
        }
        #endregion

        #region 添加多个板块视频
        public bool CreatePlateVideos(int plateId, int sortNum, IList<int> videoIds)
        {
            CheckPlateId(plateId);
            CheckVideos(videoIds);
            IList<int> videos;
            CheckVideos(videoIds, out videos);
            Plate plate;
            CheckPlateId(plateId, out plate);
            IList<Video> videoList;
            CheckVideos(videos, out videoList);
            IList<PlateVideo> plateVideos = videoList.Select(video => new PlateVideo()
            {
                PlateId = plate.Id,
                VideoId = video.Id,
                CreateTime = DateTime.Now,
                CreateManageId = UserId
            }).ToList();
            this._plateVideoRepository.CreateEntitys(plateVideos);
            return true;
        }
        #endregion

        #region 修改自定义单个板块视频信息
        /// <summary>
        /// 修改自定义单个板块视频信息
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <param name="plateId">板块ID</param>
        /// <param name="sortNum">排序</param>
        /// <param name="isHot">是否热门 1=是 0=否</param>
        /// <param name="isRecommend">是否推荐 1=是 0=否</param>
        /// <param name="videoId">视频ID</param>
        /// <returns></returns>
        public bool UpdatePlateVideo(int id, int plateId, int sortNum, int isHot, int isRecommend, int videoId)
        {
            CheckId(id);
            CheckPlateId(plateId);
            CheckVideoId(videoId);
            PlateVideo plateVideo;
            CheckId(id, out plateVideo);
            Plate plate;
            CheckPlateId(plateId, out plate);
            Video video;
            CheckVideoId(videoId, out video);
            CheckPlateVideoId(id, plateId, videoId);
            plateVideo.PlateId = plateId;
            plateVideo.VideoId = videoId;
            plateVideo.SortNum = sortNum;
            plateVideo.IsHot = isHot == 1;
            plateVideo.IsRecommend = isRecommend == 1;
            return this._plateVideoRepository.UpdateEntity(plateVideo);
        }
        #endregion

        #region 修改系统板块视频信息
        /// <summary>
        /// 修改系统板块视频信息
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <param name="sortNum">排序</param>
        /// <param name="isHot">是否热门 1=是 0=否</param>
        /// <param name="isRecommend">是否推荐 1=是 0=否</param>
        /// <param name="videoId">视频ID</param>
        /// <returns></returns>
        public bool UpdatePlateVideo(int id, int sortNum, int isHot, int isRecommend, int videoId)
        {
            CheckId(id);
            CheckVideoId(videoId);
            PlateVideo plateVideo;
            CheckId(id, out plateVideo);
            Video video;
            CheckVideoId(videoId, out video);
            CheckPlateCategoryVideoId(id, plateVideo.CategoryId, videoId);//检测系统板块视频信息是否已经存在
            plateVideo.VideoId = videoId;
            plateVideo.SortNum = sortNum;
            plateVideo.IsHot = isHot == 1;
            plateVideo.IsRecommend = isRecommend == 1;
            return this._plateVideoRepository.UpdateEntity(plateVideo);
        }
        #endregion

        #region 修改多个板块视频
        public bool UpdatePlateVideos(int id, int plateId, IList<int> videoIds)
        {
            CheckId(id);
            CheckPlateId(plateId);
            IList<int> videos;
            CheckVideos(videoIds, out videos);
            PlateVideo plateVideo;
            CheckId(id, out plateVideo);
            Plate plate;
            CheckPlateId(plateId, out plate);
            IList<Video> videoList;
            CheckVideos(videos, out videoList);
            IList<PlateVideo> plateVideos = videoList.Select(video => new PlateVideo()
            {
                Id = plateVideo.Id,
                PlateId = plate.Id,
                VideoId = video.Id
            }).ToList();
            this._plateVideoRepository.UpdateEntitys(plateVideos);
            return true;
        }
        #endregion

        #region 板块视频排序
        /// <summary>
        /// 板块视频排序
        /// </summary>
        /// <param name="p"></param>
        public bool UpdatePlateVideoSort(IList<PlateVideo> plateVideoList)
        {
            IList<PlateVideo> list = new List<PlateVideo>();
            PlateVideo plateVideo;
            foreach (PlateVideo item in plateVideoList)
            {
                CheckId(item.Id, out plateVideo);
                plateVideo.SortNum = item.SortNum;
                list.Add(plateVideo);
            }
            this._plateVideoRepository.UpdateEntitys(list);
            return true;
        }
        #endregion

        #region 删除板块视频
        public bool DeletePlateVideo(int id)
        {
            CheckId(id);
            PlateVideo plateVideo;
            CheckId(id, out plateVideo);
            return this._plateVideoRepository.DeleteEntity(plateVideo);
        }
        #endregion

        #region 删除多个板块视频
        public bool DeletePlateVideos(IList<int> ids)
        {
            CheckIds(ids);
            IList<int> idList;
            CheckIds(ids, out idList);
            IList<PlateVideo> plateVideos;
            CheckIds(idList, out plateVideos);
            this._plateVideoRepository.DeleteEntitys(plateVideos);
            return true;
        }
        #endregion

        #region 删除指定板块下的视频
        public bool DeletePlateVideos(int plateId)
        {
            CheckPlateId(plateId);
            Plate plate;
            CheckPlateId(plateId, out plate);
            IList<PlateVideo> plateVideos;
            CheckPlateId(plate.Id, out plateVideos);
            this._plateVideoRepository.DeleteEntitys(plateVideos);
            return true;
        }

        #endregion

        #endregion

        #region 传入参数检测
        /// <summary>
        /// 检测板块视频编号不能小于0
        /// </summary>
        /// <param name="id"></param>
        private void CheckId(int id)
        {
            AssertUtil.AreBigger(id, 0, LanguageUtil.Translate("api_Business_PlateVideo_CheckId"));
        }
        /// <summary>
        /// 检测板块视频编号是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <param name="plateVideo"></param>
        private void CheckId(int id, out PlateVideo plateVideo)
        {
            plateVideo = this._plateVideoRepository.GetEntity(ConditionEqualId(id));
            AssertUtil.IsNotNull(plateVideo, LanguageUtil.Translate("api_Business_PlateVideo_CheckId_plateVideo"));
        }
        /// <summary>
        /// 检测板块视频编号集合不为空
        /// </summary>
        /// <param name="ids"></param>
        private void CheckIds(IList<int> ids)
        {
            AssertUtil.IsNotEmptyCollection(ids, LanguageUtil.Translate("api_Business_PlateVideo_CheckIds"));
        }
        /// <summary>
        /// 检测大于0的视频编号集合
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="idList"></param>
        private void CheckIds(IList<int> ids, out IList<int> idList)
        {
            IList<int> pvIds = new List<int>();
            foreach (var id in ids)
            {
                if (id > 0)
                {
                    pvIds.Add(id);
                }
            }
            idList = pvIds;
            AssertUtil.IsNotEmptyCollection(idList, LanguageUtil.Translate("api_Business_PlateVideo_CheckIds_idList"));
        }
        /// <summary>
        /// 检测存在的板块视频编号集合
        /// </summary>
        /// <param name="idList"></param>
        /// <param name="outPlateVideos"></param>
        private void CheckIds(IList<int> idList, out IList<PlateVideo> outPlateVideos)
        {
            IList<PlateVideo> plateVideos = new List<PlateVideo>();
            foreach (var video in idList)
            {
                IList<Condtion> condtions = new List<Condtion>()
                {
                    //CondtionEqualState(),
                    ConditionEqualId(video)
                };
                var videoModel = this._plateVideoRepository.GetEntity(condtions);
                if (videoModel != null)
                {
                    plateVideos.Add(videoModel);
                }
            }
            outPlateVideos = plateVideos;
            AssertUtil.IsNotEmptyCollection(outPlateVideos, LanguageUtil.Translate("api_Business_PlateVideo_CheckIds_outPlateVideos"));
        }
        /// <summary>
        /// 检测板块编号不能小于0
        /// </summary>
        /// <param name="plateId"></param>
        private void CheckPlateId(int plateId)
        {
            AssertUtil.AreBigger(plateId, 0, LanguageUtil.Translate("api_Business_PlateVideo_CheckPlateId"));
        }

        /// <summary>
        /// 检测板块一级分类编号不能小于0
        /// </summary>
        /// <param name="categoryId"></param>
        private void CheckCategoryId(int categoryId)
        {
            AssertUtil.AreBigger(categoryId, 0, LanguageUtil.Translate("api_Business_PlateVideo_CheckCategoryId"));
        }

        /// <summary>
        /// 检测板块编号是否存在
        /// </summary>
        /// <param name="plateId"></param>
        /// <param name="plate"></param>
        private void CheckPlateId(int plateId, out Plate plate)
        {
            var condtions = new List<Condtion>()
            {
                CondtionEqualState(),
                ConditionEqualId(plateId)
            };
            plate = this._plateRepository.GetEntity(condtions);
            AssertUtil.IsNotNull(plate, LanguageUtil.Translate("api_Business_PlateVideo_CheckPlateId_plate"));
        }
        /// <summary>
        /// 检测板块编号是否存在板块信息
        /// </summary>
        /// <param name="plageId"></param>
        /// <param name="plateVideo"></param>
        private void CheckPlateId(int plageId, out IList<PlateVideo> plateVideo)
        {
            var queryable = this._plateVideoRepository.GetEntityList(CondtionPlateId(plageId));
            plateVideo = queryable.Any() ? queryable.ToList() : new List<PlateVideo>();
            AssertUtil.IsNotEmptyCollection(plateVideo, LanguageUtil.Translate("api_Business_PlateVideo_CheckPlateId_plateVideo"));
        }
        /// <summary>
        /// 检测板块编号不能小于0
        /// </summary>
        /// <param name="videoId"></param>
        private void CheckVideoId(int videoId)
        {
            AssertUtil.AreBigger(videoId, 0, LanguageUtil.Translate("api_Business_PlateVideo_CheckVideoId"));
        }
        /// <summary>
        /// 检测板块编号是否存在
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="video"></param>
        private void CheckVideoId(int videoId, out Video video)
        {
            var condtions = new List<Condtion>()
            {
                CondtionEqualState(),
                ConditionEqualId(videoId)
            };
            video = this._videoRepository.GetEntity(condtions);
            AssertUtil.IsNotNull(video, LanguageUtil.Translate("api_Business_PlateVideo_CheckVideoId_video"));
        }
        /// <summary>
        /// 检测视频编号集合不为空
        /// </summary>
        /// <param name="videos"></param>
        private void CheckVideos(IList<int> videos)
        {
            AssertUtil.IsNotEmptyCollection(videos, LanguageUtil.Translate("api_Business_PlateVideo_CheckVideos"));
        }
        /// <summary>
        /// 检测大于0的视频编号集合
        /// </summary>
        /// <param name="videos"></param>
        /// <param name="videoList"></param>
        private void CheckVideos(IList<int> videos, out IList<int> videoList)
        {
            IList<int> vIds = new List<int>();
            foreach (var video in videos)
            {
                if (video > 0)
                {
                    vIds.Add(video);
                }
            }
            videoList = vIds;
            AssertUtil.IsNotEmptyCollection(videoList, LanguageUtil.Translate("api_Business_PlateVideo_CheckVideos_videoList"));
        }
        /// <summary>
        /// 检测存在的视频编号集合
        /// </summary>
        /// <param name="videos"></param>
        /// <param name="outVideoList"></param>
        private void CheckVideos(IList<int> videos, out IList<Video> outVideoList)
        {
            IList<Video> videoList = new List<Video>();
            foreach (var video in videos)
            {
                IList<Condtion> condtions = new List<Condtion>()
                {
                    CondtionEqualState(),
                    ConditionEqualId(video)
                };
                var videoModel = this._videoRepository.GetEntity(condtions);
                if (videoModel != null)
                {
                    videoList.Add(videoModel);
                }
            }
            outVideoList = videoList;
            AssertUtil.IsNotEmptyCollection(videoList, LanguageUtil.Translate("api_Business_PlateVideo_CheckVideos_outVideoList"));
        }

        /// <summary>
        /// 检测板块视频编号是否存在
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="video"></param>
        private void CheckPlateVideoId(int id, int plateId, int videoId)
        {
            var condtions = new List<Condtion>()
            {
                ConditionNotEqualId(id),
                CondtionPlateId(plateId),
                CondtionVideoId(videoId)
            };
            PlateVideo plateVideo = this._plateVideoRepository.GetEntity(condtions);
            AssertUtil.IsNull(plateVideo, LanguageUtil.Translate("api_Business_PlateVideo_CheckPlateVideoId"));
        }

        private void CheckPlateVideoId(int plateId, int videoId, out PlateVideo plateVideo)
        {
            var condtions = new List<Condtion>()
            {
                CondtionPlateId(plateId),
                CondtionVideoId(videoId)
            };
            plateVideo = this._plateVideoRepository.GetEntity(condtions);
            AssertUtil.IsNull(plateVideo, LanguageUtil.Translate("api_Business_PlateVideo_CheckPlateVideoId_plateVideo"));
        }

        /// <summary>
        /// 检测板块一级分类视频是否是否存在
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="videoId"></param>
        /// <param name="plateVideo"></param>
        private void CheckPlateCategoryVideoId(int id, int categoryId, int videoId)
        {
            var condtions = new List<Condtion>()
            {
                ConditionNotEqualId(id),
                CondtionPlateId(0),
                CondtionVideoId(videoId),
                CondtionCategoryId(categoryId)
            };
            PlateVideo plateVideo = this._plateVideoRepository.GetEntity(condtions);
            AssertUtil.IsNull(plateVideo, LanguageUtil.Translate("api_Business_PlateVideo_CheckPlateCategoryVideoId"));
        }

        private void CheckPlateCategoryVideoId(int categoryId, int videoId, out PlateVideo plateVideo)
        {
            var condtions = new List<Condtion>()
            {
                CondtionPlateId(0),
                CondtionVideoId(videoId),
                CondtionCategoryId(categoryId)
            };
            plateVideo = this._plateVideoRepository.GetEntity(condtions);
            AssertUtil.IsNull(plateVideo, LanguageUtil.Translate("api_Business_PlateVideo_CheckPlateCategoryVideoId_plateVideo"));
        }
        #endregion

        #region 传入参数
        /// <summary>
        /// 比较板块编号相等
        /// </summary>
        /// <param name="plateId"></param>
        /// <returns></returns>
        private Condtion CondtionPlateId(int plateId)
        {
            return new Condtion()
            {
                FiledName = "PlateId",
                FiledValue = plateId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
        }

        /// <summary>
        /// 比较板块编号相等
        /// </summary>
        /// <param name="plateId"></param>
        /// <returns></returns>
        private Condtion CondtionVideoId(int videoId)
        {
            return new Condtion()
            {
                FiledName = "VideoId",
                FiledValue = videoId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
        }

        /// <summary>
        /// 比较一级分类编号相等
        /// </summary>
        /// <param name="plateId"></param>
        /// <returns></returns>
        private Condtion CondtionCategoryId(int categoryId)
        {
            return new Condtion()
            {
                FiledName = "CategoryId",
                FiledValue = categoryId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
        }
        #endregion

        #region 排序参数

        #endregion

    }
}
