


using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using Autofac;
using HKSJ.Utilities.Base.Common;
using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Logger;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity.ApiParaModel;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.Repository.Interface;
using System.Web;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using System.Threading;
using System.IO;
using HKSJ.WBVV.Entity;
using Lucene.Net.Search;
using HKSJ.WBVV.Business.Search;
using Newtonsoft.Json.Linq;
using PanGu;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common;
using Qiniu.RPC;
using Qiniu.RS;
using Directory = System.IO.Directory;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Business
{
    public class VideoBusiness : BaseBusiness, IVideoBusiness
    {
        private readonly IVideoRepository _videoRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPlateRepository _plateRepository;
        private readonly IPlateVideoRepository _plateVideoRepository;
        private readonly IDictionaryRepository _dictionaryRepository;
        private readonly IDictionaryItemRepository _dictionaryItemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IManageRepository _manageRepository;
        private readonly IVideoPlayRecordRepository _videoPlayRecordRepository;
        private readonly IQiniuUploadBusiness _qiniuUploadBusiness;
        private ITagsBusiness _tagsBusiness;
        private readonly IUserCollectRepository _userCollectRepository;
        private IVideoApproveBusiness _videoApproveBusiness;
        private readonly IVideoApproveRepository _videoApproveRepository;
        public VideoBusiness(
                IVideoRepository videoRepository,
                IVideoPlayRecordRepository videoPlayRecordRepository,
                ICategoryRepository categoryRepository,
                IPlateRepository plateRepository,
                IPlateVideoRepository plateVideoRepository,
                IDictionaryRepository dictionaryRepository,
                IDictionaryItemRepository dictionaryItemRepository,
                IUserRepository userRepository,
                IManageRepository manageRepository,
                IQiniuUploadBusiness qiniuUploadBusiness,
            IUserCollectRepository userCollectRepository,
            IVideoApproveRepository videoApproveRepository)
        {
            this._videoRepository = videoRepository;
            this._videoPlayRecordRepository = videoPlayRecordRepository;
            this._categoryRepository = categoryRepository;
            this._plateRepository = plateRepository;
            this._plateVideoRepository = plateVideoRepository;
            this._dictionaryRepository = dictionaryRepository;
            this._dictionaryItemRepository = dictionaryItemRepository;
            this._userRepository = userRepository;
            this._manageRepository = manageRepository;
            this._qiniuUploadBusiness = qiniuUploadBusiness;
            _userCollectRepository = userCollectRepository;
            this._videoApproveRepository = videoApproveRepository;
        }

        #region 获取用户头像七牛URL

        public string GetUserPicUrl(int uid)
        {
            var condtion = new Condtion()
            {
                FiledName = "Id",
                FiledValue = uid,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            var user = _userRepository.GetEntity(condtion);
            if (user == null || string.IsNullOrWhiteSpace(user.Picture))
            {
                return string.Empty;
            }
            if (user.Picture.Contains("/images/"))//返回用户选择的系统定义头像
            {
                return user.Picture;
            }
            return _qiniuUploadBusiness.GetDownloadUrl(user.Picture, "image");
        }


        #endregion

        #region 默认排序查询视频
        /// <summary>
        /// 获取可用的视频集合，按SortNum，CreateTime降序排序
        /// </summary>
        /// <returns></returns>
        IQueryable<Video> GetVideoListSort()
        {
            var orderCodtion = new List<OrderCondtion>()
            {
               OrderCondtionSortNum(true),
               OrderCondtionCreateTime(true)
            };
            var query = this._videoRepository.GetEntityList(CondtionEqualState(), orderCodtion);
            return query;
        }
        /// <summary>
        /// 获取审核通过的视频
        /// </summary>
        /// <returns></returns>
        IQueryable<Video> GetVideoListState()
        {
            var queryable = GetVideoListSort().Query(CondtionEqualVideoState(3));
            return queryable;
        }

        #endregion

        #region manage
        /// <summary>
        /// 数据列表
        /// </summary>
        /// <param name="condtions"></param>
        /// <param name="orderCondtions"></param>
        /// <param name="totalCount"></param>
        /// <param name="totalIndex"></param>
        /// <returns></returns>
        public IList<Entity.ViewModel.Manage.VideoView> GetVideoViews(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions, out int totalCount, out int totalIndex)
        {
            var plate = (from v in this._videoRepository.GetEntityList(CondtionEqualState())
                         join c in this._categoryRepository.GetEntityList(CondtionEqualState()) on v.CategoryId equals c.Id
                         into vc
                         from c in vc.DefaultIfEmpty()
                         join u in this._userRepository.GetEntityList(CondtionEqualState()) on v.CreateManageId equals u.Id
                         into vj
                         from u in vj.DefaultIfEmpty()
                         join va in this._videoApproveRepository.GetEntityList() on v.Id equals va.VideoId
                         into vaa
                         from va in vaa.DefaultIfEmpty()
                         select new HKSJ.WBVV.Entity.ViewModel.Manage.VideoView()
                         {
                             Id = (int)v.Id,
                             Title = (string.IsNullOrEmpty(v.Title) ? "" : v.Title),
                             CategoryId = v.CategoryId,
                             CategoryName = c == null ? "" : c.Name,
                             VideoSource = v.VideoSource,
                             CreateManageId = v.CreateManageId,
                             CreateManageName = (u == null ? "" : (u.NickName == null ? "" : u.NickName)),
                             CreateTime = v.CreateTime,
                             VideoState = v.VideoState,
                             PersistentId = v.PersistentId,
                             Account = (u == null ? "" : u.Account),
                             ApproveContent = va == null ? "" : (va.ApproveContent == null ? "" : va.ApproveContent),
                             ApproveRemark = va == null ? "" : (va.ApproveRemark == null ? "" : va.ApproveRemark),
                             ParentId = c == null ? 0 : (c.ParentId == null ? 0 : c.ParentId),
                             SmallPicturePath = v.SmallPicturePath
                         }).AsQueryable();

            if (condtions != null && condtions.Count > 0)//查询条件
            {
                plate = plate.Query(condtions);
            }
            bool isExists = plate.Any();
            if (isExists)
            {
                if (orderCondtions != null && orderCondtions.Count > 0)//排序条件
                {
                    plate = plate.OrderBy(orderCondtions);
                }
            }
            totalCount = isExists ? plate.Count() : 0;
            if (this.PageSize <= 0)
            {
                totalIndex = 0;
                var queryable = isExists
                    ? plate.ToList()
                    : new List<Entity.ViewModel.Manage.VideoView>();
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
                    ? plate.Skip((this.PageIndex - 1) * this.PageSize).Take(this.PageSize).ToList()
                    : new List<Entity.ViewModel.Manage.VideoView>();

                return queryable;
            }
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="condtions"></param>
        /// <param name="orderCondtions"></param>
        /// <returns></returns>
        public PageResult GetVideosPageResult(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            int totalCount = 0;
            int totalIndex = 0;
            IList<HKSJ.WBVV.Entity.ViewModel.Manage.VideoView> plateViews = GetVideoViews(condtions, orderCondtions, out totalCount, out totalIndex);
            return new PageResult()
            {
                PageSize = this.PageSize,
                PageIndex = this.PageIndex,
                TotalCount = totalCount,
                TotalIndex = totalIndex,
                Data = plateViews
            };
        }
        #endregion

        #region client

        #region 拆分过滤属性
        /// <summary>
        /// 读取过滤条件的集合
        /// </summary>
        /// <param name="filter">2c3r2c4r</param>
        /// <returns></returns>
        private IDictionary<string, string> GetDictionaryViewList(string filter)
        {
            IDictionary<string, string> dictionaryViews = new Dictionary<string, string>();
            IDictionary<int, int> dictionarys = GetDictionarys('r', 'c', filter);
            if (dictionarys.Count > 0)
            {
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
            }
            return dictionaryViews;
        }
        #endregion

        #region 播放页——获取视频详细信息
        /// <summary>
        /// 获取视频详细信息
        /// </summary>
        /// <param name="videoId">视频编号</param>
        /// <returns></returns>
        public VideoDetailView GetVideoDetailView(int videoId, int userId)
        {
            var videoDetailView = new VideoDetailView();
            if (videoId <= 0)
            {
                return videoDetailView;
            }
            var vedio = (from video in this._videoRepository.GetEntityList()
                         where video.State == false //启用
                                && video.VideoState == 3 //审核通过
                                && video.Id == videoId //视频编号
                         select video).FirstOrDefault();
            if (vedio == null)
            {
                return videoDetailView;
            }
            //用户上传
            if (vedio.VideoSource)
            {
                videoDetailView = (from video in this._videoRepository.GetEntityList()
                                   join cate in this._categoryRepository.GetEntityList() on video.CategoryId equals cate.Id
                                   join user in this._userRepository.GetEntityList() on video.CreateManageId equals user.Id
                                   where video.State == false //启用
                                       && video.VideoState == 3 //审核通过
                                       && video.Id == vedio.Id //视频编号
                                       && cate.State == false //启用
                                       && user.State == false //启用
                                   select new VideoDetailView()
                                   {
                                       CategoryId = cate.Id.ToString(),
                                       CategoryName = cate.Name,
                                       DictionaryViews = GetDictionaryViewList(video.Filter),
                                       Tags = video.Tags,
                                       About = video.About,
                                       Starring = video.Starring,
                                       Title = video.Title,
                                       VideoPath = video.VideoPath,
                                       BigPicturePath = video.BigPicturePath,
                                       SmallPicturePath = video.SmallPicturePath,
                                       CollectionCount = video.CollectionCount,
                                       CommentCount = video.CommentCount,
                                       PraiseCount = video.PraiseCount,
                                       BadCount = video.BadCount,
                                       PlayCount = video.PlayCount,
                                       Director = video.Director,
                                       Id = video.Id,
                                       UserId = user.Id,
                                       NickName = user.NickName,
                                       Picture = user.Picture,
                                       VideoSource = 1,
                                       RewardCount = video.RewardCount
                                   }
                        ).FirstOrDefault();
            }
            else//管理员上传
            {
                videoDetailView = (from video in this._videoRepository.GetEntityList()
                                   join cate in this._categoryRepository.GetEntityList() on video.CategoryId equals cate.Id
                                   join manage in this._manageRepository.GetEntityList() on video.CreateManageId equals manage.Id
                                   where video.State == false //启用
                                       && video.VideoState == 3 //审核通过
                                       && video.Id == vedio.Id //视频编号
                                       && cate.State == false //启用
                                       && manage.State == false //启用
                                   select new VideoDetailView()
                                   {
                                       CategoryId = cate.Id.ToString(),
                                       CategoryName = cate.Name,
                                       DictionaryViews = GetDictionaryViewList(video.Filter),
                                       Tags = video.Tags,
                                       About = video.About,
                                       Starring = video.Starring,
                                       Title = video.Title,
                                       VideoPath = video.VideoPath,
                                       BigPicturePath = video.BigPicturePath,
                                       SmallPicturePath = video.SmallPicturePath,
                                       CollectionCount = video.CollectionCount,
                                       CommentCount = video.CommentCount,
                                       PraiseCount = video.PraiseCount,
                                       BadCount = video.BadCount,
                                       PlayCount = video.PlayCount,
                                       Director = video.Director,
                                       Id = video.Id,
                                       UserId = manage.Id,
                                       NickName = manage.NickName,
                                       Picture = "",
                                       VideoSource = 0
                                   }
                      ).FirstOrDefault();
            }

            //如果有登录用户，则读取用户是否收藏该视频
            if (userId > 0 && videoDetailView != null)
            {
                var controller = (from c in this._userCollectRepository.GetEntityList(CondtionEqualState())
                                  where c.CreateUserId == userId
                                  && c.VideoId == videoDetailView.Id
                                  select c).AsQueryable<UserCollect>();

                if (controller.Any())
                    videoDetailView.IsCollected = true;
                else
                    videoDetailView.IsCollected = false;
            }

            return videoDetailView;
        }

        #endregion

        #region 一级分类页——分类视频

        #region 获取分类视频，按更新时间降序，按播放数量降序
        /// <summary>
        /// 获取分类视频
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public IList<VideoView> GetCategoryVideo(int categoryId)
        {
            var categoryVideos = new List<VideoView>();
            if (categoryId <= 0)
            {
                return categoryVideos;
            }
            var category = this._categoryRepository.GetEntity(ConditionEqualId(categoryId));
            if (category == null)
            {
                return categoryVideos;
            }

            #region 没有禁用的管理员上传的分类视频
            var manageVideos = (from c in this._categoryRepository.GetEntityList()
                                join v in this._videoRepository.GetEntityList() on c.Id equals v.CategoryId
                                join m in this._manageRepository.GetEntityList() on v.CreateManageId equals m.Id
                                where c.State == false //启用
                                    && c.LocationPath.StartsWith(category.LocationPath) //匹配分类下的所有子分类
                                    && v.State == false //启用
                                    && v.VideoState == 3 //审核通过
                                    && m.State == false //启用
                                select new VideoView()
                                {
                                    DictionaryId = GetDictionarys('r', 'c', v.Filter),
                                    DictionaryViews = GetDictionaryViewList(v.Filter),
                                    CategoryName = c.Name,//类型分类名称
                                    IsHot = v.IsHot,
                                    IsRecommend = v.IsRecommend,
                                    UpdateTime = v.UpdateTime.HasValue ? Convert.ToDateTime(v.UpdateTime) : v.CreateTime,
                                    PlayCount = v.PlayCount,
                                    About = v.About,
                                    BigPicturePath = v.BigPicturePath,
                                    CreateTime = v.CreateTime,
                                    Director = v.Director,
                                    Id = v.Id,
                                    IsOfficial = v.IsOfficial,
                                    SmallPicturePath = v.SmallPicturePath,
                                    Starring = v.Starring,
                                    Tags = v.Tags,
                                    SortNum = v.SortNum,
                                    VideoState = v.VideoState,
                                    Title = v.Title,
                                    Filter = v.Filter
                                }
                         ).AsQueryable();
            #endregion

            #region 没有禁用的用户上传的分类视频
            var userVideos = (from c in this._categoryRepository.GetEntityList()
                              join v in this._videoRepository.GetEntityList() on c.Id equals v.CategoryId
                              join u in this._userRepository.GetEntityList() on v.CreateManageId equals u.Id
                              where c.State == false //启用
                                  && c.LocationPath.StartsWith(category.LocationPath) //匹配分类下的所有子分类
                                  && v.State == false //启用
                                  && v.VideoState == 3 //审核通过
                                  && u.State == false //启用
                              select new VideoView()
                              {
                                  DictionaryId = GetDictionarys('r', 'c', v.Filter),
                                  DictionaryViews = GetDictionaryViewList(v.Filter),
                                  CategoryName = c.Name,//类型分类名称
                                  IsHot = v.IsHot,
                                  IsRecommend = v.IsRecommend,
                                  UpdateTime = v.UpdateTime.HasValue ? Convert.ToDateTime(v.UpdateTime) : v.CreateTime,
                                  PlayCount = v.PlayCount,
                                  About = v.About,
                                  BigPicturePath = v.BigPicturePath,
                                  CreateTime = v.CreateTime,
                                  Director = v.Director,
                                  Id = v.Id,
                                  IsOfficial = v.IsOfficial,
                                  SmallPicturePath = v.SmallPicturePath,
                                  Starring = v.Starring,
                                  Tags = v.Tags,
                                  SortNum = v.SortNum,
                                  VideoState = v.VideoState,
                                  Title = v.Title,
                                  Filter = v.Filter
                              }
                         ).AsQueryable();
            #endregion

            #region 合并用户视频
            if (manageVideos.Any())
            {
                categoryVideos.AddRange(manageVideos);
            }
            if (userVideos.Any())
            {
                categoryVideos.AddRange(userVideos);
            }
            #endregion

            #region 排序
            var queryable = (from v in categoryVideos
                             orderby v.SortNum descending, //按排序数量降序
                                 v.PlayCount descending, //按播放降序
                                 v.UpdateTime descending
                             //按上架时间降序
                             select v).AsQueryable();
            #endregion

            int pagesize = 10;
            if (category.PageSize <= 0)
            {
                if (this.PageSize > 0)
                {
                    pagesize = this.PageSize;
                }
            }
            else
            {
                pagesize = category.PageSize;
            }
            var pageQueryable = queryable.Take(pagesize);
            if (pageQueryable.Any())
            {
                categoryVideos = pageQueryable.ToList();
            }

            return categoryVideos;
        }
        #endregion

        #region 分类视频右边，按播放数量降序
        /// <summary>
        /// 分类视频右边
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public IList<IndexVideoView> GetCategoryVideoRight(int categoryId)
        {
            var categoryVideos = new List<IndexVideoView>();
            if (categoryId <= 0)
            {
                return categoryVideos;
            }
            var category = this._categoryRepository.GetEntity(ConditionEqualId(categoryId));
            if (category == null)
            {
                return categoryVideos;
            }
            #region 没有禁用的管理员上传的分类视频
            var manageVideos = (from c in this._categoryRepository.GetEntityList()
                                join v in this._videoRepository.GetEntityList() on c.Id equals v.CategoryId
                                join m in this._manageRepository.GetEntityList() on v.CreateManageId equals m.Id
                                where c.State == false //启用
                                    && c.LocationPath.StartsWith(category.LocationPath) //匹配分类下的所有子分类
                                    && v.State == false //启用
                                    && v.VideoState == 3 //审核通过
                                    && m.State == false //启用
                                    && v.VideoSource == false //管理员
                                select new IndexVideoView()
                                {
                                    Id = v.Id,
                                    Title = v.Title,
                                    About = v.About,
                                    SortNum = v.SortNum,
                                    SmallPicturePath = v.SmallPicturePath,
                                    BigPicturePath = v.BigPicturePath,
                                    PlayCount = v.PlayCount,
                                    CreateTime = v.CreateTime,
                                    UpdateTime = v.UpdateTime.HasValue ? Convert.ToDateTime(v.UpdateTime) : v.CreateTime
                                }
                         ).AsQueryable();
            #endregion

            #region 没有禁用的用户上传的分类视频
            var userVideos = (from c in this._categoryRepository.GetEntityList()
                              join v in this._videoRepository.GetEntityList() on c.Id equals v.CategoryId
                              join u in this._userRepository.GetEntityList() on v.CreateManageId equals u.Id
                              where c.State == false //启用
                                  && c.LocationPath.StartsWith(category.LocationPath) //匹配分类下的所有子分类
                                  && v.State == false //启用
                                  && v.VideoState == 3 //审核通过
                                  && u.State == false //启用
                                  && v.VideoSource == true //用户
                              select new IndexVideoView()
                              {
                                  Id = v.Id,
                                  Title = v.Title,
                                  About = v.About,
                                  SmallPicturePath = v.SmallPicturePath,
                                  BigPicturePath = v.BigPicturePath,
                                  PlayCount = v.PlayCount,
                                  SortNum = v.SortNum,
                                  UpdateTime = v.UpdateTime.HasValue ? Convert.ToDateTime(v.UpdateTime) : v.CreateTime,
                                  CreateTime = v.CreateTime
                              }
                         ).AsQueryable();
            #endregion

            #region 合并用户视频
            if (manageVideos.Any())
            {
                categoryVideos.AddRange(manageVideos);
            }
            if (userVideos.Any())
            {
                categoryVideos.AddRange(userVideos);
            }
            #endregion

            #region 排序
            var queryable = (from v in categoryVideos
                             orderby v.PlayCount descending, //按播放数量降序
                                 v.SortNum descending, //按排序数量降序
                                 v.UpdateTime descending
                             //按上架时间降序
                             select v).AsQueryable();
            #endregion

            int pagesize = 10;
            if (category.PageSize <= 0)
            {
                if (this.PageSize > 0)
                {
                    pagesize = this.PageSize;
                }
            }
            else
            {
                pagesize = category.PageSize;
            }
            var pageQueryable = queryable.Take(pagesize);
            if (pageQueryable.Any())
            {
                categoryVideos = pageQueryable.ToList();
            }

            return categoryVideos;
        }

        #endregion

        #region 分类视频左边，按更新时间降序
        /// <summary>
        /// 分类视频左边,按播放数量降序
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public IList<IndexVideoView> GetCategoryVideoLeft(int categoryId)
        {
            var categoryVideos = new List<IndexVideoView>();
            if (categoryId <= 0)
            {
                return categoryVideos;
            }
            var category = this._categoryRepository.GetEntity(ConditionEqualId(categoryId));
            if (category == null)
            {
                return categoryVideos;
            }
            #region 没有禁用的管理员上传的分类视频
            var manageVideos = (from c in this._categoryRepository.GetEntityList()
                                join v in this._videoRepository.GetEntityList() on c.Id equals v.CategoryId
                                join m in this._manageRepository.GetEntityList() on v.CreateManageId equals m.Id
                                where c.State == false //启用
                                    && c.LocationPath.StartsWith(category.LocationPath) //匹配分类下的所有子分类
                                    && v.State == false //启用
                                    && v.VideoState == 3 //审核通过
                                    && m.State == false //启用
                                    && v.VideoSource == false //管理员
                                select new IndexVideoView()
                                {
                                    Id = v.Id,
                                    Title = v.Title,
                                    About = v.About,
                                    PlayCount = v.PlayCount,
                                    SortNum = v.SortNum,
                                    SmallPicturePath = v.SmallPicturePath,
                                    BigPicturePath = v.BigPicturePath,
                                    CreateTime = v.CreateTime,
                                    UpdateTime = v.UpdateTime.HasValue ? Convert.ToDateTime(v.UpdateTime) : v.CreateTime
                                }
                         ).AsQueryable();
            #endregion

            #region 没有禁用的用户上传的分类视频
            var userVideos = (from c in this._categoryRepository.GetEntityList()
                              join v in this._videoRepository.GetEntityList() on c.Id equals v.CategoryId
                              join u in this._userRepository.GetEntityList() on v.CreateManageId equals u.Id
                              where c.State == false //启用
                                  && c.LocationPath.StartsWith(category.LocationPath) //匹配分类下的所有子分类
                                  && v.State == false //启用
                                  && v.VideoState == 3 //审核通过
                                  && u.State == false //启用
                                  && v.VideoSource == true //用户
                              select new IndexVideoView()
                              {
                                  Id = v.Id,
                                  Title = v.Title,
                                  About = v.About,
                                  SmallPicturePath = v.SmallPicturePath,
                                  BigPicturePath = v.BigPicturePath,
                                  CreateTime = v.CreateTime,
                                  UpdateTime = v.UpdateTime.HasValue ? Convert.ToDateTime(v.UpdateTime) : v.CreateTime,
                                  SortNum = v.SortNum,
                                  PlayCount = v.PlayCount
                              }
                         ).AsQueryable();
            #endregion

            #region 合并用户视频
            if (manageVideos.Any())
            {
                categoryVideos.AddRange(manageVideos);
            }
            if (userVideos.Any())
            {
                categoryVideos.AddRange(userVideos);
            }
            #endregion

            #region 排序
            var queryable = (from v in categoryVideos
                             orderby v.SortNum descending, //按更新时间降序
                                     v.UpdateTime descending //按排序数量降序
                             select v).AsQueryable();
            #endregion

            int pagesize = 10;
            if (category.PageSize <= 0)
            {
                if (this.PageSize > 0)
                {
                    pagesize = this.PageSize;
                }
            }
            else
            {
                pagesize = category.PageSize;
            }
            var pageQueryable = queryable.Take(pagesize);
            if (pageQueryable.Any())
            {
                categoryVideos = pageQueryable.ToList();
            }

            return categoryVideos;
        }
        #endregion

        /// <summary>
        /// 首页和一级分类页的数据
        /// </summary>
        /// <param name="categoryId">分类编号</param>
        /// <param name="isIndexPage">是否为首页数据 true:首页数据</param>
        /// <returns></returns>
        public RecommendAndHotCategoryVideoView GetCategoryVideoData(int categoryId, bool isIndexPage = false)
        {
            var result = new RecommendAndHotCategoryVideoView();

            var categoryVideos = new List<IndexVideoView>();
            if (categoryId <= 0)
                return result;

            var category = this._categoryRepository.GetEntity(ConditionEqualId(categoryId));
            if (category == null)
                return result;
            result.Category = new CategorysView()
            {
                ParentCategory = new CategoryView()
                {
                    Id = category.Id,
                    Name = category.Name,
                    ParentId = category.ParentId
                }
            };

            if (isIndexPage)
            {
                #region 首页数据

                #region 没有禁用的管理员上传的分类视频
                var manageRecommendVideos = (from pv in this._plateVideoRepository.GetEntityList()
                                             join c in this._categoryRepository.GetEntityList() on pv.CategoryId equals c.Id
                                             join v in this._videoRepository.GetEntityList() on pv.VideoId equals v.Id
                                             join m in this._manageRepository.GetEntityList() on v.CreateManageId equals m.Id
                                             where pv.State == false //启用
                                                 && pv.IsRecommend == true //推荐
                                                 && c.State == false //启用
                                                 && c.LocationPath.StartsWith(category.LocationPath) //匹配分类下的所有子分类
                                                 && v.State == false //启用
                                                 && v.VideoState == 3 //审核通过
                                                 && m.State == false//启用
                                                 && v.VideoSource == false //管理员
                                             select new IndexVideoView()
                                             {
                                                 Id = v.Id,
                                                 Title = v.Title,
                                                 About = v.About,
                                                 PlayCount = v.PlayCount,
                                                 SortNum = pv.SortNum,
                                                 CreateTime = v.CreateTime,
                                                 UpdateTime = v.UpdateTime.HasValue ? Convert.ToDateTime(v.UpdateTime) : v.CreateTime,
                                                 BigPicturePath = v.BigPicturePath,
                                                 SmallPicturePath = v.SmallPicturePath
                                             }
                             ).AsQueryable();

                var manageHotVideos = (from pv in this._plateVideoRepository.GetEntityList()
                                       join c in this._categoryRepository.GetEntityList() on pv.CategoryId equals c.Id
                                       join v in this._videoRepository.GetEntityList() on pv.VideoId equals v.Id
                                       join m in this._manageRepository.GetEntityList() on v.CreateManageId equals m.Id
                                       where pv.State == false //启用
                                           && pv.IsHot == true //热门
                                           && c.State == false //启用
                                           && c.LocationPath.StartsWith(category.LocationPath) //匹配分类下的所有子分类
                                           && v.State == false //启用
                                           && v.VideoState == 3 //审核通过
                                           && m.State == false//启用
                                           && v.VideoSource == false //管理员
                                       select new IndexVideoView()
                                       {
                                           Id = v.Id,
                                           Title = v.Title,
                                           About = v.About,
                                           PlayCount = v.PlayCount,
                                           SortNum = pv.SortNum,
                                           CreateTime = v.CreateTime,
                                           UpdateTime = v.UpdateTime.HasValue ? Convert.ToDateTime(v.UpdateTime) : v.CreateTime,
                                           BigPicturePath = v.BigPicturePath,
                                           SmallPicturePath = v.SmallPicturePath
                                       }
                           ).AsQueryable();
                #endregion

                #region 没有禁用的用户上传的分类视频
                var userRecommendVideos = (from pv in this._plateVideoRepository.GetEntityList()
                                           join c in this._categoryRepository.GetEntityList() on pv.CategoryId equals c.Id
                                           join v in this._videoRepository.GetEntityList() on pv.VideoId equals v.Id
                                           join u in this._userRepository.GetEntityList() on v.CreateManageId equals u.Id
                                           where pv.State == false //启用
                                              && pv.IsRecommend == true //推荐
                                               && c.State == false //启用
                                               && c.LocationPath.StartsWith(category.LocationPath) //匹配分类下的所有子分类
                                               && v.State == false //启用
                                               && v.VideoState == 3 //审核通过
                                               && u.State == false//启用
                                               && v.VideoSource == true //用户
                                           select new IndexVideoView()
                                           {
                                               Id = v.Id,
                                               Title = v.Title,
                                               About = v.About,
                                               SortNum = pv.SortNum,
                                               PlayCount = v.PlayCount,
                                               SmallPicturePath = v.SmallPicturePath,
                                               BigPicturePath = v.BigPicturePath,
                                               CreateTime = v.CreateTime,
                                               UpdateTime = v.UpdateTime.HasValue ? Convert.ToDateTime(v.UpdateTime) : v.CreateTime
                                           }
                             ).AsQueryable();

                var userHotVideos = (from pv in this._plateVideoRepository.GetEntityList()
                                     join c in this._categoryRepository.GetEntityList() on pv.CategoryId equals c.Id
                                     join v in this._videoRepository.GetEntityList() on pv.VideoId equals v.Id
                                     join u in this._userRepository.GetEntityList() on v.CreateManageId equals u.Id
                                     where pv.State == false //启用
                                         && pv.IsHot == true //热门
                                         && c.State == false //启用
                                         && c.LocationPath.StartsWith(category.LocationPath) //匹配分类下的所有子分类
                                         && v.State == false //启用
                                         && v.VideoState == 3 //审核通过
                                         && u.State == false//启用
                                         && v.VideoSource == true //用户
                                     select new IndexVideoView()
                                     {
                                         Id = v.Id,
                                         Title = v.Title,
                                         About = v.About,
                                         SortNum = pv.SortNum,
                                         PlayCount = v.PlayCount,
                                         SmallPicturePath = v.SmallPicturePath,
                                         BigPicturePath = v.BigPicturePath,
                                         CreateTime = v.CreateTime,
                                         UpdateTime = v.UpdateTime.HasValue ? Convert.ToDateTime(v.UpdateTime) : v.CreateTime
                                     }
                            ).AsQueryable();
                #endregion

                #region 合并与排序
                //推荐合并
                if (manageRecommendVideos.Any())
                    categoryVideos.AddRange(manageRecommendVideos);
                if (userRecommendVideos.Any())
                    categoryVideos.AddRange(userRecommendVideos);

                //推荐
                var queryableLeft = (from v in categoryVideos
                                     orderby v.SortNum descending, //按排序数量降序
                                         v.UpdateTime descending
                                     //按上架时间降序
                                     select v).AsQueryable();

                categoryVideos = new List<IndexVideoView>();

                //热门合并
                if (manageHotVideos.Any())
                    categoryVideos.AddRange(manageHotVideos);
                if (userHotVideos.Any())
                    categoryVideos.AddRange(userHotVideos);


                //热门
                var queryableRight = (from v in categoryVideos
                                      orderby v.SortNum descending, //按排序数量降序
                                          v.UpdateTime descending
                                      //按上架时间降序
                                      select v).AsQueryable();
                #endregion

                #region 分页
                int pagesize = 10;
                if (category.PageSize <= 0)
                {
                    if (this.PageSize > 0)
                    {
                        pagesize = this.PageSize;
                    }
                }
                else
                {
                    pagesize = category.PageSize;
                }
                var pageQueryable = queryableLeft.Take(pagesize);
                if (pageQueryable.Any())
                {
                    result.RecommendCategoryVideo = pageQueryable.ToList();
                }
                pageQueryable = queryableRight.Take(pagesize);
                if (pageQueryable.Any())
                {
                    result.HotCategoryVideo = pageQueryable.ToList();
                }
                #endregion
                #endregion
            }
            else
            {
                #region 一级分类数据
                #region 没有禁用的管理员上传的分类视频
                var manageVideos = (from c in this._categoryRepository.GetEntityList()
                                    join v in this._videoRepository.GetEntityList() on c.Id equals v.CategoryId
                                    join m in this._manageRepository.GetEntityList() on v.CreateManageId equals m.Id
                                    where c.State == false //启用
                                        && c.LocationPath.StartsWith(category.LocationPath) //匹配分类下的所有子分类
                                        && v.State == false //启用
                                        && v.VideoState == 3 //审核通过
                                        && m.State == false //启用
                                        && v.VideoSource == false //管理员
                                    select new IndexVideoView()
                                    {
                                        Id = v.Id,
                                        Title = v.Title,
                                        About = v.About,
                                        SortNum = v.SortNum,
                                        SmallPicturePath = v.SmallPicturePath,
                                        BigPicturePath = v.BigPicturePath,
                                        PlayCount = v.PlayCount,
                                        CreateTime = v.CreateTime,
                                        UpdateTime = v.UpdateTime.HasValue ? Convert.ToDateTime(v.UpdateTime) : v.CreateTime
                                    }
                             ).AsQueryable();
                #endregion

                #region 没有禁用的用户上传的分类视频
                var userVideos = (from c in this._categoryRepository.GetEntityList()
                                  join v in this._videoRepository.GetEntityList() on c.Id equals v.CategoryId
                                  join u in this._userRepository.GetEntityList() on v.CreateManageId equals u.Id
                                  where c.State == false //启用
                                      && c.LocationPath.StartsWith(category.LocationPath) //匹配分类下的所有子分类
                                      && v.State == false //启用
                                      && v.VideoState == 3 //审核通过
                                      && u.State == false //启用
                                      && v.VideoSource == true //用户
                                  select new IndexVideoView()
                                  {
                                      Id = v.Id,
                                      Title = v.Title,
                                      About = v.About,
                                      SmallPicturePath = v.SmallPicturePath,
                                      BigPicturePath = v.BigPicturePath,
                                      PlayCount = v.PlayCount,
                                      SortNum = v.SortNum,
                                      UpdateTime = v.UpdateTime.HasValue ? Convert.ToDateTime(v.UpdateTime) : v.CreateTime,
                                      CreateTime = v.CreateTime
                                  }
                             ).AsQueryable();
                #endregion

                #region 合并用户视频
                if (manageVideos.Any())
                {
                    categoryVideos.AddRange(manageVideos);
                }
                if (userVideos.Any())
                {
                    categoryVideos.AddRange(userVideos);
                }
                #endregion

                #region 排序
                //推荐视频
                var queryableLeft = (from v in categoryVideos
                                     orderby v.SortNum descending, //按更新时间降序
                                             v.UpdateTime descending //按排序数量降序
                                     select v).AsQueryable();
                //热门精选视频
                var queryableRight = (from v in categoryVideos
                                      orderby v.PlayCount descending, //按播放数量降序
                                          v.SortNum descending, //按排序数量降序
                                          v.UpdateTime descending
                                      //按上架时间降序
                                      select v).AsQueryable();
                #endregion

                #region 分页
                int pagesize = 10;
                if (category.PageSize <= 0)
                {
                    if (this.PageSize > 0)
                    {
                        pagesize = this.PageSize;
                    }
                }
                else
                {
                    pagesize = category.PageSize;
                }
                var pageQueryable = queryableRight.Take(pagesize);
                if (pageQueryable.Any())
                {
                    result.HotCategoryVideo = pageQueryable.ToList();
                }
                pageQueryable = queryableLeft.Take(pagesize);
                if (pageQueryable.Any())
                {
                    result.RecommendCategoryVideo = pageQueryable.ToList();
                }

                #endregion

                #endregion
            }

            return result;
        }

        #endregion

        #region 首页和一级分类页——板块视频

        #region 获取板块视频
        /// <summary>
        /// 获取板块视频
        /// </summary>
        /// <param name="plateId"></param>
        /// <returns></returns>
        public IList<VideoView> GetPlateVideo(int plateId)
        {
            var plateVideos = new List<VideoView>();
            if (plateId <= 0)
            {
                return plateVideos;
            }
            var plate = this._plateRepository.GetEntity(ConditionEqualId(plateId));
            if (plate == null)
            {
                return plateVideos;
            }
            #region 没有禁用的管理员上传的分类视频
            var manageVideos = (from pv in this._plateVideoRepository.GetEntityList()
                                join p in this._plateRepository.GetEntityList() on pv.PlateId equals p.Id
                                join v in this._videoRepository.GetEntityList() on pv.VideoId equals v.Id
                                join m in this._manageRepository.GetEntityList() on v.CreateManageId equals m.Id
                                join c in this._categoryRepository.GetEntityList(CondtionEqualState()) on v.CategoryId equals c.Id
                                into cJoin//左外联分类表 0 表示首页板块
                                from cateModel in cJoin.DefaultIfEmpty()
                                where pv.State == false //启动
                                    && p.State == false //启用
                                    && p.Id == plate.Id //板块编号
                                    && v.State == false //启用
                                    && v.VideoState == 3 //审核通过
                                    && m.State == false
                                    && v.VideoSource == false //管理员
                                select new VideoView()
                                {
                                    DictionaryId = GetDictionarys('r', 'c', v.Filter),
                                    DictionaryViews = GetDictionaryViewList(v.Filter),
                                    CategoryName = cateModel == null ? LanguageUtil.Translate("api_Business_Video_GetPlateVideo_CategoryName_homePage_manageVideos") : cateModel.Name,//类型分类名称
                                    IsHot = v.IsHot,
                                    IsRecommend = v.IsRecommend,
                                    UpdateTime = v.UpdateTime.HasValue ? Convert.ToDateTime(v.UpdateTime) : v.CreateTime,
                                    PlayCount = v.PlayCount,
                                    About = v.About,
                                    BigPicturePath = v.BigPicturePath,
                                    CreateTime = v.CreateTime,
                                    Director = v.Director,
                                    Id = v.Id,
                                    IsOfficial = v.IsOfficial,
                                    SmallPicturePath = v.SmallPicturePath,
                                    Starring = v.Starring,
                                    Tags = v.Tags,
                                    SortNum = pv.SortNum,
                                    VideoState = v.VideoState,
                                    Title = v.Title,
                                    Filter = v.Filter
                                }
                         ).AsQueryable();
            #endregion

            #region 没有禁用的用户上传的分类视频
            var userVideos = (from pv in this._plateVideoRepository.GetEntityList()
                              join p in this._plateRepository.GetEntityList() on pv.PlateId equals p.Id
                              join v in this._videoRepository.GetEntityList() on pv.VideoId equals v.Id
                              join u in this._userRepository.GetEntityList() on v.CreateManageId equals u.Id
                              join c in this._categoryRepository.GetEntityList(CondtionEqualState()) on v.CategoryId equals c.Id
                              into cJoin//左外联分类表 0 表示首页板块
                              from cateModel in cJoin.DefaultIfEmpty()
                              where pv.State == false //启动
                                  && p.State == false //启用
                                  && p.Id == plate.Id //板块编号
                                  && v.State == false //启用
                                  && v.VideoState == 3 //审核通过
                                  && u.State == false
                                  && v.VideoSource == true //用户
                              select new VideoView()
                              {
                                  DictionaryId = GetDictionarys('r', 'c', v.Filter),
                                  DictionaryViews = GetDictionaryViewList(v.Filter),
                                  CategoryName = cateModel == null ? LanguageUtil.Translate("api_Business_Video_GetPlateVideo_CategoryName_homePage_userVideos") : cateModel.Name,//类型分类名称
                                  IsHot = v.IsHot,
                                  IsRecommend = v.IsRecommend,
                                  UpdateTime = v.UpdateTime.HasValue ? Convert.ToDateTime(v.UpdateTime) : v.CreateTime,
                                  PlayCount = v.PlayCount,
                                  About = v.About,
                                  BigPicturePath = v.BigPicturePath,
                                  CreateTime = v.CreateTime,
                                  Director = v.Director,
                                  Id = v.Id,
                                  IsOfficial = v.IsOfficial,
                                  SmallPicturePath = v.SmallPicturePath,
                                  Starring = v.Starring,
                                  Tags = v.Tags,
                                  SortNum = pv.SortNum,
                                  VideoState = v.VideoState,
                                  Title = v.Title,
                                  Filter = v.Filter
                              }
                         ).AsQueryable();
            #endregion

            #region 合并用户视频
            if (manageVideos.Any())
            {
                plateVideos.AddRange(manageVideos);
            }
            if (userVideos.Any())
            {
                plateVideos.AddRange(userVideos);
            }
            #endregion

            #region 排序
            var queryable = (from v in plateVideos
                             orderby v.SortNum descending, //按排序数量降序
                                     v.PlayCount descending,//按播放次数降序
                                     v.UpdateTime descending//按上架时间降序
                             select v).AsQueryable();
            #endregion

            int pagesize = 10;
            if (plate.PageSize <= 0)
            {
                if (this.PageSize > 0)
                {
                    pagesize = this.PageSize;
                }
            }
            else
            {
                pagesize = plate.PageSize;
            }
            var pageQueryable = queryable.Take(pagesize);
            if (pageQueryable.Any())
            {
                plateVideos = pageQueryable.ToList();
            }
            return plateVideos;
        }
        #endregion

        #region 获取热门板块视频
        /// <summary>
        /// 获取热门板块视频
        /// </summary>
        /// <param name="plateId"></param>
        /// <returns></returns>
        public IList<IndexVideoView> GetHotPlateVideo(int plateId)
        {
            var hotPlateVideos = new List<IndexVideoView>();
            if (plateId <= 0)
            {
                return hotPlateVideos;
            }
            var plate = this._plateRepository.GetEntity(ConditionEqualId(plateId));
            if (plate == null)
            {
                return hotPlateVideos;
            }
            #region 没有禁用的管理员上传的板块视频
            var manageVideos = (from pv in this._plateVideoRepository.GetEntityList()
                                join p in this._plateRepository.GetEntityList() on pv.PlateId equals p.Id
                                join v in this._videoRepository.GetEntityList() on pv.VideoId equals v.Id
                                join m in this._manageRepository.GetEntityList() on v.CreateManageId equals m.Id
                                join c in this._categoryRepository.GetEntityList(CondtionEqualState()) on v.CategoryId equals c.Id
                                into cJoin//左外联分类表 0 表示首页板块
                                from cateModel in cJoin.DefaultIfEmpty()
                                where pv.State == false //启动
                                    && pv.IsHot == true //热门
                                    && p.State == false //启用
                                    && p.Id == plate.Id //板块编号
                                    && v.State == false //启用
                                    && v.VideoState == 3 //审核通过
                                    && m.State == false
                                    && v.VideoSource == false //管理员
                                select new IndexVideoView()
                                {
                                    Id = v.Id,
                                    Title = v.Title,
                                    About = v.About,
                                    SmallPicturePath = v.SmallPicturePath,
                                    BigPicturePath = v.BigPicturePath,
                                    PlayCount = v.PlayCount,
                                    SortNum = pv.SortNum,
                                    UpdateTime = v.UpdateTime.HasValue ? Convert.ToDateTime(v.UpdateTime) : v.CreateTime,
                                    CreateTime = v.CreateTime
                                }
                         ).AsQueryable();
            #endregion

            #region 没有禁用的用户上传的分类视频
            var userVideos = (from pv in this._plateVideoRepository.GetEntityList()
                              join p in this._plateRepository.GetEntityList() on pv.PlateId equals p.Id
                              join v in this._videoRepository.GetEntityList() on pv.VideoId equals v.Id
                              join u in this._userRepository.GetEntityList() on v.CreateManageId equals u.Id
                              join c in this._categoryRepository.GetEntityList(CondtionEqualState()) on v.CategoryId equals c.Id
                              into cJoin//左外联分类表 0 表示首页板块
                              from cateModel in cJoin.DefaultIfEmpty()
                              where pv.State == false //启动
                                  && pv.IsHot == true //热门
                                  && p.State == false //启用
                                  && p.Id == plate.Id //板块编号
                                  && v.State == false //启用
                                  && v.VideoState == 3 //审核通过
                                  && u.State == false
                                  && v.VideoSource == true //用户
                              select new IndexVideoView()
                              {
                                  Id = v.Id,
                                  Title = v.Title,
                                  About = v.About,
                                  SmallPicturePath = v.SmallPicturePath,
                                  BigPicturePath = v.BigPicturePath,
                                  SortNum = pv.SortNum,
                                  PlayCount = v.PlayCount,
                                  UpdateTime = v.UpdateTime.HasValue ? Convert.ToDateTime(v.UpdateTime) : v.CreateTime,
                                  CreateTime = v.CreateTime
                              }
                         ).AsQueryable();
            #endregion

            #region 合并用户视频
            if (manageVideos.Any())
            {
                hotPlateVideos.AddRange(manageVideos);
            }
            if (userVideos.Any())
            {
                hotPlateVideos.AddRange(userVideos);
            }
            #endregion

            #region 排序
            var queryable = (from v in hotPlateVideos
                             orderby v.SortNum descending, //按排序数量降序
                                     v.PlayCount descending//按播放次数降序
                             select v).AsQueryable();
            #endregion

            int pagesize = 10;
            if (plate.PageSize <= 0)
            {
                if (this.PageSize > 0)
                {
                    pagesize = this.PageSize;
                }
            }
            else
            {
                pagesize = plate.PageSize;
            }
            var pageQueryable = queryable.Take(pagesize);
            if (pageQueryable.Any())
            {
                hotPlateVideos = pageQueryable.ToList();
            }
            return hotPlateVideos;
        }
        #endregion

        #region 获取推荐板块视频
        /// <summary>
        /// 获取推荐板块视频
        /// </summary>
        /// <param name="plateId"></param>
        /// <returns></returns>
        public IList<IndexVideoView> GetRecommendPlateVideo(int plateId)
        {
            var recommendPlateVideos = new List<IndexVideoView>();
            if (plateId <= 0)
            {
                return recommendPlateVideos;
            }
            var plate = this._plateRepository.GetEntity(ConditionEqualId(plateId));
            if (plate == null)
            {
                return recommendPlateVideos;
            }
            #region 没有禁用的管理员上传的分类视频
            var manageVideos = (from pv in this._plateVideoRepository.GetEntityList()
                                join p in this._plateRepository.GetEntityList() on pv.PlateId equals p.Id
                                join v in this._videoRepository.GetEntityList() on pv.VideoId equals v.Id
                                join m in this._manageRepository.GetEntityList() on v.CreateManageId equals m.Id
                                join c in this._categoryRepository.GetEntityList(CondtionEqualState()) on v.CategoryId equals c.Id
                                into cJoin//左外联分类表 0 表示首页板块
                                from cateModel in cJoin.DefaultIfEmpty()
                                where pv.State == false //启动
                                    && pv.IsRecommend == true //推荐
                                    && p.State == false //启用
                                    && p.Id == plate.Id //板块编号
                                    && v.State == false //启用
                                    && v.VideoState == 3 //审核通过
                                    && m.State == false
                                    && v.VideoSource == false //管理员
                                select new IndexVideoView()
                                {
                                    Id = v.Id,
                                    PlayCount = v.PlayCount,
                                    Title = v.Title,
                                    About = v.About,
                                    SmallPicturePath = v.SmallPicturePath,
                                    BigPicturePath = v.BigPicturePath,
                                    UpdateTime = v.UpdateTime.HasValue ? Convert.ToDateTime(v.UpdateTime) : v.CreateTime,
                                    CreateTime = v.CreateTime,
                                    SortNum = pv.SortNum
                                }
                         ).AsQueryable();
            #endregion

            #region 没有禁用的用户上传的分类视频
            var userVideos = (from pv in this._plateVideoRepository.GetEntityList()
                              join p in this._plateRepository.GetEntityList() on pv.PlateId equals p.Id
                              join v in this._videoRepository.GetEntityList() on pv.VideoId equals v.Id
                              join u in this._userRepository.GetEntityList() on v.CreateManageId equals u.Id
                              join c in this._categoryRepository.GetEntityList(CondtionEqualState()) on v.CategoryId equals c.Id
                              into cJoin//左外联分类表 0 表示首页板块
                              from cateModel in cJoin.DefaultIfEmpty()
                              where pv.State == false //启动
                                  && pv.IsRecommend == true //推荐
                                  && p.State == false //启用
                                  && p.Id == plate.Id //板块编号
                                  && v.State == false //启用
                                  && v.VideoState == 3 //审核通过
                                  && u.State == false
                                  && v.VideoSource == true //用户
                              select new IndexVideoView()
                              {
                                  Id = v.Id,
                                  Title = v.Title,
                                  PlayCount = v.PlayCount,
                                  About = v.About,
                                  BigPicturePath = v.BigPicturePath,
                                  SmallPicturePath = v.SmallPicturePath,
                                  UpdateTime = v.UpdateTime.HasValue ? Convert.ToDateTime(v.UpdateTime) : v.CreateTime,
                                  CreateTime = v.CreateTime,
                                  SortNum = pv.SortNum
                              }
                         ).AsQueryable();
            #endregion

            #region 合并用户视频
            if (manageVideos.Any())
            {
                recommendPlateVideos.AddRange(manageVideos);
            }
            if (userVideos.Any())
            {
                recommendPlateVideos.AddRange(userVideos);
            }
            #endregion

            #region 排序
            var queryable = (from v in recommendPlateVideos
                             orderby v.SortNum descending, //按排序数量降序
                                     v.UpdateTime descending//按更新时间降序
                             select v).AsQueryable();
            #endregion

            int pagesize = 10;
            if (plate.PageSize <= 0)
            {
                if (this.PageSize > 0)
                {
                    pagesize = this.PageSize;
                }
            }
            else
            {
                pagesize = plate.PageSize;
            }
            var pageQueryable = queryable.Take(pagesize);
            if (pageQueryable.Any())
            {
                recommendPlateVideos = pageQueryable.ToList();
            }
            return recommendPlateVideos;
        }
        #endregion

        #region 推荐和热门板块视频
        /// <summary>
        /// 推荐和热门板块视频
        /// </summary>
        /// <param name="plateId"></param>
        /// <returns></returns>
        public RecommendAndHotPlateVideoView GetRecommendAndHotPlateVideo(int plateId)
        {
            var plateVideoView = new RecommendAndHotPlateVideoView();
            if (plateId <= 0)
            {
                return plateVideoView;
            }
            var plate = this._plateRepository.GetEntity(ConditionEqualId(plateId));
            if (plate == null)
            {
                return plateVideoView;
            }
            plateVideoView.HotPlateVideo = GetHotPlateVideo(plateId);
            plateVideoView.RecommendPlateVideo = GetRecommendPlateVideo(plateId);
            return plateVideoView;
        }
        #endregion

        #endregion

        #region 首页——分类视频

        #region 获取热门分类视频
        /// <summary>
        /// 获取热门分类视频
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public IList<IndexVideoView> GetHotCategoryVideo(int categoryId)
        {
            var hotCategoryVideos = new List<IndexVideoView>();
            if (categoryId <= 0)
            {
                return hotCategoryVideos;
            }
            var category = this._categoryRepository.GetEntity(ConditionEqualId(categoryId));
            if (category == null)
            {
                return hotCategoryVideos;
            }
            #region 没有禁用的管理员上传的分类视频
            var manageVideos = (from pv in this._plateVideoRepository.GetEntityList()
                                join c in this._categoryRepository.GetEntityList() on pv.CategoryId equals c.Id
                                join v in this._videoRepository.GetEntityList() on pv.VideoId equals v.Id
                                join m in this._manageRepository.GetEntityList() on v.CreateManageId equals m.Id
                                where pv.State == false //启用
                                    && pv.IsHot == true //热门
                                    && c.State == false //启用
                                    && c.LocationPath.StartsWith(category.LocationPath) //匹配分类下的所有子分类
                                    && v.State == false //启用
                                    && v.VideoState == 3 //审核通过
                                    && m.State == false//启用
                                    && v.VideoSource == false //管理员
                                select new IndexVideoView()
                                {
                                    Id = v.Id,
                                    Title = v.Title,
                                    About = v.About,
                                    PlayCount = v.PlayCount,
                                    SortNum = pv.SortNum,
                                    SmallPicturePath = v.SmallPicturePath,
                                    BigPicturePath = v.BigPicturePath,
                                    CreateTime = v.CreateTime,
                                    UpdateTime = v.UpdateTime.HasValue ? Convert.ToDateTime(v.UpdateTime) : v.CreateTime
                                }
                         ).AsQueryable();
            #endregion

            #region 没有禁用的用户上传的分类视频
            var userVideos = (from pv in this._plateVideoRepository.GetEntityList()
                              join c in this._categoryRepository.GetEntityList() on pv.CategoryId equals c.Id
                              join v in this._videoRepository.GetEntityList() on pv.VideoId equals v.Id
                              join u in this._userRepository.GetEntityList() on v.CreateManageId equals u.Id
                              where pv.State == false //启用
                                  && pv.IsHot == true //热门
                                  && c.State == false //启用
                                  && c.LocationPath.StartsWith(category.LocationPath) //匹配分类下的所有子分类
                                  && v.State == false //启用
                                  && v.VideoState == 3 //审核通过
                                  && u.State == false//启用
                                  && v.VideoSource == true //用户
                              select new IndexVideoView()
                              {
                                  Id = v.Id,
                                  Title = v.Title,
                                  About = v.About,
                                  SortNum = pv.SortNum,
                                  PlayCount = v.PlayCount,
                                  SmallPicturePath = v.SmallPicturePath,
                                  BigPicturePath = v.BigPicturePath,
                                  CreateTime = v.CreateTime,
                                  UpdateTime = v.UpdateTime.HasValue ? Convert.ToDateTime(v.UpdateTime) : v.CreateTime
                              }
                         ).AsQueryable();
            #endregion

            #region 合并用户视频
            if (manageVideos.Any())
            {
                hotCategoryVideos.AddRange(manageVideos);
            }
            if (userVideos.Any())
            {
                hotCategoryVideos.AddRange(userVideos);
            }
            #endregion

            #region 排序
            var queryable = (from v in hotCategoryVideos
                             orderby v.SortNum descending, //按排序数量降序
                                 v.UpdateTime descending
                             //按上架时间降序
                             select v).AsQueryable();
            #endregion

            int pagesize = 10;
            if (category.PageSize <= 0)
            {
                if (this.PageSize > 0)
                {
                    pagesize = this.PageSize;
                }
            }
            else
            {
                pagesize = category.PageSize;
            }
            var pageQueryable = queryable.Take(pagesize);
            if (pageQueryable.Any())
            {
                hotCategoryVideos = pageQueryable.ToList();
            }

            return hotCategoryVideos;
        }
        #endregion

        #region 获取推荐分类视频
        /// <summary>
        /// 获取推荐分类视频
        /// </summary>
        /// <returns></returns>
        public IList<IndexVideoView> GetRecommendCategoryVideo(int categoryId)
        {
            var recommendCategoryVideos = new List<IndexVideoView>();
            if (categoryId <= 0)
            {
                return recommendCategoryVideos;
            }
            var category = this._categoryRepository.GetEntity(ConditionEqualId(categoryId));
            if (category == null)
            {
                return recommendCategoryVideos;
            }
            #region 没有禁用的管理员上传的分类视频
            var manageVideos = (from pv in this._plateVideoRepository.GetEntityList()
                                join c in this._categoryRepository.GetEntityList() on pv.CategoryId equals c.Id
                                join v in this._videoRepository.GetEntityList() on pv.VideoId equals v.Id
                                join m in this._manageRepository.GetEntityList() on v.CreateManageId equals m.Id
                                where pv.State == false //启用
                                    && pv.IsRecommend == true //推荐
                                    && c.State == false //启用
                                    && c.LocationPath.StartsWith(category.LocationPath) //匹配分类下的所有子分类
                                    && v.State == false //启用
                                    && v.VideoState == 3 //审核通过
                                    && m.State == false//启用
                                    && v.VideoSource == false //管理员
                                select new IndexVideoView()
                                {
                                    Id = v.Id,
                                    Title = v.Title,
                                    About = v.About,
                                    PlayCount = v.PlayCount,
                                    SortNum = pv.SortNum,
                                    CreateTime = v.CreateTime,
                                    UpdateTime = v.UpdateTime.HasValue ? Convert.ToDateTime(v.UpdateTime) : v.CreateTime,
                                    BigPicturePath = v.BigPicturePath,
                                    SmallPicturePath = v.SmallPicturePath
                                }
                         ).AsQueryable();
            #endregion

            #region 没有禁用的用户上传的分类视频
            var userVideos = (from pv in this._plateVideoRepository.GetEntityList()
                              join c in this._categoryRepository.GetEntityList() on pv.CategoryId equals c.Id
                              join v in this._videoRepository.GetEntityList() on pv.VideoId equals v.Id
                              join u in this._userRepository.GetEntityList() on v.CreateManageId equals u.Id
                              where pv.State == false //启用
                                 && pv.IsRecommend == true //推荐
                                  && c.State == false //启用
                                  && c.LocationPath.StartsWith(category.LocationPath) //匹配分类下的所有子分类
                                  && v.State == false //启用
                                  && v.VideoState == 3 //审核通过
                                  && u.State == false//启用
                                  && v.VideoSource == true //用户
                              select new IndexVideoView()
                              {
                                  Id = v.Id,
                                  Title = v.Title,
                                  About = v.About,
                                  SortNum = pv.SortNum,
                                  PlayCount = v.PlayCount,
                                  SmallPicturePath = v.SmallPicturePath,
                                  BigPicturePath = v.BigPicturePath,
                                  CreateTime = v.CreateTime,
                                  UpdateTime = v.UpdateTime.HasValue ? Convert.ToDateTime(v.UpdateTime) : v.CreateTime
                              }
                         ).AsQueryable();
            #endregion

            #region 合并用户视频
            if (manageVideos.Any())
            {
                recommendCategoryVideos.AddRange(manageVideos);
            }
            if (userVideos.Any())
            {
                recommendCategoryVideos.AddRange(userVideos);
            }
            #endregion

            #region 排序
            var queryable = (from v in recommendCategoryVideos
                             orderby v.SortNum descending, //按排序数量降序
                                 v.UpdateTime descending
                             //按上架时间降序
                             select v).AsQueryable();
            #endregion

            int pagesize = 10;
            if (category.PageSize <= 0)
            {
                if (this.PageSize > 0)
                {
                    pagesize = this.PageSize;
                }
            }
            else
            {
                pagesize = category.PageSize;
            }
            var pageQueryable = queryable.Take(pagesize);
            if (pageQueryable.Any())
            {
                recommendCategoryVideos = pageQueryable.ToList();
            }

            return recommendCategoryVideos;
        }
        #endregion

        #region 推荐和热门分类视频
        /// <summary>
        /// 推荐和热门分类视频
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public RecommendAndHotCategoryVideoView GetRecommendAndHotCategoryVideo(int categoryId)
        {
            var categoryVideoView = new RecommendAndHotCategoryVideoView();
            var hotCategoryVideos = new List<VideoView>();
            if (categoryId <= 0)
            {
                return categoryVideoView;
            }
            var category = this._categoryRepository.GetEntity(ConditionEqualId(categoryId));
            if (category == null)
            {
                return categoryVideoView;
            }
            categoryVideoView.Category = new CategorysView()
            {
                ParentCategory = new HKSJ.WBVV.Entity.ViewModel.Client.CategoryView()
                {
                    Id = category.Id,
                    Name = category.Name,
                    ParentId = category.ParentId
                },
                ChildCategory = (from c in this._categoryRepository.GetEntityList()
                                 where c.State == false && c.ParentId == category.Id
                                 select new HKSJ.WBVV.Entity.ViewModel.Client.CategoryView()
                                 {
                                     Id = c.Id,
                                     Name = c.Name,
                                     ParentId = c.ParentId
                                 }).ToList()
            };
            categoryVideoView.HotCategoryVideo = GetHotCategoryVideo(categoryId);
            categoryVideoView.RecommendCategoryVideo = GetRecommendCategoryVideo(categoryId);
            return categoryVideoView;
        }
        #endregion

        #endregion

        #region 检索页——获取过滤页面的视频列表

        #region 获取分类下的所有视频
        /// <summary>
        /// 获取分类下的所有视频
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        private IQueryable<VideoView> GetCategoryVideos(int categoryId)
        {
            IQueryable<VideoView> queryable = null;
            if (categoryId <= 0)
            {
                return null;
            }
            var category = this._categoryRepository.GetEntity(ConditionEqualId(categoryId));
            if (category == null)
            {
                return null;
            }
            queryable = (from video in this._videoRepository.GetEntityList()
                         join cate in this._categoryRepository.GetEntityList() on video.CategoryId equals cate.Id
                         where video.State == false //启用
                               && video.VideoState == 3 //审核通过
                               && cate.State == false //启用
                               && cate.LocationPath.StartsWith(category.LocationPath) //匹配分类下的所有子分类
                         orderby video.SortNum descending,//按排序数量降序
                             video.PlayCount descending,//按播放降序
                             video.CreateTime descending//按创建时间降序
                         select new VideoView()
                         {
                             DictionaryId = GetDictionarys('r', 'c', video.Filter),
                             DictionaryViews = GetDictionaryViewList(video.Filter),
                             CategoryName = cate.Name,//类型分类名称
                             IsHot = video.IsHot,
                             IsRecommend = video.IsRecommend,
                             PlayCount = video.PlayCount,
                             About = video.About,
                             BigPicturePath = video.BigPicturePath,
                             CreateTime = video.CreateTime,
                             Director = video.Director,
                             Id = video.Id,
                             IsOfficial = video.IsOfficial,
                             SmallPicturePath = video.SmallPicturePath,
                             Starring = video.Starring,
                             Tags = video.Tags,
                             Title = video.Title,
                             Filter = video.Filter,
                             UpdateTime = video.UpdateTime.HasValue ? Convert.ToDateTime(video.UpdateTime) : video.CreateTime
                         }
                ).AsQueryable();
            return queryable;
        }
        #endregion

        #region 过滤视频
        /// <summary>
        /// 过滤视频
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sortName"></param>
        /// <param name="totalCount"></param>
        /// <param name="totalIndex"></param>
        /// <returns></returns>
        public IList<VideoView> GetCategoryVideo(string filter, string sortName, out int totalCount, out int totalIndex)
        {
            IList<VideoView> videoViews = new List<VideoView>();
            if (string.IsNullOrEmpty(filter))
            {
                totalCount = 0;
                totalIndex = 0;
                return videoViews;
            }
            var dictionarys = GetDictionarys('g', filter);
            if (dictionarys.Count <= 0)
            {
                totalCount = 0;
                totalIndex = 0;
                return videoViews;
            }
            IQueryable<VideoView> queryable = null;
            foreach (var dict in dictionarys)
            {
                //分类
                if (dict.Key.ToLower() == "c")
                {
                    var dicts = GetDictionarys('r', 'c', dict.Value);
                    if (dicts.Count > 0)
                    {
                        //只有一个分类
                        var item = dicts.First();
                        queryable = GetCategoryVideos(item.Key);
                        if (item.Value > 0)
                        {
                            queryable = GetCategoryVideos(item.Value);
                        }
                    }
                }
                //字典
                else if (dict.Key.ToLower() == "d")
                {
                    var dicts = GetDictionarys('r', 'c', dict.Value);
                    if (dicts.Count > 0)
                    {
                        foreach (var item in dicts)
                        {
                            if (item.Value > 0 && queryable != null)
                            {
                                queryable = queryable.Where(q => GetDictionarys('r', 'c', q.Filter).Any(i => i.Key == item.Key && i.Value == item.Value));
                            }
                        }
                    }
                }
            }
            if (queryable == null)
            {
                totalCount = 0;
                totalIndex = 0;
                return videoViews;
            }
            if (string.IsNullOrEmpty(sortName) || sortName.ToLower().Equals("time"))//按更新时间排序
            {
                queryable = queryable.OrderBy(OrderCondtionUpdateTime(true));
            }
            else if (sortName.ToLower().Equals("hot"))
            {
                queryable = queryable.OrderBy(OrderCondtionPlayCount(true));
            }
            bool isExists = queryable.Any();
            totalCount = isExists ? queryable.Count() : 0;
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
            if (isExists)
            {
                videoViews = queryable.Skip((this.PageIndex - 1) * this.PageSize).Take(this.PageSize).ToList();
            }
            return videoViews;
        }
        #endregion

        #region 分页显示过滤视频
        /// <summary>
        /// 分页显示过滤视频
        /// </summary>
        /// <param name="filter">过滤条件gc2c3rgd2c4r</param>
        /// <param name="sortName">排序类型默认热门</param>
        /// <returns></returns>
        public PageResult GetFilterVideo(string filter, string sortName)
        {
            int totalCount = 0;
            int totalIndex = 0;
            IList<VideoView> plateViews = GetCategoryVideo(filter, sortName, out totalCount, out totalIndex);
            return new PageResult()
            {
                PageSize = this.PageSize,
                PageIndex = this.PageIndex,
                TotalCount = totalCount,
                TotalIndex = totalIndex,
                Data = plateViews
            };
        }
        #endregion

        #endregion

        #region 根据视频ID获得视频实体
        /// <summary>
        /// 根据视频ID获得视频实体
        /// </summary>
        /// <param name="id">视频ID</param>
        /// <returns></returns>
        public Video GetAVideoInfoById(int id)
        {
            Video video;
            video = this._videoRepository.GetEntity(ConditionEqualId(id));
            video.VideoPath += "?pm3u8/0";
            video.VideoPath = _qiniuUploadBusiness.GetDownloadUrl(video.VideoPath, "video");
            return video;
        }
        #endregion

        #region 更新一个视频实体
        /// <summary>
        /// 更新一个视频实体
        /// </summary>
        /// <param name="videoPara"></param>
        /// <returns></returns>
        public bool UpdateAVideo(MyVideoPara videoPara)
        {
            var video = _videoRepository.GetEntity(ConditionEqualId(videoPara.Id));
            if (video == null || video.Id < 1) return false;
            video.CategoryId = videoPara.CategoryId;
            video.Tags = videoPara.Tags.Trim('|');
            video.Title = videoPara.Title;
            video.About = videoPara.About;
            video.Copyright = videoPara.Copyright;
            video.IsPublic = videoPara.IsPublic > 0 ? true : false;
            video.IsOfficial = videoPara.IsOfficial > 0 ? true : false;
            if (!string.IsNullOrEmpty(videoPara.BigPicturePath) && !video.BigPicturePath.Equals(videoPara.BigPicturePath))
            {
                if (!string.IsNullOrEmpty(video.BigPicturePath))
                {
                    _qiniuUploadBusiness.DeleteQiniuImageByKey(video.BigPicturePath);
                }
                video.BigPicturePath = videoPara.BigPicturePath;
            }
            if (!string.IsNullOrEmpty(videoPara.SmallPicturePath) && !video.SmallPicturePath.Equals(videoPara.SmallPicturePath))
            {
                if (!string.IsNullOrEmpty(video.SmallPicturePath))
                {
                    _qiniuUploadBusiness.DeleteQiniuImageByKey(video.SmallPicturePath);
                }
                video.SmallPicturePath = videoPara.SmallPicturePath;
            }
            if (videoPara.CreateManageId > 0)
            {
                video.CreateManageId = videoPara.CreateManageId;
            }
            if (!string.IsNullOrEmpty(videoPara.Filter))
            {
                video.Filter = videoPara.Filter;
            }
            var flag = this._videoRepository.UpdateEntity(video);
            if (flag)
            {
                UpdateAVideoIndex(video);
            }

            //TODO insert 刘强添加标签
            try
            {
                this._tagsBusiness = ((Autofac.IContainer)HttpRuntime.Cache["containerKey"]).Resolve<ITagsBusiness>();
                //上传视频的人
                this._tagsBusiness.UserId = video.CreateManageId;
                this._tagsBusiness.AsyncCreateTags();
            }
            catch (Exception ex)
            {
#if !DEBUG
                      LogBuilder.Log4Net.Error("更新标签失败", ex.MostInnerException());
#else
                Console.WriteLine(LanguageUtil.Translate("api_Business_Video_UpdateAVideo_updateTagsFailed") + ex.MostInnerException().Message);
#endif
            }

            return flag;
        }
        #endregion

        /// <summary>
        /// 获取用户的视频播放数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetPlayCountByUserId(int userId)
        {
            int playCount = 0;
            //该用户的视频播放数
            var model = (from video in
                             (from video in _videoRepository.GetEntityList().Where(p => !p.State).Where(p => p.VideoState == 3)
                              where this._videoPlayRecordRepository.GetEntityList()
                                       .Where(p => p.UserId == userId)
                                       .Where(p => p.VideoId == video.Id).Count() > 0
                              select new
                              {
                                  PlayCount = video.PlayCount,
                                  Dummy = "x"
                              })
                         group video by new { video.Dummy } into g
                         select new
                         {
                             PlayCount = g.Sum(p => p.PlayCount)
                         }).AsQueryable();
            if (model.FirstOrDefault() != null)
                playCount = model.FirstOrDefault().PlayCount;
            return playCount;
        }

        public dynamic CreateVideo(MyVideoPara videoPara)
        {
            var video = new Video();
            video.CategoryId = videoPara.CategoryId;
            video.Tags = videoPara.Tags;
            video.Title = videoPara.Title;
            video.About = videoPara.About;
            video.Copyright = videoPara.Copyright;
            video.IsPublic = videoPara.IsPublic > 0 ? true : false;
            video.BigPicturePath = videoPara.BigPicturePath;
            video.CreateManageId = videoPara.CreateManageId;

            _videoRepository.CreateEntity(video);
            return new { Id = video.Id };

        }

        #region 搜索视频分页
        /// <summary>
        /// 搜索视频分页
        /// </summary>
        /// <param name="searchKey">搜索关键字</param>
        /// <param name="totalcount">总记录熟</param>
        /// <param name="pageSize">页码</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="sortName">排序列</param>
        /// <returns></returns>
        public List<VideoView> SearchVideoByPage(string searchKey, out int totalcount, int pageSize, int pageIndex, SortName sortName)
        {
            searchKey = searchKey.UrlDecode();
            searchKey = searchKey.Replace(" ", "");
            if (searchKey.Length > 14)
            {
                searchKey = searchKey.Substring(0, 15);
            }
            List<VideoView> videoResult = new List<VideoView>();
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(IndexManager.indexPath), new NoLockFactory());
            IndexReader reader = IndexReader.Open(directory, true);
            IndexSearcher searcher = new IndexSearcher(reader);
            BooleanQuery queryOr = new BooleanQuery();
            TermQuery querytitle = null;
            TermQuery queryabout = null;
            // TermQuery querytags = null;
            string[] splitWords = SplitContent.SplitWords(searchKey);
            foreach (string word in splitWords)
            {
                querytitle = new TermQuery(new Term("title", word));
                queryabout = new TermQuery(new Term("about", word));
                // querytags = new TermQuery(new Term("tags", word));
                queryOr.Add(querytitle, BooleanClause.Occur.SHOULD);//这里设置 条件为Or关系
                queryOr.Add(queryabout, BooleanClause.Occur.SHOULD);
                //  queryOr.Add(querytags, BooleanClause.Occur.SHOULD);
            }
            BooleanQuery querystate = new BooleanQuery();
            var querystateTerm = new TermQuery(new Term("videoState", "3"));
            querystate.Add(querystateTerm, BooleanClause.Occur.MUST);
            BooleanQuery bqQuery = new BooleanQuery();
            bqQuery.Add(querystate, BooleanClause.Occur.MUST);
            bqQuery.Add(queryOr, BooleanClause.Occur.MUST);
            //--------------------------------------
            TopScoreDocCollector collector = TopScoreDocCollector.create(1000, true);
            ScoreDoc[] docs = null;
            if (sortName > 0)
            {
                string sortField = (int)sortName == 1 ? "createTimeNum" : "playCount";
                Sort sort = new Sort();
                SortField sfdata = null;
                if (sortField == "playCount")
                {
                    sfdata = new SortField(sortField, SortField.INT, true);
                }
                else
                {
                    sfdata = new SortField(sortField, SortField.LONG, true);
                }
                sort.SetSort(sfdata);
                TopFieldDocs topdocss = searcher.Search(bqQuery, null, 1000, sort);
                totalcount = topdocss.totalHits;
                docs = topdocss.scoreDocs;
            }
            else
            {
                TopDocs topdocss = searcher.Search(bqQuery, 1000);
                totalcount = topdocss.totalHits;
                docs = topdocss.scoreDocs;
            }
            //searcher.Search(queryOr, null, collector);
            //totalcount = collector.GetTotalHits();
            ////ScoreDoc[] docs = collector.TopDocs((PageIndex - 1) * PageSize, PageSize).scoreDocs;//取前十条数据  可以通过它实现LuceneNet搜索结果分页
            //ScoreDoc[] docs = collector.TopDocs(0, totalcount).scoreDocs;
            int startIndex = (pageIndex - 1) * pageSize;
            int endIndex = 0;
            if (pageIndex * pageSize - totalcount < 0)
            {
                endIndex = pageIndex * pageSize;
            }
            else
            {
                endIndex = totalcount;
            }
            for (int i = startIndex; i < endIndex; i++)
            {
                int docId = docs[i].doc;
                Document doc = searcher.Doc(docId);
                VideoView video = new VideoView();
                video.Title = SplitContent.HightLight(searchKey, doc.Get("title")) == "" ? doc.Get("title") : SplitContent.HightLight(searchKey, doc.Get("title"));
                video.About = SplitContent.HightLight(searchKey, doc.Get("about")) == "" ? doc.Get("about") : SplitContent.HightLight(searchKey, doc.Get("about"));
                video.Tags = SplitContent.HightLight(searchKey, doc.Get("tags")) == "" ? doc.Get("tags") : SplitContent.HightLight(searchKey, doc.Get("tags"));
                video.Id = Convert.ToInt32(doc.Get("id"));
                video.SmallPicturePath = doc.Get("smallPicturePath");
                video.BigPicturePath = doc.Get("bigPicturePath");
                video.CreateTime = Convert.ToDateTime(doc.Get("createTime"));
                video.IsOfficial = Convert.ToBoolean(doc.Get("isOfficial"));
                video.PlayCount = Convert.ToInt32(doc.Get("playCount"));
                videoResult.Add(video);
            }

            #region 先拿list后排序分页
            //if (sortName > 0)
            //{
            //    if (sortName == SortName.PlayCount)
            //    {
            //        videoResult =
            //            videoResult.OrderByDescending(c => c.PlayCount).ToList();
            //    }
            //    else
            //    {
            //        videoResult =
            //            videoResult.OrderByDescending(c => c.CreateTime).ToList();
            //    }
            //}
            //int count = videoResult.Count;
            //if (pageIndex * pageSize - count <0)
            //{
            //    videoResult =
            //        videoResult.GetRange((pageIndex - 1) * pageSize, pageSize);
            //}
            //else
            //{
            //    videoResult =
            //       videoResult.GetRange((pageIndex - 1) * pageSize, count - (pageIndex - 1) * pageSize);
            //} 
            #endregion
            return videoResult;
        }
        #endregion

        #region 播放页推荐视频

        /// <summary>
        /// 搜索相关推荐视频
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="videoId"></param>
        /// <param name="recommendationNum"></param>
        /// <returns></returns>
        public List<VideoView> SearchVideoByRecom(string searchKey, int videoId, int recommendationNum = 6)
        {
            searchKey = searchKey.UrlDecode();
            List<string> tags = new List<string>();
            if (searchKey.IndexOf('|') > 0)
            {
                tags = searchKey.Split('|').ToList();
            }
            else
            {
                tags.Add(searchKey);
            }

            List<VideoView> videoResult = new List<VideoView>();
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(IndexManager.indexPath), new NoLockFactory());
            IndexReader reader = IndexReader.Open(directory, true);
            IndexSearcher searcher = new IndexSearcher(reader);
            BooleanQuery queryOr = new BooleanQuery();
            TermQuery querytitle = null;
            TermQuery querytrue = null;
            foreach (string word in tags)
            {
                querytitle = new TermQuery(new Term("tags", word));
                queryOr.Add(querytitle, BooleanClause.Occur.SHOULD);
            }
            BooleanQuery querystate = new BooleanQuery();
            var querystateTerm = new TermQuery(new Term("videoState", "3"));
            querystate.Add(querystateTerm, BooleanClause.Occur.MUST);
            BooleanQuery bqQuery = new BooleanQuery();
            bqQuery.Add(querystate, BooleanClause.Occur.MUST);
            bqQuery.Add(queryOr, BooleanClause.Occur.MUST);
            Sort sort = new Sort(new SortField("playCount", SortField.INT, true));
            ScoreDoc[] docs = searcher.Search(bqQuery, null, 100, sort).scoreDocs;
            int lenNum = docs.Length < recommendationNum ? docs.Length : recommendationNum;
            for (int i = 0; i < lenNum; i++)
            {
                int docId = docs[i].doc;
                Document doc = searcher.Doc(docId);
                VideoView video = new VideoView();
                video.Title = SplitContent.HightLight(searchKey, doc.Get("title")) == "" ? doc.Get("title") : SplitContent.HightLight(searchKey, doc.Get("title"));
                video.About = SplitContent.HightLight(searchKey, doc.Get("about")) == "" ? doc.Get("about") : SplitContent.HightLight(searchKey, doc.Get("about"));
                video.Id = Convert.ToInt32(doc.Get("id"));
                if (video.Id == videoId)
                {
                    if (lenNum >= 6)
                    {
                        lenNum++;
                    }
                    continue;
                }
                video.PlayCount = Convert.ToInt32(doc.Get("playCount"));
                video.CommentCount = Convert.ToInt32(doc.Get("commentCount"));
                video.SmallPicturePath = doc.Get("smallPicturePath");
                videoResult.Add(video);
            }
            return videoResult;
        }
        #endregion


        #region 搜索头部视频
        /// <summary>
        /// 搜索头部视频
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public List<VideoView> SearchVideoByTop(string searchKey)
        {
            searchKey = searchKey.UrlDecode();
            List<VideoView> videoResult = new List<VideoView>();
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(IndexManager.indexPath), new NoLockFactory());
            IndexReader reader = IndexReader.Open(directory, true);
            IndexSearcher searcher = new IndexSearcher(reader);
            BooleanQuery queryOr = new BooleanQuery();
            TermQuery querytitle = null;
            TermQuery querytrue = null;
            string[] splitWords = SplitContent.SplitWords(searchKey);
            foreach (string word in splitWords)
            {
                querytitle = new TermQuery(new Term("title", word));
                queryOr.Add(querytitle, BooleanClause.Occur.MUST);//这里设置 条件为Or关系
            }
            if (querytitle == null) return videoResult;
            querytrue = new TermQuery(new Term("isofficialtrue", "官方"));
            queryOr.Add(querytrue, BooleanClause.Occur.MUST);
            TermQuery querystate = null;
            querystate = new TermQuery(new Term("videoState", "3"));
            queryOr.Add(querystate, BooleanClause.Occur.MUST);
            //--------------------------------------
            TopScoreDocCollector collector = TopScoreDocCollector.create(1000, true);
            searcher.Search(queryOr, null, collector);
            ScoreDoc[] docs = collector.TopDocs(0, 3).scoreDocs;//取前5条数据  可以通过它实现LuceneNet搜索结果分页
            for (int i = 0; i < docs.Length; i++)
            {
                int docId = docs[i].doc;
                Document doc = searcher.Doc(docId);
                VideoView video = new VideoView();
                video.Title = SplitContent.HightLight(searchKey, doc.Get("title")) == "" ? doc.Get("title") : SplitContent.HightLight(searchKey, doc.Get("title"));
                video.About = SplitContent.HightLight(searchKey, doc.Get("about")) == "" ? doc.Get("about") : SplitContent.HightLight(searchKey, doc.Get("about"));
                video.Tags = SplitContent.HightLight(searchKey, doc.Get("tags")) == "" ? doc.Get("tags") : SplitContent.HightLight(searchKey, doc.Get("tags"));
                video.Id = Convert.ToInt32(doc.Get("id"));
                video.SmallPicturePath = doc.Get("smallPicturePath");
                video.BigPicturePath = doc.Get("bigPicturePath");
                video.CreateTime = Convert.ToDateTime(doc.Get("createTime"));
                video.IsOfficial = Convert.ToBoolean(doc.Get("isOfficial"));
                video.PlayCount = Convert.ToInt32(doc.Get("playCount"));
                videoResult.Add(video);
            }
            // videoResult = videoResult.OrderByDescending(c => c.CreateTime).ToList();
            return videoResult;
        }
        #endregion

        #region 搜索我的视频分页
        /// <summary>
        /// 搜索我的视频分页
        /// </summary>
        /// <param name="searchKey">搜索关键字</param>
        /// <param name="pageSize">页码</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public MyVideoViewResult SearchMyVideoByPage(int userId, string searchKey, int pageIndex, int pageSize, int videoState)
        {
            searchKey = searchKey.UrlDecode();
            searchKey = searchKey.Replace(" ", "");
            if (searchKey.Length > 14)
            {
                searchKey = searchKey.Substring(0, 15);
            }
            var videoResult = new MyVideoViewResult() { MyVideoViews = new List<MyVideoView>(), TotalCount = 0 };
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(IndexManager.indexPath), new NoLockFactory());
            IndexReader reader = IndexReader.Open(directory, true);
            IndexSearcher searcher = new IndexSearcher(reader);
            BooleanQuery queryOr = new BooleanQuery();
            TermQuery querytitle = null;
            string[] splitWords = SplitContent.SplitWords(searchKey);
            foreach (string word in splitWords)
            {
                querytitle = new TermQuery(new Term("title", word));
                queryOr.Add(querytitle, BooleanClause.Occur.SHOULD);
            }
            //--------------------------------------
            TopDocs topdocs=  searcher.Search(queryOr, 1000);
            var docs = topdocs.scoreDocs;
            var docslen = docs.Length;
            List<MyVideoView> list = new List<MyVideoView>();
            for (int i = 0; i < docslen; i++)
            {
                int docId = docs[i].doc;
                Document doc = searcher.Doc(docId);
                MyVideoView video = new MyVideoView();
                video.Title = doc.Get("title");
                video.About = doc.Get("about");
                video.Tags = doc.Get("tags");
                video.Id = Convert.ToInt32(doc.Get("id"));
                video.SmallPicturePath = doc.Get("smallPicturePath");
                video.CreateTime = Convert.ToDateTime(doc.Get("createTime"));
                video.PlayCount = Convert.ToInt32(doc.Get("playCount"));
                video.VideoState = Convert.ToInt16(doc.Get("videoState"));
                video.CreateManageId = Convert.ToInt32(doc.Get("createManageId"));
                list.Add(video);
            }
            var mylist = list.Where(c => c.CreateManageId == userId && (videoState == -1 || (videoState != -1 && c.VideoState == videoState))).ToList();
            videoResult.TotalCount = mylist.Count;
            videoResult.PageCount = videoResult.TotalCount % pageSize == 0
                ? videoResult.TotalCount / pageSize
                : videoResult.TotalCount / pageSize + 1;
            int startIndex = (pageIndex - 1) * pageSize;
            int endIndex = 0;
            videoResult.MyVideoViews = mylist.Skip(startIndex).Take(pageSize).ToList();
            this._videoApproveBusiness = ((Autofac.IContainer)HttpRuntime.Cache["containerKey"]).Resolve<IVideoApproveBusiness>();
            foreach (var myVideoView in videoResult.MyVideoViews)
            {
                if (myVideoView.VideoState == 4)
                {
                    myVideoView.ApproveContent = this._videoApproveBusiness.GetApproveContentByVideoId(myVideoView.Id);
                }
            }
            return videoResult;
        }
        #endregion

        #region 新增一个视频信息索引
        public void AddAVideoIndex(Video videoInfo)
        {
            if (string.IsNullOrEmpty(videoInfo.Title) || string.IsNullOrEmpty(videoInfo.Tags))
            {
                return;
            }
            IndexManager.VideoIndex.Add(videoInfo);
        }
        #endregion

        #region 删除一个视频索引
        /// <summary>
        /// 删除一个视频索引
        /// </summary>
        /// <param name="vid">视频ID</param>s
        public void DelAVideoIndex(int vid)
        {
            IndexManager.VideoIndex.Del(vid);
        }
        #endregion

        #region 更新一个视频索引
        public void UpdateAVideoIndex(Video videoInfo)
        {
            if (string.IsNullOrEmpty(videoInfo.Title) || string.IsNullOrEmpty(videoInfo.Tags))
            {
                return;
            }
            IndexManager.VideoIndex.Mod(videoInfo);
        }
        #endregion

        #region 删除全部索引并添加所有已审核视频索引

        public void DelAndUpdateAllIndex()
        {
            IndexManager.VideoIndex.delAllIndex();
            List<Entity.Video> videolist = this._videoRepository.GetEntityList().ToList();
            foreach (var item in videolist)
            {
                AddAVideoIndex(item);
            }
        }
        #endregion

        #region 检测索引文件夹和索引文件是否存在，没有则创建
        public void CheckIndexFile()
        {
            try
            {
                if (Directory.Exists(IndexManager.indexPath) == false)
                {
                    Directory.CreateDirectory(IndexManager.indexPath);
                    FSDirectory directory = FSDirectory.Open(new DirectoryInfo(IndexManager.indexPath), new NativeFSLockFactory());
                    List<Video> videolist = this._videoRepository.GetEntityList().ToList();
                    foreach (var item in videolist)
                    {
                        AddAVideoIndex(item);
                    }
                }
                else
                {
                    FSDirectory directory = FSDirectory.Open(new DirectoryInfo(IndexManager.indexPath), new NativeFSLockFactory());
                    if (IndexReader.IndexExists(directory) == false)
                    {
                        List<Video> videolist = this._videoRepository.GetEntityList().ToList();
                        foreach (var item in videolist)
                        {
                            AddAVideoIndex(item);
                        }
                    }
                }
                StartNewThread();
            }
            catch (Exception ex)
            {
                if (ex is ThreadAbortException)
                {
                    LogBuilder.Log4Net.Info(LanguageUtil.Translate("api_Business_Video_CheckIndexFile_updateTheIndexThreadTerminates"));
                }
                else
                {
#if !DEBUG
                    LogBuilder.Log4Net.Error(ex.MostInnerException().Message);
#else
                    Console.WriteLine(ex.MostInnerException().Message);
#endif
                }
            }

        }
        #endregion

        #region 初始化盘古分词并开启后台进程处理索引更新
        public void StartNewThread()
        {
            IndexManager.VideoIndex.StartNewThread();
        }
        #endregion

        /// <summary>
        /// 删除视频数据和文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteVideoInfo(int id)
        {
            var video = this._videoRepository.GetEntity(ConditionEqualId(id));
            try
            {
                var flag = this._videoRepository.DeleteEntity(video);
                if (flag)
                {
                    //删除七牛上的文件
                    _qiniuUploadBusiness.DeleteQiniuData(video);

                    //删除索引
                    DelAVideoIndex(id);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        #region 获取昨日热点视频
        public List<VideoView> GetYesRecVideos(int num)
        {
            var list = new List<VideoView>();
            string strYes = DateTime.Now.AddDays(-1).ToShortDateString();
            DateTime dtYes1 = Convert.ToDateTime(strYes + " 00:00:00");
            DateTime dtYes2 = Convert.ToDateTime(strYes + " 23:59:59");

            var query = (from a in
                             (
                                 (from vr in _videoPlayRecordRepository.GetEntityList()
                                  where vr.CreateTime >= dtYes1 && vr.CreateTime <= dtYes2
                                  group vr by new { vr.VideoId }
                                      into g
                                      select new
                                      {
                                          VideoId = g.Key.VideoId,
                                          yPlayCount = g.Count(p => true)
                                      }))
                         join v in _videoRepository.GetEntityList(CondtionEqualState()) on new { Id = Convert.ToInt64(a.VideoId) } equals new { v.Id }
                         select new VideoView
                         {
                             Id = (int)v.Id,
                             Title = (string.IsNullOrEmpty(v.Title) ? "" : v.Title),
                             SmallPicturePath = v.SmallPicturePath,
                             About = v.About,
                             PlayCount = a.yPlayCount
                         }).AsQueryable().OrderByDescending(w => w.PlayCount);

            list = query.Take(num).ToList();
            return list;
        }
        #endregion

        #endregion

        #region 传入参数检测



        #endregion

        #region 传入参数
        /// <summary>
        /// 比较视频状态相等
        /// </summary>
        /// <param name="videoState">视频状态（0：转码中，1：转码失败，2：审核中，3：审核通过，4：审核不通过）</param>
        /// <returns></returns>
        private Condtion CondtionEqualVideoState(int videoState)
        {
            var condtion = new Condtion()
            {
                FiledName = "VideoState",
                FiledValue = videoState,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }
        /// <summary>
        /// 比较字典编号相等
        /// </summary>
        /// <param name="dictionaryId"></param>
        /// <returns></returns>
        private Condtion CondtionEquealDictionaryId(int dictionaryId)
        {
            var condtion = new Condtion()
            {
                FiledName = "DictionaryId",
                FiledValue = dictionaryId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }
        /// <summary>
        /// 比较视频是否推荐相等
        /// </summary>
        /// <param name="isRecommend">默认推荐</param>
        /// <returns></returns>
        private Condtion CondtionEqualIsRecommend(bool isRecommend = true)
        {
            var condtion = new Condtion()
            {
                FiledName = "IsRecommend",
                FiledValue = isRecommend,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }
        /// <summary>
        /// 比较视频创建时间大于或者等于
        /// </summary>
        /// <param name="createTime"></param>
        /// <returns></returns>
        private Condtion CondtionGreaterThanOrEqualCreateTime(DateTime createTime)
        {
            var condtion = new Condtion()
            {
                FiledName = "CreateTime",
                FiledValue = createTime.ToString("yyyy-MM-dd"),
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.GreaterThanOrEqual
            };
            return condtion;
        }
        /// <summary>
        /// 比较视频分类编号相等
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        private Condtion CondtionEqualCategoryId(int categoryId)
        {
            var condtion = new Condtion()
            {
                FiledName = "CategoryId",
                FiledValue = categoryId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }
        /// <summary>
        /// 比较视频分类位置路径包含
        /// </summary>
        /// <param name="locationPath"></param>
        /// <returns></returns>
        private Condtion CondtionContainsLocationPath(string locationPath)
        {
            var condtion = new Condtion()
            {
                FiledName = "LocationPath",
                FiledValue = locationPath,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Contains
            };
            return condtion;
        }
        /// <summary>
        /// 比较视频是否热门相等
        /// </summary>
        /// <param name="isHot">默认是热门</param>
        /// <returns></returns>
        private Condtion CondtionEqualIsHot(bool isHot = true)
        {
            var condtion = new Condtion()
            {
                FiledName = "IsHot",
                FiledValue = isHot,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }

        /// <summary>
        /// 比较上传用户ID相等
        /// </summary>
        /// <param name="createManageId"></param>
        /// <returns></returns>
        private Condtion CondtionEqualCreateManageId(int createManageId)
        {
            var condtion = new Condtion()
            {
                FiledName = "CreateManageId",
                FiledValue = createManageId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }
        #endregion

        #region 排序参数
        /// <summary>
        /// 按PlayCount排序
        /// </summary>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        protected OrderCondtion OrderCondtionPlayCount(bool isDesc)
        {
            var orderCodtion = new OrderCondtion()
            {
                FiledName = "PlayCount",
                IsDesc = isDesc
            };
            return orderCodtion;
        }
        /// <summary>
        /// 按UpdateTime排序
        /// </summary>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        protected OrderCondtion OrderCondtionUpdateTime(bool isDesc)
        {
            var orderCodtion = new OrderCondtion()
            {
                FiledName = "UpdateTime",
                IsDesc = isDesc
            };
            return orderCodtion;
        }

        #endregion

        #region 视频列表
        /// <summary>
        /// 获取视频分页集合
        /// </summary>
        /// <param name="condtions">查询条件</param>
        /// <param name="orderCondtions">排序条件</param>
        public PageResult GetVideoPageResult(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            int totalCount = 0;
            int totalIndex = 0;
            IList<VideoView> plateViews = GetVideoList(condtions, orderCondtions, out totalCount, out totalIndex);
            return new PageResult()
            {
                PageSize = this.PageSize,
                PageIndex = this.PageIndex,
                TotalCount = totalCount,
                Data = plateViews
            };
        }
        /// <summary>
        /// 获取视频分页集合
        /// </summary>
        /// <param name="condtions">查询条件</param>
        /// <param name="orderCondtions">排序条件</param>
        public PageResult GetVideoByCategoryIdPageResult(int categoryId, IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            int totalCount = 0;
            int totalIndex = 0;
            IList<VideoView> plateViews = new List<VideoView>();
            if (categoryId == 0)
            {
                plateViews = GetVideoByCategoryLists(categoryId, condtions, orderCondtions, out totalCount, out totalIndex);
            }
            else
            {
                plateViews = GetVideoByCategoryList(categoryId, condtions, orderCondtions, out totalCount, out totalIndex);
            }
            return new PageResult()
            {
                PageSize = this.PageSize,
                PageIndex = this.PageIndex,
                TotalCount = totalCount,
                Data = plateViews
            };
        }

        /// <summary>
        /// 根据一级分类检索视频信息
        /// </summary>
        /// <param name="categoryId">0</param>
        /// <param name="condtions"></param>
        /// <param name="orderCondtions"></param>
        /// <param name="totalCount"></param>
        /// <param name="totalIndex"></param>
        /// <returns></returns>
        public IList<VideoView> GetVideoByCategoryLists(int categoryId, IList<Condtion> condtions, IList<OrderCondtion> orderCondtions, out int totalCount, out int totalIndex)
        {
            var video = (from p in GetVideoList()
                         join c in this._categoryRepository.GetEntityList() on p.CategoryId equals c.Id
                         select new HKSJ.WBVV.Entity.ViewModel.Client.VideoView()
                         {
                             Id = p.Id,
                             Title = string.IsNullOrEmpty(p.Title) ? "" : p.Title,
                             CategoryId = p.CategoryId,
                             SmallPicturePath = p.SmallPicturePath,
                             BigPicturePath = p.BigPicturePath,
                             VideoState = p.VideoState
                         }).AsQueryable();
            if (condtions != null && condtions.Count > 0)//查询条件
            {
                video = video.Query(condtions);
            }
            if (orderCondtions != null && orderCondtions.Count > 0)//排序条件
            {
                video = video.OrderBy(orderCondtions);
            }
            bool isExists = video.Any();
            totalCount = isExists ? video.Count() : 0;
            if (this.PageSize <= 0)
            {
                totalIndex = 0;
                var queryable = isExists
                    ? video.ToList()
                    : new List<Entity.ViewModel.Client.VideoView>();
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
                    ? video.Skip((this.PageIndex - 1) * this.PageSize).Take(this.PageSize).ToList()
                    : new List<Entity.ViewModel.Client.VideoView>();

                return queryable;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="condtions"></param>
        /// <param name="orderCondtions"></param>
        /// <param name="totalCount"></param>
        /// <param name="totalIndex"></param>
        /// <returns></returns>
        public IList<VideoView> GetVideoByCategoryList(int categoryId, IList<Condtion> condtions, IList<OrderCondtion> orderCondtions, out int totalCount, out int totalIndex)
        {
            var category = new Category();
            if (categoryId > 0)
            {
                category = this._categoryRepository.GetEntity(ConditionEqualId(categoryId));
                if (category == null)
                {
                    totalCount = 0;
                    totalIndex = 0;
                    return new List<VideoView>();
                }
            }
            var video = (from p in GetVideoList()
                         join c in this._categoryRepository.GetEntityList() on p.CategoryId equals c.Id
                         where c.LocationPath.StartsWith(category.LocationPath)
                         select new HKSJ.WBVV.Entity.ViewModel.Client.VideoView()
                         {
                             Id = p.Id,
                             Title = p.Title,
                             CategoryId = p.CategoryId,
                             SmallPicturePath = p.SmallPicturePath,
                             BigPicturePath = p.BigPicturePath,
                             VideoState = p.VideoState
                         }).AsQueryable();
            if (condtions != null && condtions.Count > 0)//查询条件
            {
                video = video.Query(condtions);
            }
            if (orderCondtions != null && orderCondtions.Count > 0)//排序条件
            {
                video = video.OrderBy(orderCondtions);
            }
            bool isExists = video.Any();
            totalCount = isExists ? video.Count() : 0;
            if (this.PageSize <= 0)
            {
                totalIndex = 0;
                var queryable = isExists
                    ? video.ToList()
                    : new List<Entity.ViewModel.Client.VideoView>();
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
                    ? video.Skip((this.PageIndex - 1) * this.PageSize).Take(this.PageSize).ToList()
                    : new List<Entity.ViewModel.Client.VideoView>();

                return queryable;
            }
        }



        private IQueryable<Video> GetVideoList()
        {
            return this._videoRepository.GetEntityList(CondtionEqualState());
        }

        /// <summary>
        /// 获取视频集合
        /// </summary>
        /// <param name="condtions">查询条件</param>
        /// <param name="orderCondtions">排序条件</param>
        /// <param name="totalCount">返回总的行数</param>
        /// <param name="totalIndex">返回总的页数</param>
        /// <returns></returns>
        public IList<VideoView> GetVideoList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions, out int totalCount, out int totalIndex)
        {
            var plate = (from p in GetVideoList()
                         join c in this._categoryRepository.GetEntityList() on p.CategoryId equals c.Id
                         into cjoin
                         from cate in cjoin.DefaultIfEmpty()
                         select new HKSJ.WBVV.Entity.ViewModel.Client.VideoView()
                         {
                             Id = p.Id,
                             Title = p.Title,
                             CategoryId = p.CategoryId,
                             SmallPicturePath = p.SmallPicturePath,
                             BigPicturePath = p.BigPicturePath
                         }).AsQueryable();
            if (condtions != null && condtions.Count > 0)//查询条件
            {
                plate = plate.Query(condtions);
            }
            if (orderCondtions != null && orderCondtions.Count > 0)//排序条件
            {
                plate = plate.OrderBy(orderCondtions);
            }
            bool isExists = plate.Any();
            totalCount = isExists ? plate.Count() : 0;
            if (this.PageSize <= 0)
            {
                totalIndex = 0;
                var queryable = isExists
                    ? plate.ToList()
                    : new List<Entity.ViewModel.Client.VideoView>();
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
                    ? plate.Skip((this.PageIndex - 1) * this.PageSize).Take(this.PageSize).ToList()
                    : new List<Entity.ViewModel.Client.VideoView>();

                return queryable;
            }
        }
        #endregion


        /// <summary>
        /// 获取视频数据下载地址
        /// </summary>GetVideoViewListByPlateId
        /// <param name="id">视频Id</param>
        /// <returns></returns>
        public Entity.ViewModel.Client.VideoView GetVideoById(long id)
        {
            var condtion = new Condtion()
            {
                FiledName = "Id",
                FiledValue = id,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            Video info=_videoRepository.GetEntity(condtion);
            if (info == null) return new Entity.ViewModel.Client.VideoView(); ;
            Entity.ViewModel.Client.VideoView model = new Entity.ViewModel.Client.VideoView()
            {
                 Id=info.Id,
                 About=info.About,
                 BigPicturePath = info.BigPicturePath,
                 CategoryId = info.CategoryId,
                 Tags = info.Tags,
                 Title = info.Title,
                 SmallPicturePath = info.SmallPicturePath,
                 VideoState = info.VideoState,
                 IsPublic = info.IsPublic,
                 Copyright = info.Copyright
            };
            return model;
        }


        /// <summary>
        /// 根据源视频文件获取视频
        /// </summary>
        /// <param name="key">源视频文件件（VideoRosella 字段）</param>
        /// <returns></returns>
        private Video GetVideoByKey(string key)
        {
            var condtion = new Condtion()
            {
                FiledName = "VideoRosella",
                FiledValue = key,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return _videoRepository.GetEntity(condtion);
        }

    }
}