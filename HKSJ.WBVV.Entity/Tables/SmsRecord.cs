


using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HKSJ.WBVV.Entity
{
    /// <summary>
    /// 短信验证记录表
    /// </summary>
    [Serializable]
    public class SmsRecord
    {
        #region 私有字段
        private Int32 _id ;
        private Int32 _userId ;
        private string _mobile ;
        private string _content ;
        private DateTime _sendTime ;
        private Int32 _state ;
        private string _ipAddress ;
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
        /// 用户Id
        /// </summary>
        /// <returns></returns>
          public Int32 UserId
        {
            get { return _userId;}
            set { _userId=value;}
        }
       
        /// <summary>
        /// 手机号码
        /// </summary>
        /// <returns></returns>
          public string Mobile
        {
            get { return _mobile;}
            set { _mobile=value;}
        }
       
        /// <summary>
        /// 短信内容
        /// </summary>
        /// <returns></returns>
          public string Content
        {
            get { return _content;}
            set { _content=value;}
        }
       
        /// <summary>
        /// 短信发送时间
        /// </summary>
        /// <returns></returns>
          public DateTime SendTime
        {
            get { return _sendTime;}
            set { _sendTime=value;}
        }
       
        /// <summary>
        /// 短信状态(0.发送失败 1.发送成功)
        /// </summary>
        /// <returns></returns>
          public Int32 State
        {
            get { return _state;}
            set { _state=value;}
        }
       
        /// <summary>
        /// IP地址
        /// </summary>
        /// <returns></returns>
          public string IpAddress
        {
            get { return _ipAddress;}
            set { _ipAddress=value;}
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