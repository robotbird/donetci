using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.WebPages;
using System.Linq.Expressions;

namespace Utry.Framework.Mvc
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString JPartial(this HtmlHelper htmlHelper, string partialViewName)
        {
           return  htmlHelper.Partial("~/Views/Shared/" + partialViewName+".cshtml");
        }
    }
}
