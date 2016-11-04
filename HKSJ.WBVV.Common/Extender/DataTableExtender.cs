using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Common.Extender
{
    /// <summary>
    /// 数据表拓展
    /// </summary>
    public static class DataTableExtender
    {
        /// <summary>
        /// 转动态对象列表
        /// </summary>
        /// <param name="tb">数据表</param>
        /// <param name="op">转换函数</param>
        /// <returns>动态对象列表</returns>
        public static List<T> ToList<T>(this DataTable tb, Func<DataRow, List<string>, T> op)
        {
            List<string> colList = new List<string>();
            foreach (DataColumn col in tb.Columns)
            {
                colList.Add(col.ColumnName);
            }
            List<T> list = new List<T>();
            foreach (DataRow row in tb.Rows)
            {
                var o = op(row, colList);
                list.Add(o);
            }
            return list;
        }
    }
}
