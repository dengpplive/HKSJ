using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Business.Interface.Base;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;

namespace HKSJ.WBVV.Business.Interface
{
    public interface IBannerVideoBusiness : IBaseBusiness
    {
        IList<VideoView> GetBannerVideoList(int categoryId, IList<OrderCondtion> orderCondtions);
        bool UpdateBannerVideo(int id, int categoryId, int videoId, string bannerImagePath, string bannerSmallImagePath, int sortNum);
        PageResult GetBannerVideoPageResult(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions);
        int CreateBannerVideo(string bannerImagePath, string bannerSmallImagePath, int categoryId, int sortNum, int videoId);
        BannerVideo GetBannerVideo(int id);
        bool DeleteBannerVideo(int id);
        bool DeleteBannerVideos(IList<int> ids);
        bool UpdateBannerVideoSort(IList<BannerVideo> bannerVideoList);
    }
}
