using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Common
{
    public class CommonMethod
    {

        public static string SendUrl(string Host)
        {
            string StrReturn = "";
            WebResponse result = null;
            try
            {
                WebRequest req = WebRequest.Create(Host);
                result = req.GetResponse();
                Stream ReceiveStream = result.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("GB2312");
                StreamReader sr = new StreamReader(ReceiveStream, encode);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    StrReturn += str;
                    count = sr.Read(read, 0, 256);
                }
            }
            catch (Exception e)
            {
                //StrReturn += e.ToString();
                //StrReturn += "找不到请求 URI，或者它的格式不正确";
            }
            finally
            {
                if (result != null)
                {
                    result.Close();
                }
            }
            return StrReturn.Trim();
        }
    }
}
