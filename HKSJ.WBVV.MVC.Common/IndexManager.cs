using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Common.Config;
using Newtonsoft.Json.Linq;

namespace HKSJ.WBVV.MVC.Common
{
    public class IndexManager
    {
        public static string CheckIndexFile()
        {
            try
            {
                var res = WebApiHelper.InvokeApi<string>(WebConfig.BaseAddress + "Video/CheckIndexFile");
                return res;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string StartNewThread()
        {
            try
            {
                var res = WebApiHelper.InvokeApi<string>(WebConfig.BaseAddress + "Video/StartNewThread");
                return res;
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
