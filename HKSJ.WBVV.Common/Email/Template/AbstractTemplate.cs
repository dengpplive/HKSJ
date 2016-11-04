using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Common.Email.Template
{
    public abstract  class AbstractTemplate
    {
        public abstract string GetTemplate(Dictionary<string, string> parameters);

        protected string SetTemplate(string template, Dictionary<string, string> parameters)
        {
            var matches = Regex.Matches(template, "@(.*?)@", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
            if (parameters != null && parameters.Count > 0)
            {
                foreach (var matche in matches)
                {
                    var key = matche.ToString();
                    if (parameters.ContainsKey(key))
                    {
                        template = template.Replace(key, parameters[key]);
                    }
                }
            }
            return template;
        }
    }
}
