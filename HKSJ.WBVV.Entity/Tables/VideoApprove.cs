


using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HKSJ.WBVV.Entity
{
    /// <summary>
    /// 视频审核信息表
    /// </summary>
    [Serializable]
    public class VideoApprove
    {
        #region 私有字段
        private Int32 _id ;
        private int _approveId ;
        private string _approveContent ;
        private DateTime? _approveTime ;
        private string _approveRemark ;
        private long _videoId ;
        private DateTime _createTime ;
        private int _createAdminId ;
        private DateTime? _updateTime ;
        private int _updateAdminId ;
        private bool _status ;
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
        /// 审核Id
        /// </summary>
        /// <returns></returns>
          public int ApproveId
        {
            get { return _approveId;}
            set { _approveId=value;}
        }
       
        /// <summary>
        /// 审核内容
        /// </summary>
        /// <returns></returns>
          public string ApproveContent
        {
            get { return _approveContent;}
            set { _approveContent=value;}
        }
       
        /// <summary>
        /// 审核时间
        /// </summary>
        /// <returns></returns>
          public DateTime? ApproveTime
        {
            get { return _approveTime;}
            set { _approveTime=value;}
        }
       
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
          public string ApproveRemark
        {
            get { return _approveRemark;}
            set { _approveRemark=value;}
        }
       
        /// <summary>
        /// 视频业务Id
        /// </summary>
        /// <returns></returns>
          public long VideoId
        {
            get { return _videoId;}
            set { _videoId=value;}
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
        /// 管理员业务Id
        /// </summary>
        /// <returns></returns>
          public int CreateAdminId
        {
            get { return _createAdminId;}
            set { _createAdminId=value;}
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
       
        /// <summary>
        /// 管理员业务Id
        /// </summary>
        /// <returns></returns>
          public int UpdateAdminId
        {
            get { return _updateAdminId;}
            set { _updateAdminId=value;}
        }
       
        /// <summary>
        /// 审核状态(0.通过 1.不通过)
        /// </summary>
        /// <returns></returns>
          public bool Status
        {
            get { return _status;}
            set { _status=value;}
        }
       
        #endregion
    }
}