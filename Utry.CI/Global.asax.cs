using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Utry.Framework.Log;

namespace Utry.CI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            #region 忽略静态文件路由
            routes.IgnoreRoute("{*jpg}", new { jpg = @".*\.jpg(/.*)?" });
            routes.IgnoreRoute("{*gif}", new { gif = @".*\.gif(/.*)?" });
            routes.IgnoreRoute("{*js}", new { js = @".*\.js(/.*)?" });
            routes.IgnoreRoute("{*css}", new { css = @".*\.css(/.*)?" });
            routes.IgnoreRoute("{*html}", new { css = @".*\.html(/.*)?" });
            routes.IgnoreRoute("{*htm}", new { css = @".*\.htm(/.*)?" });
            routes.IgnoreRoute("{*shtml}", new { css = @".*\.shtml(/.*)?" });
            routes.IgnoreRoute("{*htc}", new { css = @".*\.htc(/.*)?" });
            routes.IgnoreRoute("{*map}", new { css = @".*\.map(/.*)?" });
            #endregion

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception x = Server.GetLastError().GetBaseException();
            Logger.WriteLog(LogType.ERROR,x);
        }
    }
}