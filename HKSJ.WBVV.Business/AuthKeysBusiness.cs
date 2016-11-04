using System;
using System.Collections.Generic;
using HKSJ.Utilities.Base.Security;
using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.Enums;
using HKSJ.WBVV.Repository.Interface;

namespace HKSJ.WBVV.Business
{
    public class AuthKeysBusiness : BaseBusiness, IAuthKeysBusiness
    {
        private readonly IAuthKeysRepository _authKeysRepository;

        public AuthKeysBusiness(IAuthKeysRepository authKeysRepository)
        {
            _authKeysRepository = authKeysRepository;
        }

        public AuthKeys GetAuthKeys(int uid, AuthUserType userType)
        {
            AuthKeys authKeys;
            CheckKey(uid, userType, out authKeys);
            return authKeys ?? new AuthKeys();
        }

        public string CreatePublicKey(int uid, AuthUserType userType)
        {
            return _authKeysRepository.CreatePublicKey(uid, userType);
        }

        #region private method
        private void CheckKey(int uid, AuthUserType userType, out AuthKeys authKeys)
        {
            var condtions = new List<Condtion>
            {
                new Condtion
                {
                    FiledName = "UserId",
                    FiledValue = uid,
                    ExpressionLogic = ExpressionLogic.And,
                    ExpressionType = ExpressionType.Equal
                },
                new Condtion
                {
                    FiledName = "UserType",
                    FiledValue = (int)userType,
                    ExpressionLogic = ExpressionLogic.And,
                    ExpressionType = ExpressionType.Equal
                }
            };
            authKeys = _authKeysRepository.GetEntity(condtions);
        }

        #endregion

    }
}