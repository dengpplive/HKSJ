using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using HKSJ.WBVV.Repository.Interface;
using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Common.Logger;
using HKSJ.WBVV.Common.Language;


namespace HKSJ.WBVV.Business
{
    public class VideoPlayRecordBusiness : BaseBusiness, IVideoPlayRecordBusiness
    {
        private readonly IVideoPlayRecordRepository _iVideoPlayRecordRepository;
        private readonly IUserRepository _iUserRepository;
        private readonly IVideoRepository _iVideoRepository;

        public VideoPlayRecordBusiness(IVideoPlayRecordRepository iVideoPlayRecordRepository, IUserRepository iUserRepository, IVideoRepository iVideoRepository)
        {
            this._iVideoPlayRecordRepository = iVideoPlayRecordRepository;
            this._iUserRepository = iUserRepository;
            this._iVideoRepository = iVideoRepository;
        }

        /// <summary>
        /// 播放记录，登录情况下：每个用户当天播放同一个视频只计算一次，未登录：每个ip当天只记录一次
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        public bool AddVideoPlayRecord(int videoId)
        {
            //TODO update 刘强记录观看视频的播币统计
            this._iVideoRepository.IncomeWatch(UserId, videoId, IpAddress);
            var video = (from v in this._iVideoRepository.GetEntityList()
                         where v.State == false &&v.VideoState==3&& v.Id == videoId
                         select v).FirstOrDefault();
            //ToDo 更新索引
            var ivideoBusiness = ((Autofac.IContainer)HttpRuntime.Cache["containerKey"]).Resolve<IVideoBusiness>();
            ivideoBusiness.UpdateAVideoIndex(video);
            return true;
        }






        #region 传入参数检测
        /// <summary>
        /// 检测分类名称不为空
        /// </summary>
        /// <param name="name"></param>
        private void CheckNameNotNull(string name)
        {
            AssertUtil.NotNullOrWhiteSpace(name, LanguageUtil.Translate("api_Business_VideoPlayRecord_CheckNameNotNull_categoryNameIsNull"));
        }


        /// <summary>
        /// 检测编号不小于0
        /// </summary>
        /// <param name="id"></param>
        private void CheckIdBiggerZero(int id)
        {
            AssertUtil.AreBigger(id, 0,LanguageUtil.Translate("api_Business_VideoPlayRecord_CheckIdBiggerZero_idCannotbeLessThanZero") );
        }

        /// <summary>
        /// 检测用户是否存在
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="user"></param>
        private void CheckUserId(int userId, out User user)
        {
            IList<Condtion> condtions = new List<Condtion>()
            {
                CondtionEqualState(),
                ConditionEqualId(userId)
            };
            user = this._iUserRepository.GetEntity(condtions);
            AssertUtil.IsNotNull(user,LanguageUtil.Translate("api_Business_VideoPlayRecord_CheckUserId_userNotExistOrIsDisabled"));
        }

        private void CheckVideoId(int videoId, out Video video)
        {
            IList<Condtion> condtions = new List<Condtion>()
            {
                CondtionEqualState(),
                ConditionEqualId(videoId)
            };
            video = this._iVideoRepository.GetEntity(condtions);
            AssertUtil.IsNotNull(video, LanguageUtil.Translate("api_Business_VideoPlayRecord_CheckVideoId_videoNotExistOrIsDisabled"));
        }


        #endregion

        #region 传入参数

        /// <summary>
        /// 比较用户Id相等
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private Condtion ConditionEqualUserId(int userId)
        {
            var condtion = new Condtion()
            {
                FiledName = "UserId",
                FiledValue = userId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }
        /// <summary>
        /// 比较视频Id相等
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        private Condtion ConditionEqualVideoId(long videoId)
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
        /// <summary>
        /// 比较IP地址相等
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private Condtion ConditionEqualIpAddress(string ipAddress)
        {
            var condtion = new Condtion()
            {
                FiledName = "IpAddress",
                FiledValue = ipAddress,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }

        #endregion







    }
}
