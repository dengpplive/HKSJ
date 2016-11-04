


using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Repository.Interface;
using HKSJ.Utilities;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Business
{

    public class DictionaryItemBusiness : BaseBusiness, IDictionaryItemBusiness
    {
        private readonly IDictionaryItemRepository _dictionaryItemRepository;
        private readonly IDictionaryRepository _dictionaryRepository;
        public DictionaryItemBusiness(IDictionaryItemRepository dictionaryItemRepository, IDictionaryRepository dictionaryRepository)
        {
            this._dictionaryItemRepository = dictionaryItemRepository;
            this._dictionaryRepository = dictionaryRepository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IQueryable<DictionaryItem> GetDictionaryItemQueryable()
        {
            var condtion = CondtionEqualState();
            return this._dictionaryItemRepository.GetEntityList(condtion).AsQueryable();
        }

        #region

        public int CreateDictionaryItem(string name, int parentId)
        {
            CheckNameNotNull(name);
            if (parentId > 0)
            {
                Dictionary parentDictionary;
                CheckDictionaryId(parentId, out parentDictionary);
            }
            var dictionaryItem = new DictionaryItem()
            {
                Name = name,
                DictionaryId = parentId,
                CreateManageId = 1,
                CreateTime = DateTime.Now,
                KeyWord = PinyinHelper.PinyinString(name),
            };
            this._dictionaryItemRepository.CreateEntity(dictionaryItem);
            return dictionaryItem.Id;
        }

        public bool UpdateDictionaryItem(string name, int id)
        {
            CheckNameNotNull(name);
            CheckIdBiggerZero(id);
            DictionaryItem dictionaryItem;
            CheckId(id, out dictionaryItem);
            dictionaryItem.Name = name;
            dictionaryItem.KeyWord = PinyinHelper.PinyinString(name);
            dictionaryItem.UpdateManageId = 1;
            dictionaryItem.UpdateTime = DateTime.Now;
            return this._dictionaryItemRepository.UpdateEntity(dictionaryItem);
        }

        public bool DeleteDictionaryItem(int id)
        {
            CheckIdBiggerZero(id);
            DictionaryItem dictionaryItem;
            CheckId(id, out dictionaryItem);
            return this._dictionaryItemRepository.DeleteEntity(dictionaryItem);
        }

        #endregion



        #region 传入参数检测
        /// <summary>
        /// 检测字典项名称不为空
        /// </summary>
        /// <param name="name"></param>
        private void CheckNameNotNull(string name)
        {
            AssertUtil.NotNullOrWhiteSpace(name, LanguageUtil.Translate("api_Business_DictionaryItem_CheckNameNotNull"));
        }

        /// <summary>
        /// 检测字典项编号
        /// </summary>
        /// <param name="id"></param>
        /// <param name="category"></param>
        private void CheckDictionaryId(int id, out Dictionary dictionary)
        {
            var condtionId = ConditionEqualId(id);
            dictionary = this._dictionaryRepository.GetEntity(condtionId);
            AssertUtil.IsNotNull(dictionary, LanguageUtil.Translate("api_Business_DictionaryItem_CheckDictionaryId"));
        }

        /// <summary>
        /// 检测字典项编号
        /// </summary>
        /// <param name="id"></param>
        /// <param name="category"></param>
        private void CheckId(int id, out DictionaryItem dictionaryItem)
        {
            var condtionId = ConditionEqualId(id);
            dictionaryItem = this._dictionaryItemRepository.GetEntity(condtionId);
            AssertUtil.IsNotNull(dictionaryItem, LanguageUtil.Translate("api_Business_DictionaryItem_CheckId"));
        }

        /// <summary>
        /// 检测字典项编号不小于0
        /// </summary>
        /// <param name="id"></param>
        private void CheckIdBiggerZero(int id)
        {
            AssertUtil.AreBigger(id, 0, LanguageUtil.Translate("api_Business_DictionaryItem_CheckIdBiggerZero"));
        }
        #endregion

        #region 传入参数

        #endregion

        #region 排序参数

        #endregion


    }
}