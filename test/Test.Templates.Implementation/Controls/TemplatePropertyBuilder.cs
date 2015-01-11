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
    public class TemplatePropertyBuilder : IPropertyBuilder
    {
        public bool TryParse(IContentBuilderContext context, IPropertiesCodeObject codeObject, IPropertyInfo propertyInfo, IEnumerable<IXmlNode> content)
        {
            IComponentCodeObject templateCodeObject = new TemplateCodeObject(typeof(ContentTemplate));
            codeObject.Properties.Add(new SetPropertyDescriptor(propertyInfo, templateCodeObject));

            IPropertyInfo targetProperty = new TypePropertyInfo(
                typeof(ContentTemplateContent).GetProperty(
                    TypeHelper.PropertyName<ContentTemplateContent, object>(t => t.Content)
                )
            );

            //Collection item
            IPropertyDescriptor propertyDescriptor = new ListAddPropertyDescriptor(targetProperty);
            IEnumerable<ICodeObject> values = context.TryProcessContentNodes(content);
            if (values == null)
                return false;

            propertyDescriptor.SetRangeValue(values);
            templateCodeObject.Properties.Add(propertyDescriptor);
            return true;
        }

        public bool TryParse(IContentBuilderContext context, IPropertiesCodeObject codeObject, IPropertyInfo propertyInfo, ISourceContent value)
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

            IPropertyDescriptor propertyDescriptor = new SetPropertyDescriptor(propertyInfo);
            propertyDescriptor.SetValue(templateCodeObject);
            codeObject.Properties.Add(propertyDescriptor);

            return true;
        }
    }
}
