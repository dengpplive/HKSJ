
using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
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
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Business
{

    public class UserRoomChooseBusiness : BaseBusiness, IUserRoomChooseBusiness
    {
        private readonly IUserRoomChooseRepository _userRoomChooseRepository;
        private readonly IUserRepository _userRepository;
        private readonly IVideoRepository _videoRepository;
        private readonly IUserSpecialRepository _userSpecialRepository;
        private readonly IUserSpecialSonRepository _userSpecialSonRepository;

        public UserRoomChooseBusiness(
            IUserRoomChooseRepository userRoomChooseRepository,
            IUserRepository userRepository,
            IVideoRepository videoRepository,
            IUserSpecialRepository userSpecialRepository,
            IUserSpecialSonRepository userSpecialSonRepository

            )
        {
            this._userRoomChooseRepository = userRoomChooseRepository;
            this._userRepository = userRepository;
            this._videoRepository = videoRepository;
            this._userSpecialRepository = userSpecialRepository;
            this._userSpecialSonRepository = userSpecialSonRepository;
        }

        /// <summary>
        /// 获取当前用户所有视频、专辑集合
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private IQueryable<UserRoomChoose> GetUserRoomChooseList(int userId)
        {
            return this._userRoomChooseRepository.GetEntityList(CondtionEqualUserId(userId));
        }

        /// <summary>
        /// 根据专辑ID获取旗下视频信息
        /// </summary>
        /// <param name="specialId"></param>
        /// <returns></returns>
        private IQueryable<UserSpecialSon> GetUserSpecialSonList(int specialId)
        {
            return this._userSpecialSonRepository.GetEntityList(CondtionEqualSpecialId(specialId));
        }



        /// <summary>
        /// 获取当前用户的所有视频信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private IQueryable<VideoView> GetUserRoomVideoList(int userId)
        {
            CheckIdNotNull(userId);
            var videos = (from urc in GetUserRoomChooseList(userId)
                          join user in this._userRepository.GetEntityList(CondtionEqualState()) on urc.UserId equals user.Id
                          join v in this._videoRepository.GetEntityList(CondtionEqualState()).Where(p => p.VideoState == 3) on urc.ModuleId equals v.Id
                          where urc.TypeId == 0
                          orderby urc.SortNum, urc.CreateTime
                          select new VideoView()
                          {
                              IsHot = v.IsHot,
                              IsRecommend = v.IsRecommend,
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
                              Title = v.Title,
                              Filter = v.Filter
                          }).AsQueryable();

            return videos;
        }

        private IQueryable<VideoView> GetUserRoomAllVideoList(int userId)
        {
            CheckIdNotNull(userId);
            var videos = (from urc in GetUserRoomChooseList(userId)
                          join user in this._userRepository.GetEntityList(CondtionEqualState()) on urc.UserId equals user.Id
                          join v in this._videoRepository.GetEntityList(CondtionEqualState()).Where(p => p.VideoState == 3) on urc.ModuleId equals v.Id
                          orderby urc.SortNum, urc.CreateTime
                          select new VideoView()
                          {
                              IsHot = v.IsHot,
                              IsRecommend = v.IsRecommend,
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
                              Title = v.Title,
                              Filter = v.Filter
                          }).AsQueryable();

            return videos;
        }

        /// <summary>
        /// 获取当前用户的视频列表 用于分页
        /// </summary>
        /// <param name="condtions"></param>
        /// <param name="orderCondtions"></param>
        /// <returns></returns>
        public PageResult GetUserVideoList(int userId, IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            var videos = from v in _videoRepository.GetEntityList(CondtionEqualCreateManageId(userId))
                         where v.VideoState == 3
                         select new VideoView()
                         {
                             IsHot = v.IsHot,
                             IsRecommend = v.IsRecommend,
                             PlayCount = v.PlayCount,
                             About = v.About,
                             BigPicturePath = v.BigPicturePath,
                             CreateTime = v.CreateTime,
                             UpdateTime = v.UpdateTime ?? v.CreateTime,
                             Director = v.Director,
                             Id = v.Id,
                             IsOfficial = v.IsOfficial,
                             SmallPicturePath = v.SmallPicturePath,
                             Starring = v.Starring,
                             Tags = v.Tags,
                             Title = v.Title,
                             Filter = v.Filter
                         };
            if (condtions != null && condtions.Count() > 0)
                videos = videos.Query<VideoView>(condtions);
            if (orderCondtions != null && orderCondtions.Count() > 0)
                videos = videos.OrderBy<VideoView>(orderCondtions);

            int totalCount = videos.Count();
            int pageCount = 0;
            if (this.PageSize > 0)
            {
                pageCount = totalCount % this.PageSize == 0
                  ? (totalCount / this.PageSize)
                  : (totalCount / this.PageSize + 1);
                if (this.PageIndex <= 0)
                {
                    this.PageIndex = 1;
                }
                if (this.PageIndex >= pageCount)
                {
                    this.PageIndex = pageCount;
                }
                videos = videos.Skip<VideoView>((this.PageIndex - 1) * this.PageSize).Take<VideoView>(this.PageSize);
            }
            var result = new PageResult()
            {
                PageSize = this.PageSize,
                PageIndex = this.PageIndex,
                TotalCount = totalCount,
                TotalIndex = pageCount,
                Data = videos.ToList()
            };
            return result;
        }

        #region 默认排序查询个人空间-热门视频集合


        /// <summary>
        /// 默认排序查询个人空间-热门视频集合
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public IList<VideoView> GetUserRoomVideoData(int userId, int dataNum)
        {
            CheckIdNotNull(userId);
            var videos = GetUserRoomVideoList(userId).Take(dataNum).Select(v => new VideoView()
                          {
                              IsHot = v.IsHot,
                              IsRecommend = v.IsRecommend,
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
                              Title = v.Title,
                              Filter = v.Filter
                          }).ToList();
            return videos;
            // return videos.Any() ? videos.ToList() : new List<Video>();
        }


        #endregion

        #region 默认排序查询个人空间-专辑推荐视图
        /// <summary>
        /// 默认排序查询个人空间-专辑推荐视图
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="dataNum">专辑数</param>
        /// <param name="videoNum">视频数</param>
        /// <returns></returns>
        public SpecialView GetUserRoomSpecialData(int userId, int dataNum, int videoNum = 0)
        {
            CheckIdNotNull(userId);
            SpecialView sv = new SpecialView();
            var userSpecial = (from urc in GetUserRoomChooseList(userId)
                               join special in this._userSpecialRepository.GetEntityList(CondtionEqualState()) on urc.ModuleId equals special.Id
                               where urc.TypeId == 1
                               orderby urc.SortNum, urc.CreateTime
                               select special
                               ).AsQueryable().Take(dataNum);

            List<UserSpecial> list = userSpecial.ToList<UserSpecial>();

            List<SpecialDetailView> sdvList = new List<SpecialDetailView>();
            SpecialDetailView sdv = null;

            List<SpecialVideoView> svvList = null;
            SpecialVideoView svv = null;
            List<Video> videoList = null;



            foreach (UserSpecial item in list)
            {
                sdv = new SpecialDetailView();
                sdv.Id = item.Id;
                sdv.Title = item.Title;

                var vidoes = (from ussl in GetUserSpecialSonList(item.Id)
                              join video in this._videoRepository.GetEntityList(CondtionEqualState()).Where(p => p.VideoState == 3) on (long)ussl.VideoId equals video.Id
                              select video
                           ).OrderByDescending(p => p.UpdateTime ?? p.CreateTime).AsQueryable();

                if (videoNum > 0)
                    vidoes = vidoes.Take(videoNum);

                videoList = vidoes.ToList<Video>();
                svvList = new List<SpecialVideoView>();
                foreach (Video v in videoList)
                {
                    svv = new SpecialVideoView();
                    svv.Id = v.Id;
                    svv.SmallPicturePath = v.SmallPicturePath;
                    svv.Title = v.Title;
                    svv.PlayCount = v.PlayCount;
                    svv.CommentCount = v.CommentCount;
                    svv.TimeLength = ConvertUtil.ToInt(v.TimeLength);
                    svvList.Add(svv);
                }


                sdv.SpecialVideoList = svvList;
                sdvList.Add(sdv);
            }

            sv.SpecialCount = list.Count;
            sv.SpecialVideoList = sdvList;
            return sv;
        }

        #endregion

        #region 获取排序编号
        /// <summary>
        /// 获取排序编号
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public int GetLastSortNum(int userId, int typeId)
        {
            var sortNum = (from urc in GetUserRoomChooseList(userId)
                           where
                             urc.TypeId == typeId
                           orderby
                             urc.SortNum descending
                           select new
                           {
                               urc.SortNum
                           }).Take(1).FirstOrDefault();
            int sort = 0;
            if (sortNum != null)
                sort = sortNum.SortNum;
            return sort;
        }
        #endregion

        #region 添加视频到空间展示界面
        /// <summary>
        /// 添加视频到空间展示界面
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="videoId">视频ID</param>
        /// <returns></returns>
        public int AddVideoToUserRoom(int userId, string videoIds)
        {
            CheckIdNotNull(userId);
            CheckVideoIdsNotNull(videoIds);
            string[] videoIdStrs = videoIds.Split(',');
            List<int> videoIdList = new List<int>();
            foreach (string item in videoIdStrs)
            {
                CheckIdNotNull(Convert.ToInt32(item));
                videoIdList.Add(Convert.ToInt32(item));
            }

            int sortNum = GetLastSortNum(userId, 0);

            IList<UserRoomChoose> objList = new List<UserRoomChoose>();
            List<long> vids = new List<long>();
            UserRoomChoose userRoomChoose = null;
            foreach (int item in videoIdList)
            {
                userRoomChoose = new UserRoomChoose()
               {
                   UserId = userId,
                   ModuleId = item,
                   TypeId = 0,//0：视频
                   SortNum = sortNum + 1,
                   CreateTime = DateTime.Now
               };
                vids.Add(item);
                objList.Add(userRoomChoose);
                sortNum++;
            }

            this._userRoomChooseRepository.CreateEntitys(objList);

            //修改视频的上传用户为当前用户上传 CreateManageId
            UpdateVideos(userId, vids);

            return objList.Count;
        }
        private void UpdateVideos(int userId, List<long> vids)
        {
            var vidList = this._videoRepository.GetEntityList().Where(p => vids.Contains(p.Id)).ToList();
            for (int i = 0; i < vidList.Count(); i++)
            {
                vidList[i].VideoSource = true;
                vidList[i].CreateManageId = userId;
                vidList[i].CreateTime = System.DateTime.Now;
            }
            this._videoRepository.UpdateEntitys(vidList);
        }
        #endregion




        #region 将视频从空间展示界面删除
        /// <summary>
        /// 将视频从空间展示界面删除
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="videoId">视频ID</param>
        /// <returns></returns>
        public bool RemoveVideoToUserRoom(int userId, int videoId)
        {
            CheckIdNotNull(userId);
            CheckIdNotNull(videoId);

            var userRoomChoose = (from urc in GetUserRoomChooseList(userId)
                                  where urc.TypeId == 0
                                  && urc.ModuleId == videoId
                                  select urc
                                    ).FirstOrDefault<UserRoomChoose>();

            return userRoomChoose != null ? this._userRoomChooseRepository.DeleteEntity(userRoomChoose) : false;

        }
        #endregion

        #region 添加专辑到空间展示界面
        /// <summary>
        /// 添加专辑到空间展示界面
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="albumId">视频ID</param>
        /// <returns></returns>
        public int AddAlbumToUserRoom(int userId, string albumIds)
        {
            CheckIdNotNull(userId);
            CheckAlbumIdsNotNull(albumIds);
            string[] albumIdStrs = albumIds.Split(',');
            List<int> videoIdList = new List<int>();
            foreach (string item in albumIdStrs)
            {
                CheckIdNotNull(Convert.ToInt32(item));
                videoIdList.Add(Convert.ToInt32(item));
            }

            int sortNum = GetLastSortNum(userId, 1);

            IList<UserRoomChoose> objList = new List<UserRoomChoose>();
            UserRoomChoose userRoomChoose = null;
            foreach (int item in videoIdList)
            {
                userRoomChoose = new UserRoomChoose()
                {
                    UserId = userId,
                    ModuleId = item,
                    TypeId = 1,//1：专辑
                    SortNum = sortNum + 1,
                    CreateTime = DateTime.Now
                };
                objList.Add(userRoomChoose);
                sortNum++;
            }

            this._userRoomChooseRepository.CreateEntitys(objList);
            return objList.Count;
        }
        #endregion

        #region 将专辑从空间展示界面删除
        /// <summary>
        /// 将专辑空间展示界面删除
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="albumId">视频ID</param>
        /// <returns></returns>
        public bool RemoveAlbumtToUserRoom(int userId, int albumId)
        {
            CheckIdNotNull(userId);
            CheckIdNotNull(albumId);

            var userRoomChoose = (from urc in GetUserRoomChooseList(userId)
                                  where urc.TypeId == 1
                                  && urc.ModuleId == albumId
                                  select urc
                                    ).FirstOrDefault<UserRoomChoose>();

            return userRoomChoose != null ? this._userRoomChooseRepository.DeleteEntity(userRoomChoose) : false;

        }
        #endregion


        #region 获取用户上传视频,过滤已添加到空间视频

        /// <summary>
        /// 获取用户上传视频,过滤已添加到空间视频
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public MyVideoViewResult GetUserVideoViews(int userId, int pageIndex, int pageSize)
        {
            var result = new MyVideoViewResult() { MyVideoViews = new List<MyVideoView>(), TotalCount = 0 };
            IQueryable<MyVideoView> queryable;
            queryable =
              (from myvideo in _videoRepository.GetEntityList(CondtionEqualCreateManageId(userId))
               where myvideo.VideoState == 3//TODO 刘强CheckState=1
               && !(from userroomchoose in this._userRoomChooseRepository.GetEntityList(CondtionEqualUserId(userId))
                    where
                      userroomchoose.TypeId == 0
                    select new
                    {
                        userroomchoose.ModuleId
                    }).Contains(new { ModuleId = (Int32)myvideo.Id })
               select new MyVideoView
               {
                   Id = myvideo.Id,
                   Title = myvideo.Title,
                   CommentCount = myvideo.CommentCount,
                   PlayCount = myvideo.PlayCount,
                   SmallPicturePath = myvideo.SmallPicturePath,
                   VideoState = myvideo.VideoState,
                   CreateTime = myvideo.CreateTime
               }).AsQueryable();
            if (!queryable.Any()) return result;
            result.TotalCount = queryable.Count();
            result.PageCount = queryable.Count() % pageSize == 0 ? queryable.Count() / pageSize : queryable.Count() / pageSize + 1;
            result.MyVideoViews = queryable.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return result;
        }
        #endregion

        #region 获取用户创建的专辑,过滤已添加到空间专辑

        /// <summary>
        /// 获取用户创建的专辑，过滤已添加到空间专辑
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="escapeStatestate"></param>
        /// <returns></returns>
        public SpecialView GetUserAlbumsViews(int userId, int pageIndex, int pageSize)
        {
            var sv = new SpecialView() { SpecialVideoList = new List<SpecialDetailView>(), SpecialCount = 0 };

            var userSpecial = (from urc in _userSpecialRepository.GetEntityList(CondtionEqualCreateUserId(userId))
                               where !(from userroomchoose in this._userRoomChooseRepository.GetEntityList(CondtionEqualUserId(userId))
                                       where
                                         userroomchoose.TypeId == 1
                                       select new
                                       {
                                           userroomchoose.ModuleId
                                       }).Contains(new { ModuleId = urc.Id })
                               select urc
                               ).AsQueryable();
            List<UserSpecial> list = userSpecial.ToList<UserSpecial>();

            List<SpecialDetailView> sdvList = new List<SpecialDetailView>();
            SpecialDetailView sdv = null;

            List<SpecialVideoView> svvList = new List<SpecialVideoView>();
            SpecialVideoView svv = null;
            List<Video> videoList = null;



            foreach (UserSpecial item in list)
            {
                sdv = new SpecialDetailView();
                sdv.Id = item.Id;
                sdv.Title = item.Title;

                var vidoes = (from ussl in this._userSpecialSonRepository.GetEntityList(CondtionEqualSpecialId(item.Id))
                              join video in this._videoRepository.GetEntityList(CondtionEqualState()) on (long)ussl.VideoId equals video.Id
                              where video.VideoState == 3 //TODO update 刘强2015-11-06 过滤审核状态 
                              select video
                           ).AsQueryable();
                videoList = vidoes.ToList<Video>();
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
                }


                sdv.SpecialVideoList = svvList;
                sdvList.Add(sdv);
            }
            sv.SpecialVideoList = sdvList;

            if (list.Count <= 0) return sv;
            sv.SpecialCount = list.Count;
            sv.PageCount = sv.SpecialCount % pageSize == 0 ? sv.SpecialCount / pageSize : sv.SpecialCount / pageSize + 1;
            sv.SpecialVideoList = sv.SpecialVideoList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return sv;
        }
        #endregion





        #region 传入参数检测
        /// <summary>
        /// 检测用户Id合法性
        /// </summary>
        /// <param name="name"></param>
        private void CheckIdNotNull(int Id)
        {
            AssertUtil.IsValidId(Id, false);
        }

        /// <summary>
        /// 检测视频Id
        /// </summary>
        /// <param name="videoIds"></param>
        private void CheckVideoIdsNotNull(string videoIds)
        {
            AssertUtil.NullOrWhiteSpace(videoIds, LanguageUtil.Translate("api_Business_UserRoomChoose_CheckVideoIdsNotNull"));
        }

        /// <summary>
        /// 检测视频Id
        /// </summary>
        /// <param name="videoIds"></param>
        private void CheckAlbumIdsNotNull(string albumIds)
        {
            AssertUtil.NullOrWhiteSpace(albumIds, LanguageUtil.Translate("api_Business_UserRoomChoose_CheckAlbumIdsNotNull"));
        }


        #endregion

        #region 传入参数

        /// <summary>
        /// 比较类型ID相等
        /// </summary>
        /// <param name="visible"></param>
        /// <returns></returns>
        private Condtion CondtionEqualTypeId(int typeId)
        {
            var condtion = new Condtion()
            {
                FiledName = "TypeId",
                FiledValue = typeId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }
        /// <summary>
        /// 比较用户ID相等
        /// </summary>
        /// <param name="typeId"></param>
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
        /// 比较专辑ID
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
        #endregion

        #region 排序参数


        #endregion

    }
}