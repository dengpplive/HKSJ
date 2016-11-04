using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Common.Resource;
using HKSJ.WBVV.Entity.Document;

namespace HKSJ.WBVV.Entity.Response.App
{
    /// <summary>
    /// 用户简单视图
    /// </summary>
   public  class AppUserSimpleView:IDocument
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

        public object GetSampleObject()
        {
            var view = new ResponsePackage<AppUserSimpleView>
            {
                Data = new AppUserSimpleView
                {
                    Id = 888,
                    NickName = "AxOne",
                    Picture = "http://xx/xx/xx"
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
