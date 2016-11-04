using System;
using System.Data;
using System.Collections.Generic;
using HKSJ.WBVV.Repository.Interface.IBase;
using HKSJ.WBVV.Entity;

namespace HKSJ.WBVV.Repository.Interface
{
    public interface  IPraisesRepository:IBaseAccess<Praises>
    {
        int PraisesComment(int userId, int commentId);
        int PraisesVedio(int userId, int vedioId);
        bool CancelPraisesComment(int userId, int commentId);
    }
}