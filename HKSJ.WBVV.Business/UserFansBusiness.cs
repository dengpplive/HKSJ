
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

namespace HKSJ.WBVV.Business
{
    /// <summary>
    /// 粉丝
    /// </summary>
    public class UserFansBusiness : BaseBusiness, IUserFansBusiness
    {
        private readonly IUserRepository _userRepository;
        private readonly IVideoRepository _videoRepository;
        private readonly IUserFansRepository _userFansRepository;
        private readonly IUserRoomChooseRepository _userRoomChooseRepository;
        public UserFansBusiness(
            IUserRepository userRepository,
            IVideoRepository videoRepository,
            IUserFansRepository userFansRepository,
             IUserRoomChooseRepository userRoomChooseRepository
            )
        {
            this._userRepository = userRepository;
            this._videoRepository = videoRepository;
            this._userFansRepository = userFansRepository;
            this._userRoomChooseRepository = userRoomChooseRepository;
        }
        /// <summary>
        /// 该用户的所有粉丝
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        protected IQueryable<UserFans> GetUserFansList(int userId)
        {
            return this._userFansRepository.GetEntityList()
                .Where(p => p.SubscribeUserId == userId && p.CreateUserId != userId)
                .Where(p => !p.State);
        }



        /// <summary>
        /// 关注或者取消关注 粉丝
        /// </summary>
        /// <param name="createUserId"></param>
        /// <param name="subscribeUserId"></param>
        /// <param name="careState"></param>
        /// <returns></returns>
        public ResultView<UserFans> SaveCareFanState(int createUserId, int subscribeUserId, bool careState)
        {
            var result = new ResultView<UserFans>();
            try
            {
                var model = this._userFansRepository.GetEntityList().Where(p => p.SubscribeUserId == subscribeUserId)
                    .Where(p => p.CreateUserId == createUserId).FirstOrDefault();
                if (model == null)
                {
                    //添加
                    model = new UserFans();
                    model.CreateUserId = createUserId;
                    model.SubscribeUserId = subscribeUserId;
                    model.CreateTime = System.DateTime.Now;
                    model.State = careState;
                    this._userFansRepository.CreateEntity(model);
                }
                else
                {
                    //修改
                    model.UpdateTime = System.DateTime.Now;
                    model.UpdateUserId = createUserId;
                    model.State = careState;
                    this._userFansRepository.UpdateEntity(model);
                }
                result.Data = model;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.ExceptionMessage = ex.Message;
            }
            return result;
        }

        #region 传入参数
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

        #endregion


        /// <summary>
        /// 该用户是否已被订阅
        /// </summary>
        /// <param name="createUserId"></param>
        /// <param name="subscribeUserId"></param>
        /// <returns></returns>
        public ResultView<bool> IsSubscribe(int createUserId, int subscribeUserId)
        {
            var result = new ResultView<bool>();
            try
            {
                result.Data = IsSubed(this._userFansRepository.GetEntityList(), subscribeUserId, createUserId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.ExceptionMessage = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 订阅和取消订阅
        /// </summary>
        /// <param name="createUserId">订阅的用户</param>
        /// <param name="subscribeUserId">被订阅的用户</param>
        /// <param name="careState">订阅状态</param>
        /// <returns></returns>
        public bool SaveSubscribe(int createUserId, int subscribeUserId, bool careState)
        {
            return this._userFansRepository.UserSubscribeTransaction(createUserId, subscribeUserId, careState);
        }

        /// <summary>
        /// 获取当前用户的粉丝
        /// </summary>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="condtions"></param>
        /// <param name="orderCondtions"></param>
        /// <param name="userId">浏览用户编号</param>
        /// <returns></returns>
        public PageResult GetUserFunsList(int userId, int loginUserId)
        {
            IQueryable<UserFansView> userFans = null;
            var isLogin = IsLogin(this._userRepository.GetEntityList(), loginUserId);
            if (loginUserId <= 0)
            {
                userFans = (from fan in this._userFansRepository.GetEntityList(CondtionEqualState())
                            join u in this._userRepository.GetEntityList(CondtionEqualState()) on fan.CreateUserId equals u.Id
                            join user in this._userRepository.GetEntityList(CondtionEqualState()) on fan.SubscribeUserId equals user.Id
                            where fan.SubscribeUserId == userId
                            select new UserFansView
                            {
                                Id = fan.Id,
                                UpdateTime = fan.UpdateTime ?? fan.CreateTime,
                                State = false,
                                FansCount = u.FansCount,
                                DynamicCount = (from uf in this._userFansRepository.GetEntityList()
                                                join cu in this._userRepository.GetEntityList() on fan.CreateUserId equals cu.Id
                                                join su in this._userRepository.GetEntityList() on fan.SubscribeUserId equals su.Id
                                                where uf.CreateUserId == u.Id && uf.State == false && cu.State == false && su.State == false
                                                select uf.Id).Count(),
                                VideoCount =
                                    this._videoRepository.GetEntityList(CondtionEqualCreateManageId(u.Id))
                                        .Count(p => p.VideoState == 3),
                                UserView = new UserView()
                                {
                                    Id = u.Id,
                                    NickName = u.NickName,
                                    Picture = u.Picture,
                                    PlayCount = u.PlayCount,
                                    FansCount = u.FansCount,
                                    SubscribeNum = u.SubscribeNum
                                }
                            }).AsQueryable();
            }
            else
            {
                if (userId == loginUserId)
                {
                    userFans = (from fan in this._userFansRepository.GetEntityList(CondtionEqualState())
                                join u in this._userRepository.GetEntityList(CondtionEqualState()) on fan.CreateUserId equals u.Id
                                join user in this._userRepository.GetEntityList(CondtionEqualState()) on fan.SubscribeUserId
                                    equals user.Id
                                where fan.SubscribeUserId == loginUserId
                                select new UserFansView
                                {
                                    Id = fan.Id,
                                    UpdateTime = fan.UpdateTime ?? fan.CreateTime,
                                    State = isLogin && IsSubed(this._userFansRepository.GetEntityList(), u.Id, loginUserId),
                                    FansCount = u.FansCount,
                                    DynamicCount = (from uf in this._userFansRepository.GetEntityList()
                                                    join cu in this._userRepository.GetEntityList() on fan.CreateUserId equals cu.Id
                                                    join su in this._userRepository.GetEntityList() on fan.SubscribeUserId equals su.Id
                                                    where uf.CreateUserId == u.Id && uf.State == false && cu.State == false && su.State == false
                                                    select uf.Id).Count(),
                                    VideoCount =
                                        this._videoRepository.GetEntityList(CondtionEqualCreateManageId(u.Id))
                                            .Count(p => p.VideoState == 3),
                                    UserView = new UserView()
                                    {
                                        Id = u.Id,
                                        NickName = u.NickName,
                                        Picture = u.Picture,
                                        PlayCount = u.PlayCount,
                                        FansCount = u.FansCount,
                                        SubscribeNum = u.SubscribeNum
                                    }
                                }).AsQueryable();
                }
                else
                {
                    userFans = (from fan in this._userFansRepository.GetEntityList(CondtionEqualState())
                                join u in this._userRepository.GetEntityList(CondtionEqualState()) on fan.CreateUserId equals u.Id
                                join user in this._userRepository.GetEntityList(CondtionEqualState()) on fan.SubscribeUserId equals user.Id
                                where fan.SubscribeUserId == userId && u.Id != loginUserId
                                select new UserFansView
                                {
                                    Id = fan.Id,
                                    UpdateTime = fan.UpdateTime ?? fan.CreateTime,
                                    State = isLogin && IsSubed(this._userFansRepository.GetEntityList(), u.Id, loginUserId),
                                    FansCount = u.FansCount,
                                    DynamicCount = (from uf in this._userFansRepository.GetEntityList()
                                                    join cu in this._userRepository.GetEntityList() on fan.CreateUserId equals cu.Id
                                                    join su in this._userRepository.GetEntityList() on fan.SubscribeUserId equals su.Id
                                                    where uf.CreateUserId == u.Id && uf.State == false && cu.State == false && su.State == false
                                                    select uf.Id).Count(),
                                    VideoCount =
                                        this._videoRepository.GetEntityList(CondtionEqualCreateManageId(u.Id))
                                            .Count(p => p.VideoState == 3),
                                    UserView = new UserView()
                                    {
                                        Id = u.Id,
                                        NickName = u.NickName,
                                        Picture = u.Picture,
                                        PlayCount = u.PlayCount,
                                        FansCount = u.FansCount,
                                        SubscribeNum = u.SubscribeNum
                                    }
                                }).AsQueryable();
                }
            }
            userFans = userFans.OrderByDescending(u => u.UpdateTime);
            int totalCount = userFans.Count();
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
                userFans = userFans.Skip<UserFansView>((this.PageIndex - 1) * this.PageSize).Take<UserFansView>(this.PageSize);
            }
            var result = new PageResult()
            {
                PageSize = this.PageSize,
                PageIndex = this.PageIndex,
                TotalCount = totalCount,
                TotalIndex = pageCount,
                Data = userFans.ToList()
            };
            return result;
        }
        /// <summary>
        ///  获取当前用户的订阅信息
        /// </summary>
        /// <param name="userId">浏览用户编号</param>
        /// <param name="loginUserId">登录用户编号</param>
        /// <param name="condtions"></param>
        /// <param name="orderCondtions"></param>
        /// <returns></returns>
        public PageResult GetUserSubscribeList(int userId, int loginUserId)
        {
            IQueryable<UserSubscribeView> userFans = null;
            var isLogin = IsLogin(this._userRepository.GetEntityList(), loginUserId);
            if (loginUserId <= 0)
            {
                userFans = (from fan in this._userFansRepository.GetEntityList(CondtionEqualState())
                            join u in this._userRepository.GetEntityList(CondtionEqualState()) on fan.SubscribeUserId equals u.Id
                            join user in this._userRepository.GetEntityList(CondtionEqualState()) on fan.CreateUserId equals user.Id
                            where fan.CreateUserId == userId
                            orderby fan.UpdateTime ?? fan.CreateTime descending
                            select new UserSubscribeView
                            {
                                Id = fan.Id,
                                State = isLogin,
                                UserView = new UserView()
                                {
                                    Id = u.Id,
                                    NickName = u.NickName,
                                    Picture = u.Picture,
                                    PlayCount = u.PlayCount,
                                    FansCount = u.FansCount,
                                    SubscribeNum = u.SubscribeNum
                                },
                                Videos = (from v in this._videoRepository.GetEntityList()
                                          where v.State == false && v.CreateManageId == u.Id && v.VideoState == 3
                                          orderby v.UpdateTime descending
                                          select new VideoView()
                                          {
                                              About = v.About,
                                              SmallPicturePath = v.SmallPicturePath,
                                              CreateTime = v.CreateTime,
                                              UpdateTime = v.UpdateTime ?? v.CreateTime,
                                              Director = v.Director,
                                              Id = v.Id,
                                              Starring = v.Starring,
                                              RewardCount = (int)v.RewardCount,
                                              PlayCount = v.PlayCount,
                                              TimeLength = v.TimeLength ?? 0,
                                              Title = v.Title,
                                              UserNickName = user.NickName,
                                              CommentCount = v.CommentCount
                                          }).Take(4).ToList()

                            }).AsQueryable();
            }
            else
            {
                if (userId == loginUserId)
                {
                    userFans = (from fan in this._userFansRepository.GetEntityList(CondtionEqualState())
                                join u in this._userRepository.GetEntityList(CondtionEqualState()) on fan.SubscribeUserId equals u.Id
                                join user in this._userRepository.GetEntityList(CondtionEqualState()) on fan.CreateUserId
                                    equals user.Id
                                where fan.CreateUserId == loginUserId
                                orderby fan.UpdateTime ?? fan.CreateTime descending
                                select new UserSubscribeView
                                {
                                    Id = fan.Id,
                                    State = isLogin && IsSubed(this._userFansRepository.GetEntityList(), u.Id, loginUserId),
                                    UserView = new UserView()
                                    {
                                        Id = u.Id,
                                        NickName = u.NickName,
                                        Picture = u.Picture,
                                        PlayCount = u.PlayCount,
                                        FansCount = u.FansCount,
                                        SubscribeNum = u.SubscribeNum
                                    },
                                    Videos = (from v in this._videoRepository.GetEntityList()
                                              where v.State == false && v.CreateManageId == u.Id && v.VideoState == 3
                                              orderby v.UpdateTime descending
                                              select new VideoView()
                                              {
                                                  About = v.About,
                                                  SmallPicturePath = v.SmallPicturePath,
                                                  CreateTime = v.CreateTime,
                                                  UpdateTime = v.UpdateTime ?? v.CreateTime,
                                                  Director = v.Director,
                                                  Id = v.Id,
                                                  Starring = v.Starring,
                                                  RewardCount = (int)v.RewardCount,
                                                  PlayCount = v.PlayCount,
                                                  TimeLength = v.TimeLength ?? 0,
                                                  Title = v.Title,
                                                  UserNickName = user.NickName,
                                                  CommentCount = v.CommentCount
                                              }).Take(4).ToList()

                                }).AsQueryable();
                }
                else
                {
                    userFans = (from fan in this._userFansRepository.GetEntityList(CondtionEqualState())
                                join u in this._userRepository.GetEntityList(CondtionEqualState()) on fan.SubscribeUserId equals u.Id
                                join user in this._userRepository.GetEntityList(CondtionEqualState()) on fan.CreateUserId equals user.Id
                                where fan.CreateUserId == userId && u.Id != loginUserId
                                orderby fan.UpdateTime ?? fan.CreateTime descending
                                select new UserSubscribeView
                                {
                                    Id = fan.Id,
                                    State = isLogin && IsSubed(this._userFansRepository.GetEntityList(), u.Id, loginUserId),
                                    UserView = new UserView()
                                    {
                                        Id = u.Id,
                                        NickName = u.NickName,
                                        Picture = u.Picture,
                                        PlayCount = u.PlayCount,
                                        FansCount = u.FansCount,
                                        SubscribeNum = u.SubscribeNum
                                    },
                                    Videos = (from v in this._videoRepository.GetEntityList()
                                              where v.State == false && v.CreateManageId == u.Id && v.VideoState == 3
                                              orderby v.UpdateTime descending
                                              select new VideoView()
                                              {
                                                  About = v.About,
                                                  SmallPicturePath = v.SmallPicturePath,
                                                  CreateTime = v.CreateTime,
                                                  UpdateTime = v.UpdateTime ?? v.CreateTime,
                                                  Director = v.Director,
                                                  Id = v.Id,
                                                  Starring = v.Starring,
                                                  RewardCount = (int)v.RewardCount,
                                                  PlayCount = v.PlayCount,
                                                  TimeLength = v.TimeLength ?? 0,
                                                  Title = v.Title,
                                                  UserNickName = user.NickName,
                                                  CommentCount = v.CommentCount
                                              }).Take(4).ToList()

                                }).AsQueryable();
                }
            }
            int totalCount = userFans.Count();
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
                userFans = userFans.Skip<UserSubscribeView>((this.PageIndex - 1) * this.PageSize).Take<UserSubscribeView>(this.PageSize);
            }
            var result = new PageResult()
            {
                PageSize = this.PageSize,
                PageIndex = this.PageIndex,
                TotalCount = totalCount,
                TotalIndex = pageCount,
                Data = userFans.ToList()
            };
            return result;
        }

        /// <summary>
        /// 我的订阅 用户列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="loginUserId"></param>
        /// <param name="condtions"></param>
        /// <param name="orderCondtions"></param>
        /// <returns></returns>
        public PageResult GetUserSubscribeUserList(int loginUserId)
        {
            var result = new PageResult()
            {
                PageSize = 0,
                PageIndex = 0,
                TotalCount = 0,
                TotalIndex = 0,
                Data = new List<UserView>()
            };
            if (IsLogin(this._userRepository.GetEntityList(), loginUserId))
            {
                var res = (from uf in this._userFansRepository.GetEntityList(CondtionEqualState())
                           join u in this._userRepository.GetEntityList(CondtionEqualState()) on uf.CreateUserId equals u.Id
                           join user in this._userRepository.GetEntityList(CondtionEqualState()) on uf.SubscribeUserId equals user.Id
                           where uf.CreateUserId == loginUserId 
                           orderby uf.UpdateTime ?? uf.CreateTime descending
                           select new UserView()
                           {
                               Id = user.Id,
                               NickName = user.NickName,
                               Bardian = user.Bardian,
                               Picture = user.Picture,
                               PlayCount = user.PlayCount,
                               FansCount = user.FansCount,
                               SubscribeNum = user.SubscribeNum
                           }).AsQueryable();
                int totalCount = res.Count();
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
                    res = res.Skip<UserView>((this.PageIndex - 1) * this.PageSize).Take<UserView>(this.PageSize);
                }
                result.PageSize = this.PageSize;
                result.PageIndex = this.PageIndex;
                result.TotalCount = totalCount;
                result.TotalIndex = pageCount;
                result.Data = res.ToList();
            }
            return result;
        }

        /// <summary>
        /// 我的订阅  所有用户视频列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="loginUserId"></param>
        /// <param name="condtions"></param>
        /// <param name="orderCondtions"></param>
        /// <returns></returns>
        public PageResult GetUserSubscribeVideoList(int loginUserId)
        {
            var result = new PageResult()
            {
                PageSize = 0,
                PageIndex = 0,
                TotalCount = 0,
                TotalIndex = 0,
                Data = new List<UserView>()
            };
            if (IsLogin(this._userRepository.GetEntityList(), loginUserId))
            {
                var videos = (from uf in this._userFansRepository.GetEntityList(CondtionEqualState())
                              join u in this._userRepository.GetEntityList(CondtionEqualState()) on uf.CreateUserId equals u.Id
                              join user in this._userRepository.GetEntityList(CondtionEqualState()) on uf.SubscribeUserId equals
                                  user.Id
                              join v in this._videoRepository.GetEntityList(CondtionEqualState()) on uf.SubscribeUserId equals
                                  v.CreateManageId
                              where uf.CreateUserId == loginUserId
                                    && v.VideoSource
                                    && v.VideoState == 3
                              orderby v.UpdateTime descending
                              select new VideoView()
                              {
                                  About = v.About,
                                  SmallPicturePath = v.SmallPicturePath,
                                  CreateTime = v.CreateTime,
                                  UpdateTime = v.UpdateTime ?? v.CreateTime,
                                  Director = v.Director,
                                  Id = v.Id,
                                  Starring = v.Starring,
                                  RewardCount = (int)v.RewardCount,
                                  PlayCount = v.PlayCount,
                                  TimeLength = v.TimeLength ?? 0,
                                  Title = v.Title,
                                  UserNickName = user.NickName,
                                  CommentCount = v.CommentCount
                              }).AsQueryable();
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
                result.PageSize = this.PageSize;
                result.PageIndex = this.PageIndex;
                result.TotalCount = totalCount;
                result.TotalIndex = pageCount;
                result.Data = videos.ToList();
            }
            return result;
        }
    }
}