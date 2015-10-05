﻿using Neptuo.Linq.Expressions;
using Neptuo.Models.Features;
using Neptuo.Templates.Compilation;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.CodeObjects.Features;
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
            TemplateCodeObject templateCodeObject = new TemplateCodeObject();
            templateCodeObject
                .Add<ITypeCodeObject>(new TypeCodeObject(typeof(ContentTemplate)))
                .Add<IFieldCollectionCodeObject>(new FieldCollectionCodeObject());

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
            templateCodeObject.With<IFieldCollectionCodeObject>().AddProperty(codeProperty);

            ICodeProperty resultProperty = new SetCodeProperty(context.FieldDescriptor.Name, context.FieldDescriptor.FieldType);
            resultProperty.SetValue(templateCodeObject);
            return new CodePropertyCollection(resultProperty);
        }

        public IEnumerable<ICodeProperty> TryParse(IPropertyBuilderContext context, ISourceContent value)
        {
            TemplateCodeObject templateCodeObject = new TemplateCodeObject();
            templateCodeObject
                .Add<ITypeCodeObject>(new TypeCodeObject(typeof(ContentTemplate)))
                .Add<IFieldCollectionCodeObject>(new FieldCollectionCodeObject());

            PropertyInfo propertyInfo = typeof(FileTemplate).GetProperty(TypeHelper.PropertyName<FileTemplate, string>(t => t.Path));

            ICodeProperty pathProperty = new SetCodeProperty(propertyInfo.Name, propertyInfo.PropertyType);
            pathProperty.SetValue(new LiteralCodeObject(value.TextContent));
            templateCodeObject.With<IFieldCollectionCodeObject>().AddProperty(pathProperty);

            ICodeProperty codeProperty = new SetCodeProperty(context.FieldDescriptor.Name, context.FieldDescriptor.FieldType);
            codeProperty.SetValue(templateCodeObject);
            return new CodePropertyCollection(codeProperty);
        }
    }
}
