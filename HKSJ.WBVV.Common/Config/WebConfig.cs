
using System.Configuration;
using HKSJ.WBVV.Common.Http;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Common.Config
{
    public static class WebConfig
    {/// <summary>
        /// 服务地址
        /// </summary>
        public static string BaseAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["BaseAddress"];
            }
        }

        /// <summary>
        /// 默认头像
        /// </summary>
        public static string DefaultUserPic
        {
            get
            {
                return ServerHelper.RootPath + ConfigurationManager.AppSettings["DefaultUserPic"];
            }
        }

        public static string DefaultUrl
        {
            get
            {
                return ServerHelper.RootPath + "/Home/Index";
            }
        }

        public static string ImageAddress
        {
            get;
            private set;
        }

        public static string PrivateDomain { get; private set; }


        /// <summary>
        /// QQ App Id
        /// </summary>
        public static string QQAppId
        {
            get
            {
                return ConfigurationManager.AppSettings["QQAppId"];
            }
        }

        /// <summary>
        /// Sina App Key
        /// </summary>
        public static string SinaAppKey
        {
            get
            {
                return ConfigurationManager.AppSettings["SinaAppKey"];
            }
        }

        /// <summary>
        /// QQ回调路径
        /// </summary>
        public static string QQCallBackPath
        {
            get
            {
                return ConfigurationManager.AppSettings["QQCallBackPath"];
            }
        }

        /// <summary>
        /// Sina回调路径
        /// </summary>
        public static string SinaCallBackPath
        {
            get
            {
                return ConfigurationManager.AppSettings["SinaCallBackPath"];
            }
        }

        /// <summary>
        /// 激活的语言
        /// </summary>
        public static string ActiveLanguage
        {
            get
            {
                string activeteLanguage = "";
                if (ConfigurationManager.AppSettings["ActiveLanguage"] == LanguageType.zh_cn)
                    activeteLanguage = LanguageType.zh_cn;
                else if (ConfigurationManager.AppSettings["ActiveLanguage"] == LanguageType.en_us)
                    activeteLanguage = LanguageType.en_us;
                else
                    activeteLanguage = LanguageType.zh_cn;

                return activeteLanguage;
            }
        }
    }
}
