using System;
using System.Collections;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Api.OpenPlatForm.Utils
{
    public static class DynamicHelper
    {
        public static dynamic FromJson(String jsonString)
        {
            var jsSerializer = new JavaScriptSerializer();
            jsSerializer.RegisterConverters(new[] { new DynamicJsonConverter() });
            dynamic dynamicObject = jsSerializer.Deserialize<object>(jsonString);
            return dynamicObject;
        }
    }


    /// <summary>
    /// 动态Json对象转换
    /// </summary>
    internal class DynamicJsonConverter : JavaScriptConverter
    {
        #region //重写方法
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            return type == typeof(object) ? new DynamicJsonObject(dictionary) : null;
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
            /*var result = new Dictionary<string, object>();
            var dictionary = obj as IDictionary<string, object>;
            foreach (var item in dictionary) result.Add(item.Key, item.Value);
            return result;*/
        }

        public override IEnumerable<Type> SupportedTypes
        {
            //get { return new ReadOnlyCollection<Type>(new List<Type>(new Type[] { typeof(object) })); }

            //new test
            get { return new ReadOnlyCollection<Type>(new List<Type>(new Type[] { typeof(object), typeof(DynamicJsonObject), typeof(System.Dynamic.ExpandoObject) })); }

        }
        #endregion
    }

    /// <summary>
    /// 动态Json对象
    /// </summary>
    [Serializable]
    internal class DynamicJsonObject : DynamicObject, IEnumerable
    {
        #region //私有字段
        private readonly IDictionary<string, object> _mDictionary;
        #endregion

        #region //构造方法
        public DynamicJsonObject()
        {
            this._mDictionary = new Dictionary<string, object>();
        }

        public DynamicJsonObject(IDictionary<string, object> dictionary = null)
        {
            this._mDictionary = dictionary ?? new Dictionary<string, object>();
        }

        #endregion

        #region //重写方法
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var name = binder.Name;
            if (!_mDictionary.TryGetValue(name, out result))
            {
                //以is开头的名称都判断是布尔类型
                if (name.ToLower().StartsWith("is"))
                {
                    result = false;
                }
                else
                {
                    result = string.Empty;
                }
                return true;
            }
            //处理是否布尔类型
            var stringResult = result as string;
            if (!string.IsNullOrWhiteSpace(stringResult))
            {
                if (stringResult.ToLower() == "true" || stringResult.ToLower() == "false")
                {
                    result = Convert.ToBoolean(stringResult.ToLower());
                    return true;
                }
            }
            var dictionary = result as IDictionary<string, object>;
            if (dictionary != null)
            {
                result = new DynamicJsonObject(dictionary);
                return true;
            }
            var arrayList = result as ArrayList;
            if (arrayList == null || arrayList.Count <= 0)
            {
                return true;
            }
            if (arrayList[0] is IDictionary<string, object>)
            {
                result =
                    new List<object>(arrayList.Cast<IDictionary<string, object>>().Select(x => new DynamicJsonObject(x)));
            }
            else
            {
                result = new List<object>(arrayList.Cast<object>());
            }
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this._mDictionary[binder.Name] = value;
            return true;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            ToString(ref sb);
            return sb.ToString();
        }

        private void ToString(ref StringBuilder sb, ArrayList list)
        {
            var needComma = false;
            foreach (var value in (ArrayList)list)
            {
                if (needComma)
                {
                    sb.Append(",");
                }

                needComma = true;
                var map = value as IDictionary<string, object>;
                if (map != null)
                {
                    ToString(ref sb, map);
                }
                else if (value is string)
                {
                    sb.AppendFormat("\"{0}\"", value);
                }
                else
                {
                    sb.AppendFormat("{0}", value);
                }

            }
        }

        private void ToString(ref StringBuilder sb, IDictionary<string, object> map)
        {
            new DynamicJsonObject((IDictionary<string, object>)map).ToString(ref sb);
        }

        private void ToString(ref StringBuilder sb)
        {
            sb.Append("{");

            var needComma = false;
            foreach (var pair in _mDictionary)
            {
                if (needComma)
                {
                    sb.Append(",");
                }
                needComma = true;
                var value = pair.Value;
                var name = pair.Key;
                if (value is string || value == null)
                {
                    sb.AppendFormat("\"{0}\":\"{1}\"", name, value ?? "");
                }
                else if (value is IDictionary<string, object>)
                {
                    sb.AppendFormat("\"{0}\":", name);
                    ToString(ref sb, (IDictionary<string, object>)value);
                }
                else if (value is ArrayList)
                {
                    sb.AppendFormat("\"{0}\":[", name);
                    ToString(ref sb, (ArrayList)value);
                    sb.Append("]");
                }
                else
                {
                    sb.AppendFormat("\"{0}\":{1}", name, value);
                }
            }
            sb.Append("}");
        }

        #endregion

        #region Enumeration
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return _mDictionary.GetEnumerator();
        }

        #endregion
    }
}
