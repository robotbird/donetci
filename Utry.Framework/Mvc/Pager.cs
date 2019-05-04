using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Utry.Framework.Mvc
{
    /// <summary>
    /// Renders a pager component from an IPageableModel datasource.
	/// </summary>
	public partial class Pager : IHtmlString
	{
        protected readonly IPageableModel model;
        protected readonly ViewContext viewContext;
        protected string pageQueryName = "page";
        protected bool showTotalSummary;
        protected bool showPagerItems = true;
        protected bool showFirst = true;
        protected bool showPrevious = true;
        protected bool showNext = true;
        protected bool showLast = true;
        protected bool showIndividualPages = true;
        protected int individualPagesDisplayedCount = 5;
        protected Func<int, string> urlBuilder;
        protected IList<string> booleanParameterNames;

		public Pager(IPageableModel model, ViewContext context)
		{
            this.model = model;
            this.viewContext = context;
            this.urlBuilder = CreateDefaultUrl;
            this.booleanParameterNames = new List<string>();
		}

		protected ViewContext ViewContext 
		{
			get { return viewContext; }
		}
        
        public Pager QueryParam(string value)
		{
            this.pageQueryName = value;
			return this;
		}
        public Pager ShowTotalSummary(bool value)
        {
            this.showTotalSummary = value;
            return this;
        }
        public Pager ShowPagerItems(bool value)
        {
            this.showPagerItems = value;
            return this;
        }
        public Pager ShowFirst(bool value)
        {
            this.showFirst = value;
            return this;
        }
        public Pager ShowPrevious(bool value)
        {
            this.showPrevious = value;
            return this;
        }
        public Pager ShowNext(bool value)
        {
            this.showNext = value;
            return this;
        }
        public Pager ShowLast(bool value)
        {
            this.showLast = value;
            return this;
        }
        public Pager ShowIndividualPages(bool value)
        {
            this.showIndividualPages = value;
            return this;
        }
        public Pager IndividualPagesDisplayedCount(int value)
        {
            this.individualPagesDisplayedCount = value;
            return this;
        }
		public Pager Link(Func<int, string> value)
		{
            this.urlBuilder = value;
			return this;
		}
        //little hack here due to ugly MVC implementation
        //find more info here: http://www.mindstorminteractive.com/blog/topics/jquery-fix-asp-net-mvc-checkbox-truefalse-value/
        public Pager BooleanParameterName(string paramName)
        {
            booleanParameterNames.Add(paramName);
            return this;
        }

        public override string ToString()
        {
            return ToHtmlString();
        }
		public string ToHtmlString()
		{
            if (model.TotalItems == 0) 
				return null;

            var links = new StringBuilder();

            if (showTotalSummary && (model.TotalPages > 0))
            {
                //<span class="total">共有<strong>1</strong>条</span><span class="current">1</span>
                //links.Append(string.Format("页 {0} of {1} (共{2}记录)", model.PageIndex + 1, model.TotalPages, model.TotalItems));
                links.Append(string.Format("<span class=\"total\">共<strong>{0}</strong>记录</span>", model.TotalItems));

                //links.Append("&nbsp;");
            }
            if (showPagerItems && (model.TotalPages > 1))
            {
                if (showFirst)
                {
                    if ((model.PageIndex >= 3) && (model.TotalPages > individualPagesDisplayedCount))
                    {
                        //if (showIndividualPages)
                        //{
                        //    links.Append("&nbsp;");
                        //}

                        links.Append(CreatePageLink(1, "第一页"));

                        //if ((showIndividualPages || (showPrevious && (model.PageIndex > 0))) || showLast)
                        //{
                        //    links.Append("&nbsp;...&nbsp;");
                        //}
                    }
                }
                if (showPrevious)
                {
                    if (model.PageIndex > 0)
                    {
                        links.Append(CreatePageLink(model.PageIndex, "上一页"));

                        //if ((showIndividualPages || showLast) || (showNext && ((model.PageIndex + 1) < model.TotalPages)))
                        //{
                        //    links.Append("&nbsp;");
                        //}
                    }
                }
                if (showIndividualPages)
                {
                    int firstIndividualPageIndex = GetFirstIndividualPageIndex();
                    int lastIndividualPageIndex = GetLastIndividualPageIndex();
                    for (int i = firstIndividualPageIndex; i <= lastIndividualPageIndex; i++)
                    {
                        if (model.PageIndex == i)
                        {
                            links.AppendFormat("<li class=\"disabled\"><a href=\"javascript:void(0);\"> {0} </a> </li>", (i + 1));
                        }
                        else
                        {
                            links.Append(CreatePageLink(i + 1, (i + 1).ToString()));
                        }
                        //if (i < lastIndividualPageIndex)
                        //{
                        //    links.Append("&nbsp;");
                        //}
                    }
                }
                if (showNext)
                {
                    if ((model.PageIndex + 1) < model.TotalPages)
                    {
                        //if (showIndividualPages)
                        //{
                        //    links.Append("&nbsp;");
                        //}

                        links.Append(CreatePageLink(model.PageIndex + 2, "下一页"));
                    }
                }
                if (showLast)
                {
                    if (((model.PageIndex + 3) < model.TotalPages) && (model.TotalPages > individualPagesDisplayedCount))
                    {
                        if (showIndividualPages || (showNext && ((model.PageIndex + 1) < model.TotalPages)))
                        {
                            //links.Append("&nbsp;...&nbsp;");
                            links.Append("...");
                        }

                        links.Append(CreatePageLink(model.TotalPages, "末页"));
                    }
                }
            }
			return links.ToString();
		}

        protected virtual int GetFirstIndividualPageIndex()
        {
            if ((model.TotalPages < individualPagesDisplayedCount) ||
                ((model.PageIndex - (individualPagesDisplayedCount / 2)) < 0))
            {
                return 0;
            }
            if ((model.PageIndex + (individualPagesDisplayedCount / 2)) >= model.TotalPages)
            {
                return (model.TotalPages - individualPagesDisplayedCount);
            }
            return (model.PageIndex - (individualPagesDisplayedCount / 2));
        }

        protected virtual int GetLastIndividualPageIndex()
        {
            int num = individualPagesDisplayedCount / 2;
            if ((individualPagesDisplayedCount % 2) == 0)
            {
                num--;
            }
            if ((model.TotalPages < individualPagesDisplayedCount) ||
                ((model.PageIndex + num) >= model.TotalPages))
            {
                return (model.TotalPages - 1);
            }
            if ((model.PageIndex - (individualPagesDisplayedCount / 2)) < 0)
            {
                return (individualPagesDisplayedCount - 1);
            }
            return (model.PageIndex + num);
        }
		protected virtual string CreatePageLink(int pageNumber, string text)
		{
			var builder = new TagBuilder("a");
			builder.SetInnerText(text);
			builder.MergeAttribute("href", urlBuilder(pageNumber));
			return "<li>"+ builder.ToString(TagRenderMode.Normal)+"</li>";
		}

        protected virtual string CreateDefaultUrl(int pageNumber)
		{
			var routeValues = new RouteValueDictionary();

			foreach (var key in viewContext.RequestContext.HttpContext.Request.QueryString.AllKeys.Where(key => key != null))
			{
                var value = viewContext.RequestContext.HttpContext.Request.QueryString[key];
                if (booleanParameterNames.Contains(key, StringComparer.InvariantCultureIgnoreCase))
                {
                    //little hack here due to ugly MVC implementation
                    //find more info here: http://www.mindstorminteractive.com/blog/topics/jquery-fix-asp-net-mvc-checkbox-truefalse-value/
                    if (!String.IsNullOrEmpty(value) && value.Equals("true,false", StringComparison.InvariantCultureIgnoreCase))
                    {
                        value = "true";
                    }
                }
				routeValues[key] = value;
			}

			routeValues[pageQueryName] = pageNumber;

            var routes = RouteTable.Routes;
            var context = viewContext.RequestContext;
            var url = UrlHelper.GenerateUrl(null, null, null, routeValues, routes, context, true);
			return url;
		}
    }
}
