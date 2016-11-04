using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using HKSJ.Utilities;
using HKSJ.WBVV.Common.Config;
using HKSJ.WBVV.Common.Http;
using HKSJ.WBVV.Entity.ViewModel.Client;
using HKSJ.WBVV.MVC.Common;
using Newtonsoft.Json;

namespace HKSJ.WBVV.MVC.Client.GlobalVariable
{
    /// <summary>
    /// 用户会话信息
    /// Author : AxOne
    /// </summary>
    public class GlobalMemberInfo
    {
        /// <summary>
        /// 是否记住当前用户
        /// </summary>
        public static int RemberMe
        {
            get
            {
                var remberMe = DESEncrypt.Decrypt(HttpUtility.UrlDecode(CookieHelper.GetCookie("ck5bvv", "Ck5bRMQE")), DecryptionKey);
                return !string.IsNullOrEmpty(remberMe) ? Convert.ToInt32(remberMe) : 0;
            }
        }

        /// <summary>
        /// 返回当前登录用户的登录Id
        /// </summary>
        public static int UserId
        {
            get
            {
                var userId = DESEncrypt.Decrypt(HttpUtility.UrlDecode(CookieHelper.GetCookie("ck5bvv", "Ck5bUSD")), DecryptionKey);
                return !string.IsNullOrEmpty(userId) ? Convert.ToInt32(userId) : 0;
            }
        }

        /// <summary>
        /// 当前登录用户的用户名
        /// </summary>
        public static string Account
        {
            get
            {
                return DESEncrypt.Decrypt(HttpUtility.UrlDecode(CookieHelper.GetCookie("ck5bvv", "Ck5bACCT")), DecryptionKey);
            }
        }

        /// <summary>
        /// 当前登录用户的手机号码
        /// </summary>
        public static string Phone
        {
            get
            {
                return DESEncrypt.Decrypt(HttpUtility.UrlDecode(CookieHelper.GetCookie("ck5bvv", "Ck5bPCNMB")), DecryptionKey);
            }
        }

        /// <summary>
        /// 当前登录用户的密码
        /// </summary>
        public static string PassWord
        {
            get
            {
                return DESEncrypt.Decrypt(HttpUtility.UrlDecode(CookieHelper.GetCookie("ck5bvv", "Ck5bPEWFD")), DecryptionKey);
            }
        }

        /// <summary>
        /// 当前登录用户的昵称
        /// </summary>
        public static string NickName
        {
            get
            {
                return DESEncrypt.Decrypt(HttpUtility.UrlDecode(CookieHelper.GetCookie("ck5bvv", "Ck5bPNWFM")), DecryptionKey);
            }
        }

        /// <summary>
        /// 当前登录用户的Token信息
        /// </summary>
        public static string Token
        {
            get
            {
                return DESEncrypt.Decrypt(HttpUtility.UrlDecode(CookieHelper.GetCookie("ck5bvv", "Ck5bSETKE")), DecryptionKey) ?? "";
            }
        }

        /// <summary>
        /// 当前登录用户的播币数
        /// </summary>
        public static int BB
        {
            get
            {
                var bb = DESEncrypt.Decrypt(HttpUtility.UrlDecode(CookieHelper.GetCookie("ck5bvv", "Ck5bPBWRB")), DecryptionKey);
                return !string.IsNullOrEmpty(bb) ? Convert.ToInt32(bb) : 0;
            }
        }

        /// <summary>
        /// 当前登录用户头像
        /// </summary>
        public static string Picture
        {
            get
            {
                return UserId <= 0 ? WebConfig.DefaultUserPic : DESEncrypt.Decrypt(HttpUtility.UrlDecode(CookieHelper.GetCookie("ck5bvv", "Ck5bSEPIC")), DecryptionKey);
            }
        }

        /// <summary>
        /// 当前登录用户的被订阅数
        /// </summary>
        public static int SubscribeNum
        {
            get
            {
                var snum = DESEncrypt.Decrypt(HttpUtility.UrlDecode(CookieHelper.GetCookie("ck5bvv", "Ck5bSEWFM")), DecryptionKey);
                return !string.IsNullOrEmpty(snum) ? Convert.ToInt32(snum) : 0;
            }
        }

        /// <summary>
        /// 当前登录用户的粉丝数
        /// </summary>
        public static int FansCount
        {
            get
            {
                var snum = DESEncrypt.Decrypt(HttpUtility.UrlDecode(CookieHelper.GetCookie("ck5bvv", "Ck5bPCFSB")), DecryptionKey);
                return !string.IsNullOrEmpty(snum) ? Convert.ToInt32(snum) : 0;
            }
        }

        /// <summary>
        /// 用户状态（0:启用1:禁用）
        /// </summary>
        public static bool State
        {
            get
            {
                return DESEncrypt.Decrypt(HttpUtility.UrlDecode(CookieHelper.GetCookie("ck5bvv", "Ck5bSETFA")), DecryptionKey) != "0";
            }
        }

        /// <summary>
        /// 加密密匙
        /// </summary>
        public static string DecryptionKey
        {
            get
            {
                var machineKeySection = (MachineKeySection)ConfigurationManager.GetSection("system.web/machineKey");
                var type = typeof(MachineKeySection);
                var decryptInfo = type.GetProperty("DecryptionKeyInternal", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty);
                var decryptValue = (byte[])decryptInfo.GetValue(machineKeySection);
                var decryptStr = decryptValue.Aggregate<byte, string>(null, (current, item) => current + item.ToString("x2"));
                return decryptStr;
            }
        }

        public static void SetUserCookie(UserView uv, int remb = 0)
        {
            var nameValueCollection = new NameValueCollection
            {
                {"Ck5bACCT", HttpUtility.UrlEncode(DESEncrypt.Encrypt(uv.Account,DecryptionKey))},
                {"Ck5bUSD", HttpUtility.UrlEncode(DESEncrypt.Encrypt(uv.Id.ToString(),DecryptionKey))},
                {"Ck5bPEWFD", HttpUtility.UrlEncode(DESEncrypt.Encrypt(uv.Pwd,DecryptionKey))},
                {"Ck5bPNWFM", HttpUtility.UrlEncode(DESEncrypt.Encrypt(uv.NickName,DecryptionKey))},
                {"Ck5bPBWRB", HttpUtility.UrlEncode(DESEncrypt.Encrypt(uv.BB.ToString(),DecryptionKey))},
                {"Ck5bSEWFM", HttpUtility.UrlEncode(DESEncrypt.Encrypt(uv.SubscribeNum.ToString(),DecryptionKey))},
                {"Ck5bPCNMB", HttpUtility.UrlEncode(DESEncrypt.Encrypt(uv.Phone,DecryptionKey))},
                {"Ck5bPCFSB", HttpUtility.UrlEncode(DESEncrypt.Encrypt(uv.FansCount.ToString(),DecryptionKey))},
                {"Ck5bRMQE", HttpUtility.UrlEncode(DESEncrypt.Encrypt(remb.ToString(),DecryptionKey))},
                {"Ck5bSEPIC", HttpUtility.UrlEncode(DESEncrypt.Encrypt(uv.Picture, DecryptionKey))},
                {"Ck5bSETFA", HttpUtility.UrlEncode(DESEncrypt.Encrypt(uv.State.ToString(),DecryptionKey))}
            };
            if (!string.IsNullOrWhiteSpace(uv.Token))
            {
                nameValueCollection.Add("Ck5bSETKE", HttpUtility.UrlEncode(DESEncrypt.Encrypt(uv.Token, DecryptionKey)));
            }
            CookieHelper.AddCookie("ck5bvv", nameValueCollection, remb == 0 ? 0 : 604800);
        }

        public static string GetQiniuUserPicture(int uid)
        {
            try
            {
                if (uid <= 0) return WebConfig.DefaultUserPic;
                var result = WebApiHelper.InvokeApi<string>("User/GetUserPicKey?uid=" + uid);
                var rv = JsonConvert.DeserializeObject(result, typeof(ResultView<string>)) as ResultView<string>;
                if (rv == null || !rv.Success || string.IsNullOrWhiteSpace(rv.Data))
                {
                    return WebConfig.DefaultUserPic;
                }

                var picUrl = rv.Data;
                picUrl = picUrl.Replace("\"", "");
                if (picUrl.Contains("\\\\"))
                    picUrl = picUrl.Replace("\\\\", "\\");

                if (string.Equals("NULL", picUrl))
                    return WebConfig.DefaultUserPic;

                if (picUrl.Contains(@"http://") || picUrl.Contains(@"https://"))
                    return picUrl;

                if (!picUrl.Contains("head_img"))
                    return DefaultData.ImageAddress + "/" + picUrl;

                return "http:" + ServerHelper.RootPath + "/" + picUrl;
            }
            catch (Exception)
            {
                return WebConfig.DefaultUserPic;
            }
        }

        /// <summary>
        /// 加密密匙
        /// </summary>
        public static string GetToken
        {
            get
            {
                return Token;
            }
        }

    }
}

