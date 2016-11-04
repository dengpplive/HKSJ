using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using HKSJ.Utilities;
using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.Repository.Interface;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Business
{
    public class TagsBusiness : BaseBusiness, ITagsBusiness
    {
        private readonly ITagsRepository _tagsRepository;
        private readonly IManageRepository _manageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IVideoRepository _videoRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserSpecialRepository _userSpecialRepository;
        private readonly ICategoryBusiness _categoryBusiness;
        public TagsBusiness(
            ITagsRepository tagsRepository,
            IManageRepository manageRepository,
            IUserRepository userRepository,
            IVideoRepository videoRepository,
            ICategoryRepository categoryRepository, 
            IUserSpecialRepository userSpecialRepository,
            ICategoryBusiness categoryBusiness
            )
        {
            _tagsRepository = tagsRepository;
            _manageRepository = manageRepository;
            _userRepository = userRepository;
            _videoRepository = videoRepository;
            _categoryRepository = categoryRepository;
            _userSpecialRepository = userSpecialRepository;
            _categoryBusiness = categoryBusiness;
        }

        #region manage

        #region 创建标签
        /// <summary>
        /// 创建标签
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sortNum"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public int CreateTags(string name, int sortNum,int categoryId)
        {
            var pagesize = 0;
            CheckPageSize(this.PageSize, out pagesize);
            CheckManageId();
            CheckName(name);
            CheckSortNum(sortNum);
            CheckCategoryId(categoryId);
            Manage manage;
            CheckManageId(out manage);
            //CheckExistName(name);
            var tags = new Tags()
            {
                CreateUserId = manage.Id,
                CreateTime = DateTime.Now,
                KeyWord = PinyinHelper.PinyinString(name),
                Name = name,
                SortNum = sortNum,
                CategoryId=categoryId
            };
            this._tagsRepository.CreateEntity(tags);
            return tags.Id;
        }
        #endregion

        #region 修改标签
        /// <summary>
        /// 修改标签
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="sortNum"></param>
        /// <returns></returns>
        public int UpdateTags(int id, string name, int sortNum)
        {
            var pagesize = 0;
            CheckId(id);
            CheckPageSize(this.PageSize, out pagesize);
            CheckManageId();
            CheckName(name);
            CheckSortNum(sortNum);
            Manage manage;
            CheckManageId(out manage);
            Tags tags;
            CheckId(id, out tags);
            CheckExistName(tags.Id, name);
            tags.Name = name;
            tags.KeyWord = PinyinHelper.PinyinString(name);
            tags.SortNum = sortNum;
            tags.UpdateTime = DateTime.Now;
            tags.UpdateUserId = manage.Id;
            this._tagsRepository.UpdateEntity(tags);
            return tags.Id;
        }

        #endregion

        #region 删除标签
        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteTags(int id)
        {
            CheckId(id);
            CheckManageId();
            Manage manage;
            CheckManageId(out manage);
            Tags tags;
            CheckId(id, out tags);
            return this._tagsRepository.DeleteEntity(tags);
        }

        #region 删除多个标签
        /// <summary>
        /// 删除多个标签
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteTags(IList<int> ids)
        {
            CheckIds(ids);
            CheckManageId();
            Manage manage;
            CheckManageId(out manage);
            IList<Tags> tagses;
            CheckIds(ids, out tagses);
            this._tagsRepository.DeleteEntitys(tagses);
            return true;
        }
        #endregion

        #endregion

        #region 后台获取分类的标签视图
        public IList<HKSJ.WBVV.Entity.ViewModel.Manage.CategoryTagsView> GetTagsGroupbyCategoryId()
        {
            IList<HKSJ.WBVV.Entity.ViewModel.Manage.CategoryView> CategoryIlist = _categoryBusiness.GetOneCategoryViewList();
            IList<HKSJ.WBVV.Entity.ViewModel.Manage.CategoryTagsView> CategoryTagsViewIlist = new List<HKSJ.WBVV.Entity.ViewModel.Manage.CategoryTagsView>();
            if (CategoryIlist.Count>0)
            {
            for (int i = 0; i < CategoryIlist.Count; i++)
            {
                var CategoryTagsView = new HKSJ.WBVV.Entity.ViewModel.Manage.CategoryTagsView();
                int categoryId = CategoryIlist[i].id;
                string categoryName = CategoryIlist[i].name;
                IList<TagsView> tagsList = new List<TagsView>();
           
                var tags = (from t in this._tagsRepository.GetEntityList()
                            where t.State == false && t.CategoryId == categoryId //可用
                            orderby t.SortNum ascending //排序数量,用户重复使用的时候累加
                            select new TagsView()
                            {
                                Id = t.Id,
                                content = t.Name,
                                KeyWord = t.KeyWord,
                                SortNum = t.SortNum
                            }).AsQueryable();
                    tagsList = tags.ToList();
                    int[] TagIds = new int[0];
                    List<int> TagIdsList = TagIds.ToList();
                    string[] TagNames = new string[0];
                    List<string> TagNamesList = TagNames.ToList();
                    int[] TagSortNums = new int[0];
                    List<int> TagSortNumsList = TagSortNums.ToList();
                    for (int j = 0; j < tagsList.Count; j++)
                    {
                        TagIdsList.Add(tagsList[j].Id);
                        TagNamesList.Add(tagsList[j].content);
                        TagSortNumsList.Add(tagsList[j].SortNum);
                    }
                    TagIds = TagIdsList.ToArray();
                    TagNames = TagNamesList.ToArray();
                    TagSortNums = TagSortNumsList.ToArray();
                    CategoryTagsView = new HKSJ.WBVV.Entity.ViewModel.Manage.CategoryTagsView()
                        {
                            CategoryId = categoryId,
                            CategoryName=categoryName,
                            TagIdsArray = TagIds,
                            TagNamesArray=TagNames,
                            TagSortsArray=TagSortNums
                        };
               
                CategoryTagsViewIlist.Add(CategoryTagsView);
            }
           }
            return CategoryTagsViewIlist;
        }
                #endregion
        #region 某分类id下的推荐标签(后端页面--推荐分类标签管理 top 所有的)
        public IList<TagsView> GetTagsByCategoryId(int categoryId)
        {

            IList<TagsView> tagsList = new List<TagsView>();
            var tags = (from t in this._tagsRepository.GetEntityList()
                        where t.State == false && t.CategoryId == categoryId  //可用 State == false //可用
                        orderby t.SortNum ascending //排序数量,用户重复使用的时候累加
                        select new TagsView()
                        {
                            Id = t.Id,
                            content = t.Name,
                            KeyWord = t.KeyWord,
                            SortNum = t.SortNum
                        }).AsQueryable();
            if (tags.Any())
            {
                tagsList = tags.ToList();
            }
            return tagsList;
        }       
        #endregion        
        #endregion
        #region 某分类id下的推荐标签(前端页面--“我的视频编辑”，“上传视频”；top 20个)
        public IList<TagsView> GetTagsOfWebByCategoryId(int categoryId)
        {

            IList<TagsView> tagsList = new List<TagsView>();
            var tags = (from t in this._tagsRepository.GetEntityList()
                        where t.State == false && t.CategoryId == categoryId  //可用 State == false //可用
                        orderby t.SortNum ascending //排序数量,用户重复使用的时候累加
                        select new TagsView()
                        {
                            Id = t.Id,
                            content = t.Name,
                            KeyWord = t.KeyWord,
                            SortNum = t.SortNum
                        }).Take(20).AsQueryable();
            if (tags.Any())
            {
                tagsList = tags.ToList();
            }
            return tagsList;
        }
        #endregion        

        #region 我的视频——编辑视频

        #region 添加多个标签

        /// <summary>
        /// 添加多个标签
        /// </summary>
        /// <param name="names">标签名称</param>
        /// <returns></returns>
        public bool CreateTags(IList<string> names)
        {
            if (names == null || names.Count <= 0)
            {
                return false;
            }
            if (UserId < 0)
            {
                return false;
            }
            IList<Condtion> condtions = new List<Condtion>()
            {
                ConditionEqualId(UserId),
                CondtionEqualState()
            };
            var user = this._userRepository.GetEntity(condtions);
            if (user == null)
            {
                return false;
            }
            IList<Tags> updateTags = new List<Tags>();
            IList<Tags> insertTags = new List<Tags>();
            foreach (var name in names)
            {
                var tag = (from t in this._tagsRepository.GetEntityList()
                           join u in this._userRepository.GetEntityList() on t.CreateUserId equals u.Id
                           where t.State == false
                                 && u.State == false
                           select t).FirstOrDefault();
                if (tag != null)
                {
                    tag.SortNum += 1;
                    tag.UpdateTime = DateTime.Now;
                    tag.UpdateUserId = user.Id;
                    updateTags.Add(tag);
                }
                else
                {
                    var newTag = new Tags()
                    {
                        Name = name,
                        KeyWord = PinyinHelper.PinyinString(name),
                        CreateUserId = user.Id,
                        CreateTime = DateTime.Now
                    };
                    insertTags.Add(newTag);
                }
            }
            this._tagsRepository.UpdateEntitys(updateTags);
            this._tagsRepository.CreateEntitys(insertTags);
            return true;
        }
        /// <summary>
        /// 添加多个标签和数量
        /// </summary>
        /// <param name="dicts">标签名称,排序数量</param>
        /// <returns></returns>
        public void CreateTags(IDictionary<string, int> dicts)
        {
            if (dicts == null || dicts.Count <= 0) return;
            if (UserId <= 0) return;
            IList<Condtion> condtions = new List<Condtion>()
            {
                ConditionEqualId(UserId),
                CondtionEqualState()
            };
            var user = this._userRepository.GetEntity(condtions);
            if (user == null) return;
            IList<Tags> updateTags = new List<Tags>();
            IList<Tags> insertTag = new List<Tags>();
            foreach (var dict in dicts)
            {
                var tags = (from t in this._tagsRepository.GetEntityList()
                            where t.State == false
                                  && t.Name.Trim() == dict.Key
                            select t).FirstOrDefault();

                if (tags != null)
                {
                    tags.SortNum = dict.Value;
                    tags.UpdateUserId = user.Id;
                    tags.UpdateTime = DateTime.Now;
                    updateTags.Add(tags);
                }
                else
                {
                    var newTags = new Tags()
                    {
                        Name = dict.Key,
                        KeyWord = PinyinHelper.PinyinString(dict.Key),
                        CreateUserId = user.Id,
                        CreateTime = DateTime.Now,
                        SortNum = dict.Value
                    };
                    insertTag.Add(newTags);
                }
            }
            if (updateTags.Count>0)
            {
                this._tagsRepository.UpdateEntitys(updateTags);
            }
            if (insertTag.Count > 0)
            {
                this._tagsRepository.CreateEntitys(insertTag);
            }
        }

        #endregion

        #region 添加标签
        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="name">标签名称</param>
        /// <returns></returns>
        public bool CreateTags(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            CheckUserId();
            User user;
            CheckUserId(out user);
            var tags = (from t in this._tagsRepository.GetEntityList()
                        where t.State == false
                              && t.Name == name.Trim()
                        select t).FirstOrDefault();
            var success = false;
            if (tags != null)
            {
                tags.SortNum += 1;
                tags.UpdateTime = DateTime.Now;
                tags.UpdateUserId = user.Id;
                success = this._tagsRepository.UpdateEntity(tags);
            }
            else
            {
                var newTags = new Tags()
                {
                    Name = name,
                    KeyWord = PinyinHelper.PinyinString(name),
                    CreateUserId = user.Id,
                    CreateTime = DateTime.Now
                };
                success = this._tagsRepository.CreateEntity(newTags);
            }
            return success;
        }
        #endregion

        #region 推荐标签
        /// <summary>
        /// 推荐标签
        /// </summary>
        /// <returns></returns>
        public IList<TagsView> GetTags()
        {
            IList<TagsView> tagsList = new List<TagsView>();
            var tags = (from t in this._tagsRepository.GetEntityList()
                        //  join u in this._userRepository.GetEntityList() on t.CreateUserId equals u.Id
                        where t.State == false  //可用
                        //   && u.State == false //可用
                        orderby t.SortNum descending, //排序数量,用户重复使用的时候累加
                                t.CreateTime descending //标签创建时间
                        select new TagsView()
                        {
                            Id = t.Id,
                            content = t.Name,
                            KeyWord = t.KeyWord
                        }).DistinctBy(tv => tv.content).AsQueryable();
            if (tags.Any())
            {
                tagsList = tags.Take(10).ToList();
            }
            return tagsList;
        }
        #endregion

        #region 用户搜索标签
        /// <summary>
        /// 用户搜索标签
        /// </summary>
        /// <param name="search">搜索内容</param>
        /// <returns></returns>
        public IList<TagsView> GetTags(string search)
        {
            IList<TagsView> tagsList = new List<TagsView>();
            var tags = (from t in this._tagsRepository.GetEntityList()
                        //  join u in this._userRepository.GetEntityList() on t.CreateUserId equals u.Id
                        where t.State == false //可用
                        //   && u.State == false //可用
                        orderby t.SortNum descending, //排序数量,用户重复使用的时候累加
                                t.CreateTime descending //标签创建时间
                        select new TagsView()
                        {
                            Id = t.Id,
                            content = t.Name,
                            KeyWord = t.KeyWord
                        }).DistinctBy(tv =>tv.content).AsQueryable();
            if (string.IsNullOrEmpty(search))
            {
                tagsList = tags.Take(10).ToList();
            }
            else
            {
                var newTags = tags.Where(t => t.KeyWord.Contains(search) || t.content.Contains(search));
                if (newTags.Any())
                {
                    tagsList = newTags.Take(10).ToList();
                }
            }
            return tagsList;
        }

        #endregion

        #region 异步更新标签
        /// <summary>
        /// 更新标签
        /// </summary>
        /// <returns></returns>
        public Task AsyncCreateTags()
        {
            var task = new Task(() =>
            {
                var tags = new List<string>();

                #region 没有禁用的管理员上传的分类视频
                var manageTags = (from v in this._videoRepository.GetEntityList()
                                  join c in this._categoryRepository.GetEntityList() on v.CategoryId equals c.Id
                                  join m in this._manageRepository.GetEntityList() on v.CreateManageId equals m.Id
                                  where c.State == false //启用
                                      && v.State == false //启用
                                      && v.VideoState == 3 //审核通过
                                      && m.State == false //启用
                                      && v.VideoSource == false //管理员
                                      && !string.IsNullOrEmpty(v.Tags)
                                  select v.Tags
                             ).AsQueryable();
                #endregion

                #region 没有禁用的用户上传的分类视频
                var userTags = (from v in this._videoRepository.GetEntityList()
                                join c in this._categoryRepository.GetEntityList() on v.CategoryId equals c.Id
                                join u in this._userRepository.GetEntityList() on v.CreateManageId equals u.Id
                                where c.State == false //启用
                                    && v.State == false //启用
                                    && v.VideoState == 3 //审核通过
                                    && u.State == false //启用
                                    && v.VideoSource == true //用户
                                    && !string.IsNullOrEmpty(v.Tags)
                                select v.Tags
                             ).AsQueryable();
                #endregion

                #region 专辑里的标签
                var userSpecialTag=(from us in this._userSpecialRepository.GetEntityList()
                                    join u in this._userRepository.GetEntityList() on us.CreateUserId equals  u.Id
                                    where us.State==false //启用
                                          &&u.State==false//启用
                                          && !string.IsNullOrEmpty(us.Tag)
                                    select us.Tag
                                        ).AsQueryable();
                #endregion

                #region 合并标签
                if (manageTags.Any())
                {
                    tags.AddRange(manageTags);
                }
                if (userTags.Any())
                {
                    tags.AddRange(userTags);
                }
                if (userSpecialTag.Any())
                {
                    tags.AddRange(userSpecialTag);
                }
                #endregion

                #region 获取视频中所有的Tags名称和计算数量
                IDictionary<string, int> dicts = new Dictionary<string, int>();
                foreach (var tag in tags)
                {
                    IList<string> splitTags = SplitByTags(tag, '|');
                    foreach (var splitTag in splitTags)
                    {
                        var key = splitTag.Trim();
                        if (dicts.ContainsKey(key))
                        {
                            dicts[key]++;
                        }
                        else
                        {
                            dicts.Add(key, 1);
                        }
                    }
                }
                #endregion

                CreateTags(dicts);
            });
            task.Start();
            return task;
        }
        #endregion

        #endregion

        #region 传入参数检测
        /// <summary>
        /// 检测用户是否登录
        /// </summary>
        private void CheckManageId()
        {
            AssertUtil.AreBigger(UserId, 0, LanguageUtil.Translate("api_Business_Tags_CheckManageId"));
        }
        /// <summary>
        /// 检测用户是否登录
        /// </summary>
        private void CheckUserId()
        {
            AssertUtil.AreBigger(UserId, 0, LanguageUtil.Translate("api_Business_Tags_CheckUserId"));
        }
        /// <summary>
        /// 检测管理员是否存在
        /// </summary>
        /// <param name="manage"></param>
        private void CheckManageId(out Manage manage)
        {
            manage = (from m in this._manageRepository.GetEntityList()
                      where m.State == false
                            && m.Id == UserId
                      select m).FirstOrDefault();
            AssertUtil.IsNotNull(manage, LanguageUtil.Translate("api_Business_Tags_CheckManageId_manage"));
        }
        /// <summary>
        /// 检测用户是否登录
        /// </summary>
        private void CheckUserId(out User user)
        {
            user = (from u in this._userRepository.GetEntityList()
                    where u.State == false
                          && u.Id == UserId
                    select u).FirstOrDefault();

            AssertUtil.IsNotNull(user, LanguageUtil.Translate("api_Business_Tags_CheckUserId_user"));
        }
        /// <summary>
        /// 检测编号不能小于0
        /// </summary>
        /// <param name="id"></param>
        private void CheckId(int id)
        {
            AssertUtil.AreBigger(id, 0, LanguageUtil.Translate("api_Business_Tags_CheckId"));
        }

        /// <summary>
        /// 检测编号不能小于0
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tags"></param>
        private void CheckId(int id, out Tags tags)
        {
            tags = (from t in this._tagsRepository.GetEntityList()
                    where t.State == false
                          && t.Id == id
                    select t).FirstOrDefault();
            AssertUtil.IsNotNull(tags, LanguageUtil.Translate("api_Business_Tags_CheckId_tags"));
        }
        /// <summary>
        /// 检测编号不能为空
        /// </summary>
        /// <param name="name"></param>
        private void CheckName(string name)
        {
            AssertUtil.NotNullOrWhiteSpace(name, LanguageUtil.Translate("api_Business_Tags_CheckName"));
        }
        /// <summary>
        /// 检测标签名称是否重复
        /// </summary>
        /// <param name="name"></param>
        private void CheckExistName(string name)
        {
            var tags = (from t in this._tagsRepository.GetEntityList()
                        where t.State == false
                              && t.Name == name.Trim()
                        select t).FirstOrDefault();
            AssertUtil.IsNull(tags, LanguageUtil.Translate("api_Business_Tags_CheckExistName"));
        }

        /// <summary>
        /// 检测标签名称是否重复
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        private void CheckExistName(int id, string name)
        {
            var tags = (from t in this._tagsRepository.GetEntityList()
                        where t.State == false
                              && t.Name == name.Trim()
                              && t.Id != id
                        select t).FirstOrDefault();
            AssertUtil.IsNull(tags, LanguageUtil.Translate("api_Business_Tags_CheckExistName_tags"));
        }
        /// <summary>
        /// 检测排序数量不能小于0
        /// </summary>
        /// <param name="sortNum"></param>
        private void CheckSortNum(int sortNum)
        {
            AssertUtil.AreBiggerOrEqual(sortNum, 0, LanguageUtil.Translate("api_Business_Tags_CheckSortNum"));
        }
        /// <summary>
        /// 所属分类不能小于0
        /// </summary>
        /// <param name="sortNum"></param>
        private void CheckCategoryId(int categoryId)
        {
            AssertUtil.AreBiggerOrEqual(categoryId, 0, "所属分类不能小于0");
        }
        /// <summary>
        /// 检测显示数量是否传入
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="size"></param>
        private void CheckPageSize(int pageSize, out int size)
        {
            size = pageSize > 0 ? pageSize : 10;
        }
        /// <summary>
        /// 检测是否选中了编号
        /// </summary>
        /// <param name="ids"></param>
        private void CheckIds(IList<int> ids)
        {
            AssertUtil.IsNotEmptyCollection(ids, LanguageUtil.Translate("api_Business_Tags_CheckIds"));
        }
        /// <summary>
        /// 检测可用的标签
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="tagses"></param>
        private void CheckIds(IList<int> ids, out IList<Tags> tagses)
        {
            tagses = ids.Select(id => (from t in this._tagsRepository.GetEntityList()
                                       where t.State == false
                                          && t.Id == id
                                       select t).FirstOrDefault()
                                       ).Where(tags => tags != null).ToList();
            AssertUtil.IsNotEmptyCollection(tagses, LanguageUtil.Translate("api_Business_Tags_CheckIds_tagses"));
        }


        #endregion

        #region 传入参数


        #endregion

        #region 排序参数


        #endregion
    }
}
