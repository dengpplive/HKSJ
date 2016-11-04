using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using HKSJ.WBVV.Repository.Interface;
using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Common.Logger;
using System.Collections;
using System.Threading;


namespace HKSJ.WBVV.Business
{
    public class LanguageBusiness : BaseBusiness, ILanguageBusiness
    {
        private readonly ILanguageRepository _ilanguageRepository;


        public LanguageBusiness(ILanguageRepository ilanguageRepository)
        {
            this._ilanguageRepository = ilanguageRepository;
        }

        /// <summary>
        /// 根据当前线程语言类型获取语言数据
        /// </summary>
        /// <returns></returns>
        public Hashtable GetAllTexts()
        {
            string lang = Thread.CurrentThread.CurrentCulture.Name;
            AssertUtil.IsNotNull(lang);

            Hashtable dictTable = new Hashtable();
            List<Language> langList = (from l in this._ilanguageRepository.GetEntityList()
                                       select l).AsQueryable<Language>().Where<Language>(a => a.Lang == lang).ToList<Language>();
            foreach (Language item in langList)
            {
                dictTable.Add(item.Key, item.Text);
            }
            return dictTable;

        }


        #region 传入参数检测

        #endregion

        #region 传入参数

        /// <summary>
        /// 比较语言类型
        /// </summary>
        /// <param name="languageType"></param>
        /// <returns></returns>
        private Condtion ConditionEqualLanguageType(string languageType)
        {
            var condtion = new Condtion()
            {
                FiledName = "Lang",
                FiledValue = languageType,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }

        #endregion
    }
}
