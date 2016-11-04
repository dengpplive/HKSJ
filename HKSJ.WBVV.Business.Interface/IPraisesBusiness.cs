using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Business.Interface.Base;

namespace HKSJ.WBVV.Business.Interface
{
    public interface IPraisesBusiness : IBaseBusiness
    {
        int CreatePraisesComment(int userId, int commentId);
        int CreatePraisesVedio(int userId, int vedioId);
        bool CancelPraisesComment(int userId, int commentId);
    }
}
