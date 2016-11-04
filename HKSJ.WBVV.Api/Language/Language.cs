using Autofac;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common;
using HKSJ.WBVV.Common.Cache;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Language;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Web.Caching;

namespace HKSJ.WBVV.Api
{
    public class Language : ILanguage
    {
        private const string CacheName = "I18n_Texts";

        /// <summary>
        /// 缓存语言
        /// </summary>
        /// <returns></returns>
        public Hashtable CacheAllTexts()
        {
            Hashtable dictData = null;

            ILanguageBusiness language = ((IContainer)StaticObj.Container).Resolve<ILanguageBusiness>();
            dictData = language.GetAllTexts();

            HttpRuntime.Cache.Add(CacheName, dictData, null, DateTime.Now.AddMonths(3), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);

            return dictData;
        }

        public string Translate(string key)
        {
            Hashtable dictData = HttpRuntime.Cache[CacheName] as Hashtable;
            if (dictData == null)
                dictData = CacheAllTexts();

            if (dictData.Contains(key))
                return dictData[key].ToString();
            return key;
        }
    }
}
