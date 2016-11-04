using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.Repository.Interface;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Business
{
    public class BannerVideoBusiness : BaseBusiness, IBannerVideoBusiness
    {
        private readonly IBannerVideoRepository _bannerVideoRepository;
        private readonly IVideoRepository _videoRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDictionaryRepository _dictionaryRepository;
        private readonly IDictionaryItemRepository _dictionaryItemRepository;
        private readonly IManageRepository _manageRepository;
        public BannerVideoBusiness(
                IBannerVideoRepository bannerVideoRepository,
                IVideoRepository videoRepository,
                ICategoryRepository categoryRepository,
                IDictionaryRepository dictionaryRepository,
                IDictionaryItemRepository dictionaryItemRepository,
                IManageRepository manageRepository
            )
        {
            this._bannerVideoRepository = bannerVideoRepository;
            this._videoRepository = videoRepository;
            this._categoryRepository = categoryRepository;
            this._dictionaryRepository = dictionaryRepository;
            this._dictionaryItemRepository = dictionaryItemRepository;
            this._manageRepository = manageRepository;
        }

        #region 新增
        /// <summary>
        /// 新增板览图
        /// </summary>
        /// <param name="bannerImagePath">板览图地址</param>
        /// <param name="categoryId">一级分类ID</param>
        /// <param name="sortNum">排序编号</param>
        /// <param name="videoId">视频编号</param>
        /// <returns></returns>
        public int CreateBannerVideo(string bannerImagePath, string bannerSmallImagePath, int categoryId, int sortNum, int videoId)
        {
            CheckCategoryVideoId(categoryId, videoId);//检测一级分类下的视频信息是否已经存在
            var bannerVideo = new BannerVideo()
            {
                BannerImagePath = bannerImagePath,
                BannerSmallImagePath = bannerSmallImagePath,
                CategoryId = categoryId,
                SortNum = sortNum,
                VideoId = videoId,
                CreateManageId = 1,
                CreateTime = DateTime.Now
            };
            this._bannerVideoRepository.CreateEntity(bannerVideo);
            return bannerVideo.Id;
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除单个板览图信息
        /// </summary>
        /// <param name="id">板览图编号</param>
        /// <returns></returns>
        public bool DeleteBannerVideo(int id)
        {
            CheckId(id);
            BannerVideo bannerVideo;
            CheckId(id, out bannerVideo);
            return this._bannerVideoRepository.DeleteEntity(bannerVideo);
        }

        /// <summary>
        /// 删除多个板览图信息
        /// </summary>
        /// <param name="ids">板览图编号字符串</param>
        /// <returns></returns>
        public bool DeleteBannerVideos(IList<int> ids)
        {
            CheckId(ids);
            IList<BannerVideo> bannerVideo;
            CheckId(ids, out bannerVideo);
            this._bannerVideoRepository.DeleteEntitys(bannerVideo);
            return true;
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改板临图信息
        /// </summary>
        /// <param name="id">板览编号</param>
        /// <param name="categoryId">分类编号</param>
        /// /// <param name="bannerImagePath">板览图地址</param>
        /// <param name="sortNum">排序</param>
        /// <returns></returns>
        public bool UpdateBannerVideo(int id, int categoryId, int videoId, string bannerImagePath, string bannerSmallImagePath, int sortNum)
        {
            BannerVideo bannerVideo;
            CheckId(id, out bannerVideo);
            bannerVideo.BannerImagePath = bannerImagePath;
            bannerVideo.BannerSmallImagePath = bannerSmallImagePath;
            bannerVideo.VideoId = videoId;
            bannerVideo.CategoryId = categoryId;
            bannerVideo.UpdateManageId = 1;
            bannerVideo.UpdateTime = DateTime.Now;
            bannerVideo.SortNum = sortNum;
            return this._bannerVideoRepository.UpdateEntity(bannerVideo);
        }

        /// <summary>
        /// 板览图排序
        /// </summary>
        /// <param name="p"></param>
        public bool UpdateBannerVideoSort(IList<BannerVideo> bannerVideoList)
        {
            IList<BannerVideo> list = new List<BannerVideo>();
            BannerVideo bannerVideo;
            foreach (BannerVideo item in bannerVideoList)
            {
                CheckId(item.Id, out bannerVideo);
                bannerVideo.SortNum = item.SortNum;
                list.Add(bannerVideo);
            }
            this._bannerVideoRepository.UpdateEntitys(list);
            return true;
        }
        #endregion

        #region 查询
        /// <summary>
        /// 板览图分页信息查询
        /// </summary>
        /// <param name="condtions">条件</param>
        /// <param name="orderCondtions">排序</param>
        /// <returns></returns>
        public PageResult GetBannerVideoPageResult(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            int totalCount = 0;
            int totalIndex = 0;
            IList<HKSJ.WBVV.Entity.ViewModel.Manage.BannerVideoView> plateViews = GetBannerVideoList(condtions, orderCondtions, out totalCount, out totalIndex);
            return new PageResult()
            {
                PageSize = this.PageSize,
                PageIndex = this.PageIndex,
                TotalCount = totalCount,
                Data = plateViews
            };
        }

        public IList<HKSJ.WBVV.Entity.ViewModel.Manage.BannerVideoView> GetBannerVideoList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions, out int totalCount, out int totalIndex)
        {
            var banner = (from p in this._bannerVideoRepository.GetEntityList()
                          join v in this._videoRepository.GetEntityList(CondtionEqualState()) on p.VideoId equals v.Id
                          join c in this._categoryRepository.GetEntityList(CondtionEqualState()) on p.CategoryId equals c.Id
                          where v.VideoState == 3
                          select new HKSJ.WBVV.Entity.ViewModel.Manage.BannerVideoView()
                          {
                              Id = p.Id,
                              VideoId = p.VideoId,
                              VideoName = v.Title,
                              CategoryId = p.CategoryId,
                              CategoryName = c.Name,
                              BannerImagePath = p.BannerImagePath,
                              BannerSmallImagePath = p.BannerSmallImagePath,
                              SortNum = p.SortNum,
                              CreateManageId = p.CreateManageId,
                              CreateTime = p.CreateTime,
                              State = p.State
                          }).AsQueryable();
            if (condtions != null && condtions.Count > 0)//查询条件
            {
                banner = banner.Query(condtions);
            }
            if (orderCondtions != null && orderCondtions.Count > 0)//排序条件
            {
                banner = banner.OrderBy(orderCondtions);
            }
            bool isExists = banner.Any();
            totalCount = isExists ? banner.Count() : 0;
            if (this.PageSize <= 0)
            {
                totalIndex = 0;
                var queryable = isExists
                    ? banner.ToList()
                    : new List<Entity.ViewModel.Manage.BannerVideoView>();
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
                    ? banner.Skip((this.PageIndex - 1) * this.PageSize).Take(this.PageSize).ToList()
                    : new List<Entity.ViewModel.Manage.BannerVideoView>();

                return queryable;
            }
        }

        /// <summary>
        /// 一级分类下的视频信息查询
        /// </summary>
        /// <param name="categoryId">分类编号</param>
        /// <param name="orderCondtions">排序</param>
        /// <returns></returns>
        public IList<VideoView> GetBannerVideoList(int categoryId, IList<OrderCondtion> orderCondtions)
        {
            IList<VideoView> videoViews = new List<VideoView>();
            if (categoryId <= 0)
            {
                return videoViews;
            }
            var queryable = (from bv in this._bannerVideoRepository.GetEntityList(CondtionEqualState())
                             join v in this._videoRepository.GetEntityList(CondtionEqualState()) on bv.VideoId equals v.Id
                             join c in this._categoryRepository.GetEntityList(CondtionEqualState()) on bv.CategoryId equals c.Id
                             join m in this._manageRepository.GetEntityList(CondtionEqualState()) on bv.CreateManageId equals m.Id
                             where c.Id == categoryId && v.VideoState == 3 //TODO 刘强CheckState=1
                             select new VideoView()
                             {
                                 BannerImagePath = bv.BannerImagePath,
                                 DictionaryViews = GetDictionaryViewList(v.Filter),
                                 CategoryName = c.Name,
                                 IsHot = v.IsHot,
                                 IsRecommend = v.IsRecommend,
                                 PlayCount = v.PlayCount,
                                 About = v.About,
                                 BigPicturePath = v.BigPicturePath,
                                 CreateTime = v.CreateTime,
                                 Director = v.Director,
                                 Id = v.Id,
                                 IsOfficial = v.IsOfficial,
                                 SmallPicturePath = bv.BannerSmallImagePath,
                                 Starring = v.Starring,
                                 Tags = v.Tags,
                                 SortNum = bv.SortNum
                             }
                ).Take(PageSize).AsQueryable();
            if (orderCondtions != null && orderCondtions.Count > 0)//排序条件
            {
                queryable = queryable.OrderBy(orderCondtions);
            }
            videoViews = queryable.Any() ? queryable.ToList() : new List<VideoView>();
            return videoViews;
        }

        /// <summary>
        /// 单个板览图信息
        /// </summary>
        /// <param name="id">板览图编号</param>
        /// <returns></returns>
        public BannerVideo GetBannerVideo(int id)
        {
            return id <= 0 ? new BannerVideo() : this._bannerVideoRepository.GetEntity(ConditionEqualId(id));
        }

        /// <summary>
        /// client 拆分过滤属性
        /// 读取过滤条件的集合
        /// </summary>
        /// <param name="filter">1:2;3:4;</param>
        /// <returns></returns>
        private IDictionary<string, string> GetDictionaryViewList(string filter)
        {
            IDictionary<string, string> dictionaryViews = new Dictionary<string, string>();
            IDictionary<int, int> dictionarys = GetDictionarys('r', 'c', filter);
            if (dictionarys.Count > 0)
            {
                foreach (var dictionary in dictionarys)
                {
                    var dict = this._dictionaryRepository.GetEntity(ConditionEqualId(dictionary.Key));
                    var dictItem = this._dictionaryItemRepository.GetEntity(ConditionEqualId(dictionary.Value));
                    if (dict != null)
                    {
                        if (dictionaryViews.ContainsKey(dict.Name))
                        {
                            dictionaryViews[dict.Name] = dictItem == null ? "" : dictItem.Name;
                        }
                        else
                        {
                            dictionaryViews.Add(dict.Name, dictItem == null ? "" : dictItem.Name);
                        }
                    }
                }
            }
            return dictionaryViews;
        }
        #endregion

        #region 传入参数检测
        /// <summary>
        /// 检测分类编号大于0
        /// </summary>
        /// <param name="categoryId"></param>
        private void CheckCategoryId(int categoryId)
        {
            AssertUtil.AreBigger(categoryId, 0, LanguageUtil.Translate("api_Business_BannerVideo_CheckCategoryId"));
        }
        /// <summary>
        /// 检测板览图编号大于0
        /// </summary>
        /// <param name="id"></param>
        private void CheckId(int id)
        {
            AssertUtil.AreBigger(id, 0, LanguageUtil.Translate("api_Business_BannerVideo_CheckId"));
        }
        /// <summary>
        /// 检测板览图编号是否传入
        /// </summary>
        /// <param name="ids"></param>
        private void CheckId(IList<int> ids)
        {
            AssertUtil.IsNotEmptyCollection(ids, LanguageUtil.Translate("api_Business_BannerVideo_CheckIds"));
        }
        /// <summary>
        /// 检测一级分类下的视频是否存在
        /// </summary>
        /// <param name="categoryId">一级分类ID</param>
        /// <param name="videoId">视频ID</param>
        private void CheckCategoryVideoId(int categoryId, int videoId)
        {
            var condtions = new List<Condtion>()
            {
                CondtionColumn("CategoryId",categoryId,ExpressionLogic.And,ExpressionType.Equal),
                CondtionColumn("VideoId",videoId,ExpressionLogic.And,ExpressionType.Equal)
            };
            AssertUtil.IsNull(this._bannerVideoRepository.GetEntity(condtions), LanguageUtil.Translate("api_Business_BannerVideo_CheckCategoryVideoId"));
        }

        /// <summary>
        /// 检测编号是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <param name="plate"></param>
        private void CheckId(int id, out BannerVideo bannerVideo)
        {
            IList<Condtion> condtions = new List<Condtion>()
            {
                CondtionEqualState(),
                ConditionEqualId(id)
            };
            bannerVideo = this._bannerVideoRepository.GetEntity(condtions);
            //AssertUtil.IsFalse(vedio.Any(), "板块【{0}】下存在{1}个视频".F(plate.Name, vedio.Count()));
        }

        /// <summary>
        /// 检测Banner是否存在,并且返回存在的Banner信息
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="plates"></param>
        private void CheckId(IList<int> ids, out IList<BannerVideo> bannerVideo)
        {
            IList<BannerVideo> plateList = new List<BannerVideo>();
            foreach (var id in ids)
            {
                IList<Condtion> condtions = new List<Condtion>()
                {
                    CondtionEqualState(),
                    ConditionEqualId(id)
                };
                var banner = this._bannerVideoRepository.GetEntity(condtions);
                if (banner != null)
                {
                    plateList.Add(banner);
                }
            }
            bannerVideo = plateList;
            AssertUtil.IsNotEmptyCollection(plateList, LanguageUtil.Translate("api_Business_BannerVideo_Check_IsNotEmptyCollection"));
        }
        #endregion

        #region 传入参数
        /// <summary>
        /// 比较字段值是否相等
        /// </summary>
        /// <param name="columnName">字段名称</param>
        /// <param name="columnValue">字段值</param>
        /// <param name="expressionLogic">连接表达式</param>
        /// <param name="expressionType">比较表达式</param>
        /// <returns></returns>
        private Condtion CondtionColumn(string columnName, object columnValue, ExpressionLogic expressionLogic, ExpressionType expressionType)
        {
            return new Condtion()
            {
                FiledName = columnName,
                FiledValue = columnValue,
                ExpressionLogic = expressionLogic,
                ExpressionType = expressionType
            };
        }
        #endregion
    }
}
