using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using HKSJ.WBVV.Api.Base;

using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Entity.ApiModel;

namespace HKSJ.WBVV.Api
{
    public class QiniuUploadController : ApiControllerBase
    {
        //private readonly IBannerVideoBusiness _bannerVideoBusiness;
        private readonly IQiniuUploadBusiness _qiniuUploadBusiness;

        public QiniuUploadController(IQiniuUploadBusiness qiniuUploadBusiness)
        {
            _qiniuUploadBusiness = qiniuUploadBusiness;
        }


        [HttpGet]
        public string TestMethod()
        {
            return "OK";
        }

        #region 获取Token

        [HttpGet]
        public dynamic GetToken(string type,string imgPara = "" ,long uid = 0)
        {
            return new {token = _qiniuUploadBusiness.GetUploadToken(type, imgPara, uid)};
        }

        #endregion


        [HttpGet]
        public dynamic PublicDomain()
        {
            return new {domain = _qiniuUploadBusiness.PublicDomain};
        }

        [HttpGet]
        public dynamic PrivateDomain()
        {
            return new { domain = _qiniuUploadBusiness.PublicDomain };
        }

        #region 七牛视频文件上转完后回调
        [HttpPost]
        public dynamic Callback()
        {
            return _qiniuUploadBusiness.Callback(JObject);
        }
        #endregion

        #region 转码完成后的回调

        [HttpPost]
        public dynamic Notify()
        {
            _qiniuUploadBusiness.Notify(JObject);
            
            return "OK";

        }

        #endregion

        #region 删除七年图片

        [HttpPost]
        public dynamic DelQiniuImage(string keys )
        {
            if (keys.Length <= 0)
            {
                return "Error";
            }

            foreach (string t in keys.Split(','))
            {
                _qiniuUploadBusiness.DeleteQiniuImageByKey(t);
            }
            return "OK";
        }
        #endregion

        #region 获取视频播放地址
        [HttpGet]
        public string GetVideoUrl(string key)
        {
            key += "?pm3u8/0";
            return _qiniuUploadBusiness.GetDownloadUrl(key, "video");
        }
        #endregion


        #region 重新转码
        [HttpPost]
        public void VideoTranscode()
        {
            _qiniuUploadBusiness.VideoTranscode(JObject.Value<int>("vid"));
        }
        #endregion

        [HttpGet]
        public dynamic DeleteVideoById(int id)
        {
            _qiniuUploadBusiness.DeleteQiniuData(id);
            return "OK";
        }

    }
}
