using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Shared builder helper.
    /// </summary>
    public static class BaseBuilder
    {
        private static string htmlAttributesPropertyName = TypeHelper.PropertyName<IHasHtmlAttributeCollection, HtmlAttributeCollection>(c => c.HtmlAttributes);

        private static DictionaryAddPropertyDescriptor CreatePropertyDescriptor(ITypeCodeObject typeCodeObject)
        {
            TypePropertyInfo propertyInfo = new TypePropertyInfo(typeCodeObject.Type.GetProperty(htmlAttributesPropertyName));
            DictionaryAddPropertyDescriptor propertyDescriptor = new DictionaryAddPropertyDescriptor(propertyInfo);
            return propertyDescriptor;
        }

        /// <summary>
        /// Binds attribute <paramref name="name"/> to component of type <see cref="IHasHtmlAttributeCollection"/> and parses <paramref name="value"/> using value parsers.
        /// </summary>
        public static bool BindAttributeCollection(IContentBuilderContext context, ITypeCodeObject typeCodeObject, IPropertiesCodeObject propertiesCodeObject, string name, string value)
        {
            if (typeof(IHasHtmlAttributeCollection).IsAssignableFrom(typeCodeObject.Type))
            {
                DictionaryAddPropertyDescriptor propertyDescriptor = CreatePropertyDescriptor(typeCodeObject);
                propertyDescriptor.SetValue(new PlainValueCodeObject(name));

                bool result = context.ParserContext.ParserService.ProcessValue(
                    value,
                    new DefaultParserServiceContext(
                        context.ParserContext.DependencyProvider, 
                        propertyDescriptor, 
                        context.ParserContext.Errors
                    )
                );

                if (result)
                    propertiesCodeObject.Properties.Add(propertyDescriptor);

                //TODO: Else NOT result?
                return result;
            }
            return false;
        }

        /// <summary>
        /// Binds attribute <paramref name="name"/> to component of type <see cref="IHasHtmlAttributeCollection"/> and parses <paramref name="value"/>.
        /// </summary>
        public static bool BindAttributeCollection(IContentBuilderContext context, ITypeCodeObject typeCodeObject, IPropertiesCodeObject propertiesCodeObject, string name, ICodeObject value)
        {
            if (typeof(IHasHtmlAttributeCollection).IsAssignableFrom(typeCodeObject.Type))
            {
                DictionaryAddPropertyDescriptor propertyDescriptor = CreatePropertyDescriptor(typeCodeObject);
                propertyDescriptor.SetValue(new PlainValueCodeObject(name));
                propertyDescriptor.SetValue(value);
                propertiesCodeObject.Properties.Add(propertyDescriptor);

                //TODO: Else NOT result?
                return true;
            }
            return false;
        }
    }
}
