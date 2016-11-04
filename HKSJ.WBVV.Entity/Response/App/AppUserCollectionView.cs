using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity.Document;

namespace HKSJ.WBVV.Entity.Response.App
{
    /// <summary>
    /// 喜欢视频列表视图
    /// </summary>
    public class AppUserCollectionsView : IDocument
    {
        /// <summary>
        /// 喜欢视频的总行数
        /// </summary>
         [Display(Name = "喜欢视频的总行数")]
        public int TotalCount { get; set; }
        /// <summary>
        /// 喜欢视频的列表
        /// </summary>
           [Display(Name = "喜欢视频的列表")]
        public IList<AppUserCollectionView> UserCollections { get; set; }
        public object GetSampleObject()
        {
            return new ResponsePackage<AppUserCollectionsView>
            {
                Data = new AppUserCollectionsView
                {
                    TotalCount = 999,
                    UserCollections = new List<AppUserCollectionView>()
                    {
                        new AppUserCollectionView()
                        {
                            Id = 888,
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
                                TimeLength = 100
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
    /// 喜欢视频视图
    /// </summary>
    public class AppUserCollectionView : IDocument
    {
        /// <summary>
        /// 喜欢的编号
        /// </summary>
        [Display(Name="喜欢的编号")]
        public int Id { get; set; }
        /// <summary>
        /// 喜欢的用户信息
        /// </summary>
        [Display(Name="喜欢的用户信息")]
        public AppUserSimpleView UserInfo { get; set; }
        /// <summary>
        /// 喜欢的视频信息
        /// </summary>
        [Display(Name="喜欢的视频信息")]
        public AppVideoView VideoInfo { get; set; }
        /// <summary>
        /// 视频喜欢的时间
        /// </summary>
        [Display(Name="喜欢视频的时间")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 喜欢视频的时间
        /// </summary>
        [Display(Name = "喜欢视频的时间")]
        public string CreateTimeStr
        {
            get
            {
                return CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        /// <summary>
        /// 视频喜欢的时间
        /// </summary>
        [Display(Name="喜欢视频的时间")]
        public string TimeSpan
        {
            get
            {
                DateTime startTime = CreateTime;
                DateTime endTime = DateTime.Now;
                TimeSpan ts = endTime - startTime;
                var rMonths = new int[] { 30, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
                var pMonths = new int[] { 30, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
                var curMonths = endTime.Year % 4 == 0 ? rMonths : pMonths;
                int totalseconds = (int)ts.TotalSeconds;
                int totalMinutes = (int)ts.TotalMinutes;
                int totalHours = (int)ts.TotalHours;
                int totalDays = (int)ts.TotalDays;

                string message = "发布于" + "{0}前";
                if (totalseconds < 60)
                {
                    return message.F((int)totalseconds + "秒");
                }
                else if (totalseconds >= 60 && totalMinutes < 60)
                {
                    return message.F((int)totalMinutes + "分");
                }
                else if (totalMinutes >= 60 && totalHours < 24)
                {
                    return message.F((int)totalHours + "小时");
                }
                else if (totalHours >= 24 && totalDays < 7)
                {
                    return message.F((int)totalDays + "天");
                }
                else if (totalDays >= 7 && totalDays < 14)
                {
                    return message.F("一周");
                }
                else if (totalDays >= 14 && totalDays < 21)
                {
                    return message.F("两周");
                }
                else if (totalDays >= 21 && totalDays < curMonths[endTime.Month - 1])
                {
                    return message.F("三周");
                }
                else if (totalDays >= curMonths[endTime.Month - 1] && totalDays < curMonths[endTime.Month - 1] + curMonths[endTime.Month - 2])
                {
                    return message.F("一个月");
                }
                else
                {
                    return startTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
        }

        public object GetSampleObject()
        {
            return new ResponsePackage<AppUserCollectionView>
            {
                Data = new AppUserCollectionView
                {
                    Id = 888,
                    CreateTime = DateTime.Now,
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
                        TimeLength = 100
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
