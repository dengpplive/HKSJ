using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Resource;
using HKSJ.WBVV.Entity.Document;
using HKSJ.WBVV.Entity.Response;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity.Response.App;

namespace HKSJ.WBVV.Entity.Response.App
{
    /// <summary>
    /// 视频评论视图
    /// </summary>
    public class AppComments : IDocument
    {
        /// <summary>
        /// 发表视频评论的总行数
        /// </summary>
        [Display(Name = "发表视频评论的总行数")]
        public int TotalCount { get; set; }

        /// <summary>
        /// 视频评论列表
        /// </summary>
        [Display(Name = "视频评论列表")]
        public IList<AppCommentsView> Comments { get; set; }

        public object GetSampleObject()
        {
            return new ResponsePackage<AppComments>
            {
                Data = new AppComments
                {
                    TotalCount = 888,
                    Comments = new List<AppCommentsView>()
                    {
                        new AppCommentsView()
                        {
                            ParentComment = new AppCommentView()
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
                            },
                            ChildComments = new List<AppCommentView>()
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
                ExtensionData = new ResponseExtensionData {CallResult = CallResult.Success}
            };
        }
    }

    public class AppCommentsView : IDocument
    {
        /// <summary>
        /// 上级评论
        /// </summary>
        [Display(Name = "上级评论")]
        public AppCommentView ParentComment { get; set; }

        /// <summary>
        /// 下级评论
        /// </summary>
        [Display(Name = "下级评论")]
        public IList<AppCommentView> ChildComments { get; set; }


        public object GetSampleObject()
        {
            var view = new ResponsePackage<AppCommentsView>
            {
                Data = new AppCommentsView
                {
                    ParentComment=new AppCommentView()
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
                    },
                    ChildComments = new List<AppCommentView>()
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
            return view;
        }
    }

    /// <summary>
    /// 评论视图
    /// </summary>
    [Serializable]
    public class AppCommentView : IDocument
    {
        /// <summary>
        /// 评论编号
        /// </summary>
        [Display(Name = "评论编号")]
        public int Id { get; set; }
        /// <summary>
        /// 发送消息的用户信息
        /// </summary>
        [Display(Name = "发送消息的用户信息")]
        public AppUserSimpleView FromUser { get; set; }

        /// <summary>
        /// 接受消息的用户信息
        /// </summary>
        [Display(Name = "接受消息的用户信息")]
        public AppUserSimpleView ToUser { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        [Display(Name = "评论内容")]
        public string Content { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        [Display(Name = "位置")]
        public string LocalPath { get; set; }
        /// <summary>
        /// 评论时间
        /// </summary>
        [Display(Name = "评论时间")]
        public string CreateTime { get; set; }
        public object GetSampleObject()
        {
            var view = new ResponsePackage<AppCommentView>
            {
                Data = new AppCommentView
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

