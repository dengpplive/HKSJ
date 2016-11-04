using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Common.Extender.LinqExtender
{
    public enum ExpressionType
    {
        [Description("等于")]
        Equal = 1,
        [Description("不等于")]
        NotEqual = 2,
        [Description("大于")]
        GreaterThan = 3,
        [Description("大于等于")]
        GreaterThanOrEqual = 4,
        [Description("小于")]
        LessThan = 5,
        [Description("小于等于")]
        LessThanOrEqual = 6,
        [Description("以…开始")]
        StartWith,
        [Description("以…结束")]
        EndWidth,
        [Description("包含")]
        Contains
    }
    public enum ExpressionLogic
    {
        [Description("或者")]
        Or = 1,
        [Description("并且")]
        And = 2
    }

    public class Condtion
    {
        /// <summary>
        /// 条件字段名称
        /// </summary>
        public string FiledName { get; set; }
        /// <summary>
        /// 条件字段值
        /// </summary>
        public object FiledValue { get; set; }
        /// <summary>
        /// 条件中的比较类型
        /// </summary>
        public ExpressionType ExpressionType { get; set; }
        /// <summary>
        /// 条件中逻辑类型
        /// </summary>
        public ExpressionLogic ExpressionLogic { get; set; }

        public Condtion() { }

        public Condtion(string filedName, string filedValue, ExpressionType expressionType,
            ExpressionLogic expressionLogic)
        {
            this.FiledName = filedName;
            this.FiledValue = filedValue;
            this.ExpressionType = expressionType;
            this.ExpressionLogic = expressionLogic;
        }
    }
    /// <summary>
    ///linq查询扩展，实现动态构造查询字段和方式
    /// </summary>
    public static class QueryExtender
    {
        private static ConstantExpression GetConstantExpression(Type type, object value)
        {
            if (value == null)
            {
                return Expression.Constant(null, type);
            }
            if (type == typeof(string))
            {
                if (value.Equals("''"))
                {
                    return Expression.Constant(string.Empty);
                }
                return Expression.Constant(value);
            }
            if (type == typeof(Int16) || type == typeof(Int16?))
            {
                int val = 0;
                int.TryParse(value.ToString(), out val);
                return Expression.Constant(Convert.ToInt16(val), type);
            }
            if (type == typeof(Int32) || type == typeof(Int32?))
            {
                int val = 0;
                int.TryParse(value.ToString(), out val);
                return Expression.Constant(Convert.ToInt32(val), type);
            }
            if (type == typeof(Int64) || type == typeof(Int64?))
            {
                int val = 0;
                int.TryParse(value.ToString(), out val);
                return Expression.Constant(Convert.ToInt64(val), type);
            }
            if (type == typeof(double) || type == typeof(double?))
            {
                double val = 0;
                double.TryParse(value.ToString(), out val);
                return Expression.Constant(Convert.ToDouble(val), type);
            }
            if (type == typeof(decimal) || type == typeof(decimal?))
            {
                decimal val = 0;
                decimal.TryParse(value.ToString(), out val);
                return Expression.Constant(Convert.ToDecimal(val), type);
            }
            if (type == typeof(bool) || type == typeof(bool?))
            {
                bool val = false;
                bool.TryParse(value.ToString(), out val);
                return Expression.Constant(Convert.ToBoolean(val), type);
            }
            if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                DateTime val;
                DateTime.TryParse(value.ToString(), out val);
                return Expression.Constant(Convert.ToDateTime(val), type);
            }
            if (type == typeof(Guid) || type == typeof(Guid?))
            {
                return Expression.Constant(new Guid(value.ToString()), type);
            }
            throw new AssertException(LanguageUtil.Translate("com_LinqExtender_QueryExtender_DataTypeFormat"));
        }
        private static Expression GetExpression(Type type, ParameterExpression parameter, Condtion condtion)
        {
            PropertyInfo info = type.GetProperty(condtion.FiledName);
            Type propertyType = info.PropertyType;
            Expression left = Expression.Property(parameter, info);
            Expression right = GetConstantExpression(propertyType, condtion.FiledValue);
            MethodInfo methodInfo = null;
            Expression where = null;
            switch (condtion.ExpressionType)
            {
                case ExpressionType.Equal:
                    where = Expression.Equal(left, right);
                    break;
                case ExpressionType.NotEqual:
                    where = Expression.NotEqual(left, right);
                    break;
                case ExpressionType.GreaterThan:
                    where = Expression.GreaterThan(left, right);
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    where = Expression.GreaterThanOrEqual(left, right);
                    break;
                case ExpressionType.LessThan:
                    where = Expression.LessThan(left, right);
                    break;
                case ExpressionType.LessThanOrEqual:
                    where = Expression.LessThanOrEqual(left, right);
                    break;
                case ExpressionType.StartWith:
                    methodInfo = propertyType.GetMethod("StartsWith", new Type[] { propertyType });
                    where = Expression.Call(left, methodInfo, right);
                    break;
                case ExpressionType.EndWidth:
                    methodInfo = propertyType.GetMethod("EndsWith", new Type[] { propertyType });
                    where = Expression.Call(left, methodInfo, right);
                    break;
                case ExpressionType.Contains:
                    methodInfo = propertyType.GetMethod("Contains", new Type[] { propertyType });
                    where = Expression.Call(left, methodInfo, right);
                    break;
            }
            return where;
        }

        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.Or(expr1.Body, invokedExpr), expr1.Parameters);
        }
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.And(expr1.Body, invokedExpr), expr1.Parameters);
        }
        public static IQueryable<T> Query<T>(this IQueryable<T> source, IList<Condtion> condtions)
        {
            AssertUtil.IsNotEmptyCollection(condtions, LanguageUtil.Translate("com_LinqExtender_QueryExtender_query_condition"));
            Type type = typeof(T);
            ParameterExpression parameter = Expression.Parameter(type, "item");
            Expression<Func<T, bool>> lambda = null;
            foreach (var condtion in condtions)
            {
                Expression where = GetExpression(type, parameter, condtion);
                Expression<Func<T, bool>> current = Expression.Lambda<Func<T, bool>>(where, parameter);
                if (lambda == null)
                {
                    lambda = current;
                }
                else
                {
                    switch (condtion.ExpressionLogic)
                    {
                        case ExpressionLogic.Or:
                            lambda = lambda.Or(current);
                            break;
                        case ExpressionLogic.And:
                            lambda = lambda.And(current);
                            break;
                    }
                }
            }
            AssertUtil.IsNotNull(lambda, LanguageUtil.Translate("com_LinqExtender_QueryExtender_check_lambda_null"));
            return source.Where(lambda).AsQueryable();
        }
        public static IQueryable<T> Query<T>(this IQueryable<T> source, Condtion condtion)
        {
            AssertUtil.IsNotNull(condtion, LanguageUtil.Translate("com_LinqExtender_QueryExtender_query_condition"));
            Type type = typeof(T);
            ParameterExpression parameter = Expression.Parameter(type, "item");
            Expression where = GetExpression(type, parameter, condtion);
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(where, parameter);
            AssertUtil.IsNotNull(lambda, LanguageUtil.Translate("com_LinqExtender_QueryExtender_check_lambda_null"));
            return source.Where(lambda).AsQueryable();
        }
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            return source.Where(element => seenKeys.Add(keySelector(element)));
        }
    }

}

