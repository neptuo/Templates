using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using Neptuo.Web.Html;
using Neptuo.Web.Html.Configuration;
using Neptuo.Web.Html.Compilation;
using Neptuo.Web.Html.Compilation.DirectCreation;
using Neptuo.Web.Html.Controls;
using Neptuo.Web.Html.Controls.HandlerSupport;
using Neptuo.Web.Html.Utils;
using Neptuo.Web.Html.Annotations;
using Neptuo.Web.Mvc.ViewEngine.Utils;
using Neptuo.Web.Parser;
using Neptuo.Web.Parser.Html;
using Neptuo.Web.Parser.MarkupExtension;

namespace Neptuo.Web.Mvc.ViewEngine
{
    /// <summary>
    /// TODO: Don't use here web.config configuration, transform it!
    /// + passed in registrated handler, etc.
    /// </summary>
    public class HtmlEvaluator : IEvaluator
    {
        private EvaluationHelper helper;

        public Configuration Configuration { get; set; }

        public ViewContext ViewContext { get; set; }

        public IViewHandler ViewHandler { get; set; }

        public IDependencyContainer Container { get; set; }

        public IExtensionEvaluator ExtensionEvaluator { get; set; }

        public HtmlEvaluator(Configuration configuration, ViewContext viewContext, IViewHandler viewHandler, IDependencyContainer container, IExtensionEvaluator extensionEvaluator)
        {
            Container = container;
            Configuration = configuration;
            ViewContext = viewContext;
            ViewHandler = viewHandler;
            ExtensionEvaluator = extensionEvaluator;

            helper = new EvaluationHelper(this, ViewHandler);
        }

        public ITypeHandler Evaluate(IElement element)
        {
            ITypeHandler result = null;

            //TODO: Objekty pro livecycle je nutné odchytávat jinak. Napsat vlastní activator, přes který se budou vytvářet všechny instance nových objektů.
            //      Vytvořené objekty v controlech by se mohli též registrovat do LivecycleObserver (vložený přes Dependecy) a rovnou volat i předchozí fáze podle té aktuální.
            //      V LivecycleObserver si pamatovat aktuální state.
            if (element is ILiteral)
                result = helper.EvaluateLiteral(element as ILiteral);
            else if (element is ITag)
                result = EvaluateTag(element as ITag);
            else if (element is IDocType)
                result = helper.EvaluateDocType(element as IDocType);

            return result;
        }

        public IPropertyValueHandler EvaluateAttributeValue(string value, Type targetType)
        {
            ExtensionParser parser = new ExtensionParser();
            IExtension extension = parser.Parse(value);
            if (extension != null)
            {
                IMarkupExtensionHandler result = ExtensionEvaluator.Evaluate(extension);
                return ViewHandler.CreatePropertyValue(result);
            }

            //TODO: Parse extensions
            if (targetType == typeof(Boolean))
            {
                return ViewHandler.CreatePropertyValue(Boolean.Parse(value));
            }
            else
            {
                return ViewHandler.CreatePropertyValue(TypeDescriptor.GetConverter(value.GetType()).ConvertTo(value, targetType));
            }
        }

        protected ITypeHandler EvaluateTag(ITag tag)
        {
            string tagNamespace = tag.Namespace ?? String.Empty;
            if (tagNamespace != null)
                tagNamespace = tagNamespace.ToLowerInvariant();
            string tagName = tag.Name.ToLowerInvariant();

            //Handle custom attributes
            for (int i = 0; i < tag.Attributes.Count; i++)
            {
                IAttribute attr = tag.Attributes[i];
                if (Configuration.AttributeHandlers.ContainsKey(attr.Namespace))
                {
                    foreach (IAttributeHandler attrHandler in Configuration.AttributeHandlers[attr.Namespace])
                    {
                        //TODO: AttributeHandlers ...
                        //bool? result = attrHandler.HandleAttribute(this, attr, tag);
                        //if (result == null)
                        //    return null;
                        //else if (result.Value)
                        //    break;
                    }
                }
            }

            if (Configuration.TypesInNamespaces.ContainsKey(tagNamespace)
                && Configuration.TypesInNamespaces[tagNamespace].ContainsKey(tagName))
            {
                Type controlType = Configuration.TypesInNamespaces[tagNamespace][tagName];
                return helper.EvaluateTag(tag, controlType);
            } 
            else if (Configuration.Handlers.ContainsKey(tagNamespace))
            {
                foreach (IHandler handler in Configuration.Handlers[tagNamespace])
                {
                    bool canHandle = handler.CanHandle(tag, tag.Attributes);
                    if (canHandle)
                    {
                        //Create compilation object
                        IMultiHandler multi = ViewHandler.CreateMultiHandler();
                        multi.Handler = handler;
                        multi.TagName = tag;
                        multi.Attributes = tag.Attributes;

                        //TODO: Eval attributes

                        //Resolve childs
                        foreach (IElement item in tag.Childs)
                            multi.BindChild(ViewHandler.CreatePropertyValue(Evaluate(item)));

                        return multi;
                    }
                }
            }
            
            return null;
        }
    }
}
