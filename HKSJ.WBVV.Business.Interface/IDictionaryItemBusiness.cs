


using System;
using System.Data;
using System.Collections.Generic;
using HKSJ.WBVV.Business.Interface.Base;
using HKSJ.WBVV.Entity;

namespace HKSJ.WBVV.Business.Interface
{

    public interface IDictionaryItemBusiness : IBaseBusiness
    {
        int CreateDictionaryItem(string name, int parentId);
        bool UpdateDictionaryItem(string name, int id);
        bool DeleteDictionaryItem(int id);
    }
}