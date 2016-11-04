using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Common.Email.Template
{
    public class RegisterTemplate : AbstractTemplate
    {
        private static readonly string TemplatePath =
             Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Template\\Register.html");
        public override string GetTemplate(Dictionary<string, string> parameters)
        {
            //替换内容
            using (StreamReader reader = new StreamReader(TemplatePath))
            {
                var body = reader.ReadToEnd();
                return SetTemplate(body, parameters);
            }
        }
    }
}
