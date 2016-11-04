using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Common.ExecptionExtender;
using HKSJ.WBVV.Common.Extender;
using Newtonsoft.Json.Linq;

namespace HKSJ.WBVV.Common.Cache
{
    [Serializable]
    public class MemberExecption : AppException
    {
        const string Suffix = "[MemcachedError]";

        public MemberExecption(string msg)
            : base(msg)
        {
        }
        public string SimplifyMessage
        {
            get
            {
                string tmp = null;
                {
                    var obj = Message.FromJSON<JObject>();
                    tmp = obj.Get<string>("ExceptionMessage");
                    if (tmp.IsNullOrWhiteSpace())
                        tmp = obj.Get<string>("Message");
                }
                return tmp + Suffix;
            }
        }
       
    }
}
