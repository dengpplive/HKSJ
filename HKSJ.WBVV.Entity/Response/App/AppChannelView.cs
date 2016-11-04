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
    /// 频道视图
    /// </summary>
    public class AppChannelView : IDocument
    {
        /// <summary>
        /// 分类列表
        /// </summary>
         [Display(Name="分类列表")]
        public IList<AppCategoryView> Categorys { get; set; }
        /// <summary>
        /// 推荐用户列表
        /// </summary>
         [Display(Name="推荐用户列表")]
        public IList<AppUserSimpleView> UserInfos { get; set; }
        public object GetSampleObject()
        {
            return new ResponsePackage<AppChannelView>
            {
                Data = new AppChannelView
                {
                    Categorys = new List<AppCategoryView>()
                    {
                        new AppCategoryView()
                        {
                            Id = 1,
                            Name = "搞笑"
                        }
                    },
                    UserInfos = new List<AppUserSimpleView>()
                    {
                        new AppUserSimpleView()
                        {
                            Id = 888,
                            NickName = "AxOne",
                            Picture = "http://xx/xx/xx"
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
