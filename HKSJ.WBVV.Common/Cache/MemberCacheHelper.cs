using HKSJ.WBVV.Common.Assert;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Common.Cache
{
    public static class MemberCacheHelper
    {
        private static readonly MemcachedClient Client = new MemcachedClient();

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值json格式</param>
        public static bool Set(string key, string value)
        {
            try
            {
                int mb = 1024 * 1024;
                //json字符串的大小小于1M的时候直接写入缓存
                if (System.Text.Encoding.Default.GetByteCount(value) < mb)
                {
                    return Client.Store(Enyim.Caching.Memcached.StoreMode.Set, key, value);
                }
                else
                {
                    //缓存大于1M，分块存储，返回结果,
                    int chunkSize = mb / 2;
                    bool flag = true;
                    //存缓存块对应的缓存键值
                    var subKeys = new List<string>();
                    for (int i = 0; i * chunkSize < value.Length; i++)
                    {
                        //分割字符串存到缓存的键值
                        string subKey = key + "_" + i;
                        //指定长度截取值
                        string subValue = (i + 1) * chunkSize < value.Length ? value.Substring(i * chunkSize, chunkSize) : value.Substring(i * chunkSize);

                        //分块写入缓存
                        if (Client.Store(Enyim.Caching.Memcached.StoreMode.Set, subKey, subValue))
                        {
                            subKeys.Add(subKey);
                        }
                        else
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        // 保存失败，将成功的子键缓存也清空
                        foreach (string subkey in subKeys)
                        {
                            Client.Remove(subkey);
                        }
                    }
                    else
                    {
                        //子键都保存成功，保存主键
                        flag = Client.Store(Enyim.Caching.Memcached.StoreMode.Set, key, subKeys);
                    }
                    return flag;
                }
            }
            catch (Exception ex)
            {
                throw new MemberExecption(ex.Message);
            }


        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>返回缓存值Json格式</returns>
        public static string Get(string key)
        {
            try
            {
                object result = Client.Get(key);
                if (result == null)
                {
                    return "";
                }
                if (result is IList<string>)
                {
                    var subKeys = (IList<string>)result;
                    var sb = new StringBuilder();
                    //遍历子键
                    foreach (string subKey in subKeys)
                    {
                        //获取子健分割的值
                        var subValue = Client.Get(subKey);
                        //拼接结果
                        if (subValue != null)
                        {
                            sb.Append(subValue.ToString());
                        }
                    }
                    return sb.ToString();
                }
                return result.ToString();
            }
            catch (Exception ex)
            {
                throw new MemberExecption(ex.Message);
            }


        }
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <returns></returns>
        public static void Remove(string key)
        {
            try
            {
                object result = Client.Get(key);
                if (result == null) return;
                if (result is IList<string>)
                {
                    var subKeys = (IList<string>)result;
                    var sb = new StringBuilder();
                    //遍历子键
                    foreach (string subkey in subKeys)
                    {
                        Client.Remove(subkey);
                    }
                }
                else
                {
                    Client.Remove(key);
                }
            }
            catch (Exception ex)
            {
                throw new MemberExecption(ex.Message);
            }

        }
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static void Remove(params  string[] keys)
        {
            if (keys == null || keys.Length <= 0)
            {
                Client.FlushAll();
            }
            else
            {
                foreach (var key in keys)
                {
                    Remove(key);
                }
            }
        }
        /// <summary>
        /// 清空缓存
        /// </summary>
        public static void Clear()
        {
            Remove();
        }
    }
}
