using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Logger;

namespace HKSJ.WBVV.Common.Mail
{
    public class MailHelper : IDisposable
    {
        private MailMessage _mailMessage;
        private SmtpClient _smtpClient;

        public MailHelper(SmtpClient smtpClient, string userName, string password, MailModel emailModel)
        {
            this._smtpClient = smtpClient;
            //创建服务器认证
            NetworkCredential vNetwork = new System.Net.NetworkCredential(userName, password);

            //发件人地址
            var mailAddressFrom = new MailAddress(emailModel.From.Address, emailModel.From.DisplayName);
            //指定发件人信息，包括邮箱地址和邮箱密码
            this._smtpClient.Credentials = new NetworkCredential(mailAddressFrom.Address, password);
            this._mailMessage = new MailMessage()
            {
                From = mailAddressFrom,//发件人邮箱
                Subject = emailModel.Subject,//邮件主题
                SubjectEncoding = Encoding.UTF8,
                Body = emailModel.Body,//邮件正文
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = emailModel.IsBodyHtml

            };
            //添加收件人邮箱地址
            var mailAddressTos = emailModel.To;
            if (mailAddressTos != null && mailAddressTos.Count > 0)
            {
                foreach (var mailAddressTo in mailAddressTos)
                {
                    this._mailMessage.To.Add(new MailAddress(mailAddressTo.Address, mailAddressTo.DisplayName));
                }
            }
            //添加抄送人邮箱地址
            var mailAddressCCs = emailModel.CC;
            if (mailAddressCCs != null && mailAddressCCs.Count > 0)
            {
                foreach (var mailAddressCC in mailAddressCCs)
                {
                    this._mailMessage.To.Add(new MailAddress(mailAddressCC.Address, mailAddressCC.DisplayName));
                }
            }
        }
        /// <summary>
        /// 同步发送邮件
        /// </summary>
        public void SendEmail(Action<bool> actinCompletedCallback)
        {
            _actionSendCompletedCallback = actinCompletedCallback;
            var success = true;
            try
            {
                this._smtpClient.Send(this._mailMessage);
            }
            catch (Exception ex)
            {
                success = false;
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Dispose();
                _actionSendCompletedCallback(success);
            }
            
        }
        //回调方法
        Action<bool> _actionSendCompletedCallback = null;
        /// <summary>
        /// 异步发送邮件
        /// </summary>
        public void SendEmailAsync(Action<bool> actinCompletedCallback)
        {
            _actionSendCompletedCallback = actinCompletedCallback;
            this._smtpClient.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
            try
            {
                //开始发送邮件
                this._smtpClient.SendAsync(this._mailMessage, "000000000");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
          
        }
        /// <summary>
        /// 异步操作完成后执行回调方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendCompletedCallback(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (_actionSendCompletedCallback == null) return;
            var  success = true;
            if (e.Cancelled)
            {
                success=false;
            }
            else if (e.Error != null)
            {
                success = false;
            }
            else
            {
                success = true;
            }
            _actionSendCompletedCallback(success);
        }
        public void Dispose()
        {
            if (this._mailMessage != null)
            {
                this._mailMessage.Dispose();
                this._mailMessage = null;
            }
            if (this._smtpClient != null)
            {
                this._smtpClient.Dispose();
                this._smtpClient = null;
            }
        }
    }
}
