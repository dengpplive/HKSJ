using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Common.Language
{
    public interface ILanguage
    {
        string Translate(string key);
    }

    public class LanguageUtil
    {
        public static string Translate(string key)
        {
            ILanguage language = ((IContainer)StaticObj.Container).Resolve<ILanguage>();
            return language.Translate(key);
        }
    }
}
