using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using HKSJ.WBVV.Entity.Document;
using UrlHelper = HKSJ.WBVV.Common.Resource.UrlHelper;

namespace HKSJ.WBVV.Entity.Response.App
{
    public class AppUsersView : IDocument
    {
        /// <summary>
        /// 用户列表
        /// </summary>
        [Display(Name = "用户列表")]
        public IList<AppUserView> UserInfos { get; set; }

        public object GetSampleObject()
        {
            var view = new ResponsePackage<AppUsersView>
            {
                Data = new AppUsersView()
                {
                    UserInfos = new List<AppUserView>()
                    {
                        new AppUserView
                        {
                            Id = 888,
                            Account = "18812345678",
                            Pwd = "12345678",
                            NickName = "AxOne",
                            PlayCount = 888,
                            FansCount = 888,
                            UploadCount = 888,
                            CommentCount = 55,
                            SkinId = 8,
                            Picture = "http://xx/xx/xx",
                            BannerImage = "http://xxx/xxx/xxx",
                            Bardian = "AxOne",
                            Phone = "18812345678",
                            SubscribeNum = 88,
                            Level = 0,
                            State = false,
                            IsSubed = false,
                            Token = "ASDSDGHFYHWEFAFDFGDFT"
                        }
                    }
                },
                ExtensionData = new ResponseExtensionData
                {
                    CallResult = CallResult.Success,
                    RetMsg = "请求成功",
                    ModelValidateErrors = new List<ModelValidateError>()
                }
            };
            return view;
        }
    }

    /// <summary>
    /// APP登录返回实体
    /// Author:AxOne
    /// </summary>
    public class AppUserView : IDocument
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        [Display(Name = "用户编号")]
        public int Id { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        [Display(Name = "用户昵称")]
        public string NickName { get; set; }

        private string _picture = string.Empty;
        /// <summary>
        /// 用户头像
        /// </summary>
        [Display(Name = "用户头像")]
        public string Picture
        {
            get
            {
                if (_picture == null || _picture.IndexOf("Content/images", StringComparison.Ordinal) > -1)
                {
                    return UrlHelper.Combine(ConfigurationManager.AppSettings["WebServerUrl"], _picture);
                }
                return UrlHelper.QiniuPublicCombine(_picture);
            }
            set { _picture = value; }
        }

        /// <summary>
        /// 用户帐号
        /// </summary>
        [Display(Name = "用户帐号")]
        public string Account { get; set; }

        /// <summary>
        /// 用户加密后的密码
        /// </summary>
        [Display(Name = "用户加密后的密码")]
        public string Pwd { get; set; }
        /// <summary>
        /// 播放次数
        /// </summary>
        [Display(Name = "播放次数")]
        public int PlayCount { get; set; }

        /// <summary>
        /// 粉丝数量
        /// </summary>
        [Display(Name = "粉丝数量")]
        public int FansCount { get; set; }
        public int Type { get; set; }
        /// <summary>
        /// 空间留言数量
        /// </summary>
        [Display(Name = "空间留言数量")]
        public int CommentCount { get; set; }

        /// <summary>
        /// 上传视频数量
        /// </summary>
        [Display(Name = "上传视频数量")]
        public int UploadCount { get; set; }

        /// <summary>
        /// 关联的皮肤编号
        /// </summary>
        [Display(Name = "关联的皮肤编号")]
        public int SkinId { get; set; }

        private string _bannerImage = string.Empty;

        /// <summary>
        /// 空间背景图片
        /// </summary>
        /// <returns></returns>
        [Display(Name = "空间背景图片")]
        public string BannerImage
        {
            get { return UrlHelper.QiniuPublicCombine(_bannerImage); }
            set { _bannerImage = value; }
        }

        /// <summary>
        /// 个性签名
        /// </summary>
        [Display(Name = "个性签名")]
        public string Bardian { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [Display(Name = "手机号码")]
        public string Phone { get; set; }

        /// <summary>
        /// 用户被订阅数
        /// </summary>
        [Display(Name = "用户被订阅数")]
        public int SubscribeNum { get; set; }

        /// <summary>
        /// 用户等级
        /// </summary>
        /// <returns></returns>
        [Display(Name = "用户等级")]
        public Int16 Level { get; set; }

        /// <summary>
        /// 用户状态（0:启用1:禁用）
        /// </summary>
        [Display(Name = "用户状态（0:启用1:禁用）")]
        public bool State { get; set; }

        /// <summary>
        /// 该用户是否被登录的用户订阅
        /// </summary>
        [Display(Name = "该用户是否被登录的用户订阅")]
        public bool IsSubed { get; set; }

        /// <summary>
        /// Token信息
        /// </summary>
        [Display(Name = "Token信息")]
        public string Token { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        /// <returns></returns>
        [Display(Name = "生日")]
        public string Birthdate { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        /// <returns></returns>
        [Display(Name = "城市")]
        public string City { get; set; }

        /// <summary>
        /// 性别(1:男0:女)
        /// </summary>
        /// <returns></returns>
        [Display(Name = "性别(1:男 0:女)")]
        public bool Gender { get; set; }

        object IDocument.GetSampleObject()
        {
            var view = new ResponsePackage<AppUserView>
            {
                Data = new AppUserView
                {
                    Id = 888,
                    Account = "18812345678",
                    Pwd = "12345678",
                    NickName = "AxOne",
                    PlayCount = 888,
                    FansCount = 888,
                    UploadCount = 888,
                    CommentCount = 55,
                    SkinId = 8,
                    Picture = "http://xx/xx/xx",
                    BannerImage = "http://xxx/xxx/xxx",
                    Bardian = "AxOne",
                    Phone = "18812345678",
                    SubscribeNum = 88,
                    Level = 0,
                    State = false,
                    IsSubed = false,
                    Token = "ASDSDGHFYHWEFAFDFGDFT"
                },
                ExtensionData = new ResponseExtensionData
                {
                    CallResult = CallResult.Success,
                    RetMsg = "请求成功",
                    ModelValidateErrors = new List<ModelValidateError>()
                }
            };
            return view;
        }
    }
}
