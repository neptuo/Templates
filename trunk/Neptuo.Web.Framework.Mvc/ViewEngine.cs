﻿using Neptuo.Web.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Neptuo.Web.Framework.Compilation;
using System.Web;

namespace Neptuo.Web.Framework.Mvc
{
    /// <summary>
    /// Html view engine.
    /// </summary>
    public class ViewEngine : VirtualPathProviderViewEngine, IVirtualPathProvider
    {
        private IExtendedServiceProvider serviceProvider;

        public ViewEngine(IExtendedServiceProvider serviceProvider)
        {
            base.ViewLocationFormats = new string[] { "~/Views/{1}/{0}.html" };
            base.PartialViewLocationFormats = base.ViewLocationFormats;
            this.serviceProvider = serviceProvider;
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return new View(serviceProvider.GetService<IViewService>(), serviceProvider, partialPath);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new View(serviceProvider.GetService<IViewService>(), serviceProvider, viewPath, masterPath);
        }

        public string MapPath(string path)
        {
            return HttpContext.Current.Server.MapPath(path);
        }
    }
}
