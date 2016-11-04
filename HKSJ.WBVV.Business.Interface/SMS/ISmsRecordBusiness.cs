
namespace HKSJ.WBVV.Business.Interface
{
    /// <summary>
    /// 短信发送记录接口
    /// </summary>
    public interface ISmsRecordBusiness
    {
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
        bool CreateSmsRecord(string phone, string code, int state, int createManageId = 0, int userId = 0, string ipAddress = "");

        /// <summary>
        /// 检查短信验证码是否有效
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        bool CheckSmsRecord(string phone, string code, string ipAddress);
    }
}
