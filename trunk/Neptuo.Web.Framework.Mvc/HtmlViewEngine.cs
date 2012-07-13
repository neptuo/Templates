using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Neptuo.Web.Html;
using Neptuo.Web.Html.Configuration;
using Neptuo.Web.Html.Utils;
using Neptuo.Web.Html.Annotations;

namespace Neptuo.Web.Mvc.ViewEngine
{
    /// <summary>
    /// Html view engine.
    /// </summary>
    public class HtmlViewEngine : VirtualPathProviderViewEngine, IRegistrator
    {
        public Configuration Configuration { get; protected set; }

        public HtmlViewEngine()
        {
            base.ViewLocationFormats = new string[] { "~/Views/{1}/{0}.html" };
            base.PartialViewLocationFormats = base.ViewLocationFormats;
            
            Configuration = new Configuration();
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return new HtmlView(Configuration, partialPath);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new HtmlView(Configuration, viewPath, masterPath);
        }

        #region IRegistrator

        public void RegisterNamespace(string prefix, string newNamespace)
        {
            if (prefix == null)
                prefix = String.Empty;

            if (!Configuration.Namespaces.ContainsKey(prefix))
                Configuration.Namespaces.Add(prefix, new List<string>());

            Configuration.Namespaces[prefix].Add(newNamespace);

            if (!Configuration.TypesInNamespaces.ContainsKey(prefix))
                Configuration.TypesInNamespaces.Add(prefix, new Dictionary<string, Type>());

            foreach (Type type in ReflectionHelper.GetTypesInNamespace(newNamespace))
            {
                if (ReflectionHelper.CanBeUsedInMarkup(type))
                {
                    ControlAttribute controlAttr = ControlAttribute.GetAttribute(type);
                    
                    string controlName = type.Name.ToLowerInvariant();
                    if (controlName.EndsWith("control"))
                        controlName = controlName.Substring(0, controlName.Length - 7);
                    else if (controlName.EndsWith("extension"))
                        controlName = controlName.Substring(0, controlName.Length - 9);

                    if (controlAttr != null)
                        controlName = controlAttr.Name;

                    Configuration.TypesInNamespaces[prefix][controlName] = type;
                }
            }
        }

        public void RegisterHandler(string prefix, IHandler handler)
        {
            if (prefix == null)
                prefix = String.Empty;

            if (!Configuration.Handlers.ContainsKey(prefix))
                Configuration.Handlers.Add(prefix, new List<IHandler>());

            Configuration.Handlers[prefix].Add(handler);
        }

        public void RegisterHandler(string prefix, IAttributeHandler handler)
        {
            if (!Configuration.AttributeHandlers.ContainsKey(prefix))
                Configuration.AttributeHandlers.Add(prefix, new List<IAttributeHandler>());

            Configuration.AttributeHandlers[prefix].Add(handler);
        }

        public void RegisterStatic(string prefix, Type type)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
