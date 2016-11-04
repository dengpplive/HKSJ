


using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HKSJ.WBVV.Entity
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Language
    {
        #region 私有字段
        private string _key;
        private string _lang;
        private string _text;
        #endregion
        #region 属性
        /// <summary>
        /// 键
        /// </summary>
        /// <returns></returns>
        [Key]
        [Column(Order = 1)] 
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        /// <summary>
        /// 语言
        /// </summary>
        /// <returns></returns>
        [Key]
        [Column(Order = 2)] 
        public string Lang
        {
            get { return _lang; }
            set { _lang = value; }
        }

        /// <summary>
        /// 文本
        /// </summary>
        /// <returns></returns>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        #endregion
    }
}