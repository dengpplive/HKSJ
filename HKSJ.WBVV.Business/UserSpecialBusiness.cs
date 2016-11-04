
using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using Autofac;
using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.Repository;
using HKSJ.WBVV.Repository.Interface;
using HKSJ.Utilities;
using HKSJ.WBVV.Common.Logger;
using HKSJ.WBVV.Common.Resource;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Business
{
    /// <summary>
    /// 用户专辑
    /// </summary>
    public class UserSpecialBusiness : BaseBusiness, IUserSpecialBusiness
    {
        private readonly IUserSpecialRepository _userSpecialRepository;
        private readonly IUserSpecialSonRepository _userSpecialSonRepository;
        private readonly IVideoRepository _videoRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserCollectRepository _userCollectRepository;
        private readonly IUserRoomChooseRepository _userRoomChooseRepository;
        private readonly IVideoPlayRecordRepository _videoPlayRecordRepository;
        private ITagsBusiness _tagsBusiness;
        public UserSpecialBusiness(
            IUserSpecialRepository userSpecialRepository,
            IUserSpecialSonRepository userSpecialSonRepository,
            IVideoRepository videoRepository,
            IUserRepository userRepository,
            IUserCollectRepository userCollectRepository,
            IUserRoomChooseRepository userRoomChooseRepository,
            IVideoPlayRecordRepository videoPlayRecordRepository
            )
        {
            this._userSpecialRepository = userSpecialRepository;
            this._userSpecialSonRepository = userSpecialSonRepository;
            this._videoRepository = videoRepository;
            this._userRepository = userRepository;
            this._userCollectRepository = userCollectRepository;
            this._userRoomChooseRepository = userRoomChooseRepository;
            this._videoPlayRecordRepository = videoPlayRecordRepository;
        }

        #region client

        #region 个人专辑管理
        #region 获取用户创建的专辑

        /// <summary>
        /// 获取用户创建的专辑
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="escapeStatestate"></param>
        /// <returns></returns>
        public SpecialView GetUserAlbumsViews()
        {
            var sv = new SpecialView() { SpecialVideoList = new List<SpecialDetailView>(), SpecialCount = 0 };

            var userSpecial = from dt in
                                  (from urc in _userSpecialRepository.GetEntityList(CondtionEqualCreateUserId(UserId)).OrderByDescending(o=>o.CreateTime)
                                   join uss in this._userSpecialSonRepository.GetEntityList() on urc.Id equals uss.MySpecialId into ussList
                                   from usst1 in ussList.DefaultIfEmpty()
                                   join video in _videoRepository.GetEntityList().Where(o => o.VideoState == 3).OrderByDescending(o => o.CreateTime) on
                                       usst1 == null ? 0 : usst1.VideoId equals video.Id into videoList
                                   from videot in videoList.DefaultIfEmpty()
                                   select new
                                   {
                                       Id = urc.Id,
                                       Title = urc.Title,
                                       Image = urc.Image,
                                       CreateTime = urc.CreateTime,
                                       VideoId = videot == null ? 0 : videot.Id,
                                       PlayCount = videot == null ? 0 : videot.PlayCount,
                                       CommentCount = videot == null ? 0 : videot.CommentCount,
                                       SmallPicture = videot == null ? "" : videot.SmallPicturePath
                                   })
                              group dt by dt.Id into list
                              select new SpecialDetailView
                              {
                                  Id = list.Key,
                                  Title = list.First().Title,
                                  CreateTime = list.First().CreateTime.ToString("yyyy-MM-dd HH:mm"),
                                  Thumbnail = !string.IsNullOrEmpty(list.First().Image) ? list.First().Image : list.First().SmallPicture,
                                  PlayCount = list.Sum(o => o.PlayCount),
                                  CommentCount = list.Sum(o => o.CommentCount),
                                  VideoCount = list.Count(o => o.VideoId != 0)
                              };


            sv.SpecialCount = userSpecial.Count();
            sv.PageCount = GetPageCountToDataCount(sv.SpecialCount);
            sv.SpecialVideoList = userSpecial.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();

            return sv;
        }

        public SpecialView GetUserAlbumsViews(int vid)
        {

            var result = new SpecialView() { SpecialVideoList = new List<SpecialDetailView>(), SpecialCount = 0 };
            var list = from special in
                           (from urc in _userSpecialRepository.GetEntityList(CondtionEqualCreateUserId(UserId))
                            join uss in _userSpecialSonRepository.GetEntityList() on urc.Id equals uss.MySpecialId into usslist
                            from tt in usslist.DefaultIfEmpty()
                            orderby urc.CreateTime descending
                            select new { Id = urc.Id, CreateUserId = urc.CreateUserId, Title = urc.Title, VideoId = tt == null ? 0 : tt.VideoId })
                       group special by special.Id into specialList
                       where specialList.FirstOrDefault(e => e.VideoId == vid) == null
                       select new SpecialDetailView
                         {
                             Id = specialList.Key,
                             Title = specialList.First().Title,
                         };

            result.SpecialCount = list.Count();
            result.PageCount = GetPageCountToDataCount(result.SpecialCount);
            result.SpecialVideoList = list.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            return result;
        }


        /// <summary>
        /// 个人空间 带有条件和排序分页的查询  用户专辑
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="condtions"></param>
        /// <param name="orderCondtions"></param>
        /// <returns></returns>
        public PageResult GetUserAlbumsViewsByOrder(int userId, IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            CheckUserId(userId);
            User user;
            CheckUserId(userId, out user);

            var result = new SpecialView() { SpecialVideoList = new List<SpecialDetailView>(), SpecialCount = 0 };
            //用户专辑
            var userSpecial = (from urc in _userSpecialRepository.GetEntityList(CondtionEqualCreateUserId(userId)).Where(p => !p.State)
                               orderby urc.CreateTime descending
                               select urc
                               ).AsQueryable();
            //筛选条件
            if (condtions != null && condtions.Count > 0)
                userSpecial = userSpecial.Query(condtions);


            List<UserSpecial> list = userSpecial.ToList<UserSpecial>();
            List<SpecialDetailView> sdvList = new List<SpecialDetailView>();
            SpecialDetailView sdv = null;
            List<SpecialVideoView> svvList = null;
            SpecialVideoView svv = null;
            List<Video> videoList = null;
            string thumbnail = "";


            foreach (UserSpecial item in list)
            {
                sdv = new SpecialDetailView();
                sdv.Id = item.Id;
                sdv.Title = item.Title;
                sdv.CreateTime = item.CreateTime.ToString("yyyy-MM-dd");
                sdv.OrderCreateTime = item.UpdateTime ?? item.CreateTime;
                thumbnail = item.Image;

                //专辑下面的视频
                var vidoes = (from ussl in this._userSpecialSonRepository.GetEntityList(CondtionEqualSpecialId(item.Id))
                              join video in this._videoRepository.GetEntityList(CondtionEqualState()) on (long)ussl.VideoId equals video.Id
                              where video.VideoState == 3 //TODO 刘强CheckState=1
                              orderby ussl.CreateTime
                              select video
                           ).AsQueryable();
                videoList = vidoes.ToList<Video>();

                sdv.VideoCount = videoList.Count;//专辑下视频总数
                if (videoList != null && videoList.Count > 0 && string.IsNullOrEmpty(thumbnail))
                    thumbnail = videoList[videoList.Count - 1].SmallPicturePath;

                //如果专辑图片和视频都没有,则赋值默认图片
                //if (string.IsNullOrEmpty(thumbnail))
                //thumbnail = ServerHelper.RootPath + "/Content/images/per_acc_v02.png";
                sdv.Thumbnail = thumbnail;//赋值专辑缩略图


                svvList = new List<SpecialVideoView>();
                int playCount = 0;//获取专辑下视频播放总数
                foreach (Video v in videoList)
                {
                    svv = new SpecialVideoView();
                    svv.Id = v.Id;
                    svv.SmallPicturePath = v.SmallPicturePath;
                    svv.Title = v.Title;
                    svv.PlayCount = v.PlayCount;
                    svv.CommentCount = v.CommentCount;
                    svv.TimeLength = ConvertUtil.ToInt(v.TimeLength, 0);
                    svvList.Add(svv);
                    playCount += v.PlayCount;
                }

                sdv.PlayCount = playCount;//获取专辑下视频播放总数

                sdv.SpecialVideoList = svvList;//专辑下视频集合
                sdvList.Add(sdv);
            }


            result.PageCount = sdvList.Count() % this.PageSize == 0 ? (sdvList.Count() / this.PageSize) : ((sdvList.Count() / this.PageSize) + 1);
            result.SpecialCount = sdvList.Count();
            result.SpecialVideoList = sdvList;


            //排序
            if (orderCondtions != null && orderCondtions.Count > 0)
                result.SpecialVideoList = sdvList.AsQueryable().OrderBy(orderCondtions).ToList();

            if (this.PageIndex > result.PageCount)
                this.PageIndex = result.PageCount;
            if (this.PageIndex <= 0)
                this.PageIndex = 1;
            if (this.PageSize > 0)
                result.SpecialVideoList = result.SpecialVideoList.Skip<SpecialDetailView>((this.PageIndex - 1) * this.PageSize).Take<SpecialDetailView>(this.PageSize).ToList();
            return new PageResult()
             {
                 PageSize = this.PageSize,
                 PageIndex = this.PageIndex,
                 TotalCount = result.SpecialCount,
                 TotalIndex = result.PageCount,
                 Data = result
             };
        }

        /// <summary>
        /// 个人空间  用户专辑下面的所有视频
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userSpecialId"></param>
        /// <param name="condtions"></param>
        /// <param name="orderCondtions"></param>
        /// <returns></returns>
        public PageResult GetUserAlbumVideosById(int userId, int userSpecialId, IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            CheckUserId(userId);
            User user;
            CheckUserId(userId, out user);

            var list = new List<VideoView>();
            int pageCount = 1;
            int videoCount = 0;
            //用户专辑
            UserSpecial userSpecial;
            CheckAlbumId(userSpecialId, out userSpecial);
            //专辑下面的视频
            var vidoes = (from ussl in this._userSpecialSonRepository.GetEntityList(CondtionEqualSpecialId(userSpecial.Id))
                          join v in this._videoRepository.GetEntityList(CondtionEqualState()) on (long)ussl.VideoId equals v.Id
                          where v.VideoState == 3//TODO 刘强CheckState=1
                          orderby ussl.CreateTime
                          select new VideoView()
                          {
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
                              Filter = v.Filter,
                              TimeLength = v.TimeLength ?? 0,
                              SpecialTitle = userSpecial.Title
                          }
                       ).AsQueryable();
            //筛选条件
            if (condtions != null && condtions.Count > 0)
                vidoes = vidoes.Query(condtions);
            //排序
            if (orderCondtions != null && orderCondtions.Count > 0)
                vidoes = vidoes.OrderBy(orderCondtions);

            //专辑下视频总数
            videoCount = vidoes.Count();
            if (this.PageSize > 0)
            {
                //总页数
                pageCount = videoCount % this.PageSize == 0 ? (videoCount / this.PageSize) : ((videoCount / this.PageSize) + 1);

                if (this.PageIndex > pageCount)
                    this.PageIndex = pageCount;
                if (this.PageIndex <= 0)
                    this.PageIndex = 1;
                list = vidoes.Skip<VideoView>((this.PageIndex - 1) * this.PageSize).Take<VideoView>(this.PageSize).ToList();
            }
            else
            {
                list = vidoes.ToList();
            }
            return new PageResult()
            {
                PageSize = this.PageSize,
                PageIndex = this.PageIndex,
                TotalCount = videoCount,
                TotalIndex = pageCount,
                Data = new UserSonAlbumView()
                {
                    SpecialTitle = userSpecial.Title,
                    VideoViewList = list,
                    VideoCount = videoCount
                }
            };
        }
        #endregion

        #region 创建一个新专辑
        /// <summary>
        /// 创建新专辑
        /// </summary>
        /// <param name="title"></param>
        /// <param name="profile"></param>
        /// <param name="label"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public int AddUserAlbum(string title, string profile, string label, string image)
        {
            User user;
            CheckUserId(UserId, out user);
            CheckTitleNotNull(title);
            CheckLabelNotNull(label);
            CheckTitleRepeat(title, UserId);
            var userSpecial = new UserSpecial()
            {
                CreateUserId = UserId,
                CreateTime = DateTime.Now,
                UpdateUserId = UserId,
                UpdateTime = DateTime.Now,
                State = false,
                Title = title,
                Remark = profile,
                Tag = label,
                Image = image
            };
            this._userSpecialRepository.CreateEntity(userSpecial);

            //TODO insert 刘强添加标签
            try
            {
                this._tagsBusiness = ((Autofac.IContainer)HttpRuntime.Cache["containerKey"]).Resolve<ITagsBusiness>();
                //上传视频的人
                this._tagsBusiness.UserId = user.Id;
                this._tagsBusiness.AsyncCreateTags();
            }
            catch (Exception ex)
            {
#if !DEBUG
                      LogBuilder.Log4Net.Error("更新标签失败", ex.MostInnerException());
#else
                Console.WriteLine(LanguageUtil.Translate("api_Business_UserSpecial_AddUserAlbum_updateTagsFailed") + ex.MostInnerException().Message);
#endif

            }
            return userSpecial.Id;
        }
        #endregion

        #region 获取编辑界面专辑数据
        /// <summary>
        /// 获取编辑界面专辑数据
        /// </summary>
        /// <param name="albumsId"></param>
        /// <returns></returns>
        public UserSpecial GetEditUserAlbum(int albumsId)
        {
            UserSpecial us = null;
            User user;
            CheckUserId(UserId, out user);
            var userSpecial = (from usr in _userSpecialRepository.GetEntityList(CondtionEqualCreateUserId(UserId))
                               where usr.Id == albumsId
                               select usr
                               ).AsQueryable().FirstOrDefault();
            if (userSpecial != null)
                us = userSpecial;
            us.Image = UrlHelper.QiniuPublicCombine(us.Image);
            return us;
        }

        #endregion

        #region 编辑专辑
        /// <summary>
        /// 编辑专辑
        /// </summary>
        /// <param name="title"></param>
        /// <param name="profile"></param>
        /// <param name="label"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public bool EditUserAlbum(int albumId, string title, string remark, string tag)
        {

            User user;
            UserSpecial us;
            CheckAlbumId(albumId);
            CheckTitleNotNull(title);
            CheckLabelNotNull(tag);
            CheckUserId(UserId, out user);
            CheckAlbumId(albumId, out us);

            us.UpdateUserId = UserId;
            us.UpdateTime = DateTime.Now;
            us.Title = title;
            us.Remark = remark;
            us.Tag = tag;
            //us.CategoryId = categoryId;
            //us.Image = "";

            var success = this._userSpecialRepository.UpdateEntity(us);
            //TODO insert 刘强添加标签
            try
            {
                this._tagsBusiness = ((Autofac.IContainer)HttpRuntime.Cache["containerKey"]).Resolve<ITagsBusiness>();
                //上传视频的人
                this._tagsBusiness.UserId = user.Id;
                this._tagsBusiness.AsyncCreateTags();
            }
            catch (Exception ex)
            {
#if !DEBUG
                      LogBuilder.Log4Net.Error("更新标签失败", ex.MostInnerException());
#else
                Console.WriteLine(LanguageUtil.Translate("api_Business_UserSpecial_EditUserAlbum_updateTagsFailed") + ex.MostInnerException().Message);
#endif
            }
            return success;
        }
        #endregion

        #region 删除专辑
        /// <summary>
        /// 删除专辑
        /// </summary>
        /// <param name="title"></param>
        /// <param name="profile"></param>
        /// <param name="label"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public bool DeleteUserAlbum(int albumId)
        {
            User user;
            UserSpecial us;
            CheckAlbumId(albumId);
            CheckUserId(UserId, out user);
            CheckAlbumId(albumId, out us);

            //查询专辑下的视频，首先清除
            var specialVideo = (from uss in this._userSpecialSonRepository.GetEntityList(CondtionEqualSpecialId(albumId))
                                select uss).AsQueryable();
            if (specialVideo.Any())
                this._userSpecialSonRepository.DeleteEntitys(specialVideo.ToList<UserSpecialSon>());

            return this._userSpecialRepository.DeleteEntity(us);
        }
        #endregion

        #region 获取用户专辑视频视图

        /// <summary>
        /// 获取用户专辑视频视图
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="escapeStatestate"></param>
        /// <returns></returns>
        public SpecialDetailView GetUserAlbumVideoViews(int albumId)
        {
            //User user;
            //UserSpecial us;
            //CheckAlbumId(albumId);
            //CheckUserId(UserId, out user);
            //CheckAlbumId(albumId, out us);

            var sdv = new SpecialDetailView() { SpecialVideoList = new List<SpecialVideoView>(), Title = "", VideoCount = 0, PageCount = 0,Thumbnail = ""};
            var userSpecial = this._userSpecialRepository.GetEntityList(ConditionEqualId(albumId)).FirstOrDefault(o=>o.CreateUserId == UserId);
            if (userSpecial == null)
            {
                return sdv;
            }

            var vidoes =
                (from ussl in
                     this._userSpecialSonRepository.GetEntityList(CondtionEqualSpecialId(userSpecial.Id))
                         .OrderByDescending(o => o.CreateTime)
                 join video in
                     this._videoRepository.GetEntityList(CondtionEqualState()).Where(p => p.VideoState == 3) on
                     (long)ussl.VideoId equals video.Id
                 select new SpecialVideoView
                 {
                     Id = video.Id,
                     CommentCount = video.CommentCount,
                     SmallPicturePath = video.SmallPicturePath,
                     Title = video.Title,
                     PlayCount = video.PlayCount,
                     About = video.About,
                     TimeLength = video.TimeLength.GetValueOrDefault(0)
                     //CreateTime = video.CreateTime.ToString("yyyy-MM-dd" )
                 });

            sdv.Title = userSpecial.Title;
            sdv.Remark = userSpecial.Remark;
            sdv.CreateTime = userSpecial.CreateTime.ToString("yyyy-MM-dd HH:mm");
            sdv.Thumbnail = userSpecial.Image;

            if (!vidoes.Any()) return sdv;

            if (string.IsNullOrEmpty(sdv.Thumbnail)) sdv.Thumbnail = vidoes.First().SmallPicturePath;


            sdv.VideoCount = vidoes.Count();
            sdv.PageCount = GetPageCountToDataCount(sdv.VideoCount);
            sdv.CommentCount = vidoes.Sum(o => o.CommentCount);
            sdv.PlayCount = vidoes.Sum(o => o.PlayCount);

            sdv.SpecialVideoList = vidoes.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            
            return sdv;
        }
        #endregion

        #region 删除专辑下视频
        /// <summary>
        /// 删除专辑下视频,可批量
        /// </summary>
        /// <param name="albumId"></param>
        /// <param name="videoIds"></param>
        /// <returns></returns>
        public bool DeleteAlbumVideos(int albumId, string videoIds)
        {
            User user;
            UserSpecial us;
            //Video video;
            CheckAlbumId(albumId);
            CheckUserId(UserId, out user);
            CheckAlbumId(albumId, out us);
            //CheckVideoId(videoId, out video);

            try
            {
                int[] videoId = Array.ConvertAll<string, int>(videoIds.Split(','), delegate(string s) { return ConvertUtil.ToInt(s); });

                var specialVideo = (from uss in this._userSpecialSonRepository.GetEntityList(CondtionEqualSpecialId(albumId))
                                    where (videoId).Contains(uss.VideoId)
                                    select uss).AsQueryable();
                if (specialVideo.Any())
                    this._userSpecialSonRepository.DeleteEntitys(specialVideo.ToList<UserSpecialSon>());
                return true;
            }
            catch (Exception)
            {
                return false;
            }



        }
        #endregion

        #region 添加专辑下视频（可批量）
        /// <summary>
        /// 添加专辑下视频,可批量
        /// </summary>
        /// <param name="albumId"></param>
        /// <param name="videoIds"></param>
        /// <returns></returns>
        public bool AddAlbumVideos(int albumId, string videoIds)
        {
            User user;
            UserSpecial us;
            Video video;
            CheckAlbumId(albumId);
            CheckUserId(UserId, out user);
            CheckAlbumId(albumId, out us);

            int[] videoId = Array.ConvertAll<string, int>(videoIds.Split(','), ConvertUtil.ToInt);

            try
            {
                List<UserSpecialSon> list = new List<UserSpecialSon>();
                foreach (int item in videoId)
                {
                    CheckVideoId(item, out video);//检测视频是否存在

                    var userSpecialSon = new UserSpecialSon()
                            {
                                MySpecialId = albumId,
                                VideoId = item,
                                CreateTime = DateTime.Now,
                                SortNum = 0
                            };
                    list.Add(userSpecialSon);
                }
                this._userSpecialSonRepository.CreateEntitys(list);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
        #endregion

        /// <summary>
        /// 添加视频到专辑,可批量
        /// </summary>
        /// <param name="vid"></param>
        /// <param name="albumsId"></param>
        /// <returns></returns>
        public bool AddVideo2Albums(int vid, string albumsId)
        {
            int[] albumIds = Array.ConvertAll<string, int>(albumsId.Split(','), ConvertUtil.ToInt);
            UserSpecial special;

            try
            {
                List<UserSpecialSon> list = new List<UserSpecialSon>();
                foreach (int item in albumIds)
                {
                    CheckAlbumId(item, out special);

                    var userSpecialSon = new UserSpecialSon()
                    {
                        MySpecialId = item,
                        VideoId = vid,
                        CreateTime = DateTime.Now,
                        SortNum = 0
                    };
                    list.Add(userSpecialSon);
                }
                this._userSpecialSonRepository.CreateEntitys(list);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }


        #region 获取用户上传视频,过滤已添加到专辑视频

        /// <summary>
        /// 获取用户上传视频,过滤已添加到专辑视频
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="escapeStatestate"></param>
        /// <returns></returns>
        public MyVideoViewResult GetUserVideoViews(int albumId)
        {
            var result = new MyVideoViewResult() { MyVideoViews = new List<MyVideoView>(), TotalCount = 0 };
            var queryable = (from myvideo in _videoRepository.GetEntityList(CondtionEqualState()).Where(o => o.CreateManageId == UserId && o.VideoState == 3)
                             join uss in this._userSpecialSonRepository.GetEntityList() on myvideo.Id equals uss.VideoId into ussList
                             from ut in ussList.DefaultIfEmpty()
                             orderby myvideo.CreateTime
                             select new
                             {
                                 Id = myvideo.Id,
                                 Title = myvideo.Title,
                                 SmallPicturePath = myvideo.SmallPicturePath,
                                 SpecialId = ut == null ? 0 : ut.MySpecialId
                             });
            var list = queryable.GroupBy(o => o.Id).Where(o => o.FirstOrDefault(s => s.SpecialId == albumId) == null)
                .Select(o => new MyVideoView
                {
                    Id = o.Key,
                    Title = o.First().Title,
                    SmallPicturePath = o.First().SmallPicturePath
                });


            if (!list.Any()) return result;
            result.TotalCount = list.Count();
            result.PageCount = GetPageCountToDataCount(result.TotalCount);
            result.MyVideoViews = list.Skip(PageSize * (PageIndex - 1)).Take(PageSize).ToList();

            return result;
        }
        #endregion

        #region 获取用户收藏视频,过滤已添加到专辑视频

        /// <summary>
        /// 获取用户收藏视频,过滤已添加到专辑视频
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="escapeStatestate"></param>
        /// <returns></returns>
        public MyVideoViewResult GetUserCollectVideoViews(int albumId)
        {
            var result = new MyVideoViewResult() { MyVideoViews = new List<MyVideoView>(), TotalCount = 0 };
            IQueryable<MyVideoView> queryable;
            queryable = (from usercollect in this._userCollectRepository.GetEntityList(CondtionEqualUserId(UserId))
                         join video in _videoRepository.GetEntityList(CondtionEqualState()) on new { Id = (long)usercollect.VideoId } equals new { Id = video.Id }
                         where usercollect.State == false && video.VideoState == 3//TODO 刘强CheckState=1
                         && !(from uss in this._userSpecialSonRepository.GetEntityList(CondtionEqualSpecialId(albumId))
                              select new
                              {
                                  VideoId = (long)uss.VideoId
                              }).Contains(new { VideoId = video.Id })
                         select new MyVideoView
                         {
                             Id = video.Id,
                             Title = video.Title,
                             CommentCount = video.CommentCount,
                             PlayCount = video.PlayCount,
                             SmallPicturePath = video.SmallPicturePath,
                             VideoState = video.VideoState,
                             CreateTime = video.CreateTime
                         }).AsQueryable();


            if (!queryable.Any()) return result;
            result.TotalCount = queryable.Count();
            result.PageCount = GetPageCountToDataCount(result.TotalCount);
            result.MyVideoViews = queryable.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            return result;
        }
        #endregion

        #region 设置封面
        /// <summary>
        /// 设置封面
        /// </summary>
        /// <param name="albumId"></param>
        /// <param name="videoId"></param>
        /// <returns></returns>
        public bool SetCover(int albumId, int videoId)
        {
            User user;
            UserSpecial us;
            Video video;
            CheckAlbumId(albumId);
            CheckUserId(UserId, out user);
            CheckAlbumId(albumId, out us);
            CheckVideoId(videoId, out video);
            CheckUserEqual(us.CreateUserId, UserId);

            us.Image = video.SmallPicturePath;
            return this._userSpecialRepository.UpdateEntity(us);
        }
        #endregion

        #region 更新专辑封面
        /// <summary>
        /// 更新专辑封面
        /// </summary>
        /// <param name="albumId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool UpdateAlbumPic(int albumId, string key)
        {
            User user;
            UserSpecial us;
            CheckAlbumId(albumId);
            CheckUserId(UserId, out user);
            CheckAlbumId(albumId, out us);
            CheckUserEqual(us.CreateUserId, UserId);

            us.Image = key;
            return this._userSpecialRepository.UpdateEntity(us);
        }
        #endregion

        #endregion


        #region  首页-专辑页面

        #region 获取(今日)热门专辑（所有专辑），根据分页返回
        /// <summary>
        /// 获取(今日)热门专辑（所有专辑），根据分页返回
        /// </summary>
        /// <returns></returns>
        public SpecialView GetAllAlbumsViews()
        {
            var sv = new SpecialView() { SpecialVideoList = new List<SpecialDetailView>(), SpecialCount = 0, PageCount = 0 };

            var userSpecial = (from urc in _userSpecialRepository.GetEntityList()
                               select urc
                               ).AsQueryable();
            List<UserSpecial> list = userSpecial.ToList<UserSpecial>();

            List<SpecialDetailView> sdvList = new List<SpecialDetailView>();
            SpecialDetailView sdv = null;

            List<SpecialVideoView> svvList = null;
            SpecialVideoView svv = null;
            List<Video> videoList = null;
            string thumbnail = "";


            foreach (UserSpecial item in list)
            {
                sdv = new SpecialDetailView();
                sdv.Id = item.Id;
                sdv.Title = item.Title;
                sdv.CreateTime = item.CreateTime.ToString("yyyy-MM-dd");
                sdv.Remark = item.Remark.Length > 18 ? item.Remark.Substring(0, 18) + "..." : item.Remark;
                thumbnail = item.Image;

                var videos = (from ussl in this._userSpecialSonRepository.GetEntityList(CondtionEqualSpecialId(item.Id))
                              join video in this._videoRepository.GetEntityList(CondtionEqualState()) on (long)ussl.VideoId equals video.Id
                              where video.VideoState == 3//TODO 刘强CheckState=1
                              orderby ussl.CreateTime descending
                              select video
                           ).AsQueryable();

                videoList = videos.ToList<Video>();

                sdv.VideoCount = videoList.Count;//专辑下视频总数
                if (videoList != null && videoList.Count > 0 && string.IsNullOrEmpty(thumbnail))
                    thumbnail = videoList[videoList.Count - 1].SmallPicturePath;

                //如果专辑图片和视频都没有,则赋值默认图片
                //if (string.IsNullOrEmpty(thumbnail))
                //thumbnail = ServerHelper.RootPath + "/Content/images/per_acc_v02.png";
                sdv.Thumbnail = thumbnail;//赋值专辑缩略图


                svvList = new List<SpecialVideoView>();
                int playCount = 0;//获取专辑下视频播放总数
                int dayPlayCount = 0;//获取专辑下视频当日播放总数

                foreach (Video v in videoList)
                {
                    svv = new SpecialVideoView();
                    svv.Id = v.Id;
                    svv.SmallPicturePath = v.SmallPicturePath;
                    svv.Title = v.Title;
                    svv.PlayCount = v.PlayCount;
                    svv.CommentCount = v.CommentCount;
                    svv.TimeLength = ConvertUtil.ToInt(v.TimeLength, 0);
                    svvList.Add(svv);
                    playCount += v.PlayCount;

                    var videoplayrecord = (from vpc in
                                               (from vpr in this._videoPlayRecordRepository.GetEntityList(CondtionEqualVideoId(v.Id))
                                                where vpr.CreateTime >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00.000") && vpr.CreateTime <= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59.999")
                                                select new
                                                {
                                                    Dummy = "x"
                                                })
                                           group vpc by new { vpc.Dummy } into g
                                           select new
                                           {
                                               TheDayPlayCount = g.Count()
                                           }).AsQueryable().FirstOrDefault();



                    dayPlayCount += videoplayrecord != null ? videoplayrecord.TheDayPlayCount : 0;
                }

                sdv.PlayCount = playCount;//获取专辑下视频播放总数
                sdv.TheDayPlayCount = dayPlayCount;//获取专辑下视频当日播放总数

                sdv.SpecialVideoList = svvList;//专辑下视频集合
                sdvList.Add(sdv);
            }
            if (sdvList.Count > 0)
                sdvList = sdvList.OrderByDescending(i => i.TheDayPlayCount).ToList<SpecialDetailView>();
            sv.SpecialVideoList = sdvList;

            if (list.Count <= 0) return sv;
            sv.SpecialCount = list.Count;

            int oldPageIndex = PageIndex;
            sv.PageCount = GetPageCountToDataCount(sv.SpecialCount);

            if (sv.PageCount < oldPageIndex)
                return new SpecialView() { SpecialVideoList = new List<SpecialDetailView>(), SpecialCount = 0, PageCount = 0 };
            else
                sv.SpecialVideoList = sv.SpecialVideoList.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            return sv;
        }

        #endregion

        #region 获取推荐专辑列表
        /// <summary>
        /// 获取推荐专辑列表
        /// </summary>
        /// <returns></returns>
        public SpecialView GetRecommendAlbumsViews()
        {
            var sv = new SpecialView() { SpecialVideoList = new List<SpecialDetailView>(), SpecialCount = 0 };

            var userSpecial = (from urc in _userSpecialRepository.GetEntityList(CondtionEqualIsRecommend())
                               orderby urc.SortNum
                               select urc
                               ).Take(PageSize).AsQueryable();
            List<UserSpecial> list = userSpecial.ToList<UserSpecial>();

            List<SpecialDetailView> sdvList = new List<SpecialDetailView>();
            SpecialDetailView sdv = null;

            List<SpecialVideoView> svvList = null;
            SpecialVideoView svv = null;
            List<Video> videoList = null;
            string thumbnail = "";


            foreach (UserSpecial item in list)
            {
                sdv = new SpecialDetailView();
                sdv.Id = item.Id;
                sdv.Title = item.Title;
                sdv.CreateUserNick = this._userRepository.GetEntity(ConditionEqualId(item.CreateUserId)).NickName;
                sdv.CreateTime = item.CreateTime.ToString("yyyy-MM-dd hh:mm:ss");
                sdv.UpdateUserNick = this._userRepository.GetEntity(ConditionEqualId(ConvertUtil.ToInt(item.UpdateUserId))).NickName;
                sdv.UpdateTime = Convert.ToDateTime(item.UpdateTime).ToString("yyyy-MM-dd hh:mm:ss");
                sdv.SortNum = item.SortNum;
                sdv.Remark = item.Remark;
                thumbnail = item.Image;

                var videos = (from ussl in this._userSpecialSonRepository.GetEntityList(CondtionEqualSpecialId(item.Id))
                              join video in this._videoRepository.GetEntityList(CondtionEqualState()) on (long)ussl.VideoId equals video.Id
                              where video.VideoState == 3//TODO 刘强CheckState=1
                              orderby ussl.CreateTime
                              select video
                           ).Take(5).AsQueryable();

                videoList = videos.ToList<Video>();

                sdv.VideoCount = videoList.Count;//专辑下视频总数
                if (videoList != null && videoList.Count > 0 && string.IsNullOrEmpty(thumbnail))
                    thumbnail = videoList[videoList.Count - 1].SmallPicturePath;

                //如果专辑图片和视频都没有,则赋值默认图片
                //if (string.IsNullOrEmpty(thumbnail))
                //thumbnail = ServerHelper.RootPath + "/Content/images/per_acc_v02.png";
                sdv.Thumbnail = thumbnail;//赋值专辑缩略图


                svvList = new List<SpecialVideoView>();
                int playCount = 0;//获取专辑下视频播放总数

                foreach (Video v in videoList)
                {
                    svv = new SpecialVideoView();
                    svv.Id = v.Id;
                    svv.SmallPicturePath = v.SmallPicturePath;
                    svv.Title = v.Title;
                    svv.PlayCount = v.PlayCount;
                    svv.CommentCount = v.CommentCount;
                    svv.TimeLength = ConvertUtil.ToInt(v.TimeLength, 0);
                    svvList.Add(svv);
                    playCount += v.PlayCount;
                }

                sdv.PlayCount = playCount;//获取专辑下视频播放总数

                sdv.SpecialVideoList = svvList;//专辑下视频集合
                sdvList.Add(sdv);
            }

            sv.SpecialVideoList = sdvList;

            if (list.Count <= 0) return sv;
            sv.SpecialCount = list.Count;

            sv.PageCount = GetPageCountToDataCount(sv.SpecialCount);

            sv.SpecialVideoList = sv.SpecialVideoList;//.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            return sv;
        }

        #endregion

        #region 获取专辑视频视图
        /// <summary>
        /// 获取专辑视频视图
        /// </summary>
        /// <param name="albumId"></param>
        /// <returns></returns>
        public SpecialDetailView GetAlbumVideoViews(int albumId, string isHot)
        {
            UserSpecial us;
            CheckAlbumId(albumId);
            CheckAlbumId(albumId, out us);


            var sdv = new SpecialDetailView() { SpecialVideoList = new List<SpecialVideoView>(), Title = "", VideoCount = 0, PageCount = 0 };


            var userSpecial = (from usr in this._userSpecialRepository.GetEntityList(ConditionEqualId(us.Id))
                               select usr
                               ).AsQueryable().FirstOrDefault();

            string thumbnail = "";
            if (userSpecial != null)
            {
                sdv.Id = userSpecial.Id;
                sdv.Title = userSpecial.Title;
                thumbnail = userSpecial.Image;
                sdv.CreateTime = userSpecial.CreateTime.ToString("yyyy-MM-dd");
                sdv.Remark = userSpecial.Remark;
            }

            List<SpecialVideoView> svvList = null;
            SpecialVideoView svv = null;
            List<Video> videoList = null;


            if (isHot == "Y")
            {
                var vidoes = (from ussl in this._userSpecialSonRepository.GetEntityList(CondtionEqualSpecialId(sdv.Id))
                              join video in this._videoRepository.GetEntityList(CondtionEqualState()) on (long)ussl.VideoId equals video.Id
                              where video.VideoState == 3//TODO 刘强CheckState=1
                              orderby video.PlayCount descending  //UpdateTime
                              select video
                             ).AsQueryable();
                videoList = vidoes.ToList<Video>();
            }
            else
            {
                var vidoes = (from ussl in this._userSpecialSonRepository.GetEntityList(CondtionEqualSpecialId(sdv.Id))
                              join video in this._videoRepository.GetEntityList(CondtionEqualState()) on (long)ussl.VideoId equals video.Id
                              where video.VideoState == 3//TODO 刘强CheckState=1
                              orderby video.UpdateTime descending
                              select video
                             ).AsQueryable();
                videoList = vidoes.ToList<Video>();
            }

            if (videoList.Count <= 0) return sdv;

            sdv.VideoCount = videoList.Count;//专辑下视频总数
            if (videoList != null && videoList.Count > 0 && string.IsNullOrEmpty(thumbnail))
                thumbnail = videoList[videoList.Count - 1].SmallPicturePath;

            //如果专辑图片和视频都没有,则赋值默认图片
            //if (string.IsNullOrEmpty(thumbnail))
            //thumbnail = ServerHelper.RootPath + "/Content/images/per_acc_v02.png";
            sdv.Thumbnail = thumbnail;//赋值专辑缩略图


            svvList = new List<SpecialVideoView>();
            int playCount = 0;//获取专辑下视频播放总数
            int order = 1;
            foreach (Video v in videoList)
            {
                svv = new SpecialVideoView();
                svv.orderId = order;
                svv.Id = v.Id;
                svv.SmallPicturePath = v.SmallPicturePath;
                svv.Title = v.Title;
                svv.PlayCount = v.PlayCount;
                svv.CommentCount = v.CommentCount;
                svv.TimeLength = ConvertUtil.ToInt(v.TimeLength, 0);
                svv.CreateTime = v.CreateTime.ToString("yyyy-MM-dd");
                svv.UpdateTime = ((DateTime)v.UpdateTime).ToString("yyyy-MM-dd");
                svvList.Add(svv);
                playCount += v.PlayCount;
                order++;
            }

            sdv.PlayCount = playCount;//获取专辑下视频播放总数

            sdv.SpecialVideoList = svvList;//专辑下视频集合

            sdv.VideoCount = videoList.Count;

            sdv.PageCount = GetPageCountToDataCount(sdv.VideoCount);

            sdv.SpecialVideoList = sdv.SpecialVideoList.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            return sdv;
        }
        #endregion


        #endregion

        #endregion


        #region manage

        #region 添加推荐专辑
        /// <summary>
        /// 所有专辑,过滤已添加推荐
        /// </summary>
        /// <returns></returns>
        public SpecialView GetAllAlbumsViews(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            var sv = new SpecialView() { SpecialVideoList = new List<SpecialDetailView>(), SpecialCount = 0, PageCount = 0 };

            if (condtions == null)
                condtions = new List<Condtion>();
            condtions.Add(CondtionEqualIsRecommend(false));

            var userSpecial = (from urc in _userSpecialRepository.GetEntityList(condtions)
                               select urc
                               ).AsQueryable();
            List<UserSpecial> list = userSpecial.ToList<UserSpecial>();

            List<SpecialDetailView> sdvList = new List<SpecialDetailView>();
            SpecialDetailView sdv = null;

            List<SpecialVideoView> svvList = null;
            SpecialVideoView svv = null;
            List<Video> videoList = null;
            string thumbnail = "";


            foreach (UserSpecial item in list)
            {
                sdv = new SpecialDetailView();
                sdv.Id = item.Id;
                sdv.Title = item.Title;
                sdv.CreateTime = item.CreateTime.ToString("yyyy-MM-dd hh:mm:ss");
                sdv.CreateUserNick = this._userRepository.GetEntity(ConditionEqualId(item.CreateUserId)).NickName;
                sdv.UpdateTime = Convert.ToDateTime(item.UpdateTime).ToString("yyyy-MM-dd hh:mm:ss");
                sdv.UpdateUserNick = this._userRepository.GetEntity(ConditionEqualId(ConvertUtil.ToInt(item.UpdateUserId))).NickName;
                sdv.Remark = item.Remark;
                thumbnail = item.Image;

                var videos = (from ussl in this._userSpecialSonRepository.GetEntityList(CondtionEqualSpecialId(item.Id))
                              join video in this._videoRepository.GetEntityList(CondtionEqualState()) on (long)ussl.VideoId equals video.Id
                              where video.VideoState == 3//TODO 刘强CheckState=1
                              orderby ussl.CreateTime descending
                              select video
                           ).AsQueryable();

                videoList = videos.ToList<Video>();

                sdv.VideoCount = videoList.Count;//专辑下视频总数
                if (videoList != null && videoList.Count > 0 && string.IsNullOrEmpty(thumbnail))
                    thumbnail = videoList[videoList.Count - 1].SmallPicturePath;

                //如果专辑图片和视频都没有,则赋值默认图片
                //if (string.IsNullOrEmpty(thumbnail))
                //thumbnail = ServerHelper.RootPath + "/Content/images/per_acc_v02.png";
                sdv.Thumbnail = thumbnail;//赋值专辑缩略图


                svvList = new List<SpecialVideoView>();
                int playCount = 0;//获取专辑下视频播放总数
                int dayPlayCount = 0;//获取专辑下视频当日播放总数

                foreach (Video v in videoList)
                {
                    svv = new SpecialVideoView();
                    svv.Id = v.Id;
                    svv.SmallPicturePath = v.SmallPicturePath;
                    svv.Title = v.Title;
                    svv.PlayCount = v.PlayCount;
                    svv.CommentCount = v.CommentCount;
                    svv.TimeLength = ConvertUtil.ToInt(v.TimeLength, 0);
                    svvList.Add(svv);
                    playCount += v.PlayCount;

                    var videoplayrecord = (from vpc in
                                               (from vpr in this._videoPlayRecordRepository.GetEntityList(CondtionEqualVideoId(v.Id))
                                                where vpr.CreateTime >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00.000") && vpr.CreateTime <= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59.999")
                                                select new
                                                {
                                                    Dummy = "x"
                                                })
                                           group vpc by new { vpc.Dummy } into g
                                           select new
                                           {
                                               TheDayPlayCount = g.Count()
                                           }).AsQueryable().FirstOrDefault();



                    dayPlayCount += videoplayrecord != null ? videoplayrecord.TheDayPlayCount : 0;
                }

                sdv.PlayCount = playCount;//获取专辑下视频播放总数
                sdv.TheDayPlayCount = dayPlayCount;//获取专辑下视频当日播放总数

                sdv.SpecialVideoList = svvList;//专辑下视频集合
                sdvList.Add(sdv);
            }
            if (sdvList.Count > 0)
                sdvList = sdvList.OrderByDescending(i => i.TheDayPlayCount).ToList<SpecialDetailView>();
            sv.SpecialVideoList = sdvList;

            if (list.Count <= 0) return sv;
            sv.SpecialCount = list.Count;

            int oldPageIndex = PageIndex;
            sv.PageCount = GetPageCountToDataCount(sv.SpecialCount);
            if (sv.PageCount < oldPageIndex)
                return new SpecialView() { SpecialVideoList = new List<SpecialDetailView>(), SpecialCount = 0, PageCount = 0 };
            else
                sv.SpecialVideoList = sv.SpecialVideoList.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();

            return sv;
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="condtions"></param>
        /// <param name="orderCondtions"></param>
        /// <returns></returns>
        public PageResult GetRecommendAlbumsPageResult(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {

            SpecialView sv = GetAllAlbumsViews(condtions, orderCondtions);
            return new PageResult()
            {
                PageSize = this.PageSize,
                PageIndex = this.PageIndex,
                TotalCount = sv.SpecialCount,
                TotalIndex = sv.PageCount,
                Data = sv.SpecialVideoList
            };
        }

        /// <summary>
        /// 添加推荐专辑
        /// </summary>
        /// <returns></returns>
        public bool AddRecommendAlbums(string albumIds, int limitCount)
        {
            if (string.IsNullOrEmpty(albumIds))
                return false;

            string[] albumIdArray = albumIds.Split(',');

            CheckRecommendCount(limitCount, albumIdArray.Length);

            UserSpecial us = null;
            IList<UserSpecial> list = new List<UserSpecial>();
            foreach (string albumId in albumIdArray)
            {
                us = this._userSpecialRepository.GetEntity(this.ConditionEqualId(ConvertUtil.ToInt(albumId)));
                us.IsRecommend = true;
                us.SortNum = 0;
                list.Add(us);
            }
            this._userSpecialRepository.UpdateEntitys(list);
            return true;
        }
        #endregion

        #region 删除推荐专辑
        /// <summary>
        /// 删除推荐专辑
        /// </summary>
        /// <returns></returns>
        public bool RemoveRecommendAlbums(string albumIds)
        {
            if (string.IsNullOrEmpty(albumIds))
                return false;

            string[] albumIdArray = albumIds.Split(',');

            UserSpecial us = null;
            IList<UserSpecial> list = new List<UserSpecial>();
            foreach (string albumId in albumIdArray)
            {
                us = this._userSpecialRepository.GetEntity(this.ConditionEqualId(ConvertUtil.ToInt(albumId)));
                us.IsRecommend = false;
                list.Add(us);
            }
            this._userSpecialRepository.UpdateEntitys(list);
            return true;
        }


        #endregion

        #region 保存推荐专辑排序
        /// <summary>
        /// 保存推荐专辑排序
        /// </summary>
        /// <returns></returns>
        public bool SavaRecommendAlbumsSort(string albumIds, string sortNums)
        {
            if (string.IsNullOrEmpty(albumIds) || string.IsNullOrEmpty(sortNums))
                return false;

            string[] albumIdArray = albumIds.Split(',');
            string[] sortNumArray = sortNums.Split(',');

            CheckAlbumsAndSortNumCount(albumIdArray.Length, sortNumArray.Length);

            UserSpecial us = null;
            IList<UserSpecial> list = new List<UserSpecial>();
            for (int i = 0; i < albumIdArray.Length; i++)
            {
                us = this._userSpecialRepository.GetEntity(this.ConditionEqualId(ConvertUtil.ToInt(albumIdArray[i])));
                us.SortNum = ConvertUtil.ToInt(sortNumArray[i]);
                list.Add(us);
            }
            this._userSpecialRepository.UpdateEntitys(list);
            return true;
        }


        #endregion

        #endregion




        #region 传入参数
        /// <summary>
        /// 比较上传用户相等
        /// </summary>
        /// <param name="createManageId"></param>
        /// <returns></returns>
        private Condtion CondtionEqualCreateUserId(int createUserId)
        {
            var condtion = new Condtion()
            {
                FiledName = "CreateUserId",
                FiledValue = createUserId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }

        /// <summary>
        /// 比较上传用户相等
        /// </summary>
        /// <param name="createManageId"></param>
        /// <returns></returns>
        private Condtion CondtionEqualUserId(int userId)
        {
            var condtion = new Condtion()
            {
                FiledName = "UserId",
                FiledValue = userId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }

        /// <summary>
        /// 判断用户专辑子表下专辑编号相等
        /// </summary>
        /// <param name="specialId"></param>
        /// <returns></returns>
        private Condtion CondtionEqualSpecialId(int specialId)
        {
            var condtion = new Condtion()
            {
                FiledName = "MySpecialId",
                FiledValue = specialId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }

        /// <summary>
        /// 比较上传用户相等
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

        /// <summary>
        ///比较视频Id相等
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        private Condtion CondtionEqualVideoId(long videoId)
        {
            var condtion = new Condtion()
            {
                FiledName = "VideoId",
                FiledValue = videoId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }
        /// <summary>
        /// 比较是否推荐
        /// </summary>
        /// <param name="isRecommend"></param>
        /// <returns></returns>
        private Condtion CondtionEqualIsRecommend(bool IsRecommend = true)
        {
            var condtion = new Condtion()
            {
                FiledName = "IsRecommend",
                FiledValue = IsRecommend,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }


        /// <summary>
        /// 比较标题相等
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        private Condtion CondtionEqualTitle(string title)
        {
            var condtion = new Condtion()
            {
                FiledName = "Title",
                FiledValue = title,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }
        /// <summary>
        /// 检测用户编号不能小于0
        /// </summary>
        /// <param name="userId"></param>
        private void CheckUserId(int userId)
        {
            AssertUtil.AreBigger(userId, 0, LanguageUtil.Translate("api_Business_UserSpecial_CheckUserId_userIdCannotbeLessThanzero"));
        }
        #endregion

        #region 传入参数检测
        /// <summary>
        /// 检测用户是否存在
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="user"></param>
        private void CheckUserId(int userId, out User user)
        {
            user = this._userRepository.GetEntity(ConditionEqualId(userId));
            AssertUtil.IsNotNull(user, LanguageUtil.Translate("api_Business_UserSpecial_CheckUserId_userIdNotExist"));
        }

        /// <summary>
        /// 检测专辑ID参数传递正确性
        /// </summary>
        /// <param name="albumId"></param>
        private void CheckAlbumId(int albumId)
        {
            AssertUtil.AreBigger(albumId, 0, LanguageUtil.Translate("api_Business_UserSpecial_CheckAlbumId_albumParameterErrors"));
        }
        /// <summary>
        /// 检测专辑是否存在
        /// </summary>
        /// <param name="albumId"></param>
        /// <param name="us"></param>
        private void CheckAlbumId(int albumId, out UserSpecial us)
        {
            us = this._userSpecialRepository.GetEntity(ConditionEqualId(albumId));
            AssertUtil.IsNotNull(us, LanguageUtil.Translate("api_Business_UserSpecial_CheckAlbumId_albumNotExist"));
        }

        /// <summary>
        /// 检测视频是否存在
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="video"></param>
        private void CheckVideoId(int videoId, out Video video)
        {
            video = this._videoRepository.GetEntity(ConditionEqualId(videoId));
            AssertUtil.IsNotNull(video, LanguageUtil.Translate("api_Business_UserSpecial_CheckVideoId_videoIdNotExist"));
        }

        /// <summary>
        /// 检测标题不为空
        /// </summary>
        /// <param name="title"></param>
        private void CheckTitleNotNull(string title)
        {
            AssertUtil.NotNullOrWhiteSpace(title, LanguageUtil.Translate("api_Business_UserSpecial_CheckTitleNotNull_titleNotNull"));
        }

        private void CheckTitleRepeat(string title, int userId)
        {
            IList<Condtion> conditions = new List<Condtion>();

            conditions.Add(CondtionEqualCreateUserId(userId));

            conditions.Add(CondtionEqualTitle(title));


            var userSpecial = (from us in this._userSpecialRepository.GetEntityList(conditions)
                               select us).AsQueryable();
            if (userSpecial.Any())
                AssertUtil.IsTrue(false, LanguageUtil.Translate("api_Business_UserSpecial_CheckTitleRepeat_titleRepeat"));
        }

        /// <summary>
        /// 检测标签不为空
        /// </summary>
        /// <param name="label"></param>
        private void CheckLabelNotNull(string label)
        {
            AssertUtil.NotNullOrWhiteSpace(label, LanguageUtil.Translate("api_Business_UserSpecial_CheckLabelNotNull_titleNotNull"));
        }

        /// <summary>
        /// 检测专辑创建者Id与用户Id是否相等
        /// </summary>
        /// <param name="albumUserId"></param>
        /// <param name="userId"></param>
        private void CheckUserEqual(int albumUserId, int userId)
        {
            AssertUtil.AreEqual(albumUserId, userId, LanguageUtil.Translate("api_Business_UserSpecial_CheckUserEqual_notRoot"));
        }

        /// <summary>
        /// 检测专辑推荐数量
        /// </summary>
        /// <param name="albumId"></param>
        private void CheckRecommendCount(int recommendCount, int albumsCount)
        {
            //查询推荐专辑的个数
            var userSpecial = (from usl in this._userSpecialRepository.GetEntityList(CondtionEqualIsRecommend())
                               select usl).AsQueryable<UserSpecial>();

            IList<UserSpecial> queryList = userSpecial.ToList<UserSpecial>();

            AssertUtil.AreSmallerOrEqual(queryList.Count + albumsCount, recommendCount,LanguageUtil.Translate("  api_Business_UserSpecial_CheckRecommendCount_cannotAddAlbum"));
        }

        /// <summary>
        /// 检测传值：专辑Id集与排序编号数量相等
        /// </summary>
        /// <param name="albumId"></param>
        private void CheckAlbumsAndSortNumCount(int albumsCount, int sortNumCount)
        {

            AssertUtil.AreEqual(albumsCount, sortNumCount, LanguageUtil.Translate("  api_Business_UserSpecial_CheckAlbumsAndSortNumCount_theValueOfError"));
        }
        #endregion

    }
}