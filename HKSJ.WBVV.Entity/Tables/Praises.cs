


using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HKSJ.WBVV.Entity
{
    /// <summary>
    /// 点赞表
    /// </summary>
    [Serializable]
    public class Praises
    {
        #region 私有字段
        private Int32 _id;
        private Int32 _themeId;
        private Int16 _themeTypeId;
        private Int32 _createUserId;
        private DateTime _createTime;
        private Int32? _updateUserId;
        private DateTime? _updateTime;
        private bool _state;
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
        /// 视频编号或者用户专辑编号
        /// </summary>
        /// <returns></returns>
        public Int32 ThemeId
        {
            get { return _themeId; }
            set { _themeId = value; }
        }

        /// <summary>
        /// 赞类型编号（1：用户(默认)2：视频）
        /// </summary>
        /// <returns></returns>
        public Int16 ThemeTypeId
        {
            get { return _themeTypeId; }
            set { _themeTypeId = value; }
        }

        /// <summary>
        /// 创建用户编号
        /// </summary>
        /// <returns></returns>
        public Int32 CreateUserId
        {
            get { return _createUserId; }
            set { _createUserId = value; }
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

        /// <summary>
        /// 修改用户编号
        /// </summary>
        /// <returns></returns>
        public Int32? UpdateUserId
        {
            get { return _updateUserId; }
            set { _updateUserId = value; }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        public DateTime? UpdateTime
        {
            get { return _updateTime; }
            set { _updateTime = value; }
        }

        /// <summary>
        /// 0.已取消赞  1.有效赞
        /// </summary>
        /// <returns></returns>
        public bool State
        {
            get { return _state; }
            set { _state = value; }
        }

        #endregion
    }
}