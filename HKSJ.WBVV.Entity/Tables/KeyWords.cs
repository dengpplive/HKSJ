


using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HKSJ.WBVV.Entity
{
    /// <summary>
    /// 搜索关键词表
    /// </summary>
    [Serializable]
    public class KeyWords
    {
        #region 私有字段
        private Int32 _iD ;
        private string _keyword ;
        private Int32 _searchNum ;
        private DateTime _searchTime ;
        #endregion
        #region 属性
        /// <summary>
        /// 编号ID
        /// </summary>
        /// <returns></returns>
        [Key]
        public Int32 ID
        {
            get { return _iD;}
            set { _iD=value;}
        }
       
        /// <summary>
        /// 搜索关键字
        /// </summary>
        /// <returns></returns>
          public string Keyword
        {
            get { return _keyword;}
            set { _keyword=value;}
        }
       
        /// <summary>
        /// 搜索次数
        /// </summary>
        /// <returns></returns>
          public Int32 SearchNum
        {
            get { return _searchNum;}
            set { _searchNum=value;}
        }
       
        /// <summary>
        /// 搜索时间
        /// </summary>
        /// <returns></returns>
          public DateTime SearchTime
        {
            get { return _searchTime;}
            set { _searchTime=value;}
        }

        public string SearchTimeStr
        {
            get
            {
                return SearchTime.ToShortDateString() +" "+ SearchTime.ToLongTimeString();
            }

        }

        #endregion
    }
}