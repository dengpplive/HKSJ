


using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HKSJ.WBVV.Entity
{
    /// <summary>
    /// 管理员日志表
    /// </summary>
    [Serializable]
    public class ManageLog
    {
        #region 私有字段
        private Int32 _id ;
        private Int32? _logCode ;
        private DateTime _logTime ;
        private string _logContent ;
        private string _descc ;
        private Int32 _createManageId ;
        private DateTime _createTime ;
        private Int32? _updateManageId ;
        private DateTime? _updateTime ;
        #endregion
        #region 属性
        /// <summary>
        /// 编号
        /// </summary>
        /// <returns></returns>
        [Key]
        public Int32 Id
        {
            get { return _id;}
            set { _id=value;}
        }
       
        /// <summary>
        /// 日志码
        /// </summary>
        /// <returns></returns>
          public Int32? LogCode
        {
            get { return _logCode;}
            set { _logCode=value;}
        }
       
        /// <summary>
        /// 操作时间
        /// </summary>
        /// <returns></returns>
          public DateTime LogTime
        {
            get { return _logTime;}
            set { _logTime=value;}
        }
       
        /// <summary>
        /// 日志内容
        /// </summary>
        /// <returns></returns>
          public string LogContent
        {
            get { return _logContent;}
            set { _logContent=value;}
        }
       
        /// <summary>
        /// 日志描述
        /// </summary>
        /// <returns></returns>
          public string Descc
        {
            get { return _descc;}
            set { _descc=value;}
        }
       
        /// <summary>
        /// 创建管理员编号
        /// </summary>
        /// <returns></returns>
          public Int32 CreateManageId
        {
            get { return _createManageId;}
            set { _createManageId=value;}
        }
       
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
          public DateTime CreateTime
        {
            get { return _createTime;}
            set { _createTime=value;}
        }
       
        /// <summary>
        /// 修改管理员编号
        /// </summary>
        /// <returns></returns>
          public Int32? UpdateManageId
        {
            get { return _updateManageId;}
            set { _updateManageId=value;}
        }
       
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
          public DateTime? UpdateTime
        {
            get { return _updateTime;}
            set { _updateTime=value;}
        }
       
        #endregion
    }
}