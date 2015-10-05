using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.Descriptors;
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
        /// Should create property descriptor for <paramref name="fieldDescriptor"/>.
        /// </summary>
        protected abstract ICodeProperty CreateCodeProperty(IFieldDescriptor fieldDescriptor);

        public IEnumerable<ICodeProperty> TryParse(IContentPropertyBuilderContext context, IEnumerable<IXmlNode> content)
        {
            if (typeof(string) == context.FieldDescriptor.FieldType)
            {
                //Get string and add as plain value
                StringBuilder contentValue = new StringBuilder();
                foreach (IXmlNode node in content)
                    contentValue.Append(node.OuterXml);

                ICodeProperty codeProperty = CreateCodeProperty(context.FieldDescriptor);
                codeProperty.SetValue(new LiteralCodeObject(contentValue.ToString()));
                return new CodePropertyCollection(codeProperty);
            }
            else if (typeof(IEnumerable).IsAssignableFrom(context.FieldDescriptor.FieldType))
            {
                //Collection item
                ICodeProperty codeProperty = CreateCodeProperty(context.FieldDescriptor);
                bool isTextAssignable = codeProperty.Type.IsAssignableFrom(typeof(string));
                foreach (IXmlNode node in content)
                {
                    if (!isTextAssignable && node.NodeType == XmlNodeType.Text)
                    {
                        string textValue = ((IXmlText)node).Text;
                        if (String.IsNullOrEmpty(textValue) || String.IsNullOrWhiteSpace(textValue))
                            continue;
                    }

                    IEnumerable<ICodeObject> values = context.Parser.TryProcessNode(context.BuilderContext, node);
                    if (values != null)
                        codeProperty.SetRangeValue(values);
                }

                return new CodePropertyCollection(codeProperty);
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
                        ICodeProperty codeProperty = CreateCodeProperty(context.FieldDescriptor);
                        foreach (IXmlElement node in elements)
                        {
                            IEnumerable<ICodeObject> values = context.Parser.TryProcessNode(context.BuilderContext, node);
                            if (values != null)
                            {
                                //TODO: Here MUST be only one value!
                                codeProperty.SetRangeValue(values);
                                return new CodePropertyCollection().Add(codeProperty);
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

                    ICodeProperty codeProperty = CreateCodeProperty(context.FieldDescriptor);
                    codeProperty.SetValue(new LiteralCodeObject(contentValue.ToString()));
                    return new CodePropertyCollection(codeProperty);
                }
            }
        }

        public IEnumerable<ICodeProperty> TryParse(IPropertyBuilderContext context, ISourceContent value)
        {
            ICodeObject propertyValue = context.ParserService.ProcessValue(context.Name, value, context);
            if (propertyValue != null)
            {
                ICodeProperty codeProperty = CreateCodeProperty(context.FieldDescriptor);
                codeProperty.SetValue(propertyValue);
                return new CodePropertyCollection(codeProperty);
            }

            return null;
        }
    }
}
