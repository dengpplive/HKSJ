using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Common.Logger;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Repository.Interface;
using Newtonsoft.Json.Linq;
using Qiniu.Conf;
using Qiniu.RPC;
using Qiniu.RS;
using Qiniu.Util;
using ExpressionType = HKSJ.WBVV.Common.Extender.LinqExtender.ExpressionType;
using HKSJ.Utilities;
using HKSJ.WBVV.Entity.Response.App;

namespace HKSJ.WBVV.Business
{
    public class QiniuUploadBusiness : BaseBusiness, IQiniuUploadBusiness
    {
        private readonly IVideoRepository _videoRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserSpecialRepository _userSpecialRepository;
        private readonly IQiniuFopLogRepository _qiniuFopLogRepository;


        public static readonly string privateBucket = ConfigurationManager.AppSettings["QiniuPrivateBucket"];
        public static readonly string privateDomain = ConfigurationManager.AppSettings["QiniuPrivateDomain"];
        public static readonly string publicBucket = ConfigurationManager.AppSettings["QiniuPublicBucket"];
        public static readonly string publicDomain = ConfigurationManager.AppSettings["QiniuPublicDomain"];

        public static readonly string[] pipelines = ConfigurationManager.AppSettings["QiniuPipelinePool"].Split(';');
        public static readonly string callBackUrl = ConfigurationManager.AppSettings["QiniuCallBackURL"];
        private static readonly string accessKey = ConfigurationManager.AppSettings["QiniuAccessKey"];
        private static readonly string secretKey = ConfigurationManager.AppSettings["QiniuSecretKey"];


        public QiniuUploadBusiness(IVideoRepository videoRepository, IUserRepository userRepository, IUserSpecialRepository userSpecialRepository, IQiniuFopLogRepository qiniuFopLogRepository)
        {
            _videoRepository = videoRepository;
            _userRepository = userRepository;
            this._userSpecialRepository = userSpecialRepository;
            this._qiniuFopLogRepository = qiniuFopLogRepository;

        }

        public string PublicDomain
        {
            get { return publicDomain; }
        }

        public string PrivateDomain
        {
            get { return privateDomain; }

        }

        #region CallBack

        public dynamic Callback(JObject postData)
        {
            string type = postData.Value<string>("BusinessType");
            string fileKey = postData.Value<string>("key");

            if (!IsQiniuCallback(postData))
            {
                return new { data = "Bad Request!" };
            }
            switch (type)
            {
                case "video":
                    return VideoCallback(postData);
                case "pic":
                case "album":
                    return PictureCallBack(postData);
                case "cover":
                    return CoverCallBack(postData);
                case "banner":
                    return BannerCallBack(postData);
            }


            string saveAs = type + "_" + Guid.NewGuid().ToString("N") + ".jpg";
            SaveAsImage(fileKey, saveAs);

            var returnJson = new
            {
                key = saveAs
            };
            return returnJson;
        }

        public AppUploadTokenView GetAppUploadToken(string type, long uid)
        {
            string extention = "";
            if ("video" == type)
            {
                extention = ".mp4";
            }
            else
            {
                extention = ".jpg";
            }

            string token = GetUploadToken(type, "", uid);
            string key = type + "_" + Guid.NewGuid().ToString("N") + extention;


            return new AppUploadTokenView
            {
                Token = token,
                Key = key
            };
        }

        #region 获取上传 Token
        public string GetUploadToken(string type, string imgPara = "", long uid = 0)
        {
            string bucket;
            string domain;
            GetTokenAndDomain(type, out bucket, out domain);

            string entryKey = DESEncrypt.Encrypt(GetEncryptText(DateTime.Now));

            PutPolicy put = new PutPolicy(bucket, 8 * 60 * 60);
            put.CallBackUrl = "http://" + callBackUrl + "/api/QiniuUpload/Callback";

            string callBackBody = "BusinessType=" + type +
                                   "&WOBO=" + entryKey +
                                   "&key=$(key)&hash=$(etag)&filesize=$(fsize)";


            callBackBody += type.Equals("video") ? "&avinfo=$(avinfo)" : "&imageInfo=$(imageInfo)";
            if (!string.IsNullOrEmpty(imgPara))
            {
                callBackBody += "&imgPara=" + imgPara;
            }
            if (uid >= 1)
            {
                callBackBody += "&userId=" + uid;
            }

            put.CallBackBody = callBackBody;
            return put.Token();
        }

        #endregion

        #endregion

        #region Banner CallBack

        private dynamic BannerCallBack(JObject postData)
        {

            string type = postData.Value<string>("BusinessType");
            string fileKey = postData.Value<string>("key");
            string guidName = Guid.NewGuid().ToString("N") + ".jpg";

            string saveBigAs = type + "_" + guidName;
            if (SaveAsImage(fileKey, saveBigAs))
            {
                string imgPara = "imageMogr2/thumbnail/318/crop/!200x96a59a0";
                string smallThumbKey = type + "_small_" + guidName;
                string thumbencodedEntryURI = Base64URLSafe.Encode(publicBucket + ":" + smallThumbKey);
                string thumbsaveas = "|saveas/" + thumbencodedEntryURI;

                string imgFops = imgPara + thumbsaveas;
                Pfop pfop = new Pfop();

                string pileline = GetPipeline();
                PfopDo(pfop, publicBucket, saveBigAs, imgFops, pileline, smallThumbKey, 0);

                return new
                {
                    bigKey = saveBigAs,
                    smallKey = smallThumbKey
                };
            }
            return new
            {
                bigKey = saveBigAs,
                smallKey = saveBigAs
            };
        }
        #endregion

        #region 更改封面的 CallBack
        private dynamic CoverCallBack(JObject postData)
        {
            string type = postData.Value<string>("BusinessType");
            string fileKey = postData.Value<string>("key");
            string imgPara = postData.Value<string>("imgPara");
            if (string.IsNullOrEmpty(imgPara))
            {
                return new { key = fileKey };
            }

            string guidName = Guid.NewGuid().ToString("N") + ".jpg";
            string thumbKey = type + "_" + guidName;
            string thumbencodedEntryURI = Base64URLSafe.Encode(publicBucket + ":" + thumbKey);
            string thumbsaveas = "|saveas/" + thumbencodedEntryURI;

            string pileline = GetPipeline();
            string imgFops = imgPara + thumbsaveas;
            Pfop pfop = new Pfop();
            PfopDo(pfop, publicBucket, fileKey, imgFops, pileline, thumbKey, 0);


            string smallthumbKey = type + "_small_" + guidName;
            string smallthumbencodedEntryURI = Base64URLSafe.Encode(publicBucket + ":" + smallthumbKey);
            string smallThumbsaveas = "|saveas/" + smallthumbencodedEntryURI;

            string smallImgPara = GetSmallImageMogr2(imgPara);
            string samllImgFops = smallImgPara + smallThumbsaveas;

            pileline = GetPipeline();
            PfopDo(pfop, publicBucket, fileKey, samllImgFops, pileline, smallthumbKey, 0);

            return new
            {
                bigKey = thumbKey,
                smallKey = smallthumbKey
            };
        }


        private string GetSmallImageMogr2(string imageMogr)
        {
            const string thumbRegex = @"thumbnail/(\d*)/crop/!(\d*)x(\d*)a(\d*)a(\d*)";
            Regex reg = new Regex(thumbRegex);
            var mat = reg.Match(imageMogr);
            int thumb = Int32.Parse(mat.Groups[1].ToString());
            int cropX = Int32.Parse(mat.Groups[2].ToString());
            int cropY = Int32.Parse(mat.Groups[3].ToString());
            int pX = Int32.Parse(mat.Groups[4].ToString());
            int pY = Int32.Parse(mat.Groups[5].ToString());


            float vRatio = (float)200 / cropX;

            return string.Format("imageMogr2/thumbnail/{0}/crop/!{1}x{2}a{3}a{4}", (int)(vRatio * thumb), (int)(vRatio * cropX), (int)(vRatio * cropY),
                (int)(vRatio * pX), (int)(vRatio * pY));
        }

        #endregion

        #region 更改头像的 CallBack

        public dynamic PictureCallBack(JObject postData)
        {
            string type = postData.Value<string>("BusinessType");
            string fileKey = postData.Value<string>("key");
            string imgPara = postData.Value<string>("imgPara");
            int userId = postData.Value<int>("userId");
            string thumbKey = fileKey;
            if (!string.IsNullOrEmpty(imgPara))
            {
                thumbKey = type + "_" + Guid.NewGuid().ToString("N") + ".jpg";
                string thumbencodedEntryURI = Base64URLSafe.Encode(publicBucket + ":" + thumbKey);
                string thumbsaveas = "|saveas/" + thumbencodedEntryURI;

                string imgFops = imgPara + thumbsaveas;
                Pfop pfop = new Pfop();

                string pileline = GetPipeline();
                PfopDo(pfop, publicBucket, fileKey, imgFops, pileline, thumbKey, userId);
            }

            if (type == "pic")
            {
                var user = _userRepository.GetEntity(ConditionEqualId(userId));
                DeleteQiniuImageByKey(user.Picture);
                user.Picture = thumbKey;
                _userRepository.UpdateEntity(user);
            }
            else if (type == "album")
            {
                UserSpecial us = this._userSpecialRepository.GetEntity(ConditionEqualId(userId));
                if (us != null)
                {
                    us.Image = thumbKey;
                    this._userSpecialRepository.UpdateEntity(us);
                }
            }

            return new
            {
                key = thumbKey
            };
        }
        #endregion

        #region 视频 CallBack
        public dynamic VideoCallback(JObject postData)
        {
            string fileKey = postData.Value<string>("key");
            string userIdstr = postData.Value<string>("userId");
            int userId = 0;
            if (!int.TryParse(userIdstr, out userId))
            {
                return "Please Login Account";
            }

            string saveVideo = "video_" + fileKey;
            var orign = GetVideoByKey(saveVideo);
            if (orign != null)
            {
                //数据已存储
                return "Data is stored";
            }
            if (!SaveAsVideo(fileKey, saveVideo))
            {
                return new { key = fileKey };
            }


            float outDuration = 0;
            float outBigWidth = 0;
            float outBigHeight = 0;

            float outSmallWidth = 0;
            float outSmallHeight = 0;

            string avinfoStr = postData.Value<string>("avinfo");
            string hashCode = postData.Value<string>("hash");

            if (!string.IsNullOrEmpty(avinfoStr))
            {
                JObject avinfoJobj = JObject.Parse(avinfoStr);
                string durationStr = avinfoJobj["format"].Value<string>("duration");
                durationStr = string.IsNullOrEmpty(durationStr)
                    ? avinfoJobj["video"].Value<string>("duration")
                    : durationStr;
                string widthStr = avinfoJobj["video"].Value<string>("width");
                string heightStr = avinfoJobj["video"].Value<string>("height");
                float.TryParse(durationStr, out outDuration);

                float w = 0;
                float h = 0;
                if (float.TryParse(widthStr, out w) && float.TryParse(heightStr, out h))
                {
                    setBigImageSize(w, h, out outBigWidth, out  outBigHeight);
                    setSmallImageSize(w, h, out outSmallWidth, out  outSmallHeight);
                }
            }
            Pfop pfop = new Pfop();


            //生成缩略图
            // refer to http://developer.qiniu.com/docs/v6/api/reference/fop/av/vframe.html
            // refer to http://developer.qiniu.com/docs/v6/api/reference/fop/saveas.html

            int offsetTime = (int)(outDuration / 2);
            offsetTime = offsetTime > 60 ? 60 : offsetTime;

            //大图
            string guid = Guid.NewGuid().ToString("N");
            string thumbKey = "cover_" + guid + ".jpg";
            string imgFops = VideoImgFop(thumbKey, offsetTime, (int)outBigWidth, (int)outBigHeight);

            // 小图
            string smallPictureKey = "cover_small_" + guid + ".jpg";
            string smallImgFops = VideoImgFop(smallPictureKey, offsetTime, (int)outSmallWidth, (int)outSmallHeight);

            //m3u8
            string m3u8Key = "m3u8_" + guid + ".m3u";
            string videoFops = VideoFop(m3u8Key);

            string mp4Key = "mp4_" + guid + ".mp4";
            string videoFop4mp4 = VideoFop4mp4(mp4Key);

            DateTime timeNow = DateTime.Now;
            Video video = new Video
            {
                CreateManageId = userId,
                VideoRosella = saveVideo,
                VideoState = 0,
                VideoSource = true,
                TimeLength = (int)outDuration,
                VideoPath = m3u8Key,
                BigPicturePath = thumbKey,
                SmallPicturePath = smallPictureKey,
                CreateTime = timeNow,
                UpdateTime = timeNow,
                HashCode = hashCode,
                DownloadPath = mp4Key
            };
            _videoRepository.CreateEntity(video);


            //处理m3u8
            string pipline = GetPipeline();
            PfopDo(pfop, privateBucket, saveVideo, videoFops, pipline, m3u8Key, userId, video.Id);
            //处理小图
            pipline = GetPipeline();
            PfopDo(pfop, privateBucket, saveVideo, smallImgFops, pipline, smallPictureKey, userId);
            //处理大图
            pipline = GetPipeline();
            PfopDo(pfop, privateBucket, saveVideo, imgFops, pipline, thumbKey, userId);

            //mp4
            pipline = GetPipeline();
            PfopDo(pfop, privateBucket, saveVideo, videoFop4mp4, pipline, mp4Key, userId);

            var returnJson = new
            {
                videoId = video.Id,
                smallKey = smallPictureKey,
                bigKey = thumbKey,
                key = saveVideo
            };
            return returnJson;
        }
        #endregion

        #region NOtify
        public void Notify(JObject postData)
        {
            //视频转码状态(0.转码中;1：转码成功（待审批）；2：转码失败；)
            //VideoState视频状态（0：转码中，1：转码失败，2：审核中，3：审核通过，4：审核不通过）
            //http://developer.qiniu.com/docs/v6/api/overview/fop/fop/persistent-fop.html

            string inputKey = postData.Value<string>("inputKey");

            //状态码，0 表示成功，1 表示等待处理，2 表示正在处理，3 表示处理失败，4 表示回调失败。
            short code = postData.Value<short>("code");

            int count = postData["items"].Count();
            for (int i = 0; i < count; i++)
            {
                string itemKey = postData["items"][i].Value<string>("key");
                int state = code == 0 ? 2 : 1;

                if (inputKey.StartsWith("video_") && state == 1)
                {
                    var condtion = new Condtion()
                    {
                        FiledName = "VideoRosella",
                        FiledValue = inputKey,
                        ExpressionLogic = ExpressionLogic.And,
                        ExpressionType = ExpressionType.Equal
                    };
                    var video = _videoRepository.GetEntity(condtion);
                    if (video != null)
                    {
                        video.VideoState = (short)state;
                        video.UpdateTime = DateTime.Now;
                        _videoRepository.UpdateEntity(video);
                    }
                }

                if (itemKey != null)
                {
                    if (itemKey.StartsWith("m3u8_"))
                    {
                        var condtion = new Condtion()
                        {
                            FiledName = "VideoRosella",
                            FiledValue = inputKey,
                            ExpressionLogic = ExpressionLogic.And,
                            ExpressionType = ExpressionType.Equal
                        };
                        var video = _videoRepository.GetEntity(condtion);
                        if (video != null)
                        {
                            video.VideoState = (short)state;
                            video.UpdateTime = DateTime.Now;
                            _videoRepository.UpdateEntity(video);
                        }
                    }
                    else if (itemKey.StartsWith("cover_"))
                    {
                        //DeleteQiniuImageByKey(inputKey);
                        Move2PublicBucket(itemKey);
                    }
                    else if (itemKey.StartsWith("banner_"))
                    {
                        Move2PublicBucket(itemKey);
                    }
                    else if (itemKey.StartsWith("pic_") || itemKey.StartsWith("album_"))
                    {
                        //var condtion = new Condtion()
                        //{
                        //    FiledName = "Picture",
                        //    FiledValue = inputKey,
                        //    ExpressionLogic = ExpressionLogic.And,
                        //    ExpressionType = ExpressionType.Equal
                        //};
                        //var user = _userRepository.GetEntity(condtion);
                        //DeleteQiniuImageByKey(user.Picture);
                        //user.Picture = itemKey;
                        //_userRepository.UpdateEntity(user);
                        DeleteQiniuImageByKey(inputKey);
                    }
                    //else if (itemKey.StartsWith("album_"))
                    //{
                    //    var condtion = new Condtion()
                    //    {
                    //        FiledName = "Image",
                    //        FiledValue = inputKey,
                    //        ExpressionLogic = ExpressionLogic.And,
                    //        ExpressionType = ExpressionType.Equal
                    //    };
                    //    var us = _userSpecialRepository.GetEntity(condtion);
                    //    DeleteQiniuImageByKey(us.Image);
                    //    us.Image = itemKey;
                    //    _userSpecialRepository.UpdateEntity(us);

                    //}
                }
            }

        }

        #endregion


        #region 获取下载地址
        public string GetDownloadUrl(string fileKey, string type = "image")
        {

            string bucket;
            string domain;
            GetTokenAndDomain(type, out bucket, out domain);
            if (type == "video")
            {
                if (string.IsNullOrWhiteSpace(fileKey))
                {
                    return string.Empty;
                }
                string baseUrl = GetPolicy.MakeBaseUrl(domain, fileKey);
                string privateUrl = GetPolicy.MakeRequest(baseUrl);
                return privateUrl;
            }
            else
            {
                return string.Format("http://{0}/{1}", domain, fileKey);
            }

        }

        #endregion

        #region 删除七年云中的视频数据
        public void DeleteQiniuData(Video video)
        {
            string bucket;
            string domain;
            GetTokenAndDomain("video", out bucket, out domain);


            IList<string> videoKeyList = new List<string>();
            videoKeyList.Add(video.VideoPath);
            videoKeyList.Add(video.VideoRosella);
            var subObj = _videoRepository.GetEntity(new Condtion()
            {
                FiledName = "HashCode",
                FiledValue = video.HashCode,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            });

            if (subObj == null)
            {
                string mu38Path = GetDownloadUrl(video.VideoPath, "video");
                string mu38Content = CommonMethod.SendUrl(mu38Path);
                Regex reg = new Regex(@"\.com/(.+\.ts)");
                var mat = reg.Matches(mu38Content);
                foreach (Match item in mat)
                {
                    videoKeyList.Add(item.Groups[1].ToString());
                }
            }
            DeleteQiniuDataByKey(videoKeyList.ToArray(), "video");

            IList<string> imgKeyList = new List<string>();
            imgKeyList.Add(video.BigPicturePath);
            imgKeyList.Add(video.SmallPicturePath);
            DeleteQiniuDataByKey(imgKeyList.ToArray(), "image");
        }
        #endregion


        #region 删除七牛中的数据


        public void DeleteQiniuData(long id)
        {
            var video = _videoRepository.GetEntity(ConditionEqualId((int)id));
            DeleteQiniuData(video);
        }

        public void DeleteQiniuImageByKey(string key)
        {
            string[] keys = { key };
            DeleteQiniuDataByKey(keys, "image");
        }

        public void DeleteQiniuDataByKey(string[] keys, string type)
        {
            string bucket;
            string domain;
            GetTokenAndDomain(type, out bucket, out domain);

            RSClient client = new RSClient();
            List<EntryPath> EntryPaths = new List<EntryPath>();
            StringBuilder sbLog = new StringBuilder();
            sbLog.AppendLine(string.Format(
                "Delete Qiniu Data----------------------->"));

            foreach (string key in keys)
            {
                sbLog.AppendLine(key);
                EntryPaths.Add(new EntryPath(bucket, key));
            }
            sbLog.AppendLine(string.Format(
                "Delete Qiniu Data <--------------------------"));
            client.BatchDelete(EntryPaths.ToArray());
            LogBuilder.Log4Net.Debug(sbLog.ToString());
        }
        #endregion

        #region 移动文件

        public bool Move2PublicBucket(string key)
        {
            return MoveQiniuFile(privateBucket, key, publicBucket, key);
        }

        public bool SaveAsImage(string key, string saveAs)
        {
            return MoveQiniuFile(publicBucket, key, publicBucket, saveAs);
        }

        public bool SaveAsVideo(string key, string saveAs)
        {
            return MoveQiniuFile(privateBucket, key, privateBucket, saveAs);
        }

        public bool MoveQiniuFile(string bucketSrc, string keySrc, string bucketDest, string keyDest)
        {
            //Console.WriteLine("\n===> Move {0}:{1} To {2}:{3}",
            //bucketSrc, keySrc, bucketDest, keyDest);
            RSClient client = new RSClient();
            new EntryPathPair(bucketSrc, keySrc, bucketDest, keyDest);
            CallRet ret = client.Move(new EntryPathPair(bucketSrc, keySrc, bucketDest, keyDest));
            return ret.OK;
        }


        #endregion

        #region 设置视频截图大小
        /// <summary>
        /// 设置视频截图大小
        /// </summary>
        /// <param name="inWidth"></param>
        /// <param name="inHeight"></param>
        /// <param name="outWidth"></param>
        /// <param name="outHight"></param>
        private void SetImageSize(float inWidth, float inHeight, out float outWidth, out float outHight, float oWidth, float oHeight)
        {

            if (inWidth / oWidth > inHeight / oHeight)
            {
                outWidth = oWidth;
                outHight = inHeight / inWidth * oWidth;
            }
            else
            {
                outHight = oHeight;
                outWidth = inWidth / inHeight * oHeight;
            }
        }


        private void SetBannerImageSize(float inWidth, float inHeight, out float outWidth, out float outHeight)
        {
            SetImageSize(inWidth, inHeight, out outWidth, out outHeight, 110, 70);
        }

        private void setSmallImageSize(float inWidth, float inHeight, out float outWidth, out float outHeight)
        {
            SetImageSize(inWidth, inHeight, out outWidth, out outHeight, 200, 150);
        }

        private void setBigImageSize(float inWidth, float inHeight, out float outWidth, out float outHeight)
        {
            SetImageSize(inWidth, inHeight, out outWidth, out outHeight, 400, 300);
        }
        #endregion

        #region 设置token

        private void GetTokenAndDomain(string type, out string bucket, out string domain)
        {
            switch (type)
            {
                case "video":
                    {
                        bucket = privateBucket;
                        domain = privateDomain;
                        break;
                    }
                default:
                    {
                        bucket = publicBucket;
                        domain = publicDomain;
                        break;
                    }
            }
        }

        #endregion

        #region 获取管道
        private string GetPipeline()
        {
            string result = null;

            if (pipelines != null)
            {
                Random random = new Random();
                result = pipelines[random.Next(0, pipelines.Length)];
            }

            return result;
        }
        #endregion

        #region 回调地址验证
        private bool IsQiniuCallback(JObject postData)
        {
            string encryptionKey = postData.Value<string>("WOBO");
            string decryptText = DESEncrypt.Decrypt(encryptionKey);

            DateTime nowTime = DateTime.Now;
            for (int i = 0; i < 9; i++)
            {
                string dec = GetEncryptText(nowTime.AddMinutes(-i * 56));
                if (decryptText.Equals(dec))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 视频查询
        private Video GetVideoByKey(string key)
        {
            var condtion = new Condtion()
            {
                FiledName = "VideoRosella",
                FiledValue = key,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            return _videoRepository.GetEntity(condtion);
        }
        #endregion

        #region 验证七牛回调
        public string GetEncryptText(DateTime dateTime)
        {
            return dateTime.AddDays(-22).AddMinutes(26).ToString("㈡ddCHH㈠MM");
        }
        #endregion

        public void VideoTranscode(int vid)
        {
            var video = _videoRepository.GetEntity(ConditionEqualId(vid));
            if (video == null)
            {
                return;
            }
            Pfop pfop = new Pfop();
            string videoFops = VideoFop(video.VideoPath);

            //处理m3u8
            string pipline = GetPipeline();
            PfopDo(pfop, privateBucket, video.VideoRosella, videoFops, pipline, video.VideoPath, video.CreateManageId, video.Id);

            video.VideoState = 0;
            _videoRepository.UpdateEntity(video);
        }

        private string VideoFop(string name)
        {
            //音视频切片： http://developer.qiniu.com/docs/v6/api/reference/fop/av/segtime.html
            string videoFops = "avthumb/m3u8/segtime/15/video_240k";

            string m3uencodedEntryURI = Qiniu.Util.Base64URLSafe.Encode(privateBucket + ":" + name);
            string m3usaveas = "|saveas/" + m3uencodedEntryURI;

            videoFops += m3usaveas;
            return videoFops;
        }

        private string VideoFop4mp4(string name)
        {
            string videoFops = "avthumb/mp4/video_240k";

            string m3uencodedEntryURI = Qiniu.Util.Base64URLSafe.Encode(privateBucket + ":" + name);
            string m3usaveas = "|saveas/" + m3uencodedEntryURI;

            videoFops += m3usaveas;
            return videoFops;
        }

        private string VideoImgFop(string name, int offsetTime, int width, int height)
        {

            string thumbnail = "vframe/jpg/offset/" + offsetTime + "/w/" + width + "/h/" + (int)height;
            string thumbencodedEntryURI = Qiniu.Util.Base64URLSafe.Encode(privateBucket + ":" + name);
            string thumbsaveas = "|saveas/" + thumbencodedEntryURI;
            string imgFops = thumbnail + thumbsaveas;
            return imgFops;
        }

        private async Task<string> PfopDo(Pfop pfop, string bucket, string key, string fops, string pipline, string saveAsKey, int userId, object temp = null)
        {
            return await Task.Run(() =>
            {
                string persistentId_thumb = "";
                try
                {
                    persistentId_thumb = pfop.Do(new EntryPath(bucket, key), new string[] { fops }, new Uri("http://" + callBackUrl + "/api/QiniuUpload/Notify"), pipline);
                }
                catch (Exception e)
                {
                    string mesg = string.Format("Pfop.Do Error  bucket:{0}  key:{1}   fops:{2}    pipline:{3}  saveAsKey:{4}", bucket, key,
                        fops, pipline, saveAsKey);
                    LogBuilder.Log4Net.Debug(mesg);
                    throw e;
                }
                QiniuFopLog bgImgfopLog = new QiniuFopLog
                {
                    OriginFile = key,
                    CreateTime = DateTime.Now,
                    SaveAsFile = saveAsKey,
                    PersistentId = persistentId_thumb,
                    Pipeline = pipline,
                    UserId = userId
                };
                _qiniuFopLogRepository.CreateEntity(bgImgfopLog);

                int vid = 0;
                if (temp != null && Int32.TryParse(temp.ToString(), out vid))
                {
                    var video = _videoRepository.GetEntity(ConditionEqualId(vid));
                    if (video != null)
                    {
                        video.PersistentId = persistentId_thumb;
                        _videoRepository.UpdateEntity(video);
                    }
                }

                return persistentId_thumb;

            });
        }
    }
}
