using HKSJ.WBVV.Business.Interface.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity.ViewModel.Manage;
using PlateView = HKSJ.WBVV.Entity.ViewModel.Client.PlateView;

namespace HKSJ.WBVV.Business.Interface
{
    public interface IPlateBusiness : IBaseBusiness
    {
        bool DeletePlate(int id);
        bool DeletePlates(IList<int> ids);
        int CreatePlate(int categoryId, string name, int sortNum, int pageSize);
        bool UpdatePlate(int id, int categoryId, string name, int sortNum, int pageSize);
        Plate GetPlate(int id);
        IList<PlateView> GetPlateViewByHomeList();
        IList<PlateView> GetPlateViewByCategoryIdList(int categoryId);
        PageResult GetPlatePageResult(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions);

        IList<HKSJ.WBVV.Entity.ViewModel.Manage.PlateView> GetPlateList(IList<Condtion> condtions,
            IList<OrderCondtion> orderCondtions, out int totalCount, out int totalIndex);
        bool UpdatePlateSort(IList<Plate> plateList);
    }
}
