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
    /// 上传获取Token视图
    /// </summary>
    public class AppUploadTokenView : IDocument
    {
        /// <summary>
        /// 上传的Token
        /// </summary>
        [Display(Name = "上传的Token")]
        public string Token { get; set; }
        /// <summary>
        /// 上传文件的key
        /// </summary>
        [Display(Name = "上传文件的key")]
        public string Key { get; set; }

        public object GetSampleObject()
        {
            return new ResponsePackage<AppUploadTokenView>
            {
                Data = new AppUploadTokenView
                {
                    Token = "ewrwerwerwerwerw",
                    Key = "dsfsdfsdfds"
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
