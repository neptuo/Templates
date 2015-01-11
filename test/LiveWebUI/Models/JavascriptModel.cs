using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiveWebUI.Models
{
    public class JavascriptModel
    {
        public MvcHtmlString ViewSource { get; private set; }
        public MvcHtmlString ClassFullName { get; private set; }

        public JavascriptModel(string viewSource, string classFullName)
        {
            ViewSource = new MvcHtmlString(viewSource);
            ClassFullName = new MvcHtmlString(classFullName);
        }
    }
}