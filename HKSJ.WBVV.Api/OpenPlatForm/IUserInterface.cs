using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Api.OpenPlatForm
{
    public interface IUserInterface
    {
        dynamic GetUserInfo();
        void EndSession();
    }
}
