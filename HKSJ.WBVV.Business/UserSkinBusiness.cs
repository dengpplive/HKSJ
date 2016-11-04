using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Xml.Serialization;
using HKSJ.Utilities;
using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity.ViewModel.Manage;
using HKSJ.WBVV.Repository.Interface;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using PlateView = HKSJ.WBVV.Entity.ViewModel.Client.PlateView;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.Entity.Tables;

namespace HKSJ.WBVV.Business
{
    public class UserSkinBusiness : BaseBusiness, IUserSkinBusiness
    {
        private readonly IUserSkinRepository _userSkinRepository;
        public UserSkinBusiness(IUserSkinRepository userSkinRepository)
        {
            this._userSkinRepository = userSkinRepository;
        }

        #region 传入参数
        /// <summary>
        /// 比较分类编号相等
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        private Condtion CondtionEqualUserId(int userId)
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
        #endregion

        IList<UserSkinView> IUserSkinBusiness.GetUserSkinList()
        {
            return this._userSkinRepository.GetEntityList().Select(p => new UserSkinView()
            {
                Id = p.Id,
                SkinName = p.SkinName,
                SmallImage = p.SmallImage,
                CssPath = p.CssPath,
                SkinType = p.SkinType,
                CreateTime = p.CreateTime,
                IsDefaultSkin = p.IsDefaultSkin
            }).ToList();
        }
        /// <summary>
        /// 添加皮肤
        /// </summary>
        /// <param name="skinName"></param>
        /// <param name="skinType"></param>
        /// <returns></returns>
        public UserSkinView CreateSkin(string skinName, string smallImage, string cssPath, int skinType = 0, bool isDefaultSkin = false)
        {
            var userSkin = new UserSkin()
            {
                SkinName = skinName,
                SmallImage = smallImage,
                CssPath = cssPath,
                CreateTime = System.DateTime.Now,
                SkinType = skinType,
                IsDefaultSkin = isDefaultSkin
            };
            this._userSkinRepository.CreateEntity(userSkin);
            return new UserSkinView()
            {
                Id = userSkin.Id,
                SkinName = userSkin.SkinName,
                SmallImage = userSkin.SmallImage,
                CssPath = userSkin.CssPath,
                SkinType = userSkin.SkinType,
                CreateTime = userSkin.CreateTime,
                IsDefaultSkin = userSkin.IsDefaultSkin
            };
        }
    }
}
