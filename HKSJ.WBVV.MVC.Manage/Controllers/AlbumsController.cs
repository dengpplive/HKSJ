using System;
using System.Web.Mvc;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.MVC.Common;
using HKSJ.WBVV.MVC.Manage.Common.Base;
using Newtonsoft.Json;

namespace HKSJ.WBVV.MVC.Manage.Controllers
{
    public class AlbumsController : BaseController
    {
        //
        // GET: /Classification/

        /// <summary>
        /// 推荐专辑管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 选择专辑
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectAlbums()
        {
            return View();
        }

        #region 专辑管理 Author : Axone
        /// <summary>
        /// 移除推荐专辑
        /// </summary>
        /// <param name="albumIds"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RemoveRecommendAlbums(string albumIds)
        {
            var result = WebApiHelper.InvokeAxApi<ResultView<bool?>>("UserSpecial/RemoveRecommendAlbums", new { albumIds });
            if (result == null || result.Success == false || result.Data == false)
            {
                return Json(new { Success = false, ExceptionMessage = result == null || result.ExceptionMessage == null ? "" : result.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存推荐专辑排序
        /// </summary>
        /// <param name="albumIds"></param>
        /// <param name="sortNums"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SavaRecommendAlbumsSort(string albumIds, string sortNums)
        {
            var result = WebApiHelper.InvokeAxApi<ResultView<bool?>>("UserSpecial/SavaRecommendAlbumsSort", new { albumIds, sortNums });
            if (result == null || result.Success == false || result.Data == false)
            {
                return Json(new { Success = false, ExceptionMessage = result == null || result.ExceptionMessage == null ? "" : result.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加推荐专辑
        /// </summary>
        /// <param name="albumIds"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddRecommendAlbums(string albumIds)
        {
            var result = WebApiHelper.InvokeAxApi<ResultView<bool?>>("UserSpecial/AddRecommendAlbums", new { albumIds });
            if (result == null || result.Success == false || result.Data == false)
            {
                return Json(new { Success = false, ExceptionMessage = result == null || result.ExceptionMessage == null ? "" : result.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        } 
        #endregion

    }
}
