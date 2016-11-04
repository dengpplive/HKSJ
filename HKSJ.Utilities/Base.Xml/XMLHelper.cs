//=====================================================================================
// All Rights Reserved , Copyright © HKSJ 2013
//=====================================================================================
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.IO;
using System.Web;
using System.Xml;

namespace HKSJ.Utilities
{
    public class XMLHelper
    {
        #region 对象转换
        /// <summary>
        /// Hashtable
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public static Hashtable XMLToHashtable(string xmlData)
        {
            DataTable dt = XMLToDataTable(xmlData);
            return DataHelper.DataTableToHashtable(dt);
        }
        /// <summary>
        /// DataTable
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public static DataTable XMLToDataTable(string xmlData)
        {
            if (!String.IsNullOrEmpty(xmlData))
            {
                DataSet ds = new DataSet();
                ds.ReadXml(new System.IO.StringReader(xmlData));
                if (ds.Tables.Count > 0)
                    return ds.Tables[0];
            }
            return null;
        }
        /// <summary>
        /// DataSet
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public static DataSet XMLToDataSet(string xmlData)
        {
            if (!String.IsNullOrEmpty(xmlData))
            {
                DataSet ds = new DataSet();
                ds.ReadXml(new System.IO.StringReader(xmlData));
                return ds;
            }
            return null;
        }
        /// <summary>
        /// List
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="nodeName">节点名</param>
        /// <param name="nodeName">属性名</param>
        /// <returns>节点内容</returns>
        public static List<string> GetAttributesValue(string filePath, string attribute)
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(filePath) && !string.IsNullOrEmpty(attribute))
            {
                XmlDocument xmldoc = Instance(filePath);
                if (xmldoc != null)
                {
                    try
                    {
                        XmlNode rootNode = xmldoc.DocumentElement;
                        XmlNodeList children = rootNode.ChildNodes;
                        foreach (XmlNode child in children)
                        {
                            list.Add(child.Attributes[attribute].Value);
                        }
                    }
                    catch (NullReferenceException)
                    {
                        return new List<string>();
                    }
                }
            }
            return list;
        }
        #endregion

        #region 增、删、改操作
        /// <summary>
        /// 追加节点
        /// </summary>
        /// <param name="filePath">XML文档绝对路径</param>
        /// <param name="xPath">范例: @"Skill/First/SkillItem"</param>
        /// <param name="xmlNode">XmlNode节点</param>
        /// <returns></returns>
        public static bool AppendChild(string filePath, string xPath, XmlNode xmlNode)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);
                XmlNode xn = doc.SelectSingleNode(xPath);
                XmlNode n = doc.ImportNode(xmlNode, true);
                xn.AppendChild(n);
                doc.Save(filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 从XML文档中读取节点追加到另一个XML文档中
        /// </summary>
        /// <param name="filePath">需要读取的XML文档绝对路径</param>
        /// <param name="xPath">范例: @"Skill/First/SkillItem"</param>
        /// <param name="toFilePath">被追加节点的XML文档绝对路径</param>
        /// <param name="toXPath">范例: @"Skill/First/SkillItem"</param>
        /// <returns></returns>
        public static bool AppendChild(string filePath, string xPath, string toFilePath, string toXPath)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(toFilePath);
                XmlNode xn = doc.SelectSingleNode(toXPath);

                XmlNodeList xnList = ReadNodes(filePath, xPath);
                if (xnList != null)
                {
                    foreach (XmlElement xe in xnList)
                    {
                        XmlNode n = doc.ImportNode(xe, true);
                        xn.AppendChild(n);
                    }
                    doc.Save(toFilePath);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 修改节点的InnerText的值
        /// </summary>
        /// <param name="filePath">XML文件绝对路径</param>
        /// <param name="xPath">范例: @"Skill/First/SkillItem"</param>
        /// <param name="value">节点的值</param>
        /// <returns></returns>
        public static bool UpdateNodeInnerText(string filePath, string xPath, string value)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);
                XmlNode xn = doc.SelectSingleNode(xPath);
                XmlElement xe = (XmlElement)xn;
                xe.InnerText = value;
                doc.Save(filePath);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 读取XML文档
        /// </summary>
        /// <param name="filePath">XML文件绝对路径</param>
        /// <returns></returns>
        public static XmlDocument LoadXmlDoc(string filePath)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);
                return doc;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 根据指定文件路径加载XML文件
        /// </summary>
        /// <param name="fileName">XML文件名称</param>
        /// <returns>返回XML文档</returns>
        private static XmlDocument Instance(string fileName)
        {
            string filePath = string.Empty;
            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                filePath = context.Server.MapPath("~/" + fileName);
            }
            else
            {
                filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            }
            XmlDocument xmldoc = new XmlDocument();
            if (File.Exists(filePath))
            {
                try
                {
                    xmldoc.Load(filePath);
                }
                catch (XmlException)
                {
                    return null;
                }
            }
            return xmldoc;
        }
        #endregion 增、删、改操作

        #region 扩展方法
        /// <summary>
        /// 读取XML的所有子节点
        /// </summary>
        /// <param name="filePath">XML文件绝对路径</param>
        /// <param name="xPath">范例: @"Skill/First/SkillItem"</param>
        /// <returns></returns>
        public static XmlNodeList ReadNodes(string filePath, string xPath)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);
                XmlNode xn = doc.SelectSingleNode(xPath);
                XmlNodeList xnList = xn.ChildNodes;  //得到该节点的子节点
                return xnList;
            }
            catch
            {
                return null;
            }
        }

        #endregion 扩展方法
    }
}
