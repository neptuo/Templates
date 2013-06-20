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
        private UrlHelper urlHelper;

        public string Controller { get; set; }
        public string Action { get; set; }

        public FormControl(IVirtualUrlProvider urlProvider, UrlHelper urlHelper)
        {
            this.urlProvider = urlProvider;
            this.urlHelper = urlHelper;
        }

        public override void OnInit()
        {
            string url = "~" + urlHelper.Action(Action, Controller);
            Attributes["action"] = urlProvider.ResolveUrl(url);

            base.OnInit();
        }
    }
}
