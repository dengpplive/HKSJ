using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity
{
    /// <summary>
    /// 七年预处理日志表
    /// </summary>
    [Serializable]
    public class QiniuFopLog
    {

        #region 私有字段
        private int _id;
        private string _originFile;
        private string _saveAsFile;
        private string _persistentId;
        private string _pipeline;
        private Int32? _userId;
        private DateTime _createTime;
        #endregion
        #region 属性
        /// <summary>
        /// 编号
        /// </summary>
        /// <returns></returns>
        [Key]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 原文件名
        /// </summary>
        public string OriginFile {
            get { return _originFile; }
            set { _originFile = value; }
        }

        /// <summary>
        /// 处理后文件名
        /// </summary>
        public string SaveAsFile
        {
            get { return _saveAsFile; }
            set { _saveAsFile = value; }
        }

        /// <summary>
        /// 预处理Id
        /// </summary>
        public string PersistentId
        {
            get { return _persistentId; }
            set { _persistentId = value; }
        }

        /// <summary>
        /// 管道名
        /// </summary>
        public string Pipeline
        {
            get { return _pipeline; }
            set { _pipeline = value; }
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public Int32? UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        /// <summary>
        /// 创建Id
        /// </summary>
        public DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }
        #endregion
    }
}
