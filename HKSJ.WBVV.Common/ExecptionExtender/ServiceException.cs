using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Extender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace HKSJ.WBVV.Common.ExecptionExtender
{
    [Serializable]
    public class ServiceException : AppException
    {
        const string Suffix = "[ServiceError]";
        public ServiceException(string msg)
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