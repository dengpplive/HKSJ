using System.ComponentModel.DataAnnotations;

namespace HKSJ.WBVV.Entity.Response
{
    /// <summary>
    /// 响应结果包装
    /// Author:AxOne
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponsePackage<T>
    {
        [Display(Name = "数据主体信息")]
        public T Data { get; set; }
        [Display(Name = "安全校验，响应状态等信息")]
        public ResponseExtensionData ExtensionData { get; set; }
    }
}
