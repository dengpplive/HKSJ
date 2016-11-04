using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Common.Extender
{
    /// <summary>
    /// 
    /// </summary>
    public static class ObjectExtender
    {
        #region 是否为空
        /// <summary>
        /// 判断对象是否为空
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>True为空，False非空</returns>
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }
        /// <summary>
        /// 判断对象是否非空
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>True非空，False为空</returns>
        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }
        /// <summary>
        /// 判断对象是否为空
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>True为空，False非空</returns>
        public static bool IsDbNull(this object obj)
        {
            return obj == DBNull.Value;
        }
        /// <summary>
        /// 判断对象是否非空
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>True非空，False为空</returns>
        public static bool IsNotDbNull(this object obj)
        {
            return obj != DBNull.Value;
        }
        #endregion

        #region 类型转换
        /// <summary>
        /// 基础类型转换类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static T To<T>(this object value)
        {
            return Convertor.To<T>(value);
        }
        /// <summary>
        /// 引用类型转换
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static T As<T>(this object value) where T : class
        {
            return value as T;
        }

        /// <summary>
        /// 类型强制转换
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static T Force<T>(this object value)
        {
            return (T)value;
        }
        #endregion

        #region DBValue转换
        /// <summary>
        /// 支持DBValue
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>值</returns>
        public static object ToDBValue(this object o)
        {
            if (o == null)
                return DBNull.Value;
            return o;
        }

        /// <summary>
        /// 支持DBValue
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>值</returns>
        public static T FromDBValue<T>(this object o)
        {
            if (o == null || Convert.IsDBNull(o))
                return default(T);
            return o.To<T>();
        }
        #endregion

        #region 支持动态
        /// <summary>
        /// 动态化
        /// </summary>
        /// <param name="value">对象</param>
        /// <returns>动态对象</returns>
        public static dynamic Dynamic(this object value)
        {
            return (dynamic)value;
        }

        /// <summary>
        /// 获取动态值
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="obj">对象</param>
        /// <param name="prop">属性</param>
        /// <returns>返回属性值</returns>
        public static T Get<T>(this object obj, string prop)
        {
            AssertUtil.NotNullOrWhiteSpace(prop, LanguageUtil.Translate("com_ObjectExtender_check_prop_null"));
            if (obj is Dynamic)
                return ((dynamic)obj)[prop];
            else if (obj is Dictionary<string, object>)
                return obj.As<Dictionary<string, object>>()[prop].To<T>();
            else
                return ((dynamic)obj.ToDynamic())[prop];
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="prop">属性</param>
        /// <param name="format">格式化字符</param>
        /// <param name="limit">是否限制长度</param>
        /// <param name="limitCount">限制长度</param>
        /// <returns>字符串</returns>
        public static string Get(this object obj, string prop, string format = null, bool limit = false, int limitCount = 20)
        {
            AssertUtil.NotNullOrWhiteSpace(prop, LanguageUtil.Translate("com_ObjectExtender_check_prop_null"));
            object value = null;
            if (obj is Dynamic)
                value = ((dynamic)obj)[prop];
            else if (obj is Dictionary<string, object>)
                value = obj.As<Dictionary<string, object>>()[prop];
            else
                value = ((dynamic)obj.ToDynamic())[prop];

            if (value == null)
                return null;

            string str = null;
            if (format == null)
                str = value.To<string>();
            else
                str = format.F(value);
            if (limit)
            {
                if (str.Length > limitCount)
                    str = str.Substring(0, limitCount) + "...";
            }
            return str;
        }

        /// <summary>
        /// 把对象转换为哈希表
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>哈希表</returns>
        public static Dictionary<string, object> ToDic(this object o)
        {
            AssertUtil.IsNotNull(o, LanguageUtil.Translate("com_ObjectExtender_check_change_object_null"));
            Dictionary<string, object> dic = new Dictionary<string, object>();
            PropertyInfo[] ps = o.GetType().GetProperties();
            foreach (var p in ps)
            {
                dic.Add(p.Name, p.GetValue(o, null));
            }
            return dic;
        }
        /// <summary>
        /// 把对象转为动态对象
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>动态对象</returns>
        public static Dynamic ToDynamic(this object o)
        {
            AssertUtil.IsNotNull(o, LanguageUtil.Translate("com_ObjectExtender_check_change_object_null"));
            return (Dynamic)o.ToDic();
        }
        /// <summary>
        /// 哈希转动态对象
        /// </summary>
        /// <param name="dic">哈希</param>
        /// <returns>动态对象</returns>
        public static Dynamic ToDynamic(this Dictionary<string, object> dic)
        {
            AssertUtil.IsNotNull(dic, LanguageUtil.Translate("com_ObjectExtender_check_change_object_null"));
            return new Dynamic(dic);
        }
        /// <summary>
        /// 动态调用对象的方法
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="methodName">方法名</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>方法返回值</returns>
        public static object Call(this object target, string methodName, params object[] parameters)
        {
            AssertUtil.IsNotNull(target, LanguageUtil.Translate("com_ObjectExtender_check_aims_object_null"));
            List<Type> types = new List<Type>();
            foreach (var parameter in parameters)
            {
                types.Add(parameter.GetType());
            }
            Type t = target.GetType();
            MethodInfo m = t.GetMethod(methodName, types.ToArray());
            AssertUtil.IsNotNull(m, LanguageUtil.Translate("com_ObjectExtender_check_method_null"));
            return m.Invoke(target, parameters);
        }
        #endregion

    }
}
