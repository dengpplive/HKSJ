using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Business.Interface.APP;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.Response.App;
using HKSJ.WBVV.Repository.Interface;
using Lucene.Net.Search;

namespace HKSJ.WBVV.Business.APP
{
    /// <summary>
    /// 频道
    /// </summary>
    public class ChannelBusiness : BaseBusiness, IChannelBusiness
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserFansRepository _userFansRepository;
        private readonly ICategoryRepository _categoryRepository;
        public ChannelBusiness(IUserRepository userRepository, IUserFansRepository userFansRepository, ICategoryRepository categoryRepository)
        {
            _userRepository = userRepository;
            _userFansRepository = userFansRepository;
            _categoryRepository = categoryRepository;
        }

        #region 分类列表
        /// <summary>
        /// 分类列表
        /// </summary>
        /// <param name="pageSize">显示多少行</param>
        /// <param name="pageIndex">显示第几页</param>
        /// <returns></returns>
        public AppCategorysView Category(int pageSize, int pageIndex)
        {
            AppCategorysView categorys = new AppCategorysView()
            {
                Categorys = new List<AppCategoryView>()
            };
            IQueryable<AppCategoryView> tags = (from c in this._categoryRepository.GetEntityList(CondtionEqualState())
                                                orderby c.CreateTime, c.SortNum descending
                                                select new AppCategoryView()
                                                {
                                                    Id = c.Id,
                                                    Name = c.Name
                                                });
            categorys.Categorys = PageList(tags, pageSize, pageIndex);
            return categorys;
        }
        #endregion

        #region 频道
        /// <summary>
        /// 频道视图数据
        /// </summary>
        /// <param name="loginUserId">登录用户编号（可空）</param>
        /// <returns></returns>
        public AppChannelView Channel(int? loginUserId)
        {
            AppChannelView channel = new AppChannelView()
            {
                Categorys = Category(10,1).Categorys,
                UserInfos = new List<AppUserSimpleView>()
            };
            var userId = loginUserId.HasValue ? Convert.ToInt32(loginUserId) : 0;
            int pageSize = 12;
            if (userId > 0)
            {
                channel.UserInfos = (from u in this._userRepository.GetEntityList(CondtionEqualState())
                                     where IsSubed(this._userFansRepository.GetEntityList(), u.Id, userId) == false
                                     orderby u.FansCount descending
                                     select UserEasyView(u)).Take(pageSize).ToList();
            }
            else
            {
                channel.UserInfos = (from u in this._userRepository.GetEntityList(CondtionEqualState())
                                     orderby u.FansCount descending
                                     select UserEasyView(u)).Take(pageSize).ToList();
            }
            return channel;
        }
        #endregion
    }
}
