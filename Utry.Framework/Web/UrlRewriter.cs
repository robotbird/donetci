using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using Utry.Framework.Configuration;

namespace Utry.Framework.Web
{
    public class UrlRewriter : System.Web.IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(this.Application_BeginRequest);

        }
        public void Dispose() { }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            //foreach (RewriterRule rule in GetRuleList())
            //{
            //    string lookFor = "^" + ResolveUrl(app.Request.ApplicationPath, rule.LookFor) + "$";
            //    Regex re = new Regex(lookFor, RegexOptions.IgnoreCase);
            //    app.Response.Write(lookFor + " ");
            //    app.Response.Write(rule.LookFor + " ");
            //    app.Response.Write(rule.SendTo + " ");
            //    app.Response.Write(app.Request.Path + " ");
            //    app.Response.Write(app.Request.ApplicationPath + " ");
            //    app.Response.Write(re.IsMatch(app.Request.Path) + " ");
            //    app.Response.Write("<br>");
            //    app.Response.Write(app.Request.Url.AbsoluteUri);
            //}

            foreach (RewriterRule rule in GetRuleList())
            {
                string lookFor = "^" + ResolveUrl(app.Request.ApplicationPath, rule.LookFor) + "$";
                Regex re = new Regex(lookFor, RegexOptions.IgnoreCase);
                //域名重写
                if (IsHttpUrl(rule.LookFor))
                {
                    if (re.IsMatch(app.Request.Url.AbsoluteUri))
                    {
                        string sendTo = ResolveUrl(app.Context.Request.ApplicationPath, re.Replace(app.Request.Url.AbsoluteUri, rule.SendTo));
                        RewritePath(app.Context, sendTo);
                        break;
                    }
                }
                //站内路径重写
                else
                {
                    if (re.IsMatch(app.Request.Path))
                    {
                        string sendTo = ResolveUrl(app.Context.Request.ApplicationPath, re.Replace(app.Request.Path, rule.SendTo));
                        RewritePath(app.Context, sendTo);
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 是否为URL地址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool IsHttpUrl(string url)
        {
            return url.IndexOf("http://") != -1;
        }

        /// <summary>
        /// 重写路径,主要处理参数
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sendToUrl"></param>
        protected void RewritePath(HttpContext context, string sendToUrl)
        {
            if (context.Request.QueryString.Count > 0)
            {
                if (sendToUrl.IndexOf('?') != -1)
                {
                    sendToUrl += "&" + context.Request.QueryString.ToString();
                }
                else
                {
                    sendToUrl += "?" + context.Request.QueryString.ToString();
                }
            }
            string queryString = String.Empty;
            string sendToUrlLessQString = sendToUrl;
            if (sendToUrl.IndexOf('?') > 0)
            {
                sendToUrlLessQString = sendToUrl.Substring(0, sendToUrl.IndexOf('?'));
                queryString = sendToUrl.Substring(sendToUrl.IndexOf('?') + 1);
            }
            //    context.RewritePath(sendToUrlLessQString +"?"+ queryString);
            context.RewritePath(sendToUrlLessQString, String.Empty, queryString);
        }

        /// <summary>
        /// 读取并缓存规则列表
        /// </summary>
        /// <returns></returns>
        protected ArrayList GetRuleList()
        {
            string cacheKey = ConfigHelper.SitePrefix + "rewriterulelist";

            ArrayList ruleList = (ArrayList)HttpContext.Current.Cache.Get(cacheKey);
            if (ruleList == null)
            {
                ruleList = new ArrayList();
                string urlFilePath = HttpContext.Current.Server.MapPath(string.Format("{0}common/config/rewrite.config", ConfigHelper.SitePath));
                System.Xml.XmlDocument xml = new System.Xml.XmlDocument();

                xml.Load(urlFilePath);

                XmlNode root = xml.SelectSingleNode("rewrite");
                foreach (XmlNode n in root.ChildNodes)
                {
                    if (n.NodeType != XmlNodeType.Comment && n.Name.ToLower() == "item")
                    {
                        RewriterRule rule = new RewriterRule();
                        rule.LookFor = ConfigHelper.SitePath + n.Attributes["lookfor"].Value;
                        rule.SendTo = ConfigHelper.SitePath + n.Attributes["sendto"].Value;
                        ruleList.Add(rule);
                    }
                }
                HttpContext.Current.Cache.Insert(cacheKey, ruleList, new System.Web.Caching.CacheDependency(urlFilePath));
            }
            return ruleList;
        }

        /// <summary>
        /// 处理各种路径
        /// </summary>
        /// <param name="appPath"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public string ResolveUrl(string appPath, string url)
        {
            //   return url;
            if (url.Length == 0 || url[0] != '~')
                return url;		// there is no ~ in the first character position, just return the url
            else
            {
                if (url.Length == 1)
                    return appPath;  // there is just the ~ in the URL, return the appPath
                if (url[1] == '/' || url[1] == '\\')
                {
                    // url looks like ~/ or ~\
                    if (appPath.Length > 1)
                        return appPath + "/" + url.Substring(2);
                    else
                        return "/" + url.Substring(2);
                }
                else
                {
                    // url looks like ~something
                    if (appPath.Length > 1)
                        return appPath + "/" + url.Substring(1);
                    else
                        return appPath + url.Substring(1);
                }
            }
        }
    }
    /// <summary>
    /// 规则实体
    /// </summary>
    public class RewriterRule
    {
        private string _lookfor;
        private string _sendto;
        /// <summary>
        /// 正则地址
        /// </summary>
        public string LookFor
        {
            get { return _lookfor; }
            set { _lookfor = value; }
        }
        /// <summary>
        /// 实际地址
        /// </summary>
        public string SendTo
        {
            get { return _sendto; }
            set { _sendto = value; }
        }
    }
}
