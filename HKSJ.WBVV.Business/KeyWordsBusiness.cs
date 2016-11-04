using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Repository.Interface;
using HKSJ.WBVV.Entity.ViewModel;

namespace HKSJ.WBVV.Business
{
    public class KeyWordsBusiness:BaseBusiness, IKeyWordsBusiness
    {
        private readonly IKeyWordsRepository _keyWordsRepository;
        public KeyWordsBusiness(IKeyWordsRepository iKeyWordsRepository)
        {
            this._keyWordsRepository = iKeyWordsRepository;
        }

        public bool AddOrUpdateAKeyWord(string  keyword)
        {
            if (keyword.IsNullOrWhiteSpace())
            {
                return false;
            }
            var model = this._keyWordsRepository.GetEntity(CondtionEqualKeyword(keyword));
            if (model == null)
            {
                var info = new KeyWords()
                {
                    Keyword = keyword,
                    SearchNum = 1,
                    SearchTime = DateTime.Now
                };
                return this._keyWordsRepository.CreateEntity(info);
            }
            else
            {
                model.SearchNum += 1;
                model.SearchTime = DateTime.Now;
                return UpdateAKeyWord(model);
            }

        }

        public List<KeyWords> GetKeyWordsByIpAddress(string ipAddress)
        {
            List<KeyWords> keyWordses = new List<KeyWords>();
            var query = this._keyWordsRepository.GetEntityList(CondtionEqualIpAddress(ipAddress));
            if (!query.Any()) return  keyWordses;
            keyWordses=query.Take(10).ToList();
            return keyWordses;
        }

        public List<KeyWords> GetHotKeyWords()
        {
            var query = this._keyWordsRepository.GetEntityList(OrderCondtionBySearchNum());
            return query.Any() == true ? query.Take(10).ToList() : new List<KeyWords>();
        }

        public List<KeyWords> GetFilteredKeyword(string keyword)
        {
            List<KeyWords> keyWordses=new List<KeyWords>();
            var query = this._keyWordsRepository.GetEntityList(CondtionContainsKeyword(keyword),OrderCondtionBySearchNum());
            if (!query.Any()) return keyWordses;
            return query.Take(10).ToList();
        }

        public bool AddAKeyWord(string  keyword)
        {
            if (keyword.IsNullOrWhiteSpace())
            {
                return false;
            }
            var model = this._keyWordsRepository.GetEntity(CondtionEqualKeyword(keyword));
            if(model!=null) return false;
            var info = new KeyWords()
            {
                Keyword = keyword,
                SearchNum = 1,
                SearchTime = DateTime.Now
            };
            return this._keyWordsRepository.CreateEntity(info);
        }

        public bool AddAKeyWord(KeyWords info)
        {
            if (info.Keyword.IsNullOrWhiteSpace())
            {
                return false;
            }
            var model = this._keyWordsRepository.GetEntity(CondtionEqualKeyword(info.Keyword));
            if (model != null) return false;
            info.SearchTime = DateTime.Now;
            return this._keyWordsRepository.CreateEntity(info);
        }

        public bool UpdateAKeyWord(KeyWords info)
        {
            info.SearchTime = DateTime.Now;
            return this._keyWordsRepository.UpdateEntity(info);
        }

        public bool DelAKeyWord(int id)
        {
            var info = this._keyWordsRepository.GetEntity(CondtionEqualID(id));
            return info != null && this._keyWordsRepository.DeleteEntity(info);
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="condtions"></param>
        /// <param name="orderCondtions"></param>
        /// <returns></returns>
        public PageResult GetKeyWordsPageResult(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions)
        {
            int totalCount = 0;
            int totalIndex = 0;
            IList<HKSJ.WBVV.Entity.KeyWords> plateViews = GetKeywordViews(condtions, orderCondtions, out totalCount, out totalIndex);
            return new PageResult()
            {
                PageSize = this.PageSize,
                PageIndex = this.PageIndex,
                TotalCount = totalCount,
                TotalIndex = totalIndex,
                Data = plateViews
            };
        }

        /// <summary>
        /// 数据列表
        /// </summary>
        /// <param name="condtions"></param>
        /// <param name="orderCondtions"></param>
        /// <param name="totalCount"></param>
        /// <param name="totalIndex"></param>
        /// <returns></returns>
        public IList<Entity.KeyWords> GetKeywordViews(IList<Condtion> condtions, IList<OrderCondtion> orderCondtions, out int totalCount, out int totalIndex)
        {
            var plate = (from v in this._keyWordsRepository.GetEntityList()
                         select new HKSJ.WBVV.Entity.KeyWords()
                         {
                            ID=v.ID,
                            Keyword = v.Keyword,
                            SearchNum = v.SearchNum,
                            SearchTime = v.SearchTime
                         }).AsQueryable();

            if (condtions != null && condtions.Count > 0)//查询条件
            {
                plate = plate.Query(condtions);
            }
            bool isExists = plate.Any();
            if (isExists)
            {
                if (orderCondtions != null && orderCondtions.Count > 0)//排序条件
                {
                    plate = plate.OrderBy(orderCondtions);
                }
            }
            totalCount = isExists ? plate.Count() : 0;
            if (this.PageSize <= 0)
            {
                totalIndex = 0;
                var queryable = isExists
                    ? plate.ToList()
                    : new List<Entity.KeyWords>();
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
                    : new List<Entity.KeyWords>();

                return queryable;
            }
        }

        public KeyWords GetAKeyWordById(int id)
        {
           return  this._keyWordsRepository.GetEntity(CondtionEqualID(id));
        }

        #region 传入参数

        private Condtion CondtionEqualID(int id)
        {
            var condition = new Condtion()
            {
                FiledName = "ID",
                FiledValue = id,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condition;
        }
        private Condtion CondtionEqualKeyword(string keyword)
        {
            var condition = new Condtion()
            {
                FiledName = "Keyword",
                FiledValue = keyword,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condition;
        }

        private Condtion CondtionContainsKeyword(string keyword)
        {
            var condition = new Condtion()
            {
                FiledName = "Keyword",
                FiledValue = keyword,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Contains
            };
            return condition;
        }

        private Condtion CondtionEqualIpAddress(string ipAddress)
        {
            var condition = new Condtion()
            {
                FiledName = "IpAddress",
                FiledValue = ipAddress,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condition;
        }

        private OrderCondtion OrderCondtionBySearchNum()
        {
            var orderCondtion = new OrderCondtion()
            {
                FiledName = "SearchNum",
                IsDesc = true
            };
            return orderCondtion;
        }



        #endregion


    }
}
