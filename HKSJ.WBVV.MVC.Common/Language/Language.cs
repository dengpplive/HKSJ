using HKSJ.WBVV.Common.Language;
using HKSJ.WBVV.Entity.ViewModel.Client;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Caching;


namespace HKSJ.WBVV.MVC.Common
{
    public class Language : ILanguage
    {
        private const string CacheName = "I18n_Texts";
        /// <summary>
        /// 把所有的文本写入到缓存中
        /// </summary>
        /// <returns></returns>
        public static Hashtable CacheAllTexts()
        {
            ResultView<Hashtable> result = GetAllTextsAPI();
            if (result.Success)
            {
                HttpRuntime.Cache.Add(CacheName, result.Data, null, DateTime.Now.AddMonths(3), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                return HttpRuntime.Cache[CacheName] as Hashtable;
            }
            return null;
        }

        /// <summary>
        /// 获取所有文本信息
        /// </summary>
        /// <returns></returns>
        private static Hashtable GetAllTexts()
        {
            Hashtable dictTable = HttpRuntime.Cache[CacheName] as Hashtable;
            if (dictTable == null)
                dictTable = CacheAllTexts();
            return dictTable;
        }

        /// <summary>
        /// 将所有文本写入全局JS文本
        /// </summary>
        /// <param name="data"></param>
        public static string GlobalJSAllTexts()
        {
            Hashtable data = GetAllTexts();
            StringBuilder str = new StringBuilder();
            str.Append("var languageData = new Array();");
            foreach (DictionaryEntry item in data)
            {
                str.Append("languageData[\"" + item.Key.ToString() + "\"] = '" + item.Value.ToString().Replace("'","\"") + "';");
            }
            str.Append("function Translate(key) {");
            str.Append("    var value = languageData[key];");
            str.Append("    if (value == undefined)");
            str.Append("        value = key;");
            str.Append("    return value;");
            str.Append("}");
            return str.ToString();
        }

        public string Translate(string key)
        {
            Hashtable dictTable = GetAllTexts();
            if (dictTable.Contains(key))
                return dictTable[key].ToString();
            return key;

        }

        /// <summary>
        /// 获取词典数据
        /// </summary>
        /// <returns></returns>
        private static ResultView<Hashtable> GetAllTextsAPI()
        {

            var result = WebApiHelper.InvokeApi<string>("Language/GetAllTexts");
            if (result != null)
            {
                return JsonConvert.DeserializeObject(result, typeof(ResultView<Hashtable>)) as ResultView<Hashtable>;
            }
            return new ResultView<Hashtable>();
        }
    }
}