


using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HKSJ.WBVV.Entity
{
    /// <summary>
    /// 用户专辑子表
    /// </summary>
    [Serializable]
    public class UserSpecialSon
    {
        #region 私有字段
        private Int32 _id;
        private Int32 _mySpecialId;
        private Int32 _videoId;
        private DateTime _createTime;
        private Int32 _sortNum = 0;
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
        /// 专辑编号
        /// </summary>
        /// <returns></returns>
        public Int32 MySpecialId
        {
            get { return _mySpecialId; }
            set { _mySpecialId = value; }
        }

        /// <summary>
        /// 视频编号
        /// </summary>
        /// <returns></returns>
        public Int32 VideoId
        {
            get { return _videoId; }
            set { _videoId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Int32 SortNum
        {
            get { return _sortNum; }
            set { _sortNum = value; }
        }

        #endregion
    }
}