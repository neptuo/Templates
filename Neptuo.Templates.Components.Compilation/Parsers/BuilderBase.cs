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
    //TODO: Realize using observer!



    /// <summary>
    /// Shared builder helper.
    /// </summary>
    public static class BuilderBase
    {
        private static string htmlAttributesPropertyName = TypeHelper.PropertyName<IHtmlAttributeCollectionAware, HtmlAttributeCollection>(c => c.HtmlAttributes);

        private static DictionaryAddPropertyDescriptor CreatePropertyDescriptor(ITypeCodeObject typeCodeObject)
        {
            TypePropertyInfo propertyInfo = new TypePropertyInfo(typeCodeObject.Type.GetProperty(htmlAttributesPropertyName));
            DictionaryAddPropertyDescriptor propertyDescriptor = new DictionaryAddPropertyDescriptor(propertyInfo);
            return propertyDescriptor;
        }

        /// <summary>
        /// Binds attribute <paramref name="name"/> to component of type <see cref="IHtmlAttributeCollectionAware"/> and parses <paramref name="value"/> using value parsers.
        /// </summary>
        public static bool BindAttributeCollection(IContentBuilderContext context, ITypeCodeObject typeCodeObject, IPropertiesCodeObject propertiesCodeObject, string name, ISourceContent value)
        {
            if (typeof(IHtmlAttributeCollectionAware).IsAssignableFrom(typeCodeObject.Type))
            {
                DictionaryAddPropertyDescriptor propertyDescriptor = CreatePropertyDescriptor(typeCodeObject);
                propertyDescriptor.SetValue(new PlainValueCodeObject(name));

                ICodeObject valueObject = context.ProcessValue(value);

                if (valueObject != null)
                {
                    propertyDescriptor.SetValue(valueObject);
                    propertiesCodeObject.Properties.Add(propertyDescriptor);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Binds attribute <paramref name="name"/> to component of type <see cref="IHtmlAttributeCollectionAware"/> and parses <paramref name="value"/>.
        /// </summary>
        public static bool BindAttributeCollection(IContentBuilderContext context, ITypeCodeObject typeCodeObject, IPropertiesCodeObject propertiesCodeObject, string name, ICodeObject value)
        {
            if (typeof(IHtmlAttributeCollectionAware).IsAssignableFrom(typeCodeObject.Type))
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
