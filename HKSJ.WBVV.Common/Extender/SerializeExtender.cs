using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using HKSJ.WBVV.Common.Assert;
using Newtonsoft.Json;

namespace  HKSJ.WBVV.Common.Extender
{
    /// <summary>
    /// 序列化
    /// </summary>
    public static class SerializeExtender
    {
        #region 二进制
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">原始数据</param>
        /// <returns>二进制</returns>
        public static byte[] ToBinary(this object obj)
        {
            AssertUtil.IsNotNull(obj);
            BinaryFormatter f = new BinaryFormatter();
            using (MemoryStream inStream = new MemoryStream())
            {
                f.Serialize(inStream, obj);//对象序列化 
                inStream.Position = 0;
                return inStream.ToArray();
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="buffer">二进制</param>
        /// <returns>原始数据</returns>
        public static object FromBinary(this byte[] buffer)
        {
            AssertUtil.IsNotNull(buffer);
            BinaryFormatter f = new BinaryFormatter();
            using (MemoryStream inStream = new MemoryStream(buffer))
            {
                return f.Deserialize(inStream);
            }
        }
        #endregion

        #region JSON格式
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">原始对象</param>
        /// <returns>JSON格式字符串</returns>
        public static string ToJSON(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// JSON字符串转换为对象
        /// </summary>
        /// <param name="json">JSON格式字符串</param>
        /// <returns>对象</returns>
        public static T FromJSON<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        #endregion
    }
}
