
using System;
using System.Data;
using System.Collections.Generic;
using HKSJ.WBVV.Business.Interface.Base;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.Entity.ViewModel.Manage;

namespace HKSJ.WBVV.Business.Interface
{

    public interface ICategoryBusiness : IBaseBusiness
    {
        IList<HKSJ.WBVV.Entity.ViewModel.Manage.CategoryView> GetOneCategoryViewList();
        IList<MenuView> GetMenuViewList();
        IList<HKSJ.WBVV.Entity.ViewModel.Manage.CategoryView> GetCategoryViewList();
        IList<MenuView> GetMenuViewListVisible();
        int CreateCategory(string name, int pid);
        bool UpdateCategory(string name, int id);
        bool MoveCategory(int id, int pid);
        bool DeleteCategory(int id);
        int GetParentId(int cid);
        Category GetParentInfo(int cid);
    }
}