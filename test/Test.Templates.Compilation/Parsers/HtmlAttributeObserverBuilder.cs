using Neptuo.Models.Features;
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
    /// Observer which sets attribute to dictionary property.
    /// </summary>
    public class HtmlAttributeObserverBuilder : IObserverBuilder
    {
        private readonly Type requiredInterface;
        private readonly string propertyName;

        /// <summary>
        /// Creates new instance for required interface <paramref name="requiredInterface"/> 
        /// and <paramref name="propertyName"/> as Dictionary&lt;string, string&gt; property to set HTML attributes to.
        /// </summary>
        /// <param name="requiredInterface">Required interface on type code object.</param>
        /// <param name="propertyName">Dictionary&lt;string, string&gt; property to set HTML attributes to.</param>
        public HtmlAttributeObserverBuilder(Type requiredInterface, string propertyName)
        {
            Ensure.NotNull(requiredInterface, "requiredInterface");
            Ensure.NotNullOrEmpty(propertyName, "propertyName");
            this.requiredInterface = requiredInterface;
            this.propertyName = propertyName;
        }

        private MapCodeProperty CreateCodeProperty(ITypeCodeObject typeCodeObject)
        {
            TypePropertyInfo propertyInfo = new TypePropertyInfo(typeCodeObject.Type.GetProperty(propertyName));
            MapCodeProperty codeProperty = new MapCodeProperty(propertyInfo.Name, propertyInfo.Type);
            return codeProperty;
        }

        public bool TryParse(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlAttribute attribute)
        {
            ITypeCodeObject typeCodeObject;
            if (!codeObject.TryWith(out typeCodeObject))
                return false;

            IFieldCollectionCodeObject fields = codeObject.With<IFieldCollectionCodeObject>();
            if (requiredInterface.IsAssignableFrom(typeCodeObject.Type))
            {
                MapCodeProperty codeProperty = CreateCodeProperty(typeCodeObject);
                ICodeObject value = context.TryProcessValue(attribute.GetValue());
                if (value != null)
                {
                    codeProperty.SetKeyValue(new LiteralCodeObject(attribute.Name), value);
                    fields.AddProperty(codeProperty);
                    return true;
                }
            }
            return false;
        }
    }
}
