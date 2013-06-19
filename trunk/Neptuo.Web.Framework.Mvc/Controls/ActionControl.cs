﻿using System;
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

        public string RouteName { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

        public ActionControl(IRequestHelper requestHelper, IVirtualUrlProvider urlProvider)
        {
            this.requestHelper = requestHelper;
            this.urlProvider = urlProvider;
        }

        public override void OnInit()
        {
            string url = "~" + UrlHelper.GenerateUrl(RouteName, Action, Controller, null, RouteTable.Routes, HttpContext.Current.Request.RequestContext, false);
            Attributes["href"] = urlProvider.ResolveUrl(url);

            if (requestHelper.AppRelativeCurrentExecutionFilePath == url)
                Attributes["class"] = "active";

            base.OnInit();
        }
    }
}
