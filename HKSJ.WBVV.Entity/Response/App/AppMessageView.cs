using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity.Document;
using HKSJ.WBVV.Entity.Enums;

namespace HKSJ.WBVV.Entity.Response.App
{
    /// <summary>
    /// 消息视图
    /// </summary>
    public class AppMessageView : IDocument
    {
        /// <summary>
        /// 未读评论数量
        /// </summary>
        [Display(Name = "未读评论数量")]
        public int UnreadCommentCount { get; set; }
        /// <summary>
        /// 未读喜欢数量
        /// </summary>
        [Display(Name = "未读喜欢数量")]
        public int UnreadCollectionCount { get; set; }
        /// <summary>
        /// 系统消息列表
        /// </summary>
        [Display(Name = "系统消息列表")]
        public IList<AppSystemMessageView> SystemMessages { get; set; }
        public object GetSampleObject()
        {
            return new ResponsePackage<AppMessageView>
            {
                Data = new AppMessageView
                {
                    UnreadCommentCount = 99,
                    UnreadCollectionCount = 99,
                    SystemMessages = new List<AppSystemMessageView>()
                   {
                       new AppSystemMessageView()
                       {
                               Id = 888,
                                Content = "消息内容",
                                CreateTime ="一个月前"
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
    /// 系统消息视图
    /// </summary>
    public class AppSystemMessageView : IDocument
    {
        /// <summary>
        /// 消息编号
        /// </summary>
        [Display(Name = "消息编号")]
        public int Id { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        [Display(Name = "消息内容")]
        public string Content { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public string CreateTime { get; set; }

        public object GetSampleObject()
        {
            return new ResponsePackage<AppSystemMessageView>
            {
                Data = new AppSystemMessageView
                {
                    Id = 888,
                    Content = "消息内容",
                    CreateTime = "一个月前"
                },
                ExtensionData = new ResponseExtensionData { CallResult = CallResult.Success }
            };
        }
    }
    /// <summary>
    /// 我的消息--评论列表
    /// </summary>
    public class AppVideoCommentsView : IDocument
    {
        /// <summary>
        /// 评论的总条数
        /// </summary>
        [Display(Name = "评论的总条数")]
        public int TotalCount { get; set; }
        /// <summary>
        /// 评论列表
        /// </summary>
        [Display(Name = "评论列表")]
        public IList<AppVideoCommentView> Comments { get; set; }

        public object GetSampleObject()
        {
            return new ResponsePackage<AppVideoCommentsView>
            {
                Data = new AppVideoCommentsView
                {
                    TotalCount = 99,
                    Comments = new List<AppVideoCommentView>()
                    {
                        new AppVideoCommentView()
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
                ExtensionData = new ResponseExtensionData { CallResult = CallResult.Success }
            };
        }
    }

    /// <summary>
    /// 我的消息--评论视图
    /// </summary>
    public class AppVideoCommentView : IDocument
    {
        /// <summary>
        /// 消息编号
        /// </summary>
        [Display(Name = "消息编号")]
        public int Id { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        [Display(Name = "消息内容")]
        public string Content { get; set; }
        /// <summary>
        /// 发消息的人
        /// </summary>
        [Display(Name = "发消息的人")]
        public AppUserSimpleView FromUser { get; set; }
        /// <summary>
        /// 接消息的人
        /// </summary>
        [Display(Name = "接消息的人")]
        public AppUserSimpleView ToUser { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public string CreateTime { get; set; }
        /// <summary>
        /// 是否已读
        /// </summary>
        [Display(Name = "是否已读")]
        public bool IsRead { get; set; }
        public object GetSampleObject()
        {
            return new ResponsePackage<AppVideoCommentView>
            {
                Data = new AppVideoCommentView
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
                ExtensionData = new ResponseExtensionData { CallResult = CallResult.Success }
            };
        }
    }

    /// <summary>
    /// 我的消息--喜欢列表
    /// </summary>
    public class AppUserCollectsView : IDocument
    {
        /// <summary>
        /// 评论的总条数
        /// </summary>
        [Display(Name = "评论的总条数")]
        public int TotalCount { get; set; }
        /// <summary>
        /// 评论列表
        /// </summary>
        [Display(Name = "评论列表")]
        public IList<AppUserCollectView> Comments { get; set; }

        public object GetSampleObject()
        {
            return new ResponsePackage<AppUserCollectsView>
            {
                Data = new AppUserCollectsView
                {
                    TotalCount = 99,
                    Comments = new List<AppUserCollectView>()
                    {
                        new AppUserCollectView()
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
                            Video = new AppVideoSimpleView()
                            {
                                Id = 888,
                                Title = "AxOne"
                            }
                        }
                    }
                },
                ExtensionData = new ResponseExtensionData { CallResult = CallResult.Success }
            };
        }
    }

    /// <summary>
    /// 我的消息--喜欢视图
    /// </summary>
    public class AppUserCollectView : IDocument
    {
        /// <summary>
        /// 消息编号
        /// </summary>
        [Display(Name = "消息编号")]
        public int Id { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        [Display(Name = "消息内容")]
        public string Content { get; set; }
        /// <summary>
        /// 发消息的人
        /// </summary>
        [Display(Name = "发消息的人")]
        public AppUserSimpleView FromUser { get; set; }
        /// <summary>
        /// 视频信息
        /// </summary>
        [Display(Name = "视频信息")]
        public AppVideoSimpleView Video { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public string CreateTime { get; set; }
        /// <summary>
        /// 是否已读
        /// </summary>
        [Display(Name = "是否已读")]
        public bool IsRead { get; set; }
        public object GetSampleObject()
        {
            return new ResponsePackage<AppUserCollectView>
            {
                Data = new AppUserCollectView
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
                    Video = new AppVideoSimpleView()
                    {
                        Id = 888,
                        Title = "AxOne"
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
