using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Language;
using HKSJ.WBVV.Common.Logger;
using HKSJ.WBVV.Entity;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;

namespace HKSJ.WBVV.Business.Search
{
    public class IndexManager
    {
        public static readonly IndexManager _videoIndex;
        public static readonly string indexPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "IndexData");
        private static readonly object lockHelper = new object();

        static IndexManager()
        {
            if (_videoIndex == null)
            {
                lock (lockHelper)
                {
                    if (_videoIndex == null)
                    {
                        _videoIndex = new IndexManager();
                        string panGuXMLPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "PanGu.xml");
                        PanGu.Segment.Init(panGuXMLPath);
                    }
                }
            }
        }

        public static IndexManager VideoIndex
        {
            get
            {
                return _videoIndex;
            }
        }


        private IndexManager()
        {

        }
        //请求队列 解决索引目录同时操作的并发问题
        private Queue<VideoViewMode> VideoQueue = new Queue<VideoViewMode>();
        /// <summary>
        /// 新增Videos表信息时 添加邢增索引请求至队列
        /// </summary>
        /// <param name="Videos"></param>
        public void Add(Video Videos)
        {
            VideoViewMode bvm = new VideoViewMode();
            bvm.Id = Videos.Id;
            bvm.Title = Videos.Title;
            bvm.IT = IndexType.Insert;
            bvm.About = Videos.About;
            bvm.Tags = Videos.Tags;
            bvm.CategoryId = Videos.CategoryId;
            bvm.BigPicturePath = Videos.BigPicturePath;
            bvm.SmallPicturePath = Videos.SmallPicturePath;
            bvm.CreateTime = Videos.CreateTime;
            bvm.IsOfficial = Videos.IsOfficial;
            bvm.PlayCount = Videos.PlayCount;
            bvm.CommentCount = Videos.CommentCount;
            bvm.VideoState = Videos.VideoState;
            bvm.CreateManageId = Videos.CreateManageId;
            VideoQueue.Enqueue(bvm);
        }
        /// <summary>
        /// 删除Videos表信息时 添加删除索引请求至队列
        /// </summary>
        /// <param name="bid"></param>
        public void Del(int vid)
        {
            VideoViewMode bvm = new VideoViewMode();
            bvm.Id = vid;
            bvm.IT = IndexType.Delete;
            VideoQueue.Enqueue(bvm);
        }
        /// <summary>
        /// 修改Videos表信息时 添加修改索引(实质上是先删除原有索引 再新增修改后索引)请求至队列
        /// </summary>
        /// <param name="Videos"></param>
        public void Mod(Video Videos)
        {
            VideoViewMode bvm = new VideoViewMode();
            bvm.Id = Videos.Id;
            bvm.Title = Videos.Title;
            bvm.IT = IndexType.Modify;
            bvm.About = Videos.About;
            bvm.Tags = Videos.Tags;
            bvm.CategoryId = Videos.CategoryId;
            bvm.BigPicturePath = Videos.BigPicturePath;
            bvm.SmallPicturePath = Videos.SmallPicturePath;
            bvm.CreateTime = Videos.CreateTime;
            bvm.IsOfficial = Videos.IsOfficial;
            bvm.PlayCount = Videos.PlayCount;
            bvm.CommentCount = Videos.CommentCount;
            bvm.VideoState = Videos.VideoState;
            bvm.CreateManageId = Videos.CreateManageId;
            VideoQueue.Enqueue(bvm);
        }

        public void StartNewThread()
        {
            //string panGuXMLPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "PanGu.xml");
            //PanGu.Segment.Init(panGuXMLPath);
            //try
            //{
            //    LogBuilder.Log4Net.Info("开启索引进程2");
            //    Thread th = new Thread(() =>
            //    {
            QueueToIndex();
            //                });
            //                th.IsBackground = true;

            //                th.Start();
            //                LogBuilder.Log4Net.Info("开启索引进程成功2");
            //            }
            //            catch (Exception ex)
            //            {
            //#if !DEBUG
            //                        LogBuilder.Log4Net.Error("开启索引进程失败2", ex.MostInnerException());
            //#else
            //                Console.WriteLine("开启索引进程失败2:" + ex.MostInnerException().Message);
            //#endif

            //            }


            // ThreadPool.QueueUserWorkItem(new WaitCallback(QueueToIndex));
        }

        //定义一个线程 将队列中的数据取出来 插入索引库中
        private void QueueToIndex()
        {
            try
            {
                while (true)
                {
                    if (VideoQueue.Count > 0)
                    {
                        CRUDIndex();
                    }
                    else
                    {
                        Thread.Sleep(3000);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is ThreadAbortException)
                {
                    LogBuilder.Log4Net.Info(LanguageUtil.Translate("api_Business_Search_IndexManager_Info"));
                }
                else
                {
                    LogBuilder.Log4Net.Error(LanguageUtil.Translate("api_Business_Search_IndexManager_error"), ex.MostInnerException());
                }
            }
        }
        /// <summary>
        /// 更新索引库操作
        /// </summary>
        private void CRUDIndex()
        {
            FSDirectory fsDirectory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory());

            bool isExist = IndexReader.IndexExists(fsDirectory);
            if (isExist)
            {
                if (IndexWriter.IsLocked(fsDirectory))
                {
                    IndexWriter.Unlock(fsDirectory);
                }
            }
            IndexWriter writer = new IndexWriter(fsDirectory, new PanGuAnalyzer(), !isExist,
                IndexWriter.MaxFieldLength.UNLIMITED);

            // writer.SetMergeFactor(30);
            try
            {
                #region 更新索引
                while (VideoQueue.Count > 0)
                {
                    Document document = new Document();
                    VideoViewMode Video = VideoQueue.Dequeue();
                    if (Video.IT == IndexType.Insert)
                    {
                        document.Add(new Field("id", Video.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                        document.Add(new Field("categoryId", Video.CategoryId.ToString(), Field.Store.YES,
                            Field.Index.NOT_ANALYZED));
                        document.Add(new Field("title", string.IsNullOrEmpty(Video.Title) ? "" : Video.Title,
                            Field.Store.YES, Field.Index.ANALYZED,
                            Field.TermVector.WITH_POSITIONS_OFFSETS));
                        document.Add(new Field("about", string.IsNullOrEmpty(Video.About) ? "" : Video.About,
                            Field.Store.YES, Field.Index.ANALYZED,
                            Field.TermVector.WITH_POSITIONS_OFFSETS));
                        document.Add(new Field("tags", string.IsNullOrEmpty(Video.Tags) ? "" : Video.Tags,
                            Field.Store.YES, Field.Index.ANALYZED,
                            Field.TermVector.WITH_POSITIONS_OFFSETS));
                        document.Add(new Field("videoState",Video.VideoState.ToString(),Field.Store.YES, Field.Index.ANALYZED,
                           Field.TermVector.WITH_POSITIONS_OFFSETS));
                        document.Add(new Field("playCount", Video.PlayCount.ToString(), Field.Store.YES,
                            Field.Index.NOT_ANALYZED));
                        document.Add(new Field("commentCount", Video.CommentCount.ToString(), Field.Store.YES,
                            Field.Index.NOT_ANALYZED));
                        document.Add(new Field("bigPicturePath", Video.BigPicturePath, Field.Store.YES,
                            Field.Index.NOT_ANALYZED));
                        document.Add(new Field("smallPicturePath", Video.SmallPicturePath, Field.Store.YES,
                            Field.Index.NOT_ANALYZED));
                        document.Add(new Field("createTime", Video.CreateTime.ToString(), Field.Store.YES,
                            Field.Index.NOT_ANALYZED));
                        document.Add(new Field("createTimeNum", Video.CreateTime.ToString("yyyyMMddHHmmss"), Field.Store.YES,
                            Field.Index.NOT_ANALYZED));
                        document.Add(new Field("createManageId", Video.CreateManageId.ToString(), Field.Store.YES,
                           Field.Index.NOT_ANALYZED));
                        document.Add(new Field("isOfficial", Video.IsOfficial.ToString(), Field.Store.YES,
                            Field.Index.NOT_ANALYZED));
                        if (Video.IsOfficial == true)
                        {
                            document.Add(new Field("isofficialtrue", LanguageUtil.Translate("api_Business_Search_IndexManager_CRUDIndex_isofficialtrue_Insert"), Field.Store.YES, Field.Index.ANALYZED,
                                Field.TermVector.WITH_POSITIONS_OFFSETS));
                        }
                        document.GetField("title").SetBoost(2F);
                        writer.AddDocument(document);
                    }
                    else if (Video.IT == IndexType.Delete)
                    {
                        writer.DeleteDocuments(new Term("id", Video.Id.ToString()));
                    }
                    else if (Video.IT == IndexType.Modify)
                    {
                        //先删除 再新增
                        writer.DeleteDocuments(new Term("id", Video.Id.ToString()));
                        document.Add(new Field("id", Video.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                        document.Add(new Field("categoryId", Video.CategoryId.ToString(), Field.Store.YES,
                            Field.Index.NOT_ANALYZED));
                        document.Add(new Field("title", string.IsNullOrEmpty(Video.Title) ? "" : Video.Title,
                            Field.Store.YES, Field.Index.ANALYZED,
                            Field.TermVector.WITH_POSITIONS_OFFSETS));
                        document.Add(new Field("about", string.IsNullOrEmpty(Video.About) ? "" : Video.About,
                            Field.Store.YES, Field.Index.ANALYZED,
                            Field.TermVector.WITH_POSITIONS_OFFSETS));
                        document.Add(new Field("tags", string.IsNullOrEmpty(Video.Tags) ? "" : Video.Tags,
                            Field.Store.YES, Field.Index.ANALYZED,
                            Field.TermVector.WITH_POSITIONS_OFFSETS));
                        document.Add(new Field("videoState", Video.VideoState.ToString(), Field.Store.YES, Field.Index.ANALYZED,
                           Field.TermVector.WITH_POSITIONS_OFFSETS));
                        document.Add(new Field("playCount", Video.PlayCount.ToString(), Field.Store.YES,
                            Field.Index.NOT_ANALYZED));
                        document.Add(new Field("commentCount", Video.CommentCount.ToString(), Field.Store.YES,
                            Field.Index.NOT_ANALYZED));
                        document.Add(new Field("bigPicturePath", Video.BigPicturePath, Field.Store.YES,
                            Field.Index.NOT_ANALYZED));
                        document.Add(new Field("smallPicturePath", Video.SmallPicturePath, Field.Store.YES,
                            Field.Index.NOT_ANALYZED));
                        document.Add(new Field("createTimeNum", Video.CreateTime.ToString("yyyyMMddHHmmss"), Field.Store.YES,
                            Field.Index.NOT_ANALYZED));
                        document.Add(new Field("createTime", Video.CreateTime.ToString(), Field.Store.YES,
                           Field.Index.NOT_ANALYZED));
                        document.Add(new Field("createManageId", Video.CreateManageId.ToString(), Field.Store.YES,
                           Field.Index.NOT_ANALYZED));
                        document.Add(new Field("isOfficial", Video.IsOfficial.ToString(), Field.Store.YES,
                            Field.Index.NOT_ANALYZED));
                        if (Video.IsOfficial == true)
                        {
                            document.Add(new Field("isofficialtrue", LanguageUtil.Translate("api_Business_Search_IndexManager_CRUDIndex_isofficialtrue_Modify"), Field.Store.YES, Field.Index.ANALYZED,
                                Field.TermVector.WITH_POSITIONS_OFFSETS));
                        }
                        //设置索引字段权重
                        document.GetField("title").SetBoost(2F);
                        writer.AddDocument(document);
                    }

                }
                #endregion
            }
            catch (Exception ex)
            {
#if !DEBUG
                       LogBuilder.Log4Net.Error(LanguageUtil.Translate("api_Business_Search_IndexManager_CRUDIndex_catch"), ex.MostInnerException());
#else
                Console.WriteLine("更新索引操作CRUD失败:" + ex.MostInnerException().Message);
#endif

            }
            finally
            {
                writer.Optimize(); //添加完后合并
                writer.Close();
                fsDirectory.Close();
            }
        }

        //删除全部索引
        public void delAllIndex()
        {
            if (System.IO.Directory.Exists(indexPath) == false)
            {
                System.IO.Directory.CreateDirectory(indexPath);
            }
            FSDirectory fsDirectory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory());
            if (!IndexReader.IndexExists(fsDirectory)) return;
            else
            {
                if (IndexReader.IsLocked(fsDirectory))
                {
                    IndexReader.Unlock(fsDirectory);
                }
            }
            Lucene.Net.Index.IndexWriter iw = new Lucene.Net.Index.IndexWriter(indexPath, new PanGuAnalyzer(), false);
            //  iw.DeleteDocuments(new Lucene.Net.Index.Term("Key", key));
            iw.DeleteAll();
            iw.Optimize();//删除文件后并非从磁盘中移除，而是生成一个.del的文件，需要调用Optimize方法来清除。在清除文件前可以使用UndeleteAll方法恢复
            iw.Close();
        }
    }

    public class VideoViewMode
    {
        public long Id
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        public string Tags
        {
            get;
            set;
        }
        public string About
        {
            get;
            set;
        }
        public int CategoryId
        {
            get;
            set;
        }
        public string SmallPicturePath
        {
            get;
            set;
        }
        public string BigPicturePath
        {
            get;
            set;
        }

        public int PlayCount { get; set; }
        public int CommentCount { get; set; }

        public IndexType IT
        {
            get;
            set;
        }

        public DateTime CreateTime { get; set; }

        public bool IsOfficial { get; set; }
        public short VideoState { get; set; }
        public int CreateManageId { get; set; }

    }
    //操作类型枚举
    public enum IndexType
    {
        Insert,
        Modify,
        Delete
    }

}
