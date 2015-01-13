using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.Controls
{
    public class TemplatePropertyBuilder : IContentPropertyBuilder, IPropertyBuilder
    {
        public IEnumerable<IPropertyDescriptor> TryParse(IContentPropertyBuilderContext context, IEnumerable<IXmlNode> content)
        {
            IComponentCodeObject templateCodeObject = new TemplateCodeObject(typeof(ContentTemplate));
            IPropertyInfo targetProperty = new TypePropertyInfo(
                typeof(ContentTemplateContent).GetProperty(
                    TypeHelper.PropertyName<ContentTemplateContent, object>(t => t.Content)
                )
            );

            //Collection item
            IPropertyDescriptor propertyDescriptor = new ListAddPropertyDescriptor(targetProperty);
            IEnumerable<ICodeObject> values = context.BuilderContext.TryProcessContentNodes(content);
            if (values == null)
                return null;

            propertyDescriptor.SetRangeValue(values);
            templateCodeObject.Properties.Add(propertyDescriptor);
            return new PropertyDescriptorList(new SetPropertyDescriptor(context.PropertyInfo, templateCodeObject));
        }

        public IEnumerable<IPropertyDescriptor> TryParse(IPropertyBuilderContext context, ISourceContent value)
        {
            IComponentCodeObject templateCodeObject = new ComponentCodeObject(typeof(FileTemplate));
            templateCodeObject.Properties.Add(
                new SetPropertyDescriptor(
                    new TypePropertyInfo(
                        typeof(FileTemplate).GetProperty(TypeHelper.PropertyName<FileTemplate, string>(t => t.Path))
                    ),
                    new PlainValueCodeObject(value.TextContent)
                )
            );

            IPropertyDescriptor propertyDescriptor = new SetPropertyDescriptor(context.PropertyInfo);
            propertyDescriptor.SetValue(templateCodeObject);
            return new PropertyDescriptorList(propertyDescriptor);
        }
    }
}
