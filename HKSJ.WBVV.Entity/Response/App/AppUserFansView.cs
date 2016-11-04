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
    /// 粉丝列表视图
    /// </summary>
    public class AppUserFanssView : IDocument
    {
        /// <summary>
        /// 粉丝总行数
        /// </summary>
        [Display(Name = "打赏总行数")]
        public int TotalCount { get; set; }
        /// <summary>
        /// 粉丝列表
        /// </summary>
        [Display(Name = "粉丝列表")]
        public IList<AppUserFansView> UserFanss { get; set; }
        public object GetSampleObject()
        {
            return new ResponsePackage<AppUserFanssView>
            {
                Data = new AppUserFanssView
                {
                    TotalCount = 999,
                    UserFanss = new List<AppUserFansView>()
                    {
                      new AppUserFansView()
                      {
                           Id = 888,
                    UploadCount=100,
                    IsSubed = false,
                    SubscribeUser = new AppUserSimpleView()
                    {
                        Id = 888,
                        NickName = "AxOne",
                        Picture = "http://xx/xx/xx",
                    },
                    UserInfo = new AppUserSimpleView()
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

    /// <summary>
    /// 粉丝视图
    /// </summary>
    public class AppUserFansView : IDocument
    {
        /// <summary>
        /// 粉丝编号
        /// </summary>
        [Display(Name = "粉丝编号")]
        public int Id { get; set; }
        /// <summary>
        /// 最近上传视频数量
        /// </summary>
        [Display(Name = "最近上传视频数量")]
        public int UploadCount { get; set; }
        /// <summary>
        /// 是否互相关注
        /// </summary>
        [Display(Name = "是否互相关注")]
        public bool IsSubed { get; set; }
        /// <summary>
        /// 被关注的用户
        /// </summary>
        [Display(Name = "被关注的用户")]
        public AppUserSimpleView SubscribeUser { get; set; }
        /// <summary>
        /// 关注的用户
        /// </summary>
        [Display(Name = "关注的用户")]
        public AppUserSimpleView UserInfo { get; set; }
        public object GetSampleObject()
        {
            return new ResponsePackage<AppUserFansView>
            {
                Data = new AppUserFansView
                {
                    Id = 888,
                    UploadCount=100,
                    IsSubed = false,
                    SubscribeUser = new AppUserSimpleView()
                    {
                        Id = 888,
                        NickName = "AxOne",
                        Picture = "http://xx/xx/xx",
                    },
                    UserInfo = new AppUserSimpleView()
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
        }
    }
}
