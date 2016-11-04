using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Entity.Document;
using HKSJ.WBVV.Entity.Response;
using HKSJ.WBVV.Entity.ViewModel.Client;

namespace HKSJ.WBVV.Entity.ApiModel
{
    /// <summary>
    /// 客户端响应视图
    /// </summary>
    public class Result
    {
        /// <summary>
        /// api返回用户信息
        /// </summary>
        public Result()
        {
            this.Success = true;
        }
        public bool Success { get; set; }
        public object Data { get; set; }

        public string ExceptionMessage { get; set; }
    }

    public class SMSResult : IDocument
    {
        /// <summary>
        /// 状态码
        /// </summary>
        [DisplayName("状态码")]
        public int Code { get; set; }
        /// <summary>
        /// 响应信息
        /// </summary>
        [DisplayName("响应信息")]
        public string Message { get; set; }

        object IDocument.GetSampleObject()
        {
            var view = new ResponsePackage<SMSResult>
            {
                Data = new SMSResult
                {
                    Code = 1,
                    Message = "提交成功"
                },
                ExtensionData = new ResponseExtensionData
                {
                    CallResult = CallResult.Success, RetMsg = "请求成功", ModelValidateErrors = new List<ModelValidateError>()
                }
            };
            return view;
        }
    }
}
