using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Logger;
using HKSJ.WBVV.Entity.ApiModel;
using HKSJ.WBVV.Message.Config;
using HKSJ.WBVV.Message.WebSocket;

namespace HKSJ.WBVV.Message.Service
{
    partial class MessageService : ServiceBase
    {
         WebSocketServer _webSocket = new WebSocketServer();
        public MessageService()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
          
        }
        /// <summary>
        /// 时间间隔到达后执行的代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //PushMessage();
        }

        internal  void PushMessage()
        {
           
            try
            {
                _webSocket.StartServer();
            }
            catch (Exception ex)
            {
                LogBuilder.Log4Net.Fatal("推送消息错误：{0}".F(ex.Message));
            }

        }
        protected override void OnStop()
        {
          this.Dispose();
          GC.SuppressFinalize(this);
        }
    }
}
