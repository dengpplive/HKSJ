using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Business.Interface.Base;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity.ViewModel.Manage;

namespace HKSJ.WBVV.Business.Interface
{
    public interface IPlateVideoBusiness : IBaseBusiness
    {
        PageResult GetPlateVideoPageResult(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions);
        PlateVideo GetPlateVideo(int id);
        int CreatePlateVideo(int plateId, int sortNum, int isHot, int isRecommend, int videoId);
        int CreatePlateCategoryVideo(int categoryId, int sortNum, int isHot, int isRecommend, int videoId);
        bool UpdatePlateVideo(int id, int plateId, int sortNum, int isHot, int isRecommend, int videoId);
        bool UpdatePlateVideo(int id, int sortNum, int isHot, int isRecommend, int videoId);
        bool DeletePlateVideo(int id);
        bool DeletePlateVideos(IList<int> ids);
        bool DeletePlateVideos(int plateId);
        bool UpdatePlateVideoSort(IList<PlateVideo> plateVideoList);
    }
}
