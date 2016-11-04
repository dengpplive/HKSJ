
using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.Repository;
using HKSJ.WBVV.Repository.Interface;
using HKSJ.Utilities;

namespace HKSJ.WBVV.Business
{
    /// <summary>
    /// 用户子专辑
    /// </summary>
    public class UserSpecialSonBusiness : BaseBusiness, IUserSpecialSonBusiness
    {
        private readonly IUserSpecialSonRepository _userSpecialSonRepository;
        public UserSpecialSonBusiness(IUserSpecialSonRepository userSpecialSonRepository)
        {
            this._userSpecialSonRepository = userSpecialSonRepository;
        }


        #region 传入参数

        #endregion

    }
}