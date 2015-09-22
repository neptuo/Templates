using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
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
            ICodeProperty codeProperty = new ListAddCodeProperty(targetProperty);
            IEnumerable<ICodeObject> values = context.BuilderContext.TryProcessContentNodes(content);
            if (values == null)
                return null;

            codeProperty.SetRangeValue(values);
            templateCodeObject.Properties.Add(codeProperty);
            return new CodePropertyList(new SetCodeProperty(context.PropertyInfo, templateCodeObject));
        }

        public IEnumerable<ICodeProperty> TryParse(IPropertyBuilderContext context, ISourceContent value)
        {
            XComponentCodeObject templateCodeObject = new XComponentCodeObject(typeof(FileTemplate));
            templateCodeObject.Properties.Add(
                new SetCodeProperty(
                    new TypePropertyInfo(
                        typeof(FileTemplate).GetProperty(TypeHelper.PropertyName<FileTemplate, string>(t => t.Path))
                    ),
                    new PlainValueCodeObject(value.TextContent)
                )
            );

            ICodeProperty codeProperty = new SetCodeProperty(context.PropertyInfo);
            codeProperty.SetValue(templateCodeObject);
            return new CodePropertyList(codeProperty);
        }
    }
}
