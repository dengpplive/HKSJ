using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HKSJ.WBVV.Common.Config;
using HKSJ.WBVV.Common.Language;
using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.Utilities;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.MVC.Client.GlobalVariable;
using HKSJ.WBVV.MVC.Client.Attribute;
using HKSJ.WBVV.MVC.Common;
using HKSJ.WBVV.Entity.ViewModel.Client;

namespace HKSJ.WBVV.MVC.Client.Controllers
{
    public class PlayController : BaseController
    {
        //
        // GET: /Player/

        public ActionResult Index()
        {
            ViewBag.Uid = GlobalMemberInfo.UserId;
            var videoId = GetVideoId();
            var model = new VideoDetailView();
            string title = LanguageUtil.Translate("web_Controllers_Play_Index_title");
            if (videoId > 0)
            {
                var url = WebConfig.BaseAddress + "Video/GetVideoDetailView?videoId=" + videoId + "&userId=" + GlobalMemberInfo.UserId;
                string strData = WebApiHelper.InvokeApi<string>(url);

                if (!string.IsNullOrEmpty(strData) && !strData.StartsWith("null"))
                    model = strData.JsonToEntity<VideoDetailView>();
                else
                    model = new VideoDetailView();
                title = !string.IsNullOrEmpty(model.Title) ? model.Title : "";
            }
            //视频的标题
            ViewBag.Title = title;
            ViewBag.VideoId = videoId;
            ViewBag.WatchTime = WebApiHelper.InvokeApi<long>("User/GetUserWatchTime?uId={0}&videoId={1}".F(GlobalMemberInfo.UserId, videoId)); //获取视频当前用户历史播放时间
            return View(model);
        }

        private int GetVideoId()
        {
            int videoId = -1;
            if (Request.QueryString["videoId"] != null
                && Request.QueryString["videoId"] != ""
                )
            {
                videoId = int.Parse(Request.QueryString["videoId"].ToString());
            }
            return videoId;
        }

        [Member]
        public ActionResult Check()
        {
            return View();
        }
    }
}
