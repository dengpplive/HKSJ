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
using HKSJ.WBVV.Entity.Tables;
using HKSJ.WBVV.Entity.ViewModel.Client;

namespace HKSJ.WBVV.Business.Interface
{
    public interface IUserSkinBusiness : IBaseBusiness
    {
        IList<UserSkinView> GetUserSkinList();

        UserSkinView CreateSkin(string skinName, string smallImage, string cssPath, int skinType = 0, bool isDefaultSkin = false);
    }
}
