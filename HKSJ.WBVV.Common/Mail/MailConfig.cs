using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace HKSJ.WBVV.Common.Mail
{
    public static class MailConfig
    {
        public static readonly string Host;
        public static readonly int Port;
        public static readonly string UserName;
        public static readonly string PassWord;
        public static readonly string Subject;
        public static readonly string From;
        public static readonly string To;
        static MailConfig()
        {
            Host = ConfigurationManager.AppSettings["SmtpHost"];
            Port = Convert.ToInt16(ConfigurationManager.AppSettings["SmtpPort"]);
            UserName = ConfigurationManager.AppSettings["SmtpUserName"];
            PassWord = ConfigurationManager.AppSettings["SmtpPassWord"];
            Subject = ConfigurationManager.AppSettings["SmtpSubject"];
            From = ConfigurationManager.AppSettings["SmtpFrom"];
            To = ConfigurationManager.AppSettings["SmtpTo"];

        }
    }
}
