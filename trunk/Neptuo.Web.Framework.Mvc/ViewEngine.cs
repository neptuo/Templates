using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Neptuo.Web.Framework.Compilation;

namespace Neptuo.Web.Framework.Mvc
{
    /// <summary>
    /// Html view engine.
    /// </summary>
    public class ViewEngine : VirtualPathProviderViewEngine
    {
        private StandartCodeCompiler compiler;

        public ViewEngine()
        {
            base.ViewLocationFormats = new string[] { "~/Views/{1}/{0}.html" };
            base.PartialViewLocationFormats = base.ViewLocationFormats;

            compiler = new StandartCodeCompiler();
            compiler.GeneratedCodeFolder = @"C:\Temp\NeptuoFramework";
            compiler.Registrator.RegisterNamespace("h", "Neptuo.Web.Framework.Controls");
            compiler.Registrator.RegisterNamespace("h", "Neptuo.Web.Framework.Extensions");
            compiler.Registrator.RegisterObserver("ui", "visible", typeof(Neptuo.Web.Framework.Observers.VisibleObserver));
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return new View(compiler, partialPath);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new View(compiler, viewPath, masterPath);
        }
    }
}
