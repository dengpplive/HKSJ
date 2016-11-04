using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Repository.Interface;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Business
{
    public class PraisesBusiness : BaseBusiness, IPraisesBusiness
    {
        private readonly IPraisesRepository _praisesRepository;
        private readonly IUserRepository _userRepository;
        private readonly IVideoRepository _videoRepository;
        public PraisesBusiness(IPraisesRepository praisesRepository, IUserRepository userRepository, IVideoRepository videoRepository)
        {
            this._praisesRepository = praisesRepository;
            this._userRepository = userRepository;
            this._videoRepository = videoRepository;
        }

        #region 赞
        /// <summary>
        /// 赞评论
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public int CreatePraisesComment(int userId, int commentId)
        {
            return this._praisesRepository.PraisesComment(userId, commentId);
        }
        /// <summary>
        /// 取消赞评论
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public bool CancelPraisesComment(int userId, int commentId)
        {
            return this._praisesRepository.CancelPraisesComment(userId, commentId);
        }
        /// <summary>
        /// 赞视频
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="vedioId"></param>
        /// <returns></returns>
        public int CreatePraisesVedio(int userId, int vedioId)
        {
            return this._praisesRepository.PraisesVedio(userId, vedioId);
        }
        #endregion

        #region 传入参数检测
        /// <summary>
        /// 检测用户编号不能小于0
        /// </summary>
        /// <param name="userId"></param>
        private void CheckUserId(int userId)
        {
            AssertUtil.AreBigger(userId, 0, LanguageUtil.Translate("api_Business_Praises_CheckUserId"));
        }
        /// <summary>
        /// 检测用户是否存在
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="user"></param>
        private void CheckUserId(int userId, out User user)
        {
            user = this._userRepository.GetEntity(ConditionEqualId(userId));
            AssertUtil.IsNotNull(user, LanguageUtil.Translate("api_Business_Praises_CheckUserId_user"));
        }


        /// <summary>
        /// 检测评论编号不小于0
        /// </summary>
        /// <param name="commentId"></param>
        private void CheckCommentId(int commentId)
        {
            AssertUtil.AreBigger(commentId, 0, LanguageUtil.Translate("api_Business_Praises_CheckCommentId"));
        }

        /// <summary>
        /// 检测视频编号不小于0
        /// </summary>
        /// <param name="vedioId"></param>
        private void CheckVedioId(int vedioId)
        {
            AssertUtil.AreBigger(vedioId, 0, LanguageUtil.Translate("api_Business_Praises_CheckVedioId"));
        }

        /// <summary>
        /// 检测评论编号不小于0
        /// </summary>
        /// <param name="vedioId"></param>
        /// <param name="video"></param>
        private void CheckVedioId(int vedioId, out Video video)
        {
            video = this._videoRepository.GetEntity(ConditionEqualId(vedioId));
            AssertUtil.IsNotNull(video, LanguageUtil.Translate("api_Business_Praises_CheckVedioId_video"));
        }
        /// <summary>
        /// 检测是否已经赞过
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="themeId"></param>
        /// <param name="themeTypeId"></param>
        private void CheckComment(int userId, int themeId, int themeTypeId)
        {
            var condtions = new List<Condtion>()
            {
                CondtionEqualState(true),
                CondtionEqualCreateUserId(userId),
                CondtionEqualThemeId(themeId),
                CondtionEqualThemeTypeId(themeTypeId)
            };
            var praises = this._praisesRepository.GetEntity(condtions);
            AssertUtil.IsNull(praises, LanguageUtil.Translate("api_Business_Praises_CheckComment"));
        }

        #endregion

        #region 传入参数
        /// <summary>
        /// 比较赞的类型相等
        /// </summary>
        /// <param name="themeId"></param>
        /// <returns></returns>
        private Condtion CondtionEqualThemeId(int themeId)
        {
            var condtion = new Condtion()
            {
                FiledName = "ThemeId",
                FiledValue = themeId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }
        /// <summary>
        /// 比较赞的类型编号相等
        /// </summary>
        /// <param name="themeTypeId"></param>
        /// <returns></returns>
        private Condtion CondtionEqualThemeTypeId(int themeTypeId)
        {
            var condtion = new Condtion()
            {
                FiledName = "ThemeTypeId",
                FiledValue = themeTypeId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }
        /// <summary>
        /// 比较用户编号相等
        /// </summary>
        /// <param name="createUserId"></param>
        /// <returns></returns>
        private Condtion CondtionEqualCreateUserId(int createUserId)
        {
            var condtion = new Condtion()
            {
                FiledName = "CreateUserId",
                FiledValue = createUserId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }
        #endregion

        #region 排序参数


        #endregion
    }
}
