using System.Web.Mvc;
using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.MVC.Common;
using HKSJ.WBVV.MVC.Manage.Common;
using HKSJ.WBVV.MVC.Manage.Common.Base;

namespace HKSJ.WBVV.MVC.Manage.Controllers
{
    public class ManageController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 分类管理
        /// </summary>
        /// <returns></returns>
        public ActionResult ClassifyManage()
        {
            return View();
        }

        /// <summary>
        /// 属性数据管理
        /// </summary>
        /// <returns></returns>
        public ActionResult PropertyDataManagement()
        {
            return View();
        }

        #region 分类管理 Author : Axone
        /// <summary>
        /// 添加分类
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateCategory(string name, int pid)
        {
            var result = WebApiHelper.InvokeAxApi<ResultView<int?>>("Category/CreateCategory", new { name, pid });
            if (result == null || result.Success == false || result.Data == 0)
            {
                return Json(new { Success = false, ExceptionMessage = result == null || result.ExceptionMessage == null ? "" : result.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true, result.Data }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改分类
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateCategory(string name, int id)
        {
            var result = WebApiHelper.InvokeAxApi<ResultView<bool?>>("Category/UpdateCategory", new { name, id });
            if (result == null || result.Success == false || result.Data == false)
            {
                return Json(new { Success = false, ExceptionMessage = result == null || result.ExceptionMessage == null ? "" : result.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteCategory(int id)
        {
            var result = WebApiHelper.InvokeAxApi<ResultView<bool?>>("Category/DeleteCategory", new { id });
            if (result == null || result.Success == false || result.Data == false)
            {
                return Json(new { Success = false, ExceptionMessage = result == null || result.ExceptionMessage == null ? "" : result.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 属性数据管理 Author : Axone
        /// <summary>
        /// 增加字典
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateDictionary(string name, int pid)
        {
            var result = WebApiHelper.InvokeAxApi<ResultView<int?>>("Dictionary/CreateDictionary", new { name, pid });
            if (result == null || result.Success == false || result.Data == 0)
            {
                return Json(new { Success = false, ExceptionMessage = result == null || result.ExceptionMessage == null ? "" : result.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true, result.Data }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 增加节点
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateDictionaryItem(string name, int pid)
        {
            var result = WebApiHelper.InvokeAxApi<ResultView<int?>>("DictionaryItem/CreateDictionaryItem", new { name, pid });
            if (result == null || result.Success == false || result.Data == 0)
            {
                return Json(new { Success = false, ExceptionMessage = result == null || result.ExceptionMessage == null ? "" : result.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true, result.Data }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 修改字典
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateDictionary(string name, int id)
        {
            var result = WebApiHelper.InvokeAxApi<ResultView<bool?>>("Dictionary/UpdateDictionary", new { name, id });
            if (result == null || result.Success == false || result.Data == false)
            {
                return Json(new { Success = false, ExceptionMessage = result == null || result.ExceptionMessage == null ? "" : result.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改节点
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateDictionaryItem(string name, int id)
        {
            var result = WebApiHelper.InvokeAxApi<ResultView<bool?>>("DictionaryItem/UpdateDictionaryItem", new { name, id });
            if (result == null || result.Success == false || result.Data == false)
            {
                return Json(new { Success = false, ExceptionMessage = result == null || result.ExceptionMessage == null ? "" : result.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除字典（包含子节点一并删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteDictionary(int id)
        {
            var result = WebApiHelper.InvokeAxApi<ResultView<bool?>>("Dictionary/DeleteDictionary", new { id });
            if (result == null || result.Success == false || result.Data == false)
            {
                return Json(new { Success = false, ExceptionMessage = result == null || result.ExceptionMessage == null ? "" : result.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteDictionaryItem(int id)
        {
            var result = WebApiHelper.InvokeAxApi<ResultView<bool?>>("DictionaryItem/DeleteDictionaryItem", new { id });
            if (result == null || result.Success == false || result.Data == false)
            {
                return Json(new { Success = false, ExceptionMessage = result == null || result.ExceptionMessage == null ? "" : result.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 修改密码
        [HttpPost]
        public JsonResult ChangePwd(string oldPwd, string newPwd, string confirmPwd)
        {
            var result = WebApiHelper.InvokeAxApi<Result>("Manage/ChangePwd", new { id = WebContext.Manage.Id, oldPwd, newPwd, confirmPwd });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}