using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Api.Host
{
    public static class QiniuData
    {
        public static void InitQiniuData()
        {
            Qiniu.Conf.Config.ACCESS_KEY = ConfigurationManager.AppSettings["QiniuAccessKey"];
            Qiniu.Conf.Config.SECRET_KEY = ConfigurationManager.AppSettings["QiniuSecretKey"];
        }
    }
}
