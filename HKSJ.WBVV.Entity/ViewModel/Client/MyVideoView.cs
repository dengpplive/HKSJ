using HKSJ.WBVV.Common.Language;
using HKSJ.WBVV.Common.Resource;
using System;
using System.Collections.Generic;

namespace HKSJ.WBVV.Entity.ViewModel.Client
{
    [Serializable]
    public class MyVideoView
    {
        #region Fields

        /// <summary>
        /// Local veriable used to combine Small Pic Path the a video.
        /// </summary>
        string _smallPicPath;

        #endregion

        /// <summary>
        /// 视频编号
        /// </summary>
        public long Id { get; set; }


        /// <summary>
        /// 标题
        /// </summary>
        /// <returns></returns>
        public string Title { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        /// <returns></returns>
        public string About { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        /// <returns></returns>
        public string Tags { get; set; }
        /// <summary>
        /// 上传者ID
        /// </summary>
        public int CreateManageId { get; set; }
        /// <summary>
        /// 审核内容
        /// </summary>
        public string ApproveContent { get; set; }
        /// <summary>
        /// 审核内容
        /// </summary>
        public string ApproveRemark { get; set; }

        /// <summary>
        /// 审核原因描述
        /// </summary>
        public string ApproveStr
        {
            get
            {
                if (ApproveContent=="1")
                {
                    return LanguageUtil.Translate("Entity_ViewModel_Client_MyVideoView_ApproveStr_ApproveContent1");//原因：视频含有广告、灌水刷屏等相关内容
                }
                else if (ApproveContent=="2")
                {
                    return LanguageUtil.Translate("Entity_ViewModel_Client_MyVideoView_ApproveStr_ApproveContent2");//("原因：视频内容与标题、简介等不符");   
                }
                else if (ApproveContent == "3")
                {
                    return LanguageUtil.Translate("Entity_ViewModel_Client_MyVideoView_ApproveStr_ApproveContent3");//("原因：视频画面质量低");
                }
                else if (ApproveContent == "4")
                {
                    return LanguageUtil.Translate("Entity_ViewModel_Client_MyVideoView_ApproveStr_ApproveContent4"); //("原因：视频含有情色低俗等相关内容");
                }
                else if (ApproveContent == "5")
                {
                    return LanguageUtil.Translate("Entity_ViewModel_Client_MyVideoView_ApproveStr_ApproveContent5");//("原因：视频含有侮辱他人等相关内容");
                }
                else if (ApproveContent == "6")
                {
                    return LanguageUtil.Translate("Entity_ViewModel_Client_MyVideoView_ApproveStr_ApproveContent6");//("原因：视频含有泄露他人隐私信息的内容");
                }
                else if (ApproveContent == "7")
                {
                    return LanguageUtil.Translate("Entity_ViewModel_Client_MyVideoView_ApproveStr_ApproveContent7");//("原因：视频含有暴力血腥的相关内容");
                }
                else if (ApproveContent == "8")
                {
                    return LanguageUtil.Translate("Entity_ViewModel_Client_MyVideoView_ApproveStr_ApproveContent8");//("原因：视频内容违反相关法律法规");
                }
                else if (ApproveContent == "9")
                {
                    return ApproveRemark;
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 视频小图片
        /// </summary>
        public string SmallPicturePath
        {
            get
            {
                return UrlHelper.QiniuPublicCombine(_smallPicPath);
            }

            set
            {
                _smallPicPath = value;
            }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建时间字符串
        /// </summary>
        public String CreateTimeStr
        {
            get
            {
                return CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        /// <summary>
        /// 播放次数
        /// </summary>
        /// <returns></returns>
        public Int32 PlayCount { get; set; }

        /// <summary>
        /// 评论次数
        /// </summary>
        /// <returns></returns>
        public Int32 CommentCount { get; set; }

        /// <summary>
        /// 视频状态（0：转码中，1：转码失败，2：审核中，3：审核通过，4：审核不通过）
        /// </summary>
        public short VideoState { get; set; }

        /// <summary>
        /// 视频状态
        /// </summary>
        public string VideoStateStr
        {
            get
            {
                if (VideoState == 0)
                {
                    return LanguageUtil.Translate("entity_Table_Video_VideoState_transcoding");
                }
                else if (VideoState == 1)
                {
                    return LanguageUtil.Translate("entity_Table_Video_VideoState_transcodingFails");
                }
                else if (VideoState == 2)
                {
                    return LanguageUtil.Translate("entity_Table_Video_VideoState_underReview");
                }
                else if (VideoState == 3)
                {
                    return LanguageUtil.Translate("entity_Table_Video_VideoState_examinationPassed");
                }
                else if (VideoState == 4)
                {
                    return LanguageUtil.Translate("entity_Table_Video_VideoState_auditNotPassed");
                }
                return null;
            }
        }
    }

    public class MyVideoViewResult
    {
        public List<MyVideoView> MyVideoViews { get; set; }
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
    }
}
