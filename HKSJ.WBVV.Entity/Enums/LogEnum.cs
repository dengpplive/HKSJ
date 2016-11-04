using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKSJ.WBVV.Entity.Enums
{
    public enum LogEnum
    {
        [Description("BannerVideoController")]
        BannerVideoController = 1,
        [Description("BBEventController")]
        BBEventController = 2,
        [Description("CategoryController")]
        CategoryController = 3,
        [Description("CommentController")]
        CommentController = 4,
        [Description("DictionaryController")]
        DictionaryController = 5,
        [Description("DictionaryItemController")]
        DictionaryItemController = 6,
        [Description("IntegrationDetailController")]
        IntegrationDetailController = 7,
        [Description("LoginController")]
        LoginController = 8,
        [Description("ManageController")]
        ManageController = 9,
        [Description("OAuthController")]
        OAuthController = 10,
        [Description("PlateController")]
        PlateController = 11,
        [Description("PlateVideoController")]
        PlateVideoController = 12,
        [Description("PraiseController")]
        PraiseController = 13,
        [Description("RegisterController")]
        RegisterController = 14,
        [Description("RewardController")]
        RewardController = 15,
        [Description("SMSController")]
        SMSController = 16,
        [Description("SystemMessageController")]
        SystemMessageController = 17,
        [Description("UserCollectController")]
        UserCollectController = 18,
        [Description("UserController")]
        UserController = 19,
        [Description("UserFansController")]
        UserFansController = 20,
        [Description("UserMessageController")]
        UserMessageController = 21,
        [Description("UserRecommendController")]
        UserRecommendController = 22,
        [Description("UserRoomChooseController")]
        UserRoomChooseController = 23,
        [Description("UserSpecialController")]
        UserSpecialController = 24,
        [Description("UserSubscribeController")]
        UserSubscribeController = 25,
        [Description("UserVisitLogController")]
        UserVisitLogController = 26,
        [Description("VideoApproveController")]
        VideoApproveController = 27,
        [Description("VideoController")]
        VideoController = 28
    }
}