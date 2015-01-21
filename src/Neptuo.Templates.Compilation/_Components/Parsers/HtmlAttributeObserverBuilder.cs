﻿using Neptuo.Linq.Expressions;
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
        private readonly string propertyName;

        /// <summary>
        /// Creates new instance for required interface <paramref name="requiredInterface"/> 
        /// and <paramref name="propertyName"/> as <see cref="Dictionary<string, string>"/> property to set HTML attributes to.
        /// </summary>
        /// <param name="requiredInterface">Required interface on type code object.</param>
        /// <param name="propertyName"><see cref="Dictionary<string, string>"/> property to set HTML attributes to.</param>
        public HtmlAttributeObserverBuilder(Type requiredInterface, string propertyName)
        {
            Guard.NotNull(requiredInterface, "requiredInterface");
            Guard.NotNullOrEmpty(propertyName, "propertyName");
            this.requiredInterface = requiredInterface;
            this.propertyName = propertyName;
        }

        private DictionaryAddCodeProperty CreateCodeProperty(ITypeCodeObject typeCodeObject)
        {
            TypePropertyInfo propertyInfo = new TypePropertyInfo(typeCodeObject.Type.GetProperty(propertyName));
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