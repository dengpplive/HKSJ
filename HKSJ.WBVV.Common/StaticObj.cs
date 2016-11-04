using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;

namespace HKSJ.WBVV.Common
{
    public class StaticObj
    {
        //IoC容器
        public static object Container { get; set; }

        public Dictionary<string, object> NamedParameter { get; set; }

        public static IContainer ObjContainer { get { return Container as IContainer; } }

        private static IEnumerable<NamedParameter> ConvertNamedParameter(Dictionary<string, object> para)
        {
            return para.Select(p => new NamedParameter(p.Key, p.Value)).ToList();
        }

        public static T ObjResolve<T>(Dictionary<string, object> para = null)
        {
            
            if (para == null)
            {
                return ObjContainer.Resolve<T>();
            }
            var parameters = ConvertNamedParameter(para);
            return ObjContainer.Resolve<T>(parameters);
        }
    }
}
