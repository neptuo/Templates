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
        private readonly Type requiredInterface;
        private readonly string methodName;

        public HtmlAttributeObserverBuilder(Type requiredInterface, string methodName)
        {
            Guard.NotNull(requiredInterface, "requiredInterface");
            Guard.NotNullOrEmpty(methodName, "methodName");
            this.requiredInterface = requiredInterface;
            this.methodName = methodName;
        }

        private DictionaryAddCodeProperty CreateCodeProperty(ITypeCodeObject typeCodeObject)
        {
            TypePropertyInfo propertyInfo = new TypePropertyInfo(typeCodeObject.Type.GetProperty(methodName));
            DictionaryAddCodeProperty codeProperty = new DictionaryAddCodeProperty(propertyInfo);
            return codeProperty;
        }

        public bool TryParse(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlAttribute attribute)
        {
            ITypeCodeObject typeCodeObject = codeObject as ITypeCodeObject;
            if (typeCodeObject == null)
                return false;

            if (requiredInterface.IsAssignableFrom(typeCodeObject.Type))
            {
                DictionaryAddCodeProperty codeProperty = CreateCodeProperty(typeCodeObject);
                codeProperty.SetValue(new PlainValueCodeObject(attribute.Name));

                ICodeObject value = context.TryProcessValue(attribute.GetValue());
                if (value != null)
                {
                    codeProperty.SetValue(value);
                    codeObject.Properties.Add(codeProperty);
                    return true;
                }
            }
            return false;
        }
    }
}
