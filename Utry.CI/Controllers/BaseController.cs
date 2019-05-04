using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Utry.Framework.Configuration;
using Utry.Core.Domain;
using Utry.Core.Domain.Enum;
using Utry.Core.Services;
using Utry.Framework.Utils;
using Utry.Framework.Web;

namespace Utry.CI.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// web site path
        /// </summary>
        public string SiteUrl;
        /// <summary>
        /// 
        /// 用户COOKIE名
        /// </summary>
        private static readonly string CookieUser = ConfigHelper.SitePrefix + "utry";
        /// <summary>
        /// 登陆用户ID
        /// </summary>
        public string CurrentUserId;

        /// <summary>
        /// 当前用户
        /// </summary>
        public static CIUser CurrentUser;
        /// <summary>
        /// Initialize controller
        /// </summary>
        /// <param name="requestContext">Request context</param>
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            SiteUrl = PressRequest.GetCurrentFullHost();
            string url = PressRequest.GetRawUrl();

        }
        protected virtual void ValidateLogin(ActionExecutingContext filterContext)
        {
            var cookie = System.Web.HttpContext.Current.Request.Cookies[CookieUser];
            if (cookie != null)
            {
                CurrentUserId = TypeConverter.ObjectToString(cookie["username"]);
                CurrentUser = (new UserService()).GetUser(CurrentUserId);
                ViewBag.Role = CurrentUser.Role;
                if (string.IsNullOrEmpty(CurrentUserId))
                {
                    filterContext.Result = RedirectToAction("login", "home", new { area = "admin" });
                }
            }
            else
            {
                //filterContext.Result = RedirectToAction("login", "home", new { area = "admin" });
            }

        }
        protected BaseController()
        {
        }

        /// <summary>
        /// On action executing
        /// </summary>
        /// <param name="filterContext">Filter context</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //validate if login
            ValidateLogin(filterContext);

            base.OnActionExecuting(filterContext);
        }
        /// <summary>
        /// On exception
        /// </summary>
        /// <param name="filterContext">Filter context</param>
        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception != null)
                // LogException(filterContext.Exception);
                base.OnException(filterContext);
        }
        /// <summary>
        /// Access denied view
        /// </summary>
        /// <returns>Access denied view</returns>
        protected ActionResult AccessDeniedView()
        {
            //return new HttpUnauthorizedResult();
            return RedirectToAction("AccessDenied", "Security", new { pageUrl = this.Request.RawUrl });
        }
        /// <summary>
        /// Display success notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void SuccessNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Success, message, persistForTheNextRequest);
        }
        /// <summary>
        /// Display error notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void ErrorNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Error, message, persistForTheNextRequest);
        }
        /// <summary>
        /// Display error notification
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        /// <param name="logException">A value indicating whether exception should be logged</param>
        protected virtual void ErrorNotification(Exception exception, bool persistForTheNextRequest = true, bool logException = true)
        {
           // if (logException)
              //  LogException(exception);
            AddNotification(NotifyType.Error, exception.Message, persistForTheNextRequest);
        }
        /// <summary>
        /// Display notification
        /// </summary>
        /// <param name="type">Notification type</param>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void AddNotification(NotifyType type, string message, bool persistForTheNextRequest)
        {
            string dataKey = string.Format("Utry.notifications.{0}", type);
            if (persistForTheNextRequest)
            {
                if (TempData[dataKey] == null)
                    TempData[dataKey] = new List<string>();
                ((List<string>)TempData[dataKey]).Add(message);
            }
            else
            {
                if (ViewData[dataKey] == null)
                    ViewData[dataKey] = new List<string>();
                ((List<string>)ViewData[dataKey]).Add(message);
            }
        }
        /// <summary>
        /// Render partial view to string
        /// </summary>
        /// <returns>Result</returns>
        protected string RenderPartialViewToString()
        {
            return RenderPartialViewToString(null, null);
        }
        /// <summary>
        /// Render partial view to string
        /// </summary>
        /// <param name="viewName">View name</param>
        /// <returns>Result</returns>
        protected string RenderPartialViewToString(string viewName)
        {
            return RenderPartialViewToString(viewName, null);
        }
        /// <summary>
        /// Render partial view to string
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns>Result</returns>
        protected string RenderPartialViewToString(object model)
        {
            return RenderPartialViewToString(null, model);
        }
        /// <summary>
        /// Render partial view to string
        /// </summary>
        /// <param name="viewName">View name</param>
        /// <param name="model">Model</param>
        /// <returns>Result</returns>
        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
