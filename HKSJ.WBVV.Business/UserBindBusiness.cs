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


namespace HKSJ.WBVV.Business
{
    public class UserBindBusiness : BaseBusiness, IUserBindBusiness
    {
        private readonly IUserBindRepository _iuserBindRepository;


        public UserBindBusiness(IUserBindRepository iUserBindRepository)
        {
            this._iuserBindRepository = iUserBindRepository;
        }

        /// <summary>
        /// 是否绑定第三方帐号
        /// </summary>
        /// <param name="uniquelyId">第三方身份标识ID</param>
        /// <param name="typeCode">第三方类型</param>
        /// <returns></returns>
        public UserBind IsExistedThirdPartyById(string uniquelyId, string typeCode)
        {
            CheckIdNotNull(uniquelyId);
            CheckTypeCodeNotNull(typeCode);
            IQueryable<UserBind> queryable = this._iuserBindRepository.GetEntityList(CondtionEqualState());
            UserBind userBind = null;
            if ("qq" == typeCode)
                userBind = (from ub in queryable
                            where ub.TypeCode == "qq" && ub.RelatedId == uniquelyId
                            select ub).FirstOrDefault<UserBind>();
            else if ("sina" == typeCode)
                userBind = (from ub in this._iuserBindRepository.GetEntityList(CondtionEqualState())
                            where ub.TypeCode == "sina" && ub.RelatedId == uniquelyId
                            select ub).FirstOrDefault<UserBind>();
            return userBind;

        }






        #region 传入参数检测

        /// <summary>
        /// 检测第三方身份标识不为空
        /// </summary>
        /// <param name="id"></param>
        private void CheckIdNotNull(string id)
        {
            AssertUtil.IsNotNull(id);
        }
        /// <summary>
        /// 检测第三方类型编码
        /// </summary>
        /// <param name="typeCode"></param>
        private void CheckTypeCodeNotNull(string typeCode)
        {
            AssertUtil.IsNotNull(typeCode);
        }

        #endregion

        #region 传入参数

        /// <summary>
        /// 比较第三方标识
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private Condtion ConditionEqualRelatedId(string RelatedId)
        {
            var condtion = new Condtion()
            {
                FiledName = "RelatedId",
                FiledValue = RelatedId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }

        /// <summary>
        /// 比较第三方标识码
        /// </summary>
        /// <param name="TypeCode"></param>
        /// <returns></returns>
        private Condtion ConditionEqualTypeCode(string TypeCode)
        {
            var condtion = new Condtion()
            {
                FiledName = "TypeCode",
                FiledValue = TypeCode,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }


        #endregion
    }
}
