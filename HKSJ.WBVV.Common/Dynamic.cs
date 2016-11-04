using HKSJ.WBVV.Common.Assert;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Common
{
    /// <summary>
    /// 动态对象，支持obj["PropName"]或者obj.PropName存取值
    /// </summary>
    [Serializable]
    public class Dynamic : DynamicObject
    {
        Dictionary<string, object> dic = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Dynamic()
        {
            dic = new Dictionary<string, object>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dic">数据</param>
        public Dynamic(Dictionary<string, object> dic)
        {
            this.dic = dic;
        }

        /// <summary>
        /// 属性个数
        /// </summary>
        public int Count
        {
            get
            {
                return dic.Count;
            }
        }

        /// <summary>
        /// 访问索引
        /// </summary>
        /// <param name="name">属性名</param>
        /// <returns>属性值</returns>
        public object this[string name]
        {
            get
            {
                AssertUtil.IsTrue(dic.ContainsKey(name), LanguageUtil.Translate("com_Dynamic_name_get").F(name));
                return dic[name];
            }
            set
            {
                dic[name] = value;
            }
        }

        /// <summary>
        /// 获取所有属性名
        /// </summary>
        /// <returns>所有属性名</returns>
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return dic.Keys;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override DynamicMetaObject GetMetaObject(System.Linq.Expressions.Expression parameter)
        {
            return base.GetMetaObject(parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string name = binder.Name;
            return dic.TryGetValue(name, out result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            dic[binder.Name] = value;
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="indexes"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetIndex(GetIndexBinder binder, Object[] indexes, out Object result)
        {
            string name = indexes[0] as string;
            return dic.TryGetValue(name, out result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="indexes"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            string name = indexes[0] as string;
            dic[name] = value;
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            if (binder.Type == typeof(Dictionary<string, object>))
            {
                result = this.dic;
                return true;
            }
            return base.TryConvert(binder, out result);
        }

        #region 重载
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static implicit operator Dynamic(Dictionary<string, object> dic)
        {
            if (dic == null)
                return null;
            else
                return new Dynamic(dic);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static implicit operator Dictionary<string, object>(Dynamic d)
        {
            if (d == null)
                return null;
            else
                return d.dic;
        }
        #endregion
    }
}
