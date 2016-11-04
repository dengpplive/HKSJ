using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Common.Mail
{
    public class AddressModel
    {
        public string Address { get; private set; }
        public string DisplayName { get; private set; }
        public Encoding DisplayNameEncoding { get; private set; }
        public AddressModel(string address, string displayName, Encoding displayNameEncoding)
        {
            this.Address = address;
            this.DisplayName = displayName;
            this.DisplayNameEncoding = displayNameEncoding;
        }
        public AddressModel(string address, string displayName)
            : this(address, displayName, Encoding.UTF8)
        {
        }
        public AddressModel(string address)
            : this(address, "", Encoding.UTF8)
        {
        }
    }
    public class MailModel
    {
        /// <summary>
        /// 发送邮件地址
        /// </summary>
        public AddressModel From { get; private set; }
        /// <summary>
        /// 接收邮件地址
        /// </summary>
        public IList<AddressModel> To { get; private set; }
        /// <summary>
        /// 抄送邮件地址
        /// </summary>
        public IList<AddressModel> CC { get; private set; }
        /// <summary>
        /// 邮件主题
        /// </summary>
        public string Subject { get; private set; }
        /// <summary>
        /// 邮件正文
        /// </summary>
        public string Body { get; private set; }
        /// <summary>
        /// 邮件内容是否是html
        /// </summary>
        public bool IsBodyHtml { get; private set; }
        /// <summary>
        /// 邮件附件
        /// </summary>
        public IList<Attachment> Attachments { get; private set; }
        /// <summary>
        /// 邮件查看格式
        /// </summary>
        public IList<AlternateView> AlternateViews { get; private set; }
        public MailModel(string subject, string body, AddressModel from, IList<AddressModel> to, IList<AddressModel> cc, IList<Attachment> attachments, IList<AlternateView> alternateViews, bool isBodyHtml = false)
        {
            this.Subject = subject;
            this.Body = body;
            this.IsBodyHtml = isBodyHtml;
            this.From = from;
            this.To = to;
            this.CC = cc;
            this.Attachments = attachments;
            this.AlternateViews = alternateViews;
        }
        public MailModel(string subject, string body, AddressModel from, AddressModel to, IList<AddressModel> cc, IList<Attachment> attachments, IList<AlternateView> alternateViews, bool isBodyHtml = false)
            : this(subject, body, from, new List<AddressModel>() { to }, cc, attachments, alternateViews, isBodyHtml)
        {
        }
        public MailModel(string subject, string body, string from, IList<AddressModel> to, IList<AddressModel> cc, IList<Attachment> attachments, IList<AlternateView> alternateViews, bool isBodyHtml = false)
            : this(subject, body, new AddressModel(from), to, cc,attachments,alternateViews, isBodyHtml)
        {
        }
        public MailModel(string subject, string body, string from, AddressModel to, IList<AddressModel> cc, IList<Attachment> attachments, IList<AlternateView> alternateViews, bool isBodyHtml = false)
            : this(subject, body, new AddressModel(from), new List<AddressModel>() { to }, cc, attachments, alternateViews, isBodyHtml)
        {
        }
        public MailModel(string subject, string body, string from, IList<string> to, IList<string>  cc, IList<Attachment> attachments, IList<AlternateView> alternateViews, bool isBodyHtml = false)
        {
            this.Subject = subject;
            this.Body = body;
            this.IsBodyHtml = isBodyHtml;
            this.From = new AddressModel(from);
            if (cc!=null&&cc.Count>0)
            {
                 this.CC= cc.Select(c => new AddressModel(c)).ToList();
            }
            if (to != null && to.Count > 0)
            {
                this.To = to.Select(t => new AddressModel(t)).ToList();
            }
            this.Attachments = attachments;
            this.AlternateViews = alternateViews;
        }
        public MailModel(string subject, string body, AddressModel  from, IList<AddressModel> to, IList<AddressModel> cc, IList<Attachment> attachments, bool isBodyHtml = false)
            : this(subject, body, from, to, cc, attachments, null, isBodyHtml)
        {

        }
        public MailModel(string subject, string body, AddressModel from, IList<AddressModel> to, IList<AddressModel> cc, bool isBodyHtml = false)
            : this(subject, body, from, to, cc, null, isBodyHtml)
        {

        }
        public MailModel(string subject, string body, AddressModel from, IList<AddressModel> to, bool isBodyHtml = false)
            : this(subject, body, from, to, null, isBodyHtml)
        {

        }
        public MailModel(string subject, string body, string from, IList<AddressModel> to, bool isBodyHtml = false)
            : this(subject, body, new AddressModel(from), to, null, isBodyHtml)
        {

        }
        public MailModel(string subject, string body, string from, IList<string> to, bool isBodyHtml = false)
            : this(subject, body, from, to,null,null,null,isBodyHtml)
        {
        }
        public MailModel(string subject, string body, string from, string to, bool isBodyHtml = false)
            : this(subject, body, new AddressModel(from), new List<AddressModel>() { new AddressModel(to) }, null, isBodyHtml)
        {

        }
    }
}
