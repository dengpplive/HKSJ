using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using HKSJ.WBVV.Business.Interface.Base;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ApiParaModel;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Common;
using HKSJ.WBVV.Entity.ViewModel.Client;
using Newtonsoft.Json.Linq;
using VideoView = HKSJ.WBVV.Entity.ViewModel.Client.VideoView;
using HKSJ.WBVV.Entity.ViewModel.Manage;
using HKSJ.WBVV.Common.Extender.LinqExtender;

namespace HKSJ.WBVV.Business.Interface
{
    public interface ILanguageBusiness : IBaseBusiness
    {
        Hashtable GetAllTexts();
    }
}
