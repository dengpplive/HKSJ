using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using HKSJ.Utilities;
using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Business.Interface.Base;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ApiParaModel;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.Repository.Interface;
using HKSJ.WBVV.Common.Language;


namespace HKSJ.WBVV.Business
{
    /// <summary>
    /// 用户最近访客
    /// </summary>
    public class UserVisitLogBusiness : BaseBusiness, IUserVisitLogBusiness
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserVisitLogRepository _iuserVisitLogRepository;
        public UserVisitLogBusiness(IUserRepository userRepository, IUserVisitLogRepository userVisitLogRepository)
        {
            this._userRepository = userRepository;
            this._iuserVisitLogRepository = userVisitLogRepository;
        }



        public PageResult UserVisitLogPagerList(int userId, IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 最近访问的用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="condtions"></param>
        /// <param name="orderCondtions"></param>
        /// <returns></returns>
        public IList<UserVisitView> GetUserVisitLogList(int userId, IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            var result = (from log in this._iuserVisitLogRepository.GetEntityList(CondtionEqualUserId(userId))
                          join user in this._userRepository.GetEntityList(CondtionEqualState()) on log.VisitorUserId equals user.Id
                          orderby log.UpdateTime descending
                          select new UserVisitView
                          {
                              UserVisitLog = log,
                              UserView = new UserView()
                              {
                                  Account = user.Account,
                                  BB = user.BB,
                                  Id = user.Id,
                                  NickName = user.NickName,
                                  Picture = user.Picture,
                                  Pwd = user.Password,
                                  PlayCount = user.PlayCount,
                                  FansCount = user.FansCount,
                                  State = user.State,
                                  SubscribeNum = user.SubscribeNum
                              }
                          }).AsQueryable();
            if (this.PageSize > 0)
            {
                result = result.Take(this.PageSize);
            }
            return result.ToList();
        }


        public ResultView<bool> CreateAndUpdateUserVisitLog(int CreateUserId, int VisitorUserId, int VisitedUserId)
        {
            var result = new ResultView<bool>();
            if (CreateUserId == VisitedUserId)
            {
                result.Data = false;
                result.ExceptionMessage = LanguageUtil.Translate("api_Business_UserVisitLog_CreateAndUpdateUserVisitLog_visityourself");
                return result;
            }
            try
            {
                var resultList = this._iuserVisitLogRepository.GetEntityList(CondtionEqualUserId(VisitedUserId));
                var log = new UserVisitLog();
                var model = resultList.Where(p => p.VisitorUserId == VisitorUserId
                    && p.CreateUserId == CreateUserId).FirstOrDefault();
                if (model != null)
                {
                    //修改
                    log = model;
                    log.UpdateTime = System.DateTime.Now;
                    log.UpdateUserId = CreateUserId;
                    result.Data = this._iuserVisitLogRepository.UpdateEntity(log);
                }
                else
                {
                    //浏览的用户存在添加历史记录
                    if (this._userRepository.GetEntityList().Where(p => p.Id == VisitedUserId).Count() > 0)
                    {
                        //添加
                        log.VisitedUserId = VisitedUserId;
                        log.VisitorUserId = VisitorUserId;
                        log.CreateUserId = CreateUserId;
                        log.CreateTime = System.DateTime.Now;
                        log.UpdateTime = System.DateTime.Now;
                        log.UpdateUserId = CreateUserId;
                        result.Data = this._iuserVisitLogRepository.CreateEntity(log);
                    }
                    else
                    {
                        result.Data = false;
                        result.ExceptionMessage = string.Format(LanguageUtil.Translate("api_Business_UserVisitLog_CreateAndUpdateUserVisitLog_visitedUsernotExist"), VisitedUserId);
                    }
                }
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ExceptionMessage = ex.Message;
            }
            return result;
        }

        public ResultView<bool> DeleteUserVisitLog(int Id)
        {
            var result = new ResultView<bool>();
            try
            {
                result.Data = this._iuserVisitLogRepository.DeleteEntity(new UserVisitLog()
                {
                    Id = Id
                });
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ExceptionMessage = ex.Message;
            }
            return result;
        }


        public ResultView<bool> CreateUserVisitLog(UserVisitLog userVisitLog)
        {
            throw new NotImplementedException();
        }

        public ResultView<bool> UpdateUserVisitLog(UserVisitLog userVisitLog)
        {
            throw new NotImplementedException();
        }


        public ResultView<bool> DeleteUserVisitLogs(List<UserVisitLog> userVisitLogs)
        {
            var result = new ResultView<bool>();
            try
            {
                this._iuserVisitLogRepository.DeleteEntitys(userVisitLogs);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ExceptionMessage = ex.Message;
            }
            return result;
        }
        #region 传入参数

        /// <summary>
        /// 比较类型ID相等
        /// </summary>
        /// <param name="visible"></param>
        /// <returns></returns>
        private Condtion CondtionEqualUserId(int visitedUserId)
        {
            var condtion = new Condtion()
            {
                FiledName = "VisitedUserId",
                FiledValue = visitedUserId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }
        #endregion
    }
}
