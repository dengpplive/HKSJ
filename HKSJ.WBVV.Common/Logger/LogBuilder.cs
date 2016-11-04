
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HKSJ.WBVV.Common.Logger
{
    /// <summary>
    /// 日志操作构造器
    /// </summary>
    public static class LogBuilder
    {
        /// <summary>
        /// 日志操作对象
        /// </summary>
        public static ILogger Log4Net { private set; get; }

        /// <summary>
        /// 日志初始化方法
        /// </summary>
        /// <param name="configKey">配置文件中log4net/logger 中的name值</param>
        public static void InitLog4Net(string configKey)
        {
            Log4Net = new Log4Net(configKey);
        }

        /// <summary>
        /// 日志初始化方法
        /// </summary>
        /// <param name="configPath">配置文件路径</param>
        /// <param name="configKey">配置文件中log4net/logger 中的name值</param>
        public static void InitLog4Net(string configPath, string configKey)
        {
            Log4Net = new Log4Net(configPath, configKey);
        }


    }
}
