using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Business.Interface.Base
{
    public interface IBaseBusiness
    {
        string IpAddress { get; set; }
        int UserId { get; set; }
        int PageSize { get; set; }
        int PageIndex { get; set; }
        string Token { get; set; }
    }
}
