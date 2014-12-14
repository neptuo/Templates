using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Observer which sets attribute to <see cref="IHtmlAttributeCollectionAware"/>.
    /// </summary>
    public class HtmlAttributeObserverBuilder : IObserverBuilder
    {
        private static string htmlAttributesPropertyName = TypeHelper.PropertyName<IHtmlAttributeCollectionAware, HtmlAttributeCollection>(c => c.HtmlAttributes);

        private static DictionaryAddPropertyDescriptor CreatePropertyDescriptor(ITypeCodeObject typeCodeObject)
        {
            TypePropertyInfo propertyInfo = new TypePropertyInfo(typeCodeObject.Type.GetProperty(htmlAttributesPropertyName));
            DictionaryAddPropertyDescriptor propertyDescriptor = new DictionaryAddPropertyDescriptor(propertyInfo);
            return propertyDescriptor;
        }

        public bool TryParse(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlAttribute attribute)
        {
            ITypeCodeObject typeCodeObject = codeObject as ITypeCodeObject;
            if (typeCodeObject == null)
                return false;

            if (typeof(IHtmlAttributeCollectionAware).IsAssignableFrom(typeCodeObject.Type))
            {
                DictionaryAddPropertyDescriptor propertyDescriptor = CreatePropertyDescriptor(typeCodeObject);
                propertyDescriptor.SetValue(new PlainValueCodeObject(attribute.Name));

                ICodeObject value = context.ProcessValue(attribute.GetValue());
                if (value != null)
                {
                    propertyDescriptor.SetValue(value);
                    codeObject.Properties.Add(propertyDescriptor);
                    return true;
                }
            }
            return false;
        }
    }
}
