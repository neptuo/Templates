using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Some magic logic to create right proproperty descriptors for right property types.
    /// </summary>
    public abstract class DefaultPropertyBuilder : IContentPropertyBuilder, IPropertyBuilder
    {
        /// <summary>
        /// Should create property descriptor for <paramref name="propertyInfo"/>.
        /// </summary>
        protected abstract IPropertyDescriptor CreatePropertyDescriptor(IPropertyInfo propertyInfo);

        public IEnumerable<IPropertyDescriptor> TryParse(IContentPropertyBuilderContext context, IEnumerable<IXmlNode> content)
        {
            if (typeof(string) == context.PropertyInfo.Type)
            {
                //Get string and add as plain value
                StringBuilder contentValue = new StringBuilder();
                foreach (IXmlNode node in content)
                    contentValue.Append(node.OuterXml);

                IPropertyDescriptor propertyDescriptor = CreatePropertyDescriptor(context.PropertyInfo);
                propertyDescriptor.SetValue(new PlainValueCodeObject(contentValue.ToString()));
                return new PropertyDescriptorList(propertyDescriptor);
            }
            else if (typeof(IEnumerable).IsAssignableFrom(context.PropertyInfo.Type))
            {
                //Collection item
                IPropertyDescriptor propertyDescriptor = CreatePropertyDescriptor(context.PropertyInfo);
                foreach (IXmlNode node in content)
                {
                    IEnumerable<ICodeObject> values = context.Parser.TryProcessNode(context.BuilderContext, node);
                    if (values != null)
                        propertyDescriptor.SetRangeValue(values);
                }

                return new PropertyDescriptorList(propertyDescriptor);
            }
            else
            {
                // Count elements
                IEnumerable<IXmlElement> elements = content.OfType<IXmlElement>();
                if (elements.Any())
                {
                    // One IXmlElement is ok
                    if (elements.Count() == 1)
                    {
                        IPropertyDescriptor propertyDescriptor = CreatePropertyDescriptor(context.PropertyInfo);
                        foreach (IXmlElement node in elements)
                        {
                            IEnumerable<ICodeObject> values = context.Parser.TryProcessNode(context.BuilderContext, node);
                            if (values != null)
                            {
                                //TODO: Here MUST be only one value!
                                propertyDescriptor.SetRangeValue(values);
                                return new PropertyDescriptorList().Add(propertyDescriptor);
                            }
                        }
                        return null;
                    }
                    else
                    {
                        IXmlElement element = elements.ElementAt(1);
                        context.AddError(element, "Property supports only single value.");
                        return null;
                    }
                }
                else
                {
                    //Get string and add as plain value
                    StringBuilder contentValue = new StringBuilder();
                    foreach (IXmlNode node in content)
                        contentValue.Append(node.OuterXml);

                    IPropertyDescriptor propertyDescriptor = CreatePropertyDescriptor(context.PropertyInfo);
                    propertyDescriptor.SetValue(new PlainValueCodeObject(contentValue.ToString()));
                    return new PropertyDescriptorList(propertyDescriptor);
                }
            }
        }

        public IEnumerable<IPropertyDescriptor> TryParse(IPropertyBuilderContext context, ISourceContent value)
        {
            ICodeObject propertyValue = context.ParserService.ProcessValue(value, context);
            if (propertyValue != null)
            {
                IPropertyDescriptor propertyDescriptor = CreatePropertyDescriptor(context.PropertyInfo);
                propertyDescriptor.SetValue(propertyValue);
                return new PropertyDescriptorList(propertyDescriptor);
            }

            return null;
        }
    }
}
