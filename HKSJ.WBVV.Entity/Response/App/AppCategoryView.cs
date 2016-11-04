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
    /// 分类列表视图
    /// </summary>
    public class AppCategorysView : IDocument
    {
        /// <summary>
        /// 分类列表
        /// </summary>
        [Display(Name = "分类列表")]
        public IList<AppCategoryView> Categorys { get; set; }
        public object GetSampleObject()
        {
            return new ResponsePackage<AppCategorysView>
            {
                Data = new AppCategorysView
                {
                    Categorys = new List<AppCategoryView>()
                    {
                       new AppCategoryView()
                       {
                              Id = 1,
                              Name = "搞笑"
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
    /// 分类视图
    /// </summary>
    public class AppCategoryView : IDocument
    {
        /// <summary>
        /// 分类编号
        /// </summary>
        [Display(Name = "分类编号")]
        public int Id { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        [Display(Name = "分类名称")]
        public string Name { get; set; }

        public object GetSampleObject()
        {
            return new ResponsePackage<AppCategoryView>
            {
                Data = new AppCategoryView
                {
                    Id = 1,
                    Name = "搞笑"
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
