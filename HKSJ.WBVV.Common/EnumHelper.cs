using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Common
{
    public class EnumHelper
    {
        /// <summary>
        /// 枚举Description缓存器
        /// </summary>
        private static Dictionary<Enum, string> enumCache = new Dictionary<Enum, string>();

        private EnumHelper()
        {
        }

        public static string GetEnumDescription(Enum value)
        {
            if (enumCache.ContainsKey(value))
            {
                return enumCache[value];
            }

            FieldInfo fi = value.GetType().GetField(value.ToString());
            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes
                                             (typeof(DescriptionAttribute), false);

            string desc = (attributes.Length > 0) ? attributes[0].Description : value.ToString();
            enumCache.Add(value, desc);

            return desc;
        }
    }
}
