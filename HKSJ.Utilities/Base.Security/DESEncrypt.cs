//=====================================================================================
// All Rights Reserved , Copyright © HKSJ 2013
//=====================================================================================
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace HKSJ.Utilities
{
    /// <summary>
    /// 加密、解密帮助类
    /// 版本：2.0
    /// <author>
    ///		<name>shecixiong</name>
    ///		<date>2013.09.27</date>
    /// </author>
    /// </summary>
    public class DESEncrypt
    {
        #region ========加密========
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Encrypt(string Text)
        {
            return Encrypt(Text, "HKSJ###***");
        }
        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Encrypt(string Text, string sKey)
        {
            if (string.IsNullOrWhiteSpace(Text)) return "";
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        #endregion

        #region ========解密========
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Decrypt(string Text)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                return Decrypt(Text, "HKSJ###***");
            }
            else
            {
                return "";
            }
        }
        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Decrypt(string Text, string sKey)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Text)) return "";
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                int len;
                len = Text.Length / 2;
                byte[] inputByteArray = new byte[len];
                int x, i;
                for (x = 0; x < len; x++)
                {
                    i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                    inputByteArray[x] = (byte)i;
                }
                des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
                des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Encoding.Default.GetString(ms.ToArray());
            }
            catch (Exception)
            {
                return "";
            }
        }

        #endregion


        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="originStr"></param>
        /// <returns></returns>
        public string EncryptAx(string originStr)
        {
            if (originStr == null)
            {
                return null;
            }
            char[] arrResult = new char[originStr.Length];
            for (int i = 0; i < originStr.Length; i++)
            {
                arrResult[i] = xor(originStr[i]);
            }
            return getUrlStr(arrResult);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="encryptedStr"></param>
        /// <returns></returns>
        public string DecryptAx(string encryptedStr)
        {

            if (encryptedStr == null)
            {
                return null;
            }
            var arr = fromUrlStr(encryptedStr);
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = xor(arr[i]);
            }
            return new string(arr);
        }


        /// <summary>
        /// 异或运算
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private char xor(char str)
        {
            char result = str;
            string key = "①";//SystemConfig.EncryptKey;
            foreach (var item in key)
            {
                result = (char)(result ^ item);
            }
            return result;
        }

        private string getUrlStr(char[] chars, string encodingStr = "utf-8")
        {
            Encoding encoding = Encoding.GetEncoding(encodingStr);
            return Convert.ToBase64String(encoding.GetBytes(chars), Base64FormattingOptions.None);
        }
        private char[] fromUrlStr(string str, string encodingStr = "utf-8")
        {
            Encoding encoding = Encoding.GetEncoding(encodingStr);
            try
            {
                var bts = Convert.FromBase64String(str);
                return encoding.GetString(bts).ToCharArray();
            }
            catch
            {
                return null; //throw new ParameterErrorException("身份认证字符串错误！");
            }
        }

    }
}