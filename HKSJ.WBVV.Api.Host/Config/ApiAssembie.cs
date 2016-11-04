using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Common.Assert;

namespace HKSJ.WBVV.Api.Host.Config
{
    public static class ApiAssembie
    {
        public static void LoadApiAssembie(string apiAssembly)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + apiAssembly;
            AssemblyName assemblyName = AssemblyName.GetAssemblyName(path);
            if (!AppDomain.CurrentDomain.GetAssemblies().Any(assembly => AssemblyName.ReferenceMatchesDefinition(assembly.GetName(), assemblyName)))
            {
                AppDomain.CurrentDomain.Load(assemblyName);
            }
        }
        public static void LoadApiAssembie(ArrayList apiAssemblies)
        {
            AssertUtil.IsNotNull(apiAssemblies, "没有添加程序集配置");
            foreach (var apiAssembly in apiAssemblies)
            {
                LoadApiAssembie(apiAssembly.ToString());
            }
        }
    }
}
