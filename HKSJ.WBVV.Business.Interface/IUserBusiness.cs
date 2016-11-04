using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Business.Interface.Base;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ApiParaModel;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity.ViewModel.Client;

namespace HKSJ.WBVV.Business.Interface
{
    public interface IUserBusiness : IBaseBusiness
    {
        PageResult SearchUser(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions);

        bool ForbiddenUser(int uid, bool state);

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="account">用户账户</param>
        /// <param name="password">用户密码</param>
        /// <returns></returns>
        UserView LoginUser(string account, string password);

        /// <summary>
        /// 用户登录SSO
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        UserView LoginUser(int userId);

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="account">用户账户</param>
        /// <param name="password">用户密码</param>
        /// <param name="type">账户类型(0:手机1:邮箱)</param>
        /// <returns></returns>
        UserView RegisterUser(string account, string password, int type);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserView GetUserInfo(int userId);

        /// <summary>
        /// 检测账户是否已经存在
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        bool CheckAccount(string account);

        /// <summary>
        /// 检测邮箱是否已经存在
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        bool CheckEmail(string email);

        /// <summary>
        /// 检测用户昵称是否已经存在
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="nickName"></param>
        /// <returns></returns>
        bool CheckNickName(int uid, string nickName);

        /// <summary>
        /// 根据手机号修改密码
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        UserView UpdatePwdByPhone(string account, string pwd);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        UserView SetPassword(int uid, string pwd);

        /// <summary>
        /// 重置手机号码
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        UserView SetPhone(int uid, string phone, string pwd = "");

        /// <summary>
        /// 重置邮箱
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        UserView SetEmail(int uid, string email, string pwd = "");

        /// <summary>
        /// 根据用户ID查询用户
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        User GetEntityByUserId(int uid);

        /// <summary>
        /// 根据用户ID查询用户粉丝数
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        int GetFansByUserId(int uid);

        /// <summary>
        /// 得到播放次数
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        int GetPlayCountByUserId(int uid);
        /// <summary>
        /// 用户观看记录
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        UserHistoryViews GetHistoryVideoByUserId(int uid, int pageIndex, int pageSize);
        /// <summary>
        /// 分享视频：【分享者】获得播币
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="DemandUserId"></param>
        /// <param name="IpAddress"></param>
        /// <param name="ShareUserId"></param>
        /// <param name="ShareUserIp"></param>
        /// <returns></returns>
        bool IncomeShare(int videoId, int DemandUserId, string IpAddress, int ShareUserId, string ShareUserIp);

        /// <summary>
        /// 新增一个视频观看记录
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="userId"></param>
        /// <param name="watchTime"></param>
        /// <returns></returns>
        bool AddHistoryVideo(int videoId, int userId, int watchTime);
        /// <summary>
        /// 账号设置
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        UserView AccountSet(AccountPara para);

        /// <summary>
        /// 获取用户账户信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        AccountView GetAccountInfo(int uid);

        UserView GetUser(int id);

        /// <summary>
        /// 获取用户上传视频
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="videoState"></param>
        /// <returns></returns>
        MyVideoViewResult GetUserVideoViews(int userId, int pageIndex, int pageSize, int videoState);

        /// <summary>
        /// 检查密码
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        bool CheckPwd(int uid, string pwd);

        /// <summary>
        /// 检查手机号
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        bool CheckPhone(int uid, string phone);

        /// <summary>
        /// 修改用户头像,修改key用来获取七牛图片
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        UserView UpdateUserPic(int uid, string key);

        /// <summary>
        /// 获取用户头像Key
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        string GetUserPicKey(int uid);


        /// <summary>
        /// 修改用户个性签名
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="Bardian"></param>
        /// <returns></returns>
        bool UpdateUserBardian(int userId, string bardian);

        /// <summary>
        /// 修改用户个人空间背景图片
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="Bardian"></param>
        /// <returns></returns>
        UserView UpdateUserBannerImage(int userId, string bannerImage);

        /// <summary>
        /// 获取用户空间的用户信息
        /// </summary>
        /// <param name="loginUserId"></param>
        /// <param name="browserUserId"></param>
        /// <returns></returns>
        UserView GetUserRoomInfo(int loginUserId, int browserUserId);

        bool SaveSkinByUserId(int userId, int skinId);

        bool DelAllRecByUserId(int uid);

        /// <summary>
        /// 获取当前用户观看视频的记录
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="videoId">视频ID</param>
        /// <returns>已观看视频的时间，以秒为单位</returns>
        long GetUserWatchTime(int uid, int videoId);

        #region 第三方绑定、登录,注册、绑定，自动注册、绑定（跳过）处理
        /// <summary>
        /// 第三方绑定、登录处理
        /// </summary>
        /// <param name="account">用户账户</param>
        /// <param name="password">用户密码</param>
        /// <param name="typeCode">第三方类型编码</param>
        /// <param name="relatedId">第三方唯一身份标识</param>
        /// <param name="nickName">昵称</param>
        /// <param name="figureURL">头像路径</param>
        /// <returns></returns>
        UserView ThirdPartyBindAndLogin(string account, string password, string typeCode, string relatedId, string nickName, string figureURL);

        /// <summary>
        /// 第三方注册、绑定处理
        /// </summary>
        /// <param name="account">用户账户</param>
        /// <param name="password">用户密码</param>
        /// <param name="typeCode">第三方类型编码</param>
        /// <param name="relatedId">第三方唯一身份标识</param>
        /// <param name="nickName">昵称</param>
        /// <param name="figureURL">头像路径</param>
        /// <param name="type">手机、邮箱类型</param>
        /// <returns></returns>
        UserView ThirdPartyBindAndRegister(string account, string password, string typeCode, string relatedId, string nickName, string figureURL, int type);
        /// <summary>
        /// 自动注册并绑定第三方
        /// </summary>
        /// <param name="typeCode">第三方类型编码</param>
        /// <param name="relatedId">第三方唯一身份标识</param>
        /// <param name="nickName">昵称</param>
        /// <param name="figureURL">头像路径</param>
        /// <returns></returns>
        UserView AutoRegisterAndBindThirdParty(string typeCode, string relatedId, string nickName, string figureURL);

        #endregion



    }
}
