using Neptuo.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebMvc
{
    public class HttpContextRequestHelper : IRequestHelper
    {
        public string ApplicationPath
        {
            get { return HttpContext.Current.Request.ApplicationPath; }
        }

        public string AppRelativeCurrentExecutionFilePath
        {
            get { return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath; }
        }
    }
}