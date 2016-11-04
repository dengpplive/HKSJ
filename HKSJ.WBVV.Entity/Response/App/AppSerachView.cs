using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Entity.Document;

namespace HKSJ.WBVV.Entity.Response.App
{
    /// <summary>
    /// 搜索结果视图
    /// </summary>
    public class AppSerachView : IDocument
    {
        /// <summary>
        /// 搜索用户列表
        /// </summary>
        [Display(Name = "搜索用户列表")]
        public IList<AppUserSimpleView> UserInfos { get; set; }
        /// <summary>
        /// 搜索视频列表
        /// </summary>
        [Display(Name = "搜索视频列表")]
        public IList<AppVideoView> VideoInfos { get; set; }
        public object GetSampleObject()
        {
            return new ResponsePackage<AppSerachView>
            {
                Data = new AppSerachView
                {
                    UserInfos = new List<AppUserSimpleView>()
                    {
                        new AppUserSimpleView()
                        {
                            Id = 888,
                            NickName = "AxOne",
                            Picture = "http://xx/xx/xx"
                        }
                    },
                    VideoInfos = new List<AppVideoView>()
                    {
                        new AppVideoView()
                        {
                            Id = 888,
                            Title = "视频名称",
                            About = "视频简介",
                            PlayCount = 888,
                            CollectionCount = 8,
                            CommentCount = 88,
                            CreateTime = DateTime.Now,
                            UpdateTime = DateTime.Now,
                            TimeLength = 100
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
        }
    }
    /// <summary>
    /// 搜索用户详细视图
    /// </summary>
    public class AppSerachUserView : IDocument
    {
        /// <summary>
        /// 搜索用户结果数量
        /// </summary>
        [Display(Name = "搜索用户结果数量")]
        public int TotalCount { get; set; }
        /// <summary>
        /// 搜索用户结果列表
        /// </summary>
        [Display(Name = "搜索用户结果列表")]
        public IList<AppUserView> UserInfos { get; set; }
        public object GetSampleObject()
        {
            return new ResponsePackage<AppSerachUserView>
            {
                Data = new AppSerachUserView
                {
                    TotalCount = 888,
                    UserInfos = new List<AppUserView>()
                    {
                        new AppUserView()
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
        }
    }
    /// <summary>
    /// 搜索用户详细视图
    /// </summary>
    public class AppSerachVideoView : IDocument
    {
        /// <summary>
        /// 搜索视频结果数量
        /// </summary>
        [Display(Name = "搜索视频结果数量")]
        public int TotalCount { get; set; }
        /// <summary>
        /// 搜索用户结果列表
        /// </summary>
        [Display(Name = "搜索视频结果列表")]
        public IList<AppVideoView> VideoInfos { get; set; }
        public object GetSampleObject()
        {
            return new ResponsePackage<AppSerachVideoView>
            {
                Data = new AppSerachVideoView
                {
                    TotalCount = 888,
                    VideoInfos = new List<AppVideoView>()
                    {
                      new AppVideoView()
                        {
                            Id = 888,
                            Title = "视频名称",
                            About = "视频简介",
                            PlayCount = 888,
                            CollectionCount = 8,
                            CommentCount = 88,
                            CreateTime = DateTime.Now,
                            UpdateTime = DateTime.Now,
                            TimeLength = 100
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
        }
    }
}
