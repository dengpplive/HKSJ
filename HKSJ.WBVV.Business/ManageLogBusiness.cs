using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Business.Base;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Repository.Interface;

namespace HKSJ.WBVV.Business
{
    public class ManageLogBusiness : BaseBusiness, IManageLogBusiness
    {
        private readonly IManageLogRepository _manageLogRepository;

        public ManageLogBusiness(IManageLogRepository manageLogRepository)
        {
            this._manageLogRepository = manageLogRepository;
        }

        /// <summary>
        /// 创建管理信息
        /// </summary>
        /// <param name="logCode"></param>
        /// <param name="logContent"></param>
        /// <param name="descc"></param>
        /// <returns></returns>
        public bool CreateManageLog(int? logCode, string logContent, string descc)
        {
            var manageLog = new ManageLog()
            {
                LogCode = logCode,
                LogContent = logContent,
                Descc = descc,
                CreateTime = DateTime.Now,
                CreateManageId = 1,
                LogTime = DateTime.Now

            };
            return this._manageLogRepository.CreateEntity(manageLog);
        }
    }
}
