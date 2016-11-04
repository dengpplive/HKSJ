using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Repository.Interface;
using HKSJ.WBVV.Business.Search;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Logger;
using HKSJ.WBVV.Common.Language;
using HKSJ.WBVV.Entity.ViewModel.Manage;

namespace HKSJ.WBVV.Business
{
    public class VideoApproveBusiness:BaseBusiness,IVideoApproveBusiness
    {
        private readonly IVideoApproveRepository _iVideoApproveRepository;
        private ITagsBusiness _tagsBusiness;
        private IVideoRepository _videoRepository;
        private IVideoBusiness _videoBusiness;
        public VideoApproveBusiness(IVideoApproveRepository iVideoApproveRepository, IVideoRepository videoRepository)
        {
            this._iVideoApproveRepository = iVideoApproveRepository;
            this._videoRepository = videoRepository;
        }

        /// <summary>
        /// 添加一条视频审核记录
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool AddApproveInfo(VideoApproveView info)
        {
            if (info==null)
            {
                return false;
            }
           this._videoRepository.IncomeUpload(info.ApproveContent, info.ApproveRemark, info.VideoId, (info.Status==4),
                info.CreateAdminId);
           var video = (from v in this._videoRepository.GetEntityList()
                        where v.State == false && v.Id == info.VideoId
                        select v).FirstOrDefault();
           if (video!=null)
            {
                if (video.VideoState == 3)
                {
                    //TODO insert 刘强添加标签
                    try
                    {
                        this._tagsBusiness = ((Autofac.IContainer)HttpRuntime.Cache["containerKey"]).Resolve<ITagsBusiness>();
                        //上传视频的人
                        this._tagsBusiness.UserId = info.CreateAdminId;
                        this._tagsBusiness.AsyncCreateTags();
                    }
                    catch (Exception ex)
                    {
#if !DEBUG
                      LogBuilder.Log4Net.Error("更新标签失败", ex.MostInnerException());
#else
                        Console.WriteLine(LanguageUtil.Translate("api_Business_VideoApprove_AddApproveInfo_updateTagsFailed") + ex.MostInnerException().Message);
#endif
                    }
                }
                this._videoBusiness = ((Autofac.IContainer)HttpRuntime.Cache["containerKey"]).Resolve<IVideoBusiness>();
                this._videoBusiness.UpdateAVideoIndex(video);
                //审核时改变了分类
                video.CategoryId = info.CategoryId;
                this._videoRepository.UpdateEntity(video);
            }
            return true;
        }

        public bool UpdateApproveInfo(VideoApprove info)
        {
            return this._iVideoApproveRepository.UpdateEntity(info);
        }

        public string GetApproveContentByVideoId(long videoId)
        {
            var info=this._iVideoApproveRepository.GetEntity(CondtionEqualVideoId(videoId));
            if (info != null)
            {
                return info.ApproveContent;
            }
            return null;
        }

        #region 传入参数
        /// <summary>
        /// 视频ID编号是否相等
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        private Condtion CondtionEqualVideoId(long videoId)
        {
            var condtion = new Condtion()
            {
                FiledName = "VideoId",
                FiledValue = videoId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }
        #endregion
    }
}
