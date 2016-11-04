using System.Web.Mvc;

namespace HKSJ.WBVV.Common.Validation.ModelClientRule
{
    /// <summary>
    /// 服务端Model非空验证规则
    /// Author:AxOne
    /// </summary>
    public class ModelClientValidationRequriedToRule : ModelClientValidationRule
    {
        public ModelClientValidationRequriedToRule(string errorMessage)
        {
            ValidationType = "required";
            ErrorMessage = errorMessage;
        }
    }
}
