using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.Framework.Controls;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Neptuo.Web.Framework.Mvc.Controls
{
    [Html("a")]
    public class ActionControl : BaseContentControl
    {
        private IRequestHelper requestHelper;
        private IVirtualUrlProvider urlProvider;
        private UrlHelper urlHelper;

        public string Controller { get; set; }
        public string Action { get; set; }
        public ICollection<Parameter> Parameters { get; set; }

        public ActionControl(IRequestHelper requestHelper, IVirtualUrlProvider urlProvider, UrlHelper urlHelper)
        {
            this.requestHelper = requestHelper;
            this.urlProvider = urlProvider;
            this.urlHelper = urlHelper;
        }

        public override void OnInit()
        {
            ComponentManager.Init(Parameters);

            RouteValueDictionary parameters = new RouteValueDictionary();
            if (Parameters != null)
            {
                foreach (Parameter parameter in Parameters)
                {
                    ComponentManager.Init(parameter);
                    parameters.Add(parameter.Name, parameter.Value);
                }
            }

            string url = "~" + urlHelper.Action(Action, Controller, parameters);
            Attributes["href"] = urlProvider.ResolveUrl(url);

            if (requestHelper.AppRelativeCurrentExecutionFilePath == url)
                Attributes["class"] = "active";

            base.OnInit();
        }
    }
}
