using System.Web.Mvc;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.MVC.Common;
using HKSJ.WBVV.MVC.Manage.Common.Base;

namespace HKSJ.WBVV.MVC.Manage.Controllers
{
    public class SystemMessageController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        #region 系统消息管理 Author : Axone
        [HttpPost]
        public JsonResult CreateSystemMessage(int uid, short utype, string suser, string messageDesc)
        {
            var result = WebApiHelper.InvokeAxApi<ResultView<int?>>("SystemMessage/CreateSystemMessage", new { loginUserId = uid, userByType = utype, selectUser = suser, messageDesc = messageDesc });
            if (result == null || result.Success == false || result.Data < 1)
            {
                return Json(new { Success = false, ExceptionMessage = result == null || result.ExceptionMessage == null ? "" : result.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
