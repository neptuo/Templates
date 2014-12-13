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
    public abstract class DefaultPropertyBuilder : IPropertyBuilder
    {
        /// <summary>
        /// Should create property descriptor for <paramref name="propertyInfo"/>.
        /// </summary>
        protected abstract IPropertyDescriptor CreatePropertyDescriptor(IPropertyInfo propertyInfo);

        public bool TryParse(IContentBuilderContext context, IPropertiesCodeObject codeObject, IPropertyInfo propertyInfo, IEnumerable<IXmlNode> content)
        {
            // TODO: All properties through IPropertyBuilder.
            if (typeof(string) == propertyInfo.Type)
            {
                //Get string and add as plain value
                StringBuilder contentValue = new StringBuilder();
                foreach (IXmlNode node in content)
                    contentValue.Append(node.OuterXml);

                IPropertyDescriptor propertyDescriptor = CreatePropertyDescriptor(propertyInfo);
                propertyDescriptor.SetValue(new PlainValueCodeObject(contentValue.ToString()));
                codeObject.Properties.Add(propertyDescriptor);
                return true;
            }
            else if (typeof(IEnumerable).IsAssignableFrom(propertyInfo.Type))
            {
                //Collection item
                IPropertyDescriptor propertyDescriptor = CreatePropertyDescriptor(propertyInfo);
                codeObject.Properties.Add(propertyDescriptor);

                foreach (IXmlNode node in content)
                {
                    ICodeObject valueObject = context.Parser.TryProcessNode(context, node);
                    propertyDescriptor.SetValue(valueObject);
                }

                return true;
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
                        IPropertyDescriptor propertyDescriptor = CreatePropertyDescriptor(propertyInfo);
                        codeObject.Properties.Add(propertyDescriptor);

                        foreach (IXmlElement node in elements)
                        {
                            ICodeObject valueObject = context.Parser.TryProcessNode(context, node);
                            propertyDescriptor.SetValue(valueObject);
                        }
                        return true;
                    }
                    else
                    {
                        IXmlElement element = elements.ElementAt(1);
                        context.AddError(element, "Property supports only single value.");
                        return false;
                    }
                }
                else
                {
                    //Get string and add as plain value
                    StringBuilder contentValue = new StringBuilder();
                    foreach (IXmlNode node in content)
                        contentValue.Append(node.OuterXml);

                    IPropertyDescriptor propertyDescriptor = CreatePropertyDescriptor(propertyInfo);
                    propertyDescriptor.SetValue(new PlainValueCodeObject(contentValue.ToString()));
                    codeObject.Properties.Add(propertyDescriptor);
                    return true;
                }
            }
        }

        public bool TryParse(IContentBuilderContext context, IPropertiesCodeObject codeObject, IPropertyInfo propertyInfo, ISourceContent attributeValue)
        {
            ICodeObject propertyValue = context.ProcessValue(attributeValue);
            if (propertyValue != null)
            {
                IPropertyDescriptor propertyDescriptor = CreatePropertyDescriptor(propertyInfo);
                propertyDescriptor.SetValue(propertyValue);
                codeObject.Properties.Add(propertyDescriptor);
                return true;
            }

            return false;
        }
    }
}
