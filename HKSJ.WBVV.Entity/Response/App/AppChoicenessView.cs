using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HKSJ.WBVV.Entity.Document;
using HKSJ.WBVV.Entity.ViewModel.Client;

namespace HKSJ.WBVV.Entity.Response.App
{
    public class AppChoicenesssView : IDocument
    {
        /// <summary>
        /// 精选视频列表
        /// </summary>
        [Display(Name = "精选视频列表")]
        public IList<AppChoicenessView> Choiceness { get; set; }
        public object GetSampleObject()
        {
            return new ResponsePackage<AppChoicenesssView>
            {
                Data = new AppChoicenesssView()
                {
                    Choiceness = new List<AppChoicenessView>()
                    {
                            new AppChoicenessView()
                            {
                                IsSubed = false,
                                IsCollect = false,
                                UserInfo = new AppUserSimpleView()
                                {
                                    Id = 888,
                                    NickName = "AxOne",
                                    Picture = "http://xx/xx/xx",
                                },
                                VideoInfo = new AppVideoView()
                                {
                                    Id = 888,
                                    Title = "视频名称",
                                    About = "视频简介",
                                    PlayCount = 888,
                                    CollectionCount = 8,
                                    CommentCount = 88,
                                    CreateTime = DateTime.Now,
                                    UpdateTime = DateTime.Now,
                                    TimeLength = 100,
                                },
                                Comments = new List<AppCommentView>()
                          {
                                new AppCommentView()
                                {
                                    Id = 888,
                                    Content = "呵呵",
                                    CreateTime = "发表11分钟前",
                                    FromUser = new AppUserSimpleView()
                                    {
                                        Id = 888,
                                        NickName = "AxOne",
                                        Picture = "http://xx/xx/xx",
                                    },
                                    ToUser = new AppUserSimpleView()
                                    {
                                        Id = 888,
                                        NickName = "AxOne",
                                        Picture = "http://xx/xx/xx",
                                    }
                                }
                             }
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
    /// 精选视图
    /// </summary>
    public class AppChoicenessView : IDocument
    {
        /// <summary>
        /// 是否关注
        /// </summary>
        [Display(Name = "是否关注")]
        public bool IsSubed { get; set; }
        /// <summary>
        /// 是否收藏
        /// </summary>
        [Display(Name = "是否收藏")]
        public bool IsCollect { get; set; }
        /// <summary>
        /// 用户信息
        /// </summary>
        [Display(Name = "用户信息")]
        public AppUserSimpleView UserInfo { get; set; }
        /// <summary>
        /// 视频信息
        /// </summary>
        [Display(Name = "视频信息")]
        public AppVideoView VideoInfo { get; set; }
        /// <summary>
        /// 视频评论列表
        /// </summary>
        [Display(Name = "视频评论列表")]
        public IList<AppCommentView> Comments { get; set; }
        public object GetSampleObject()
        {
            return new ResponsePackage<AppChoicenessView>
            {
                Data = new AppChoicenessView()
                {
                    IsSubed = false,
                    IsCollect = false,
                    UserInfo = new AppUserSimpleView()
                    {
                        Id = 888,
                        NickName = "AxOne",
                        Picture = "http://xx/xx/xx"
                    },
                    VideoInfo = new AppVideoView()
                    {
                        Id = 888,
                        Title = "视频名称",
                        About = "视频简介",
                        PlayCount = 888,
                        CollectionCount = 8,
                        CommentCount = 88,
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now,
                        TimeLength = 100,
                    },
                    Comments = new List<AppCommentView>()
                  {
                      new AppCommentView()
                      {
                         Id = 888,
                            Content = "呵呵",
                            CreateTime = "发表11分钟前",
                            FromUser = new AppUserSimpleView()
                            {
                                Id = 888,
                                NickName = "AxOne",
                                Picture = "http://xx/xx/xx",
                            },
                            ToUser = new AppUserSimpleView()
                            {
                                Id = 888,
                                NickName = "AxOne",
                                Picture = "http://xx/xx/xx",
                            }
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
