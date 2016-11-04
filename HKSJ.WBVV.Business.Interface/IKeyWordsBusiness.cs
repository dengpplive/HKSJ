using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Business.Interface.Base;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ViewModel;

namespace HKSJ.WBVV.Business.Interface
{
    public interface IKeyWordsBusiness:IBaseBusiness
    {
        bool AddOrUpdateAKeyWord(string keyword);
        List<KeyWords> GetKeyWordsByIpAddress(string ipAddress);
        List<KeyWords> GetFilteredKeyword(string keyword);
        bool AddAKeyWord(string keyword);
        bool UpdateAKeyWord(KeyWords info);
        List<KeyWords> GetHotKeyWords();
        PageResult GetKeyWordsPageResult(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions);
        KeyWords GetAKeyWordById(int id);
        bool AddAKeyWord(KeyWords info);
        bool DelAKeyWord(int id);
    }
}
