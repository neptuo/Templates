using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Neptuo.Web.Html;
using Neptuo.Web.Html.Compilation;
using Neptuo.Web.Html.Utils;
using Neptuo.Web.Parser.MarkupExtension;

namespace Neptuo.Web.Mvc.ViewEngine
{
    public class HtmlExtensionEvaluator : IExtensionEvaluator
    {
        public Configuration Configuration { get; set; }

        public IViewHandler ViewHandler { get; set; }

        public HtmlExtensionEvaluator(Configuration configuration, IViewHandler viewHandler)
        {
            Configuration = configuration;
            ViewHandler = viewHandler;
        }

        public IMarkupExtensionHandler Evaluate(IExtension element)
        {
            string extensionNamespace = element.Namespace ?? String.Empty;
            if (extensionNamespace != null)
                extensionNamespace = extensionNamespace.ToLowerInvariant();

            string extensionName = element.Name.ToLowerInvariant();

            if (Configuration.TypesInNamespaces.ContainsKey(extensionNamespace)
                && Configuration.TypesInNamespaces[extensionNamespace].ContainsKey(extensionName))
            {
                Type controlType = Configuration.TypesInNamespaces[extensionNamespace][extensionName];
                IMarkupExtensionHandler result = ViewHandler.CreateMarkupExtensionHandler();
                result.Type = controlType;

                DefaultPropertyAttribute defProp = ReflectionHelper.GetAttribute<DefaultPropertyAttribute>(controlType);
                if (defProp != null)
                {
                    PropertyInfo prop = controlType.GetProperty(defProp.Name);
                    if (prop != null)
                        result.BindProperty(prop, ViewHandler.CreatePropertyValue(element.DefaultParam));
                    //TODO: else throw error! Badly defined extension!
                }

                //TODO: Register and use default param!
                foreach (KeyValuePair<string, string> val in element.Params)
                {
                    PropertyInfo prop = controlType.GetProperty(val.Key);
                    if (prop != null)
                        result.BindProperty(prop, ViewHandler.CreatePropertyValue(val.Value));
                    //TODO: else throw error!
                }

                return result;
            }

            return null;
        }
    }
}
