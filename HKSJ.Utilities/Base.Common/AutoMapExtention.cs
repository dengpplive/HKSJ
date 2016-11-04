using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace HKSJ.Utilities
{
    public static class AutoMapExtention
    {

        /// <summary>
        /// 类型映射
        /// </summary>
        /// <typeparam name="F">From</typeparam>
        /// <typeparam name="T">To</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T MapTo<F, T>(this F source)
            where T : class
            where F : class
        {
            if (source == null) { return default(T); }
            Mapper.CreateMap<F, T>();
            return Mapper.Map<T>(source);
        }


        /// <summary>
        /// 集合映射
        /// </summary>
        /// <typeparam name="F">From</typeparam>
        /// <typeparam name="T">To</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<T> MapTo<F, T>(this IEnumerable<F> source, bool ignoreNull = false)
            where T : class
            where F : class
        {
            if (source == null) { return new List<T> { }; }
            Type ty = source.GetType();
            var m = Mapper.CreateMap<F, T>();
            if (ignoreNull)
            {
                m.ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));
            }
            return Mapper.Map<List<T>>(source);
        }

        /// <summary>
        /// 集合映射   设置默认值
        /// </summary>
        /// <typeparam name="F"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="defaultValueSets">默认值</param>
        /// <returns></returns>
        public static List<T> MapTo<F, T>(this IEnumerable<F> source, Func<Dictionary<Expression<Func<T, object>>, object>> defaultValueSets)
            where T : class
            where F : class
        {
            if (source == null) { return new List<T> { }; }
            Type ty = source.GetType();
            var m = Mapper.CreateMap<F, T>();
            if (defaultValueSets != null)
            {
                var arr = defaultValueSets();
                foreach (var item in arr)
                {
                    m.ForMember(item.Key, e => e.UseValue(item.Value));
                }
            }
            return Mapper.Map<List<T>>(source);
        }
    }
}
