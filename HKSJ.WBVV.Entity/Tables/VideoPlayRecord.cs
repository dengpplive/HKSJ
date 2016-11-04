


using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HKSJ.WBVV.Entity
{
    /// <summary>
    /// 视频播放记录表
    /// </summary>
    [Serializable]
    public class VideoPlayRecord
    {
        #region 私有字段
        private Int32 _id;
        private string _ipAddress;
        private Int32 _userId;
        private Int64 _videoId;
        private DateTime _createTime;
        #endregion
        #region 属性
        /// <summary>
        /// 编号
        /// </summary>
        /// <returns></returns>
        [Key]
        public Int32 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 观看IP地址
        /// </summary>
        /// <returns></returns>
        public string IpAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        /// <summary>
        /// 用户编号
        /// </summary>
        /// <returns></returns>
        public Int32 UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        /// <summary>
        /// 视频编号
        /// </summary>
        /// <returns></returns>
        public Int64 VideoId
        {
            get { return _videoId; }
            set { _videoId = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        public DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }

        #endregion
    }
}