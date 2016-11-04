using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Business.Interface.Base;
using HKSJ.WBVV.Entity.ViewModel.Client;

namespace HKSJ.WBVV.Business.Interface
{
    public interface ITagsBusiness : IBaseBusiness
    {
        Task AsyncCreateTags();
        bool CreateTags(string name);
        bool CreateTags(IList<string> names);
        IList<TagsView> GetTags();
        IList<TagsView> GetTags(string search);
        IList<HKSJ.WBVV.Entity.ViewModel.Manage.CategoryTagsView> GetTagsGroupbyCategoryId();
        IList<TagsView> GetTagsByCategoryId(int id);
        IList<TagsView> GetTagsOfWebByCategoryId(int id);
        int UpdateTags(int id, string name, int sortNum);
        bool DeleteTags(IList<int> ids);
        int CreateTags(string name, int sortNum, int categoryId);
    }
}
