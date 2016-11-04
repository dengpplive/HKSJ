using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.ViewModel.Client
{
    /// <summary>
    /// 用户空间 访客历史记录
    /// </summary>
    /// 
    [Serializable]
    public class UserVisitView
    {
        public UserVisitLog UserVisitLog { get; set; }
        public UserView UserView { get; set; }
    }
}
