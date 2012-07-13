using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.Html;

namespace Neptuo.Web.Mvc.ViewEngine
{
    public interface IViewProcessor : ILivecycle
    {
        IViewPage LoadView(string viewName);

        IViewPage Load(string viewName, string viewContent);
    }
}
