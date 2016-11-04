
using System;
using System.Data;
using System.Collections.Generic;
using HKSJ.WBVV.Business.Interface.Base;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.Entity.ViewModel.Manage;
using HKSJ.WBVV.Common.Extender.LinqExtender;

namespace HKSJ.WBVV.Business.Interface
{

    public interface IUserFansBusiness : IBaseBusiness
    {
        //粉丝列表
        PageResult GetUserFunsList(int userId, int loginUserId);

        //订阅列表
        PageResult GetUserSubscribeList(int userId, int loginUserId);
      
        /// <summary>
        /// 我的订阅用户
        /// </summary>
        /// <param name="loginUserId"></param>
        /// <param name="condtions"></param>
        /// <param name="orderCondtions"></param>
        /// <returns></returns>
        PageResult GetUserSubscribeUserList(int loginUserId);
        /// <summary>
        /// 我的订阅用户所有的视频
        /// </summary>
        /// <param name="loginUserId"></param>
        /// <param name="condtions"></param>
        /// <param name="orderCondtions"></param>
        /// <returns></returns>
        PageResult GetUserSubscribeVideoList(int loginUserId);

        /// <summary>
        /// 检测该用户是否被订阅
        /// </summary>
        /// <param name="createUserId"></param>
        /// <param name="subscribeUserId"></param>
        /// <returns></returns>
        ResultView<bool> IsSubscribe(int createUserId, int subscribeUserId);


        /// <summary>
        /// 订阅和取消订阅
        /// </summary>
        /// <param name="createUserId"></param>
        /// <param name="subscribeUserId"></param>
        /// <param name="careState"></param>
        /// <returns></returns>
        bool SaveSubscribe(int createUserId, int subscribeUserId, bool careState);

    }
}