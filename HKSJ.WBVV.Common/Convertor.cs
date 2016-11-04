using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using HKSJ.WBVV.Common.Extender;

namespace HKSJ.WBVV.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class Convertor
    {
        readonly static Type GuidType = typeof(Guid);
        readonly static Type NullableGuidType = typeof(Guid?);
        readonly static Type StringType = typeof(string);
        static Convertor()
        {
        }

        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <typeparam name="T">基础类型,String,Guid</typeparam>
        /// <param name="value">待转换的值</param>
        /// <returns>T类型的值</returns>
        public static T To<T>(object value)
        {
            object ret = default(T);
            if (value.IsNotNull())
            {
                var type = value.GetType();
                if (GuidType == typeof(T) && type == StringType)
                    ret = new Guid(value as string);
                else if (NullableGuidType == typeof(T) && type == StringType && value.ToString() != string.Empty)
                    ret = new Guid(value as string);
                else if (StringType == typeof(T) && type == GuidType)
                    ret = value.ToString();
                else
                {
                    var t = typeof(T);
                    bool isGeneric = false;//是否为泛型
                    if (t.IsGenericType)
                    {
                        isGeneric = true;
                        t = t.GetGenericArguments()[0];
                    }
                    if (isGeneric && value.ToString() == string.Empty)
                        ret = default(T);
                    else
                        ret = (T)Convert.ChangeType(value, t);
                }
            }
            return (T)ret;
        }

    }
}
