using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Email.Enum;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Mail;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Common.Email
{
    public class EmailHelper
    {
        private static readonly Regex EmailRegex = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// 邮件队列
        /// </summary>
        public static void SendEmailThread()
        {
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    while (EmailQueue.EmailModel != null && EmailQueue.EmailModel.Count > 0)
                    {
                        var queue = EmailQueue.EmailModel.Dequeue();
                        if (queue.IsAsync)
                        {
                            SendEmailAsync(queue.Received, queue.Subject, queue.Body, queue.IsBodyHtml, (r) => { });
                        }
                        else
                        {
                            SendEmail(queue.Received, queue.Subject, queue.Body, queue.IsBodyHtml, (r) => { });
                        }
                        queue = null;
                        Thread.Sleep(2000);
                    }
                    Thread.Sleep(2000);
                }
            });
            thread.Start();
        }

        #region 同步发送邮件
        /// <summary>
        /// 同步发送Html邮件模版
        /// </summary>
        /// <param name="emailEnum">模版类型</param>
        /// <param name="to">接受人（单个）</param>
        /// <param name="parameters">模版参数【key】【value】</param>
        /// <param name="action">发送邮件成功调用的方法</param>
        public static void SendEmailHtml(EmailEnum emailEnum, string to, Dictionary<string, string> parameters, Action<bool> action)
        {
            SendEmail(emailEnum, new List<string>() { to }, true, parameters, action);
        }
        /// <summary>
        /// 同步发送Text邮件模版
        /// </summary>
        /// <param name="emailEnum">模版类型</param>
        /// <param name="to">接受人（单个）</param>
        /// <param name="parameters">模版参数【key】【value】</param>
        /// <param name="action">发送邮件成功调用的方法</param>
        public static void SendEmailText(EmailEnum emailEnum, string to, Dictionary<string, string> parameters, Action<bool> action)
        {
            SendEmail(emailEnum, new List<string>() { to }, false, parameters, action);
        }
        /// <summary>
        /// 同步发送邮件模版
        /// </summary>
        /// <param name="emailEnum">模版类型</param>
        /// <param name="to">接受人（单个）</param>
        /// <param name="isbodyHtml">是否是Html</param>
        /// <param name="parameters">模版参数【key】【value】</param>
        /// <param name="action">发送邮件成功调用的方法</param>
        public static void SendEmail(EmailEnum emailEnum, string to, bool isbodyHtml, Dictionary<string, string> parameters, Action<bool> action)
        {
            SendEmail(emailEnum, new List<string>() { to }, isbodyHtml, parameters, action);
        }
        /// <summary>
        /// 同步发送Html邮件模版
        /// </summary>
        /// <param name="emailEnum">模版类型</param>
        /// <param name="to">接受人（多个）</param>
        /// <param name="parameters">模版参数【key】【value】</param>
        /// <param name="action">发送邮件成功调用的方法</param>
        public static void SendEmailHtml(EmailEnum emailEnum, IList<string> to, Dictionary<string, string> parameters, Action<bool> action)
        {
            SendEmail(to, EnumHelper.GetEnumDescription(emailEnum), EmailFactory.ProcessTemplate(emailEnum).GetTemplate(parameters), true, action);
        }
        /// <summary>
        /// 同步发送Text邮件模版
        /// </summary>
        /// <param name="emailEnum">模版类型</param>
        /// <param name="to">接受人（多个）</param>
        /// <param name="parameters">模版参数【key】【value】</param>
        /// <param name="action">发送邮件成功调用的方法</param>
        public static void SendEmailText(EmailEnum emailEnum, IList<string> to, Dictionary<string, string> parameters, Action<bool> action)
        {
            SendEmail(to, EnumHelper.GetEnumDescription(emailEnum), EmailFactory.ProcessTemplate(emailEnum).GetTemplate(parameters), false, action);
        }
        /// <summary>
        /// 同步发送邮件模版
        /// </summary>
        /// <param name="emailEnum">模版类型</param>
        /// <param name="to">接受人（多个）</param>
        /// <param name="isbodyHtml">是否是Html</param>
        /// <param name="parameters">模版参数【key】【value】</param>
        /// <param name="action">发送邮件成功调用的方法</param>
        public static void SendEmail(EmailEnum emailEnum, IList<string> to, bool isbodyHtml, Dictionary<string, string> parameters, Action<bool> action)
        {
            SendEmail(to, EnumHelper.GetEnumDescription(emailEnum), EmailFactory.ProcessTemplate(emailEnum).GetTemplate(parameters), isbodyHtml, action);
        }
        /// <summary>
        /// 同步发送邮件
        /// </summary>
        /// <param name="to">接受人(单个)</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="isbodyHtml">是否是Html</param>
        /// <param name="action">发送邮件成功调用的方法</param>
        public static void SendEmail(string to, string subject, string body, bool isbodyHtml, Action<bool> action)
        {
            SendEmail(new List<string>() { to }, subject, body, isbodyHtml, action);
        }
        /// <summary>
        /// 同步发送Html邮件
        /// </summary>
        /// <param name="to">接受人(单个)</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="action">发送邮件成功调用的方法</param>
        public static void SendEmailHtml(string to, string subject, string body, Action<bool> action)
        {
            SendEmail(new List<string>() { to }, subject, body, true, action);
        }
        /// <summary>
        /// 同步发送Text邮件
        /// </summary>
        /// <param name="to">接受人(单个)</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="action">发送邮件成功调用的方法</param>
        public static void SendEmailText(string to, string subject, string body, Action<bool> action)
        {
            SendEmail(new List<string>() { to }, subject, body, false, action);
        }
        /// <summary>
        /// 同步发送邮件
        /// </summary>
        /// <param name="to">接受人(多个)</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="isbodyHtml">是否是Html</param>
        /// <param name="action">发送邮件成功调用的方法</param>
        public static void SendEmail(IList<string> to, string subject, string body, bool isbodyHtml, Action<bool> action)
        {
            SendEmailAsync(MailConfig.Host, MailConfig.Port, MailConfig.UserName, MailConfig.PassWord, MailConfig.From, to, subject, body, isbodyHtml, false, action);
        }
        /// <summary>
        /// 同步发送Html邮件
        /// </summary>
        /// <param name="to">接受人(多个)</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="action">发送邮件成功调用的方法</param>
        public static void SendEmailHtml(IList<string> to, string subject, string body, Action<bool> action)
        {
            SendEmailAsync(MailConfig.Host, MailConfig.Port, MailConfig.UserName, MailConfig.PassWord, MailConfig.From, to, subject, body, true, false, action);
        }
        /// <summary>
        /// 同步发送Text邮件
        /// </summary>
        /// <param name="to">接受人(多个)</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="action">发送邮件成功调用的方法</param>
        public static void SendEmailText(IList<string> to, string subject, string body, Action<bool> action)
        {
            SendEmailAsync(MailConfig.Host, MailConfig.Port, MailConfig.UserName, MailConfig.PassWord, MailConfig.From, to, subject, body, false, false, action);
        }
        #endregion

        #region 异步发送邮件
        /// <summary>
        /// 异步发送HTML邮件模版
        /// </summary>
        /// <param name="emailEnum">模版类型</param>
        /// <param name="to">接受人（单个）</param>
        /// <param name="parameters">模版参数【key】【value】</param>
        /// <param name="action">发送邮件成功调用的方法</param>
        public static void SendEmailHtmlAsync(EmailEnum emailEnum, string to, Dictionary<string, string> parameters, Action<bool> action)
        {
            SendEmailAsync(emailEnum, new List<string>() { to }, true, parameters, action);
        }
        /// <summary>
        /// 异步发送Text邮件模版
        /// </summary>
        /// <param name="emailEnum">模版类型</param>
        /// <param name="to">接受人（单个）</param>
        /// <param name="parameters">模版参数【key】【value】</param>
        /// <param name="action">发送邮件成功调用的方法</param>
        public static void SendEmailTextAsync(EmailEnum emailEnum, string to, Dictionary<string, string> parameters, Action<bool> action)
        {
            SendEmailAsync(emailEnum, new List<string>() { to }, false, parameters, action);
        }
        /// <summary>
        /// 异步发送邮件模版
        /// </summary>
        /// <param name="emailEnum">模版类型</param>
        /// <param name="to">接受人（单个）</param>
        /// <param name="isbodyHtml">是否是Html</param>
        /// <param name="parameters">模版参数【key】【value】</param>
        /// <param name="action">发送邮件成功调用的方法</param>
        public static void SendEmailAsync(EmailEnum emailEnum, string to, bool isbodyHtml, Dictionary<string, string> parameters, Action<bool> action)
        {
            SendEmailAsync(emailEnum, new List<string>() { to }, isbodyHtml, parameters, action);
        }
        /// <summary>
        /// 异步发送HTML邮件模版
        /// </summary>
        /// <param name="emailEnum">模版类型</param>
        /// <param name="to">接受人（多个）</param>
        /// <param name="parameters">模版参数【key】【value】</param>
        /// <param name="action">发送邮件成功调用的方法</param>
        public static void SendEmailHtmlAsync(EmailEnum emailEnum, IList<string> to, Dictionary<string, string> parameters, Action<bool> action)
        {
            SendEmailAsync(to, EnumHelper.GetEnumDescription(emailEnum), EmailFactory.ProcessTemplate(emailEnum).GetTemplate(parameters), true, action);
        }
        /// <summary>
        /// 异步发送Text邮件模版
        /// </summary>
        /// <param name="emailEnum">模版类型</param>
        /// <param name="to">接受人（多个）</param>
        /// <param name="parameters">模版参数【key】【value】</param>
        /// <param name="action">发送邮件成功调用的方法</param>
        public static void SendEmailTextAsync(EmailEnum emailEnum, IList<string> to, Dictionary<string, string> parameters, Action<bool> action)
        {
            SendEmailAsync(to, EnumHelper.GetEnumDescription(emailEnum), EmailFactory.ProcessTemplate(emailEnum).GetTemplate(parameters), false, action);
        }
        /// <summary>
        /// 异步发送邮件模版
        /// </summary>
        /// <param name="emailEnum">模版类型</param>
        /// <param name="to">接受人（多个）</param>
        /// <param name="isbodyHtml">是否是Html</param>
        /// <param name="parameters">模版参数【key】【value】</param>
        /// <param name="action">发送邮件成功调用的方法</param>
        public static void SendEmailAsync(EmailEnum emailEnum, IList<string> to, bool isbodyHtml, Dictionary<string, string> parameters, Action<bool> action)
        {
            SendEmailAsync(to, EnumHelper.GetEnumDescription(emailEnum), EmailFactory.ProcessTemplate(emailEnum).GetTemplate(parameters), isbodyHtml, action);
        }
        /// <summary>
        /// 异步发送邮件
        /// </summary>
        /// <param name="to">接受人（单个）</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="isbodyHtml">是否是Html</param>
        /// <param name="action">发送邮件成功调用的方法</param>
        public static void SendEmailAsync(string to, string subject, string body, bool isbodyHtml, Action<bool> action)
        {
            SendEmailAsync(new List<string>() { to }, subject, body, isbodyHtml, action);
        }
        /// <summary>
        /// 异步发送Html邮件
        /// </summary>
        /// <param name="to">接受人（单个）</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="action">发送邮件成功调用的方法</param>
        public static void SendEmailHtmlAsync(string to, string subject, string body, Action<bool> action)
        {
            SendEmailAsync(new List<string>() { to }, subject, body, true, action);
        }
        /// <summary>
        /// 异步发送Text邮件
        /// </summary>
        /// <param name="to">接受人（单个）</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="action">发送邮件成功调用的方法</param>
        public static void SendEmailTextAsync(string to, string subject, string body, Action<bool> action)
        {
            SendEmailAsync(new List<string>() { to }, subject, body, false, action);
        }
        /// <summary>
        /// 异步发送邮件
        /// </summary>
        /// <param name="to">接受人（多个）</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="isbodyHtml">是否是Html</param>
        /// <param name="action">发送邮件成功调用的方法</param>
        public static void SendEmailAsync(IList<string> to, string subject, string body, bool isbodyHtml, Action<bool> action)
        {
            SendEmailAsync(MailConfig.Host, MailConfig.Port, MailConfig.UserName, MailConfig.PassWord, MailConfig.From, to, subject, body, isbodyHtml, true, action);
        }
        /// <summary>
        /// 异步发送Html邮件
        /// </summary>
        /// <param name="to">接受人（多个）</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="action">发送邮件成功调用的方法</param>
        public static void SendEmailHtmlAsync(IList<string> to, string subject, string body, Action<bool> action)
        {
            SendEmailAsync(MailConfig.Host, MailConfig.Port, MailConfig.UserName, MailConfig.PassWord, MailConfig.From, to, subject, body, true, true, action);
        }
        /// <summary>
        /// 异步发送Text邮件
        /// </summary>
        /// <param name="to">接受人（多个）</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="action">发送邮件成功调用的方法</param>
        public static void SendEmailTextAsync(IList<string> to, string subject, string body, Action<bool> action)
        {
            SendEmailAsync(MailConfig.Host, MailConfig.Port, MailConfig.UserName, MailConfig.PassWord, MailConfig.From, to, subject, body, false, true, action);
        }
        /// <summary>
        /// 异步发送邮件
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="from">发送人</param>
        /// <param name="to">接受人(可多个)</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="isbodyHtml">是否是Html</param>
        /// <param name="isAsync">是否是异步</param>
        /// <param name="action">发送邮件成功调用的方法</param>
        private static void SendEmailAsync(string host, int port, string userName, string password, string from, IList<string> to, string subject, string body, bool isbodyHtml, bool isAsync, Action<bool> action)
        {

            AssertUtil.NotNullOrWhiteSpace(host, LanguageUtil.Translate("com_EmailHelper_SendEmailAsync_check_server"));
            AssertUtil.Between(port, 1, 65536, LanguageUtil.Translate("com_EmailHelper_SendEmailAsync_check_port"));
            AssertUtil.NotNullOrWhiteSpace(userName, LanguageUtil.Translate("com_EmailHelper_SendEmailAsync_check_username"));
            AssertUtil.NotNullOrWhiteSpace(password, LanguageUtil.Translate("com_EmailHelper_SendEmailAsync_check_pwd"));
            AssertUtil.NotNullOrWhiteSpace(from, LanguageUtil.Translate("com_EmailHelper_SendEmailAsync_check_from"));
            AssertUtil.IsNotEmptyCollection(to, LanguageUtil.Translate("com_EmailHelper_SendEmailAsync_check_to"));
            IList<string> newTo = to.Where(item => EmailRegex.IsMatch(item)).ToList();
            AssertUtil.IsNotEmptyCollection(newTo, LanguageUtil.Translate("com_EmailHelper_SendEmailAsync_check_toformat"));
            var smtpClient = new SmtpHelper(host, port).SmtpClient;
            var email = new MailModel(subject, body, from, newTo, isbodyHtml);
            var mailMessage = new MailHelper(smtpClient, userName, password, email);
            if (isAsync)
            {
                mailMessage.SendEmailAsync(action);
            }
            else
            {
                mailMessage.SendEmail(action);
            }
        }
        #endregion
    }
}