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
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Business
{
    public class PlateBusiness : BaseBusiness, IPlateBusiness
    {
        private readonly IPlateRepository _plateRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPlateVideoRepository _plateVideoRepository;
        private readonly IVideoRepository _videoRepository;
        private readonly IManageRepository _manageRepository;
        public PlateBusiness(
            IPlateRepository plateRepository,
            ICategoryRepository categoryRepository,
            IPlateVideoRepository plateVideoRepository,
            IVideoRepository videoRepository,
            IManageRepository manageRepository
            )
        {
            this._plateRepository = plateRepository;
            this._categoryRepository = categoryRepository;
            this._plateVideoRepository = plateVideoRepository;
            this._videoRepository = videoRepository;
            this._manageRepository = manageRepository;
        }

        #region manage

        private IQueryable<Plate> GetPlateList()
        {
            return this._plateRepository.GetEntityList();
        }

        #region 板块列表
        /// <summary>
        /// 获取板块分页集合
        /// </summary>
        /// <param name="condtions">查询条件</param>
        /// <param name="orderCondtions">排序条件</param>
        public PageResult GetPlatePageResult(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            int totalCount = 0;
            int totalIndex = 0;
            IList<HKSJ.WBVV.Entity.ViewModel.Manage.PlateView> plateViews = GetPlateList(condtions, orderCondtions, out totalCount, out totalIndex);
            return new PageResult()
            {
                PageSize = this.PageSize,
                PageIndex = this.PageIndex,
                TotalCount = totalCount,
                Data = plateViews
            };
        }

        /// <summary>
        /// 获取板块集合
        /// </summary>
        /// <param name="condtions">查询条件</param>
        /// <param name="orderCondtions">排序条件</param>
        /// <param name="totalCount">返回总的行数</param>
        /// <param name="totalIndex">返回总的页数</param>
        /// <returns></returns>
        public IList<HKSJ.WBVV.Entity.ViewModel.Manage.PlateView> GetPlateList(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions, out int totalCount, out int totalIndex)
        {
            var plate = (from p in GetPlateList()
                         join m in this._manageRepository.GetEntityList(CondtionEqualState()) on p.CreateManageId equals m.Id
                         join c in this._categoryRepository.GetEntityList() on p.CategoryId equals c.Id
                             into cjoin
                         from cate in cjoin.DefaultIfEmpty()
                         join mm in this._manageRepository.GetEntityList(CondtionEqualState()) on p.UpdateManageId equals mm.Id
                         into mmJoin
                         from manage in mmJoin.DefaultIfEmpty()
                         select new HKSJ.WBVV.Entity.ViewModel.Manage.PlateView()
                         {
                             Id = p.Id,
                             CategoryId = p.CategoryId,
                             CategoryName = cate == null ? LanguageUtil.Translate("api_Business_Plate_GetPlateList_CategoryName") : cate.Name,
                             Name = p.Name,
                             PageSize = p.PageSize,
                             SortNum = p.SortNum,
                             CreateManageName = m.LoginName,
                             CreateTime = p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                             UpdateManageName = manage == null ? "" : manage.LoginName,
                             UpdateTime = p.UpdateTime.HasValue ? Convert.ToDateTime(p.UpdateTime).ToString("yyy-MM-dd HH:mm:ss") : "",
                             KeyWord = p.KeyWord,
                             State = (p.State ? LanguageUtil.Translate("api_Business_Plate_GetPlateList_State_Disabled") : LanguageUtil.Translate("api_Business_Plate_GetPlateList_State_Enable"))
                         }).AsQueryable();
            if (condtions != null && condtions.Count > 0)//查询条件
            {
                plate = plate.Query(condtions);
            }
            if (orderCondtions != null && orderCondtions.Count > 0)//排序条件
            {
                plate = plate.OrderBy(orderCondtions);
            }
            bool isExists = plate.Any();
            totalCount = isExists ? plate.Count() : 0;
            if (this.PageSize <= 0)
            {
                totalIndex = 0;
                var queryable = isExists
                    ? plate.ToList()
                    : new List<Entity.ViewModel.Manage.PlateView>();
                return queryable;
            }
            else
            {
                totalIndex = totalCount % this.PageSize == 0
                    ? (totalCount / this.PageSize)
                    : (totalCount / this.PageSize + 1);
                if (this.PageIndex <= 0)
                {
                    this.PageIndex = 1;
                }
                if (this.PageIndex >= totalIndex)
                {
                    this.PageIndex = totalIndex;
                }

                var queryable = isExists
                    ? plate.Skip((this.PageIndex - 1) * this.PageSize).Take(this.PageSize).ToList()
                    : new List<Entity.ViewModel.Manage.PlateView>();

                return queryable;
            }
        }
        #endregion

        #region 单个板块信息
        /// <summary>
        /// 单个板块信息
        /// </summary>
        /// <returns></returns>
        public Plate GetPlate(int id)
        {
            return id <= 0 ? new Plate() : this._plateRepository.GetEntity(ConditionEqualId(id));
        }

        #endregion

        #region 添加板块
        /// <summary>
        /// 添加板块
        /// </summary>
        /// <param name="categoryId">分类编号</param>
        /// <param name="name">板块名称</param>
        /// <param name="sortNum">排序数量</param>
        /// <param name="pageSize">显示多少条</param>
        /// <returns></returns>
        public int CreatePlate(int categoryId, string name, int sortNum, int pageSize)
        {
            categoryId = categoryId <= 0 ? 0 : categoryId;
            CheckName(name);
            CheckPageSize(pageSize);
            Category category = null;
            if (categoryId > 0)
            {
                CheckCategoryId(categoryId);
                CheckCategoryId(categoryId, out category);
            }
            CheckName(name, category);
            var plate = new Plate()
            {
                CategoryId = categoryId,
                CreateManageId = 1,
                CreateTime = DateTime.Now,
                KeyWord = PinyinHelper.PinyinString(name),
                PageSize = pageSize,
                Name = name,
                SortNum = sortNum
            };
            this._plateRepository.CreateEntity(plate);
            return plate.Id;
        }

        #endregion

        #region 修改板块
        /// <summary>
        /// 修改板块信息
        /// </summary>
        /// <param name="id">板块编号</param>
        /// <param name="categoryId">分类编号</param>
        /// <param name="name">板块名称</param>
        /// <param name="sortNum">排序数量</param>
        /// <param name="pageSize">显示行数</param>
        /// <returns></returns>
        public bool UpdatePlate(int id, int categoryId, string name, int sortNum, int pageSize)
        {
            CheckId(id);
            CheckName(name);
            CheckPageSize(pageSize);
            categoryId = categoryId <= 0 ? 0 : categoryId;
            Category category = null;
            if (categoryId > 0)
            {
                CheckCategoryId(categoryId);
                CheckCategoryId(categoryId, out category);
            }
            Plate plate;
            CheckId(id, out plate);
            CheckName(id, name, category);
            plate.CategoryId = categoryId;
            plate.Name = name;
            plate.KeyWord = PinyinHelper.PinyinString(name);
            plate.UpdateManageId = 1;
            plate.UpdateTime = DateTime.Now;
            plate.SortNum = sortNum;
            plate.PageSize = pageSize;
            return this._plateRepository.UpdateEntity(plate);
        }
        #endregion

        #region 板块排序
        /// <summary>
        /// 板块排序
        /// </summary>
        /// <param name="p"></param>
        public bool UpdatePlateSort(IList<Plate> plateList)
        {
            IList<Plate> list = new List<Plate>();
            Plate plate;
            foreach (Plate item in plateList)
            {
                CheckId(item.Id, out plate);
                plate.SortNum = item.SortNum;
                list.Add(plate);
            }
            this._plateRepository.UpdateEntitys(list);
            return true;
        }
        #endregion

        #region 删除板块
        /// <summary>
        /// 删除板块
        /// </summary>
        /// <param name="id">板块编号</param>
        /// <returns></returns>
        public bool DeletePlate(int id)
        {
            CheckId(id);
            Plate plate;
            CheckId(id, out plate);
            CheckHasVideo(plate);
            var success = this._plateRepository.DeleteEntity(plate);
            var plateVideos = (from pv in this._plateVideoRepository.GetEntityList()
                               where pv.PlateId == plate.Id
                               select pv).AsQueryable();
            if (plateVideos.Any())
            {
                this._plateVideoRepository.DeleteEntitys(plateVideos.ToList());
            }
            return success;
        }

        #endregion

        #region 删除多个板块

        /// <summary>
        /// 删除多个板块
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeletePlates(IList<int> ids)
        {
            CheckId(ids);
            IList<Plate> plates;
            CheckId(ids, out plates);
            IList<Plate> plateList;
            CheckHasVideo(plates, out plateList);
            this._plateRepository.DeleteEntitys(plateList);
            var plateVideos = new List<PlateVideo>();
            foreach (var plate in plateList)
            {
                var plateVideoList = (from pv in this._plateVideoRepository.GetEntityList()
                                      where pv.PlateId == plate.Id
                                      select pv).AsQueryable();
                if (plateVideoList.Any())
                {
                    plateVideos.AddRange(plateVideoList.ToList());
                }
            }
            if (plateVideos.Count > 0)
            {
                this._plateVideoRepository.DeleteEntitys(plateVideos);
            }
            return true;
        }

        #endregion

        #endregion

        #region 获取首页板块信息
        /// <summary>
        /// 获取首页显示板块信息
        /// </summary>
        /// <returns></returns>
        public IList<PlateView> GetPlateViewByHomeList()
        {
            return GetPlateViewByCategoryIdList(0);
        }

        #endregion

        #region 获取指定分类下的板块信息
        /// <summary>
        /// 获取指定分类下的板块信息
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public IList<PlateView> GetPlateViewByCategoryIdList(int categoryId)
        {
            IList<PlateView> plateViews = new List<PlateView>();
            if (categoryId < 0)
            {
                return plateViews;
            }
            var query = (from p in this._plateRepository.GetEntityList(CondtionEqualState())
                         join c in this._categoryRepository.GetEntityList(CondtionEqualState()) on p.CategoryId equals c.Id
                         into cJoin
                         from cate in cJoin.DefaultIfEmpty()
                         where p.CategoryId == categoryId
                         orderby p.SortNum descending, p.CreateTime descending
                         select new PlateView()
                         {
                             Id = p.Id,
                             Name = p.Name,
                             CategoryId = cate == null ? -1 : cate.Id,
                             CategoryName = cate == null ? "" : cate.Name
                         }).AsQueryable();

            plateViews = query.Any() ? query.ToList() : new List<PlateView>();
            return plateViews;
        }
        #endregion

        #region 传入参数检测
        /// <summary>
        /// 检测板块编号是否传入
        /// </summary>
        /// <param name="ids"></param>
        private void CheckId(IList<int> ids)
        {
            AssertUtil.IsNotEmptyCollection(ids, LanguageUtil.Translate("api_Business_Plate_CheckId"));
        }
        /// <summary>
        /// 检测板块是否存在,并且返回存在的板块信息
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="plates"></param>
        private void CheckId(IList<int> ids, out IList<Plate> plates)
        {
            IList<Plate> plateList = new List<Plate>();
            foreach (var id in ids)
            {
                IList<Condtion> condtions = new List<Condtion>()
                {
                    CondtionEqualState(),
                    ConditionEqualId(id)
                };
                var plate = this._plateRepository.GetEntity(condtions);
                if (plate != null)
                {
                    plateList.Add(plate);
                }
            }
            plates = plateList;
            AssertUtil.IsNotEmptyCollection(plateList, LanguageUtil.Translate("api_Business_Plate_CheckId_plates"));
        }

        /// <summary>
        /// 检测板块下是否存在视频，并且取出不存在视频的板块信息
        /// </summary>
        /// <param name="plates"></param>
        /// <param name="outPlate"></param>
        private void CheckHasVideo(IList<Plate> plates, out IList<Plate> outPlate)
        {
            IList<Plate> plateList = new List<Plate>();
            foreach (var plate in plates)
            {
                var vedio = (from pv in this._plateVideoRepository.GetEntityList()
                             join v in this._videoRepository.GetEntityList(CondtionEqualState()) on pv.VideoId equals v.Id
                             where pv.PlateId == plate.Id && v.VideoState == 3 //TODO 刘强CheckState=1
                             select v).AsQueryable();
                if (!vedio.Any())
                {
                    plateList.Add(plate);
                }
            }
            outPlate = plateList;
            AssertUtil.IsNotEmptyCollection(plateList, LanguageUtil.Translate("api_Business_Plate_CheckHasVideo"));
        }

        /// <summary>
        /// 检测板块编号大于0
        /// </summary>
        /// <param name="id"></param>
        private void CheckId(int id)
        {
            AssertUtil.AreBigger(id, 0, LanguageUtil.Translate("api_Business_Plate_CheckId_AreBigger"));
        }
        /// <summary>
        /// 检测编号是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <param name="plate"></param>
        private void CheckId(int id, out Plate plate)
        {
            IList<Condtion> condtions = new List<Condtion>()
            {
                CondtionEqualState(),
                ConditionEqualId(id)
            };
            plate = this._plateRepository.GetEntity(condtions);
            AssertUtil.IsNotNull(plate, LanguageUtil.Translate("api_Business_Plate_CheckId_plate").F(id));
        }
        /// <summary>
        /// 检测板块是否有视频
        /// </summary>
        /// <param name="plate"></param>
        private void CheckHasVideo(Plate plate)
        {
            var vedio = (from pv in this._plateVideoRepository.GetEntityList()
                         join p in this._plateRepository.GetEntityList(CondtionEqualState()) on pv.PlateId equals p.Id
                         join v in this._videoRepository.GetEntityList(CondtionEqualState()) on pv.VideoId equals v.Id
                         where p.Id == plate.Id
                         select v).AsQueryable();
            AssertUtil.IsFalse(vedio.Any(), LanguageUtil.Translate("api_Business_Plate_CheckHasVideo_vedio").F(plate.Name, vedio.Count()));
        }

        /// <summary>
        /// 检测分类编号大于0
        /// </summary>
        /// <param name="categoryId"></param>
        private void CheckCategoryId(int categoryId)
        {
            AssertUtil.AreBigger(categoryId, 0, LanguageUtil.Translate("api_Business_Plate_CheckCategoryId"));
        }

        /// <summary>
        /// 检测分类是否存在
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="category"></param>
        private void CheckCategoryId(int categoryId, out Category category)
        {
            var condtions = new List<Condtion>()
            {
                CondtionEqualState(),
                ConditionEqualId(categoryId)
            };
            category = this._categoryRepository.GetEntity(condtions);
            AssertUtil.IsNotNull(category, LanguageUtil.Translate("api_Business_Plate_CheckCategoryId_category").F(category.Name));
        }
        /// <summary>
        /// 检测板块名称不能为空
        /// </summary>
        /// <param name="name"></param>
        private void CheckName(string name)
        {
            AssertUtil.NotNullOrWhiteSpace(name, LanguageUtil.Translate("api_Business_Plate_CheckName"));
        }
        /// <summary>
        /// 检测分类下板块名称不能相同
        /// </summary>
        /// <param name="name"></param>
        /// <param name="category"></param>
        private void CheckName(string name, Category category)
        {
            category = category ?? new Category() { Id = 0, Name = LanguageUtil.Translate("api_Business_Plate_CheckName_notSame_Name") };
            IList<Condtion> condtions = new List<Condtion>()
           {
               CondtionEqualState(),
               CondtionEqualName(name),
               CondtionEqualCategoryId(category.Id)
           };
            var plate = this._plateRepository.GetEntity(condtions);
            AssertUtil.IsNull(plate, LanguageUtil.Translate("api_Business_Plate_CheckName_notSame_plate").F(category.Name, name));
        }
        /// <summary>
        /// 检测板块名称是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="category"></param>
        private void CheckName(int id, string name, Category category)
        {
            category = category ?? new Category() { Id = 0, Name = LanguageUtil.Translate("api_Business_Plate_CheckName_isExist_name") };
            IList<Condtion> condtions = new List<Condtion>()
           {
               CondtionEqualState(),
               CondtionEqualName(name),
               CondtionEqualCategoryId(category.Id),
               CondtionNotEqualId(id)
           };
            var plate = this._plateRepository.GetEntity(condtions);
            AssertUtil.IsNull(plate, LanguageUtil.Translate("api_Business_Plate_CheckName_isExist_plate").F(category.Name, name));
        }

        /// <summary>
        /// 检测显示数量大于0
        /// </summary>
        /// <param name="pageSize"></param>
        private void CheckPageSize(int pageSize)
        {
            AssertUtil.AreBigger(pageSize, 0, LanguageUtil.Translate("api_Business_Plate_CheckPageSize"));
        }

        #endregion

        #region 传入参数
        /// <summary>
        /// 比较分类编号相等
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        private Condtion CondtionEqualCategoryId(int categoryId)
        {
            var condtion = new Condtion()
            {
                FiledName = "CategoryId",
                FiledValue = categoryId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }
        /// <summary>
        /// 比较板块名称相等
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Condtion CondtionEqualName(string name)
        {
            var condtion = new Condtion()
            {
                FiledName = "Name",
                FiledValue = name,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }
        /// <summary>
        /// 比较板块编号不相等
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Condtion CondtionNotEqualId(int id)
        {
            var condtion = new Condtion()
            {
                FiledName = "Id",
                FiledValue = id,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.NotEqual
            };
            return condtion;
        }
        #endregion

        #region 排序参数

        #endregion
    }
}
