
using System;
using System.Linq;
using HKSJ.WBVV.Business.Interface;
using HKSJ.WBVV.Common.Assert;
using HKSJ.WBVV.Common.Extender;
using HKSJ.WBVV.Common.Extender.LinqExtender;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Repository.Interface;
using HKSJ.WBVV.Common.Language;

namespace HKSJ.WBVV.Business
{
    /// <summary>
    /// 短信验证记录
    /// </summary>
    public class SmsRecordBusiness : ISmsRecordBusiness
    {
        private readonly ISmsRecordRepository _smsRecordRepository;

        public SmsRecordBusiness(ISmsRecordRepository smsRecordRepository)
        {
            _smsRecordRepository = smsRecordRepository;
        }

        /// <summary>
        /// 新增短信发送记录
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <param name="ipAddress"></param>
        /// <param name="state"></param>
        /// <param name="createManageId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool CreateSmsRecord(string phone, string code, int state, int createManageId = 0, int userId = 0, string ipAddress = "")
        {
            phone = phone.UrlDecode();
            code = code.UrlDecode();
            ipAddress = string.IsNullOrWhiteSpace(ipAddress) ? "" : ipAddress.UrlDecode();
            CheckPhoneNotNull(phone);
            CheckCodeNotNull(code);
            var smsRecord = new SmsRecord
            {
                UserId = userId,
                Mobile = phone,
                Content = code,
                SendTime = DateTime.Now,
                State = state,
                IpAddress = ipAddress,
                CreateManageId = createManageId,
                CreateTime = DateTime.Now
            };
            return _smsRecordRepository.CreateEntity(smsRecord);
        }

        /// <summary>
        /// 检查短信验证码是否有效
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public bool CheckSmsRecord(string phone, string code, string ipAddress)
        {
            var curTime = DateTime.Now;
            phone = phone.UrlDecode();
            code = code.UrlDecode();
            CheckPhoneNotNull(phone);
            CheckCodeNotNull(code);
            var smsRecord = GetEntityByMobile(phone);
            if (smsRecord == null) return false;
            var result = smsRecord.SendTime.AddSeconds(120) > curTime;
            return result;
        }

        public SmsRecord GetEntityByUserId(int userId)
        {
            var condtion = new Condtion
            {
                FiledName = "UserId",
                FiledValue = userId,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            var smsRecord = _smsRecordRepository.GetEntity(condtion);
            return smsRecord;
        }
        public SmsRecord GetEntityByMobile(string mobile)
        {
            var condtion = new Condtion
            {
                FiledName = "Mobile",
                FiledValue = mobile,
                ExpressionLogic = ExpressionLogic.And,
                ExpressionType = ExpressionType.Equal
            };
            var smsRecord = _smsRecordRepository.GetEntityList(condtion).OrderByDescending(p => p.CreateTime).FirstOrDefault();
            return smsRecord;
        }

        #region 传入参数检测
        private void CheckPhoneNotNull(string phone)
        {
            AssertUtil.NotNullOrWhiteSpace(phone, LanguageUtil.Translate("api_Business_SMSRecord_CheckPhoneNotNull"));
        }

        private void CheckCodeNotNull(string code)
        {
            AssertUtil.NotNullOrWhiteSpace(code, LanguageUtil.Translate("api_Business_SMSRecord_CheckCodeNotNull"));
        } 
        #endregion

    }
}
