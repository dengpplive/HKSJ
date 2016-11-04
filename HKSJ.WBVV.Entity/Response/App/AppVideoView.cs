using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Resource;
using HKSJ.WBVV.Entity.Document;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Entity.Response.App
{
    /// <summary>
    /// 视频信息视图
    /// </summary>
    public class AppVideoView : IDocument
    {
        #region Fields

        /// <summary>
        /// Local veriable used to combine Small Pic Path the a video.
        /// </summary>
        string _smallPicPath;

        /// <summary>
        /// Local veriable used to combine Big Pic Path the a video.
        /// </summary>
        string _bigPicPath;

        /// <summary>
        /// Local veriable used to combine Banner Image Path the a video.
        /// </summary>
        string _bannerImgPath;

        private string _videoPath;
        #endregion
        /// <summary>
        /// 视频编号
        /// </summary>
        [Display(Name = "视频编号")]
        public int Id { get; set; }
        /// <summary>
        /// 视频名称
        /// </summary>
        [Display(Name = "视频名称")]
        public string Title { get; set; }
        /// <summary>
        /// 视频简介
        /// </summary>
        [Display(Name = "视频简介")]
        public string About { get; set; }
        /// <summary>
        /// 视频被观看次数
        /// </summary>
        /// <returns></returns>
        [Display(Name = "视频被观看次数")]
        public int PlayCount { get; set; }
        /// <summary>
        /// 被喜欢数
        /// </summary>
        [Display(Name = "被喜欢数")]
        public int CollectionCount { get; set; }
        /// <summary>
        /// 被评论数
        /// </summary>
        [Display(Name = "被评论数")]
        public int CommentCount { get; set; }
        /// <summary>
        /// 视频小图片
        /// </summary>
        [Display(Name = "视频小图片")]
        public string SmallPicturePath
        {
            get
            {
                return UrlHelper.QiniuPublicCombine(_smallPicPath);
            }

            set
            {
                _smallPicPath = value;
            }
        }
        /// <summary>
        /// 视频大图片
        /// </summary>
        [Display(Name = "视频大图片")]
        public string BigPicturePath
        {
            get
            {
                return UrlHelper.QiniuPublicCombine(_bigPicPath);
            }

            set
            {
                _bigPicPath = value;
            }
        }
        /// <summary>
        /// 视频播放路径
        /// </summary>
        [Display(Name = "视频播放路径")]
        public string VideoPath
        {
            get
            {
                return UrlHelper.QiniuPublicCombine(_videoPath);
            }

            set
            {
                _videoPath = value;
            }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public string CreateTimeStr
        {
            get
            {
                return CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        public string UpdateTimeStr
        {
            get
            {
                return UpdateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        /// <summary>
        /// 视频时间长度(秒)
        /// </summary>
        [Display(Name = "视频时间长度(秒)")]
        public int TimeLength { get; set; }

        /// <summary>
        /// 视频上传时间
        /// </summary>
        [Display(Name = "视频上传时间")]
        public string TimeSpan
        {
            get
            {
                if (UpdateTime.IsDbNull() || UpdateTime == DateTime.MinValue)
                {
                    UpdateTime = CreateTime;
                }
                DateTime startTime = UpdateTime;
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
                    //                    int month = (int)totalDays / curMonths[endTime.Month - 1];
                    //                    if (month < 12)
                    //                    {
                    //                        return message.F(month + "月");
                    //                    }
                    //                    else
                    //                    {
                    //                        int year = month / 12;
                    //                        return message.F(year + "年");
                    //                    }
                }
            }
        }

        /// <summary>
        /// 下載地址
        /// </summary>
        [Display(Name = "下載地址")]
        public string DownloadPath { get; set; }
        /// <summary>
        /// 上传者
        /// </summary>
        [Display(Name = "上传者")]
        public AppUserSimpleView UserInfo { get; set; }

        public object GetSampleObject()
        {
            var view = new ResponsePackage<AppVideoView>
            {
                Data = new AppVideoView
                {
                    Id = 888,
                    Title =LanguageUtil.Translate("Entity_Response_App_GetSampleObject_Title"),//"视频名称",
                    About = LanguageUtil.Translate("Entity_Response_App_GetSampleObject_About"),//"视频简介",
                    PlayCount = 888,
                    CollectionCount = 8,
                    CommentCount = 88,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    TimeLength = 100,
                    UserInfo = new AppUserSimpleView
                    {
                        Id = 888,
                        NickName = "AxOne",
                        Picture = "http://xx/xx/xx"
                    }
                },
                ExtensionData = new ResponseExtensionData
                {
                    CallResult = CallResult.Success,
                    RetMsg = LanguageUtil.Translate("Entity_Response_App_GetSampleObject_RequestSuccess"),//"请求成功",
                    ModelValidateErrors = new List<ModelValidateError>()
                }
            };
            return view;
        }
    }

    /// <summary>
    /// 简单视频视图
    /// </summary>
    public class AppVideoSimpleView : IDocument
    {
        /// <summary>
        /// 视频编号
        /// </summary>
        [Display(Name = "视频编号")]
        public int Id { get; set; }
        /// <summary>
        /// 视频名称
        /// </summary>
        [Display(Name = "视频名称")]
        public string Title { get; set; }

        public object GetSampleObject()
        {
            return new ResponsePackage<AppVideoSimpleView>
            {
                Data = new AppVideoSimpleView
                {
                    Id = 888,
                    Title = LanguageUtil.Translate("Entity_Response_App_GetSampleObject_Title")//"视频名称"
                },
                ExtensionData = new ResponseExtensionData { CallResult = CallResult.Success }
            };
        }
    }
}
