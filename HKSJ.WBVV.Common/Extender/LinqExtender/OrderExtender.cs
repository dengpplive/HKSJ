using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Common.Extender.LinqExtender
{
    /// <summary>
    /// 排序模型
    /// </summary>
    public class OrderCondtion
    {
        /// <summary>
        /// 排序字段名称
        /// </summary>
        public string FiledName { get; set; }
        /// <summary>
        /// 是否降序
        /// </summary>
        public bool IsDesc { get; set; }
    }

    /// <summary>
    /// linq排序扩展，实现动态构造排序字段和方式
    /// </summary>
    public static class OrderExtender
    {
        /// <summary>
        /// Linq动态排序（单字段）
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="source">要排序的数据源</param>
        /// <param name="orderCondtion">排序模型</param>
        /// <returns>IOrderedQueryable</returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, OrderCondtion orderCondtion)
        {
            AssertUtil.IsNotNull(orderCondtion, LanguageUtil.Translate("com_LinqExtender_OrderExtender_check_order_null"));
            string orderType = orderCondtion.IsDesc ? "OrderByDescending" : "OrderBy";
            return ApplyOrder<T>(source, orderCondtion.FiledName, orderType);
        }

        /// <summary>
        /// Linq动态排序再排序
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="source">要排序的数据源</param>
        /// <param name="orderCondtion">排序模型</param>
        /// <returns>IOrderedQueryable</returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, OrderCondtion orderCondtion)
        {
            AssertUtil.IsNotNull(orderCondtion, LanguageUtil.Translate("com_LinqExtender_OrderExtender_check_order_null"));
            string orderType = orderCondtion.IsDesc ? "ThenByDescending" : "ThenBy";
            return ApplyOrder<T>(source, orderCondtion.FiledName, orderType);
        }


        /// <summary>
        /// Linq动态排序(多字段)
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="source">要排序的数据源</param>
        /// <param name="orderCondtions">排序集合</param>
        /// <returns>IOrderedQueryable</returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, IList<OrderCondtion> orderCondtions)
        {
            AssertUtil.IsNotEmptyCollection(orderCondtions, LanguageUtil.Translate("com_LinqExtender_OrderExtender_check_order"));
            IOrderedQueryable<T> result = null;
            int count = 0;
            foreach (var orderCondtion in orderCondtions)
            {
                count++;
                //多个排序条件
                result = count == 1 ? OrderBy<T>(source, orderCondtion) : ThenBy<T>(result, orderCondtion);
            }
            return result;
        }

        static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {

            AssertUtil.IsNotNull(source, LanguageUtil.Translate("com_LinqExtender_OrderExtender_check_source"));
            AssertUtil.NotNullOrWhiteSpace(property, LanguageUtil.Translate("com_LinqExtender_OrderExtender_check_order_name"));
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "item");
            PropertyInfo pi = type.GetProperty(property, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
            AssertUtil.IsNotNull(pi, LanguageUtil.Translate("com_LinqExtender_OrderExtender_check_null").F(property));
            Expression expr = Expression.Property(arg, pi);
            type = pi.PropertyType;
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);
            object result = typeof(Queryable).GetMethods().Single(
                                                                    item => item.Name == methodName//调用的方法名称
                                                                    && item.IsGenericMethodDefinition//参数是否是泛型
                                                                    && item.GetGenericArguments().Length == 2//泛型类型个数<T,T>
                                                                    && item.GetParameters().Length == 2//参数个数
                //构造MethodInfo对象
                                                                    ).MakeGenericMethod(
                                                                            typeof(T), //第一个泛型类型
                                                                             type//第二个泛型类型
                                                                 ).Invoke(
                                                                 null,
                                                                 new object[] { source, lambda }
                                                                 );
            return (IOrderedQueryable<T>)result;
        }
    }
}
