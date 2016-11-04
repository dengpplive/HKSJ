using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ApiParaModel;
using HKSJ.WBVV.Entity.Response.App;
using Newtonsoft.Json.Linq;

namespace HKSJ.WBVV.Business.Interface
{
    public interface IQiniuUploadBusiness
    {
        string PublicDomain { get; }
        string PrivateDomain { get; }

        dynamic Callback(JObject postData);
        string GetUploadToken(string type, string imgPara, long uid);
        string GetDownloadUrl(string fileKey, string type);
        void DeleteQiniuData(Video video);
        void DeleteQiniuData(long video);
        void DeleteQiniuImageByKey(string keys);
        void DeleteQiniuDataByKey(string[] keys, string type);
        void Notify(JObject postData);
        void VideoTranscode(int vid);
        AppUploadTokenView GetAppUploadToken(string type, long uid);
    }
}
