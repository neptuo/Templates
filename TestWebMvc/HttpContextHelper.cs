using Neptuo.Web.Framework;
using Neptuo.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebMvc
{
    public class HttpContextHelper : IRequestHelper, IVirtualUrlProvider
    {
        private HttpContext httpContext;

        string IRequestHelper.ApplicationPath
        {
            get { return httpContext.Request.ApplicationPath; }
        }

        string IRequestHelper.AppRelativeCurrentExecutionFilePath
        {
            get { return httpContext.Request.AppRelativeCurrentExecutionFilePath; }
        }

        public HttpContextHelper()
        {
            httpContext = HttpContext.Current;
        }

        string IVirtualUrlProvider.ResolveUrl(string path)
        {
            return VirtualPathUtility.ToAbsolute(path);
        }
    }
}