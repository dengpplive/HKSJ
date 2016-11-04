using System.Collections.Generic;
using HKSJ.WBVV.Business.Interface.Base;
using HKSJ.WBVV.Entity.ViewModel.Client;

namespace HKSJ.WBVV.Business.Interface
{
    public interface IDictionaryBusiness : IBaseBusiness
    {
        IList<DictionaryView> GetDictionaryViewList(int categoryId);
        IList<DictionaryView> GetDictionaryViewList(int categoryId, string filter);
        IList<HKSJ.WBVV.Entity.ViewModel.Manage.CategoryView> GetCategoryAndDictionaryViewList();
        int CreateDictionary(string name, int parentId);
        bool UpdateDictionary(string name, int id);
        bool DeleteDictionary(int id);

        /// <summary>
        /// 一级分类下的属性列表
        /// Author:axone
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        IList<DictionaryView> GetDictionaryAndItemViewList(int categoryId);
    }
}