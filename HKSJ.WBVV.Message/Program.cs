using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Message.Service;

namespace HKSJ.WBVV.Message
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            #if !DEBUG
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
            new MessageService()
            };
            ServiceBase.Run(ServicesToRun);
            #else
            new MessageService().PushMessage();
            Console.WriteLine("服务已经启动......");
            Console.ReadLine();
            #endif
        }
    }
}
