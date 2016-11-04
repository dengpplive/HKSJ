using HKSJ.WBVV.Common.Resource;
using System;

namespace HKSJ.WBVV.Entity.ViewModel.Manage
{
    [Serializable]
    public class BannerVideoView
    {
        #region Fields

        /// <summary>
        /// Local veriable used to combine Banner Image Path the a video.
        /// </summary>
        string _bannerImgPath;
        string _bannerSmallImgPath;

        #endregion
        public int Id { get; set; }
        public long VideoId { get; set; }
        public string VideoName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string BannerImagePath
        {
            get
            {
                return UrlHelper.QiniuPublicCombine(_bannerImgPath);
            }

            set
            {
                _bannerImgPath = value;
            }
        }
        public string BannerSmallImagePath
        {
            get
            {
                return UrlHelper.QiniuPublicCombine(_bannerSmallImgPath);
            }

            set
            {
                _bannerSmallImgPath = value;
            }
        }
        public int SortNum { get; set; }
        public int CreateManageId { get; set; }
        public DateTime CreateTime { get; set; }
        public int UpdateManageId { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool State { get; set; }
    }
}
