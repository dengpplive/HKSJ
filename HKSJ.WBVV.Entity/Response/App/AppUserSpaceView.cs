using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Entity.Document;

namespace HKSJ.WBVV.Entity.Response.App
{
    /// <summary>
    /// 用户空间视图
    /// </summary>
    public class AppUserSpaceView : IDocument
    {
        /// <summary>
        /// 空间用户信息
        /// </summary>
        [Display(Name = "空间用户信息")]
        public AppUserView UserInfo { get; set; }
        /// <summary>
        /// 空间上传视频
        /// </summary>
        [Display(Name = "空间上传视频")]
        public IList<AppChoicenessView> VideoInfos { get; set; }
        public object GetSampleObject()
        {
            return new ResponsePackage<AppUserSpaceView>
            {
                Data = new AppUserSpaceView
                {
                    UserInfo = new AppUserView()
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
                    VideoInfos =new List<AppChoicenessView>()
                    {
                        new AppChoicenessView()
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
