using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Test.Templates.Compilation.CodeObjects;
using Test.Templates.UI;

namespace Test.Templates.Compilation.Parsers
{
    public class TemplatePropertyBuilder : IContentPropertyBuilder, IPropertyBuilder
    {
        public IEnumerable<ICodeProperty> TryParse(IContentPropertyBuilderContext context, IEnumerable<IXmlNode> content)
        {
            TemplateCodeObject templateCodeObject = new TemplateCodeObject(typeof(ContentTemplate));
            IPropertyInfo targetProperty = new TypePropertyInfo(
                typeof(ContentTemplateContent).GetProperty(
                    TypeHelper.PropertyName<ContentTemplateContent, object>(t => t.Content)
                )
            );

            //Collection item
            ICodeProperty codeProperty = new AddCodeProperty(targetProperty.Name, targetProperty.Type);
            IEnumerable<ICodeObject> values = context.BuilderContext.TryProcessContentNodes(content);
            if (values == null)
                return null;

            codeProperty.SetRangeValue(values);
            templateCodeObject.Properties.Add(codeProperty);

            ICodeProperty resultProperty = new SetCodeProperty(context.PropertyInfo.Name, context.PropertyInfo.Type);
            resultProperty.SetValue(templateCodeObject);
            return new CodePropertyCollection(resultProperty);
        }

        public IEnumerable<ICodeProperty> TryParse(IPropertyBuilderContext context, ISourceContent value)
        {
            XComponentCodeObject templateCodeObject = new XComponentCodeObject(typeof(FileTemplate));

            PropertyInfo propertyInfo = typeof(FileTemplate).GetProperty(TypeHelper.PropertyName<FileTemplate, string>(t => t.Path));

            ICodeProperty pathProperty = new SetCodeProperty(propertyInfo.Name, propertyInfo.PropertyType);
            pathProperty.SetValue(new PlainValueCodeObject(value.TextContent));
            templateCodeObject.Properties.Add(pathProperty);

            ICodeProperty codeProperty = new SetCodeProperty(context.PropertyInfo.Name, context.PropertyInfo.Type);
            codeProperty.SetValue(templateCodeObject);
            return new CodePropertyCollection(codeProperty);
        }
    }
}
