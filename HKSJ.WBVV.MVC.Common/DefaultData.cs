using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Common.Config;
using Newtonsoft.Json.Linq;

namespace HKSJ.WBVV.MVC.Common
{
    public static class DefaultData
    {
        public static void InitData()
        {
            var publicJson = WebApiHelper.InvokeApi<JObject>(WebConfig.BaseAddress + "QiniuUpload/PublicDomain");
            string publicDomain = publicJson.Value<string>("domain");
            string privateDomain = WebApiHelper.InvokeApi<JObject>("QiniuUpload/PrivateDomain").Value<string>("domain");
            ImageAddress = string.Format("http://{0}", publicDomain);
            PrivateDomain = string.Format("http://{0}", privateDomain);
        }


        public static string ImageAddress { get; set; }

        public static string PrivateDomain{ get; set; }

    }
}
