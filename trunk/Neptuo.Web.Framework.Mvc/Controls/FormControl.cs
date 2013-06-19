using Neptuo.Web.Framework.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Neptuo.Web.Framework.Mvc.Controls
{
    [Html("form")]
    public class FormControl : BaseContentControl
    {
        private IVirtualUrlProvider urlProvider;

        public string RouteName { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

        public FormControl(IVirtualUrlProvider urlProvider)
        {
            this.urlProvider = urlProvider;
        }

        public override void OnInit()
        {
            string url = "~" + UrlHelper.GenerateUrl(RouteName, Action, Controller, null, RouteTable.Routes, HttpContext.Current.Request.RequestContext, false);
            Attributes["action"] = urlProvider.ResolveUrl(url);

            base.OnInit();
        }
    }
}
