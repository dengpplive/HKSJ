using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Common.Mail
{
    public class SmtpHelper 
    {
        public SmtpClient SmtpClient { get; private set; }

        public SmtpHelper(string host, int port, bool enableSsl, bool useDefaultCredentials, SmtpDeliveryMethod deliveryMethod, int timeOut)
        {
            SmtpClient = new SmtpClient(host, port)
            {
                EnableSsl = enableSsl,
                UseDefaultCredentials = useDefaultCredentials,
                DeliveryMethod = deliveryMethod,
                Timeout = timeOut
            };
        }
        public SmtpHelper(string host, int port, bool enableSsl, bool useDefaultCredentials, SmtpDeliveryMethod deliveryMethod)
            : this(host, port, enableSsl, useDefaultCredentials, deliveryMethod, 10000)
        {

        }
        public SmtpHelper(string host, int port, bool enableSsl, bool useDefaultCredentials)
            : this(host, port, enableSsl, useDefaultCredentials, SmtpDeliveryMethod.Network)
        {

        }
        public SmtpHelper(string host, int port, bool enableSsl)
            : this(host, port, enableSsl, false)
        {

        }
        public SmtpHelper(string host, int port, SmtpDeliveryMethod deliveryMethod)
            : this(host, port, false, false,deliveryMethod)
        {

        }
        public SmtpHelper(string host, int port)
            : this(host, port, false)
        {

        }
    }
}
