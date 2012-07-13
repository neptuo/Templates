using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Reflection;
using Neptuo.Web.Html;
using Neptuo.Web.Html.Annotations;
using Neptuo.Web.Html.Compilation;
using Neptuo.Web.Html.Controls.HandlerSupport;
using Neptuo.Web.Html.Utils;
using Neptuo.Web.Mvc.ViewEngine;
using Neptuo.Web.Parser;

namespace Neptuo.Web.Mvc.ViewEngine.Utils
{
    /// <summary>
    /// Helper to resolve tag registered by IControl.
    /// </summary>
    internal class EvaluationHelper
    {
        public HtmlEvaluator Evaluator { get; set; }

        public IViewHandler ViewHandler { get; set; }

        public EvaluationHelper(HtmlEvaluator evaluator, IViewHandler viewHandler)
        {
            Evaluator = evaluator;
            ViewHandler = viewHandler;
        }

        public ITypeHandler EvaluateTag(ITag tag, Type controlType)
        {
            ITypeHandler handler = ViewHandler.CreateTypeHandler();
            handler.Type = controlType;
   
            BindProperties(tag, handler, controlType);
            
            return handler;
        }

        private void BindProperties(ITag tag, ITypeHandler instance, Type controlType)
        {
            List<string> boundProperties = new List<string>();

            PropertyInfo defaultProperty = null;
            DefaultPropertyAttribute defPropAttr = ReflectionHelper.GetAttribute<DefaultPropertyAttribute>(controlType);
            if (defPropAttr != null)
                defaultProperty = controlType.GetProperty(defPropAttr.Name);

            PropertyInfo[] properties = controlType.GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                string propertyName = prop.Name.ToLowerInvariant();
                ControlPropertyAttribute controlAttribute = ControlPropertyAttribute.GetAttribute(prop);
                if (controlAttribute != null && controlAttribute.Name != null)
                    propertyName = controlAttribute.Name;


                IAttribute attribute = tag.Attributes.FirstOrDefault(a => a.Name.ToLowerInvariant() == propertyName);
                if (attribute != null)
                {
                    if (!boundProperties.Contains(prop.Name))
                    {
                        //Bind property using tag attribute
                        ResolvePropertyValue(prop, instance, attribute.Value);
                        boundProperties.Add(prop.Name);
                    }
                    else
                    {
                        //TODO: Throw exception, "Property already bound"
                    }
                }
                else if (!tag.IsSelfClosing)
                {
                    ITag child = (ITag)tag.Childs.FirstOrDefault(e => (e is ITag) && (e as ITag).FullName.ToLowerInvariant() == propertyName);
                    if (child != null)
                    {
                        if (!boundProperties.Contains(prop.Name))
                        {
                            //Bind property using child
                            ResolvePropertyValue(prop, instance, child);
                            boundProperties.Add(prop.Name);
                        }
                        else
                        {
                            //TODO: Throw exception, "Property already bound"
                        }
                    }
                }
            }

            //TODO: Solve defaultProperty
            if (defaultProperty != null && !boundProperties.Contains(defaultProperty.Name))
            {
                //Solve defaultProperty
                ResolvePropertyValue(defaultProperty, instance, tag, (el) => !(el is ITag) || ((el is ITag) && !boundProperties.Contains((el as ITag).FullName)));
                boundProperties.Add(defaultProperty.Name);

                //foreach (IElement item in tag.Childs)
                //{
                //    ITag child = item as ITag;
                //    if (child != null && !boundProperties.Contains(tag.Name))
                //    {
                //        //Add to default property
                //    }
                //}
            }
        }

        private void ResolvePropertyValue(PropertyInfo prop, ITypeHandler handler, string value)
        {
            //TODO: Implement
            //prop.SetValue(instance, Evaluator.EvaluateAttributeValue(value, prop.PropertyType), null);
            handler.BindProperty(prop, Evaluator.EvaluateAttributeValue(value, prop.PropertyType));
        }

        private void ResolvePropertyValue(PropertyInfo prop, ITypeHandler handler, ITag child, Func<IElement, bool> childSelector = null)
        {
            if (ReflectionHelper.IsGenericType<IList, object>(prop.PropertyType))
            {
                //object target = Activator.CreateInstance(prop.PropertyType);

                foreach (IElement childChild in child.Childs)
                {
                    ITypeHandler control = Evaluator.Evaluate(childChild);
                    if ((childSelector == null || childSelector(childChild)) && control != null && ReflectionHelper.IsGenericType(prop.PropertyType, typeof(IList), control.Type))
                        handler.BindPropertyAddToCollection(prop, ViewHandler.CreatePropertyValue(control));
                }

                //handler.BindProperty(prop, ViewHandler.CreatePropertyValue(target));
            }
            else if (prop.PropertyType == typeof(String))
            {
                //TODO: Accept any primitive type
                ResolvePropertyValue(prop, handler, child.ToStringBody());
            }
            else if (child.Childs.Count == 1)
            {
                //TODO: Do it nicely
                handler.BindProperty(prop, ViewHandler.CreatePropertyValue(Evaluator.Evaluate(child.Childs[0])));
            }
            else
            {
                //TODO: Thrown exception, no way to resolve
            }
        }



        public ITypeHandler EvaluateLiteral(ILiteral literal)
        {
            ITypeHandler handler = ViewHandler.CreateTypeHandler();
            handler.Type = typeof(GenericLiteralControl);
            handler.BindProperty(handler.Type.GetProperty("TextContent"), ViewHandler.CreatePropertyValue(literal.ToString()));
            return handler;
        }

        public ITypeHandler EvaluateDocType(IDocType docType)
        {
            ITypeHandler handler = ViewHandler.CreateTypeHandler();
            handler.Type = typeof(GenericLiteralControl);
            handler.BindProperty(handler.Type.GetProperty("TextContent"), ViewHandler.CreatePropertyValue(docType.ToString()));
            return handler;
        }
    }
}
