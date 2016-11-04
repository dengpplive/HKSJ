


using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HKSJ.WBVV.Entity
{
    /// <summary>
    /// 个人空间视频和专辑选择
    /// </summary>
    [Serializable]
    public class UserRoomChoose
    {
        #region 私有字段
        private Int32 _id;
        private Int64 _userid;
        private Int32 _moduleid;
        private Int32 _typeid;
        private Int32 _sortnum;
        private DateTime _createtime;
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
        /// 登录用户Id
        /// </summary>
        /// <returns></returns>
        public Int64 UserId
        {
            get { return _userid; }
            set { _userid = value; }
        }
        /// <summary>
        /// 视频和专辑id
        /// </summary>
        /// <returns></returns>
        public Int32 ModuleId
        {
            get { return _moduleid; }
            set { _moduleid = value; }
        }
        /// <summary>
        /// 视频或者专辑
        /// </summary>
        /// <returns></returns>
        public Int32 TypeId
        {
            get { return _typeid; }
            set { _typeid = value; }
        }
        /// <summary>
        /// 视频或者专辑排序
        /// </summary>
        /// <returns></returns>
        public Int32 SortNum
        {
            get { return _sortnum; }
            set { _sortnum = value; }
        }

        public DateTime CreateTime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
        #endregion
    }
}