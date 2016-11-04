using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using HKSJ.WBVV.Common.Validation.ModelClientRule;

namespace HKSJ.WBVV.Common.Validation.Attributes
{
    /// <summary>
    /// 服务端Model非空验证
    /// Author:AxOne
    /// </summary>
    public class ParaRequiredAttribute : RequiredAttribute, IClientValidatable
    {
        public ParaRequiredAttribute()
        {
            ErrorMessage = ModelTips.RequiredTip;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRequriedToRule(FormatErrorMessage(metadata.GetDisplayName()));
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name);
        }

    }
}
