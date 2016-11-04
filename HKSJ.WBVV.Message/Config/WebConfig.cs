using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Message.Config
{
    public static class WebConfig
    {
        /// <summary>
        /// 服务地址
        /// </summary>
        public static string BaseAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["BaseAddress"];
            }
        }
    }
}
