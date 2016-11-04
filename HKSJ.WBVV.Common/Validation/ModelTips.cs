using HKSJ.WBVV.Common.Language;
namespace HKSJ.WBVV.Common.Validation
{
    /// <summary>
    /// 服务端Model验证错误提示
    /// Author:AxOne
    /// </summary>
    public class ModelTips
    {
        //1、为空时提示：您输入的**不能为空！
        //2、字符长度不符合规范时：您输入的字符长度不正确，请输入（{0}位以上/{0}-{1}位字符长度！）
        //3、不符合格式时提示：您输入的**格式不正确！例：****
        //4、下拉框为选择时：请选择***！
        public static string RequiredTip = LanguageUtil.Translate("com_Validation_ModelTips_RequiredTip");
        public static string RequiredSelectTip = "com_Validation_ModelTips_RequiredSelectTip";
        public static string MinLengthTip = LanguageUtil.Translate("com_Validation_ModelTips_MinLengthTip");
        public static string RangeLengthTip = LanguageUtil.Translate("com_Validation_ModelTips_RangeLengthTip");
        public static string MaxLengthTip = LanguageUtil.Translate("com_Validation_ModelTips_MaxLengthTip");
        public static string EqualLengthTip = LanguageUtil.Translate("com_Validation_ModelTips_EqualLengthTip");
        public static string RegexTip = LanguageUtil.Translate("com_Validation_ModelTips_RegexTip");
        public static string SelectTip = LanguageUtil.Translate("com_Validation_ModelTips_SelectTip");
        public static string RangeIntTip = LanguageUtil.Translate("com_Validation_ModelTips_RangeIntTip");
        public static string RangeDoubleTip = LanguageUtil.Translate("com_Validation_ModelTips_RangeDoubleTip");
        public static string DoucleRegexTip = LanguageUtil.Translate("com_Validation_ModelTips_DoucleRegexTip");
        public static string WebSiteRegexTip = LanguageUtil.Translate("com_Validation_ModelTips_WebSiteRegexTip");
        public static string IntRegexTip = LanguageUtil.Translate("com_Validation_ModelTips_IntRegexTip");
        public static string DoubleRegexTip = LanguageUtil.Translate("com_Validation_ModelTips_DoubleRegexTip");
        public static string MobileRegexTip = LanguageUtil.Translate("com_Validation_ModelTips_MobileRegexTip");
        public static string TelephoneRegexTip = LanguageUtil.Translate("com_Validation_ModelTips_TelephoneRegexTip");
        public static string TelephoneAndMobileRegexTip = LanguageUtil.Translate("com_Validation_ModelTips_TelephoneAndMobileRegexTip");
        public static string EmailRegexTip = LanguageUtil.Translate("com_Validation_ModelTips_EmailRegexTip");
        public static string QQRegexTip = LanguageUtil.Translate("com_Validation_ModelTips_QQRegexTip");
        public static string IPRegexTip = LanguageUtil.Translate("com_Validation_ModelTips_IPRegexTip");
        public static string TrueNameRegexTip = LanguageUtil.Translate("com_Validation_ModelTips_TrueNameRegexTip");
        public static string UniqueTip = LanguageUtil.Translate("com_Validation_ModelTips_UniqueTip");
        public static string MACTip = LanguageUtil.Translate("com_Validation_ModelTips_MACTip");
    }
}
