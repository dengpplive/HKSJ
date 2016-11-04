using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Common.Email.Enum;
using HKSJ.WBVV.Common.Email.Template;

namespace HKSJ.WBVV.Common.Email
{
    public class EmailFactory
    {
        public static AbstractTemplate ProcessTemplate(EmailEnum emailEnum)
        {
            switch (emailEnum)
            {
                case EmailEnum.Register:
                    return new RegisterTemplate();
                    break;
                case EmailEnum.FindPwd:
                    return new FindPwdTemplate();
                    break;
                case EmailEnum.UpdateEmail:
                    return new UpdateEmailTemplate();
                    break;
                default:
                    throw new ArgumentNullException("emailEnum");
                    break;
            }
        }
    }
}
