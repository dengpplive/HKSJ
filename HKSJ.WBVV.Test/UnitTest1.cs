using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Threading;
using HKSJ.WBVV.Business;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.Enums;
using HKSJ.WBVV.Entity.RequestPara;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.Repository;
using HKSJ.WBVV.Repository.Base;
using HKSJ.WBVV.Repository.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HKSJ.Utilities;
using HKSJ.WBVV.Common.Email;
using HKSJ.WBVV.Common.Mail;
using HKSJ.WBVV.Common.Email.Enum;
using HKSJ.WBVV.Common.Email.Model;
using System.Collections;
using System.Text;

namespace HKSJ.WBVV.Test
{
    [TestClass]
    public class UnitTest1
    {
        static ICategoryRepository categoryRepository = new CategoryRepository();
        IVideoRepository _videoRepository = new VideoRepository();
        static IDictionaryRepository dictionaryRepository = new DictionaryRepository();
        static IDictionaryItemRepository dictionaryItemRepository = new DictionaryItemRepository();

        private readonly IUserSpecialRepository _userSpecialRepository = new UserSpecialRepository();
        private readonly IUserSpecialSonRepository _userSpecialSonRepository = new UserSpecialSonRepository();

        [TestMethod]
        public void TestGetCategory()
        {
            var model = categoryRepository.GetEntity(new Condtion()
            {
                FiledName = "Id",
                FiledValue = 1,
                ExpressionLogic = ExpressionLogic.Or,
                ExpressionType = ExpressionType.Equal
            });
            Assert.IsNotNull(model);
        }

        [TestMethod]
        public void TestGetCategoryList()
        {
            var list = categoryRepository.GetEntityList().ToList();
            Assert.IsTrue(list != null && list.Count > 0);
        }
        [TestMethod]
        public void TestUpdateCategory()
        {
            var model = categoryRepository.GetEntity(new Condtion()
            {
                FiledName = "Id",
                FiledValue = 1,
                ExpressionLogic = ExpressionLogic.Or,
                ExpressionType = ExpressionType.Equal
            });
            model.Name = "哦哦哦";
            bool success = categoryRepository.UpdateEntity(model);
            var list = categoryRepository.GetEntityList().ToList();
            Assert.IsTrue(success);
        }
        [TestMethod]
        public void TestCreateCategory()
        {
            var model = new Category()
            {
                Name = "sfsdf",
                CreateManageId = 1,
                CreateTime = DateTime.Now,
                KeyWord = "fff",
                ParentId = 0
            };
            bool success = categoryRepository.CreateEntity(model);
            var list = categoryRepository.GetEntityList().ToList();
            Assert.IsTrue(success);
        }
        [TestMethod]
        public void TestDeleteCategory()
        {
            var model = categoryRepository.GetEntity(new Condtion()
            {
                FiledName = "Id",
                FiledValue = 1,
                ExpressionLogic = ExpressionLogic.Or,
                ExpressionType = ExpressionType.Equal
            });
            bool success = categoryRepository.DeleteEntity(model);
            var list = categoryRepository.GetEntityList().ToList();
            Assert.IsTrue(success);
        }
        [TestMethod]
        public void ClearMem()
        {
            var authKeys = HKSJ.WBVV.Common.Cache.MemberCacheHelper.Get("AuthKeys").FromJSON<IList<AuthKeys>>();
            HKSJ.WBVV.Common.Cache.MemberCacheHelper.Clear();
        }

        [TestMethod]
        public void TestPinyinHelper()
        {
            var aa = PinyinHelper.PinyinString("ffff");
        }

        [TestMethod]
        public void Test()
        {
            var s = "dsfsdfsdf";
            var a = s.Substring(s.Length - 1, 1);
            var c = s.Substring(0, s.Length - 1);
            //AssertUtil.AreBigger(0, 0, "分类编号不能小于0");
            AssertUtil.IsFalse(1 == 1, "分类编号不能小于0");
        }

        [TestMethod]
        public void TestDate()
        {
            DateTime startTime = Convert.ToDateTime("2015-9-22");
            DateTime endTime = DateTime.Now;
            TimeSpan ts = endTime - startTime;
            var rMonths = new int[] { 30, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            var pMonths = new int[] { 30, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            var curMonths = endTime.Year % 4 == 0 ? rMonths : pMonths;
            double totalseconds = ts.TotalSeconds;
            double totalMinutes = ts.TotalMinutes;
            double totalHours = ts.TotalHours;
            double totalDays = ts.TotalDays;
            const string message = "发表于{0}前";
            string outmessage = "";
            if (totalseconds < 60)
            {
                outmessage = message.F((int)totalseconds + "秒");
            }
            else if (totalseconds >= 60 && totalMinutes < 60)
            {
                outmessage = message.F((int)totalMinutes + "分");
            }
            else if (totalMinutes >= 60 && totalHours < 24)
            {
                outmessage = message.F((int)totalHours + "小时");
            }
            else if (totalHours >= 24 && totalDays < curMonths[endTime.Month - 1])
            {
                outmessage = message.F((int)totalDays + "天");
            }
            else
            {
                int month = (int)totalDays / curMonths[endTime.Month - 1];
                if (month < 12)
                {
                    outmessage = message.F(month + "月");
                }
                else
                {
                    int year = month / 12;
                    outmessage = message.F(year + "年");
                }
            }
            Console.WriteLine(outmessage);
        }

        [TestMethod]

        public void TestEncrypt()
        {
            string strPwd = "A123456789";
            string str1 = DESEncrypt.Encrypt(strPwd, "①");
            string str2 = DESEncrypt.Decrypt(str1, "①");

        }

        [TestMethod]
        public void CreateVideo()
        {
            string fileKey = "abcdef";
            float outDuration = 1123;
            string m3u8Key = "m3u8Key";
            string thumbKey = "thumKey";

            DateTime timeNow = DateTime.Now;
            Video video = new Video()
            {
                VideoRosella = fileKey,
                VideoState = 0,
                VideoSource = true,
                TimeLength = (int)outDuration,
                VideoPath = m3u8Key,
                BigPicturePath = thumbKey,
                SmallPicturePath = thumbKey + "?imageView2/2/w/100/h/100",
                CreateTime = timeNow,
                UpdateTime = timeNow,
                RewardCount = 10
            };
            video.VideoState = 0;

            Assert.IsTrue(_videoRepository.CreateEntity(video), "创建成功");
        }

        [TestMethod]
        public void TestEmail()
        {
            //            EmailHelper.SendEmailThread();
            //            EmailQueue.AddQueue(EmailEnum.Register,"413615975@qq.com", true, true);
            //            EmailQueue.AddQueue(EmailEnum.FindPwd, "413615975@qq.com", true, true);
            //            EmailQueue.AddQueue(EmailEnum.UpdatePwd, "413615975@qq.com", true, true);

            EmailHelper.SendEmailHtmlAsync(EmailEnum.Register, "413615975@qq.com", new Dictionary<string, string>() { { "@code@", "4566" }, { "@imageUrl@", @"http://www.5bvv.com/Content/images/icon_img/5BVV_logo_03.png" } },
                (r) =>
                {
                    if (r)
                    {
                        Console.Write("发送成功");
                    }
                    else
                    {
                        Console.Write("发送失败");
                    }
                });


            Thread.Sleep(100000);

        }

        protected Condtion ConditionEqualId(int id)
        {

            var condtion = new Condtion()
            {
                FiledName = "Id",
                FiledValue = id,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }

        //protected Condtion ConditionEqualId(int id)
        //{
        //    var condtion = new Condtion()
        //    {
        //        FiledName = "Id",
        //        FiledValue = id,
        //        ExpressionLogic = ExpressionLogic.And,
        //        ExpressionType = ExpressionType.Equal
        //    };
        //    return condtion;
        //}

        private Condtion CondtionEqualCreateUserId(int createUserId)
        {
            var condtion = new Condtion()
            {
                FiledName = "CreateUserId",
                FiledValue = createUserId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }


        [TestMethod]
        public void GetAlbumVideoViews()
        {
            //int albumId = 83;


            //User user;
            //UserSpecial userSpecial;
            //CheckAlbumId(albumId);
            //CheckUserId(UserId, out user);
            //CheckAlbumId(albumId, out userSpecial);


            //var sdv = new SpecialDetailView() { SpecialVideoList = new List<SpecialVideoView>(), Title = "", VideoCount = 0, PageCount = 0 };


            //string thumbnail = "";
            //if (userSpecial != null)
            //{
            //    sdv.Id = userSpecial.Id;
            //    sdv.Title = userSpecial.Title;
            //    sdv.Thumbnail = userSpecial.Image;
            //    sdv.CreateTime = userSpecial.CreateTime.ToString("yyyy-MM-dd HH:mm");
            //    sdv.Remark = userSpecial.Remark;
            //}

            //SpecialVideoView svv = null;


            //var vidoes = (from ussl in this._userSpecialSonRepository.GetEntityList(CondtionEqualSpecialId(sdv.Id))
            //              join video in this._videoRepository.GetEntityList(CondtionEqualState()).Where(p => p.VideoState == 3) on (long)ussl.VideoId equals video.Id
            //              orderby ussl.CreateTime
            //              select video
            //               ).AsQueryable();
            //sdv.VideoCount = vidoes.Count(); //专辑下视频总数


            //if (sdv.VideoCount <= 0) return sdv;

            //if (string.IsNullOrEmpty(sdv.Thumbnail))
            //{
            //    sdv.Thumbnail = vidoes.First().SmallPicturePath;//赋值专辑缩略图    
            //}

            //from vidoe in vidoes


            //svvList = new List<SpecialVideoView>();
            //int playCount = 0;//获取专辑下视频播放总数
            //int commentCount = 0;
            //int order = 1;
            //foreach (Video v in videoList)
            //{
            //    svv = new SpecialVideoView();
            //    svv.orderId = order;
            //    svv.Id = v.Id;
            //    svv.SmallPicturePath = v.SmallPicturePath;
            //    svv.Title = v.Title;
            //    svv.PlayCount = v.PlayCount;
            //    svv.CommentCount = v.CommentCount;
            //    commentCount += v.CommentCount;
            //    svv.TimeLength = ConvertUtil.ToInt(v.TimeLength, 0);
            //    svv.CreateTime = v.CreateTime.ToString("yyyy-MM-dd");
            //    svv.UpdateTime = ((DateTime)v.UpdateTime).ToString("yyyy-MM-dd");
            //    playCount += v.PlayCount;
            //    order++;
            //}

            //sdv.CommentCount = commentCount; //评论总数

            //sdv.PlayCount = playCount;//获取专辑下视频播放总数

            //sdv.SpecialVideoList = svvList;//专辑下视频集合

            //sdv.VideoCount = videoList.Count;

            //sdv.PageCount = GetPageCountToDataCount(sdv.VideoCount);

            //sdv.SpecialVideoList = sdv.SpecialVideoList.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();

        }



        int PageSize = 5;
        int PageIndex = 1;
        [TestMethod]
        public void TestTime()
        {
            List<long> idlist = new List<long>();


            Stopwatch www = new Stopwatch();

            for (int i = 0; i < 5; i++)
            {
                www.Restart();
                www.Start();


                SpecialDetailView sv1 = GetUserAlbumVideoViews1(144);
                www.Stop();
                idlist.Add(www.ElapsedMilliseconds);

                PageIndex++;
            }

            //www.Restart();
            //www.Start();

            //SpecialView sv2 = GetUserAlbumsViews();
            //www.Stop();
            //long ttt2 = www.ElapsedMilliseconds;

        }

        public SpecialDetailView GetUserAlbumVideoViews1(int albumId)
        {
            var sdv = new SpecialDetailView() { SpecialVideoList = new List<SpecialVideoView>(), Title = "", VideoCount = 0, PageCount = 0 };
            var userSpecial = this._userSpecialRepository.GetEntity(ConditionEqualId(albumId));

            var vidoes =
                (from ussl in
                     this._userSpecialSonRepository.GetEntityList(CondtionEqualSpecialId(userSpecial.Id))
                         .OrderByDescending(o => o.CreateTime)
                 join video in
                     this._videoRepository.GetEntityList(CondtionEqualState()).Where(p => p.VideoState == 3) on
                     (long)ussl.VideoId equals video.Id
                 select new SpecialVideoView
                 {
                     Id = video.Id,
                     CommentCount = video.CommentCount,
                     SmallPicturePath = video.SmallPicturePath,
                     Title = video.Title,
                     PlayCount = video.PlayCount,
                     About = video.About,
                     //CreateTime = video.CreateTime.ToString("yyyy-MM-dd" )
                 });

            sdv.VideoCount = vidoes.Count();
            sdv.PageCount = GetPageCountToDataCount(sdv.VideoCount);
            sdv.CommentCount = vidoes.Sum(o => o.CommentCount);
            sdv.PlayCount = vidoes.Sum(o => o.PlayCount);

            sdv.Title = userSpecial.Title;
            sdv.Remark = userSpecial.Remark;
            sdv.CreateTime = userSpecial.CreateTime.ToString("yyyy-MM-dd HH:mm");

            sdv.SpecialVideoList = vidoes.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            return sdv;
        }



        public SpecialDetailView GetUserAlbumVideoViews(int albumId)
        {
            User user;
            UserSpecial us;
            CheckAlbumId(albumId);
            //CheckUserId(UserId, out user);
            CheckAlbumId(albumId, out us);


            var sdv = new SpecialDetailView() { SpecialVideoList = new List<SpecialVideoView>(), Title = "", VideoCount = 0, PageCount = 0 };

            var userSpecial = (from usr in this._userSpecialRepository.GetEntityList(ConditionEqualId(us.Id))
                               select usr
                               ).AsQueryable().FirstOrDefault();

            string thumbnail = "";
            if (userSpecial != null)
            {
                sdv.Id = userSpecial.Id;
                sdv.Title = userSpecial.Title;
                sdv.Thumbnail = userSpecial.Image;
                sdv.CreateTime = userSpecial.CreateTime.ToString("yyyy-MM-dd HH:mm");
                sdv.Remark = userSpecial.Remark;
            }

            List<SpecialVideoView> svvList = null;
            SpecialVideoView svv = null;
            List<Video> videoList = null;


            var vidoes = (from ussl in this._userSpecialSonRepository.GetEntityList(CondtionEqualSpecialId(sdv.Id))
                          join video in this._videoRepository.GetEntityList(CondtionEqualState()).Where(p => p.VideoState == 3) on (long)ussl.VideoId equals video.Id
                          orderby ussl.CreateTime
                          select video
                           ).AsQueryable();
            videoList = vidoes.ToList<Video>();

            if (videoList.Count <= 0) return sdv;

            sdv.VideoCount = videoList.Count;//专辑下视频总数
            if (videoList != null && videoList.Count > 0 && string.IsNullOrEmpty(thumbnail))
                thumbnail = videoList[videoList.Count - 1].SmallPicturePath;

            //如果专辑图片和视频都没有,则赋值默认图片
            //if (string.IsNullOrEmpty(thumbnail))
            //thumbnail = ServerHelper.RootPath + "/Content/images/per_acc_v02.png";
            sdv.Thumbnail = thumbnail;//赋值专辑缩略图


            svvList = new List<SpecialVideoView>();
            int playCount = 0;//获取专辑下视频播放总数
            int commentCount = 0;
            int order = 1;
            foreach (Video v in videoList)
            {
                svv = new SpecialVideoView();
                svv.orderId = order;
                svv.Id = v.Id;
                svv.SmallPicturePath = v.SmallPicturePath;
                svv.Title = v.Title;
                svv.PlayCount = v.PlayCount;
                svv.CommentCount = v.CommentCount;
                commentCount += v.CommentCount;
                svv.TimeLength = ConvertUtil.ToInt(v.TimeLength, 0);
                svv.CreateTime = v.CreateTime.ToString("yyyy-MM-dd");
                svv.UpdateTime = ((DateTime)v.UpdateTime).ToString("yyyy-MM-dd");
                svvList.Add(svv);
                playCount += v.PlayCount;
                order++;
            }

            sdv.CommentCount = commentCount; //评论总数

            sdv.PlayCount = playCount;//获取专辑下视频播放总数

            sdv.SpecialVideoList = svvList;//专辑下视频集合

            sdv.VideoCount = videoList.Count;

            sdv.PageCount = GetPageCountToDataCount(sdv.VideoCount);

            sdv.SpecialVideoList = sdv.SpecialVideoList.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();

            return sdv;
        }

        public SpecialView GetUserAlbumsViews1()
        {
            int UserId = 42;
            int albumId = 144;

            var sv = new SpecialView() { SpecialVideoList = new List<SpecialDetailView>(), SpecialCount = 0 };

            var userSpecial = from dt in
                                  (from urc in _userSpecialRepository.GetEntityList(CondtionEqualCreateUserId(UserId))
                                   join uss in this._userSpecialSonRepository.GetEntityList() on urc.Id equals uss.MySpecialId into ussList
                                   from usst1 in ussList.DefaultIfEmpty()
                                   join video in _videoRepository.GetEntityList().Where(o => o.VideoState == 3).OrderByDescending(o => o.CreateTime) on
                                       usst1 == null ? 0 : usst1.VideoId equals video.Id into videoList
                                   from videot in videoList.DefaultIfEmpty()
                                   orderby urc.CreateTime
                                   select new
                                   {
                                       Id = urc.Id,
                                       Title = urc.Title,
                                       Image = urc.Image,
                                       CreateTime = urc.CreateTime,
                                       VideoId = videot == null ? 0 : videot.Id,
                                       PlayCount = videot == null ? 0 : videot.PlayCount,
                                       CommentCount = videot == null ? 0 : videot.CommentCount,
                                       SmallPicture = videot == null ? "" : videot.SmallPicturePath
                                   })
                              group dt by dt.Id into list
                              select new SpecialDetailView
                              {
                                  Id = list.Key,
                                  Title = list.First().Title,
                                  CreateTime = list.First().CreateTime.ToString("yyyy-MM-dd HH:mm"),
                                  Thumbnail = !string.IsNullOrEmpty(list.First().Image) ? list.First().Image : list.First().SmallPicture,
                                  PlayCount = list.Sum(o => o.PlayCount),
                                  CommentCount = list.Sum(o => o.CommentCount)
                              };


            sv.SpecialCount = userSpecial.Count();
            sv.PageCount = GetPageCountToDataCount(sv.SpecialCount);
            sv.SpecialVideoList = userSpecial.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();

            return sv;
        }

        public SpecialView GetUserAlbumsViews()
        {

            int UserId = 42;
            int albumId = 144;

            var sv = new SpecialView() { SpecialVideoList = new List<SpecialDetailView>(), SpecialCount = 0 };

            var userSpecial = (from urc in _userSpecialRepository.GetEntityList(CondtionEqualCreateUserId(UserId))
                               orderby urc.CreateTime descending
                               select urc
                               ).AsQueryable();
            List<UserSpecial> list = userSpecial.ToList<UserSpecial>();

            List<SpecialDetailView> sdvList = new List<SpecialDetailView>();
            SpecialDetailView sdv = null;

            List<SpecialVideoView> svvList = null;
            SpecialVideoView svv = null;
            List<Video> videoList = null;
            string thumbnail = "";


            foreach (UserSpecial item in list)
            {
                sdv = new SpecialDetailView();
                sdv.Id = item.Id;
                sdv.Title = item.Title;
                sdv.CreateTime = item.CreateTime.ToString("yyyy-MM-dd HH:mm");
                thumbnail = item.Image;

                var vidoes = (from ussl in this._userSpecialSonRepository.GetEntityList(CondtionEqualSpecialId(item.Id))
                              join video in this._videoRepository.GetEntityList(CondtionEqualState()) on (long)ussl.VideoId equals video.Id
                              where video.VideoState == 3 //todo 刘强2015-11-06添加审核通过的过滤条件
                              orderby ussl.CreateTime
                              select video
                           ).AsQueryable();
                videoList = vidoes.ToList<Video>();

                sdv.VideoCount = videoList.Count;//专辑下视频总数
                if (videoList != null && videoList.Count > 0 && string.IsNullOrEmpty(thumbnail))
                    thumbnail = videoList[videoList.Count - 1].SmallPicturePath;

                //如果专辑图片和视频都没有,则赋值默认图片
                //if (string.IsNullOrEmpty(thumbnail))
                //thumbnail = ServerHelper.RootPath + "/Content/images/per_acc_v02.png";
                sdv.Thumbnail = thumbnail;//赋值专辑缩略图


                svvList = new List<SpecialVideoView>();
                int playCount = 0;//获取专辑下视频播放总数
                foreach (Video v in videoList)
                {
                    svv = new SpecialVideoView();
                    svv.Id = v.Id;
                    svv.SmallPicturePath = v.SmallPicturePath;
                    svv.Title = v.Title;
                    svv.PlayCount = v.PlayCount;
                    svv.CommentCount += v.CommentCount;
                    svv.TimeLength = ConvertUtil.ToInt(v.TimeLength, 0);
                    svvList.Add(svv);
                    playCount += v.PlayCount;
                }

                sdv.PlayCount = playCount;//获取专辑下视频播放总数
                sdv.SpecialVideoList = svvList;//专辑下视频集合
                sdvList.Add(sdv);
            }
            sv.SpecialVideoList = sdvList;

            if (list.Count <= 0) return sv;
            sv.SpecialCount = list.Count;

            sv.PageCount = GetPageCountToDataCount(sv.SpecialCount);


            sv.SpecialVideoList = sv.SpecialVideoList.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();

            return sv;
        }



        [TestMethod]
        public void GetUserVideoViews()
        {
            int UserId = 42;
            int albumId = 144;
            Stopwatch ww = new Stopwatch();
            ww.Start();


            var result = new MyVideoViewResult() { MyVideoViews = new List<MyVideoView>(), TotalCount = 0 };
            //IQueryable<MyVideoView> queryable;
            //queryable =
            //  (from myvideo in _videoRepository.GetEntityList(CondtionEqualState())
            //   where myvideo.CreateManageId == UserId && myvideo.VideoState == 3//TODO 刘强CheckState=1
            //   && !(from uss in this._userSpecialSonRepository.GetEntityList(CondtionEqualSpecialId(albumId))
            //        select new
            //        {
            //            uss.VideoId
            //        }).Contains(new { VideoId = (Int32)myvideo.Id })
            //   select new MyVideoView
            //   {
            //       Id = myvideo.Id,
            //       Title = myvideo.Title,
            //       CommentCount = myvideo.CommentCount,
            //       PlayCount = myvideo.PlayCount,
            //       SmallPicturePath = myvideo.SmallPicturePath,
            //       VideoState = myvideo.VideoState,
            //       CreateTime = myvideo.CreateTime,
            //       About = myvideo.About
            //   }).AsQueryable();

            #region new
            var queryable = (from myvideo in _videoRepository.GetEntityList(CondtionEqualState()).Where(o => o.CreateManageId == UserId && o.VideoState == 3)
                             join uss in this._userSpecialSonRepository.GetEntityList() on myvideo.Id equals uss.VideoId into ussList
                             from ut in ussList.DefaultIfEmpty()
                             orderby myvideo.CreateTime
                             select new
                             {
                                 Id = myvideo.Id,
                                 Title = myvideo.Title,
                                 SmallPicturePath = myvideo.SmallPicturePath,
                                 SpecialId = ut == null ? 0 : ut.MySpecialId
                             });
            var list = queryable.GroupBy(o => o.Id).Where(o => o.FirstOrDefault(s => s.SpecialId == albumId) == null)
                .Select(o => new MyVideoView
                {
                    Id = o.Key,
                    Title = o.First().Title,
                    SmallPicturePath = o.First().SmallPicturePath
                });
            #endregion



            //if (!queryable.Any()) return result;
            result.TotalCount = list.Count();
            result.PageCount = GetPageCountToDataCount(result.TotalCount);
            result.MyVideoViews = list.Skip(PageSize * (PageIndex - 1)).Take(PageSize).ToList();

            ww.Stop();
            long ttt = ww.ElapsedTicks;
            //return result;
        }

        private void CheckAlbumId(int albumId, out UserSpecial us)
        {
            us = this._userSpecialRepository.GetEntity(ConditionEqualId(albumId));
            AssertUtil.IsNotNull(us, "专辑不存在");
        }

        private void CheckAlbumId(int albumId)
        {
            AssertUtil.AreBigger(albumId, 0, "专辑参数传递错误，专辑不存在！");
        }

        private Condtion CondtionEqualSpecialId(int specialId)
        {
            var condtion = new Condtion()
            {
                FiledName = "MySpecialId",
                FiledValue = specialId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return condtion;
        }

        protected Condtion CondtionEqualState(bool state = false)
        {
            var condtion = new Condtion()
            {
                FiledName = "State",
                FiledValue = state,
                ExpressionType = ExpressionType.Equal,
                ExpressionLogic = ExpressionLogic.And
            };
            return condtion;
        }

        protected int GetPageCountToDataCount(int dataCount)
        {

            PageSize = PageSize <= 0 ? 10 : PageSize;
            int PageCount = dataCount % PageSize == 0 ? dataCount / PageSize : (dataCount / PageSize + 1);
            PageIndex = PageIndex <= 0 ? 1 : PageIndex;
            PageIndex = PageCount <= PageIndex ? PageCount : PageIndex;
            return PageCount;
        }

        [TestMethod]
        public void MyTestKey()
        {
            try
            {
                string strReadPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\a.txt";
                string strWritePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\b.txt";
                string strData = File.ReadAllText(strReadPath);
                string[] lines = strData.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                List<string> List = new List<string>();
                List.AddRange(lines);
                //for (int i = 0; i < lines.Length; i++)
                //{
                //    if ((i + 1) % 2 == 0)
                //    {
                //        List.Add(lines[i]);
                //    }
                //}
                Hashtable table = new Hashtable();
                for (int i = 0; i < List.Count; i++)
                {
                     string[] strArr= List[i].Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                     string key = strArr[0] + strArr[1];
                    if (!table.ContainsKey(key))
                    {
                        table.Add(key, List[i]);
                        File.AppendAllText(strWritePath, string.Join("\t", strArr) + "\r\n", Encoding.Default);
                    }
                    else
                    {
                        // File.AppendAllText(strWritePath, List[i] + "\r\n");
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
