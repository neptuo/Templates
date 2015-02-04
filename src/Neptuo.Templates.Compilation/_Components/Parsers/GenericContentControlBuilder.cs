using Neptuo;
using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Content builder that can handle any XML element. 
    /// Must have defined property, where XML element name is set.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericContentControlBuilder<T> : DefaultTypeComponentBuilder
    {
        private readonly string tagNameProperty;

        /// <summary>
        /// Creates new instance, where <typeparamref name="T"/> is the control to create and <paramref name="tagNameProperty"/> is expression
        /// to the property, where XML element name should be set.
        /// </summary>
        /// <param name="tagNameProperty">Expression to the property, where XML element name should be set.</param>
        /// <param name="propertyFactory"></param>
        /// <param name="observerFactory"></param>
        public GenericContentControlBuilder(Expression<Func<T, string>> tagNameProperty)
            : base(typeof(T))
        {
            this.tagNameProperty = TypeHelper.PropertyName(tagNameProperty);
        }

        protected override IComponentCodeObject CreateCodeObject(IContentBuilderContext context, IXmlElement element)
        {
            IComponentCodeObject codeObject = base.CreateCodeObject(context, element);
            codeObject.Properties.Add(new SetCodeProperty(new TypePropertyInfo(GetControlType(element).GetProperty(tagNameProperty)), new PlainValueCodeObject(element.Name)));
            return codeObject;
        }

        protected bool IsComponentRequired(IContentBuilderContext context, IXmlElement element)
        {
            ComponentCodeObject codeObject = new ComponentCodeObject(typeof(T));

            bool isComponentRequired = false;
            foreach (IXmlAttribute attribute in element.Attributes)
            {
                ICodeObject value = context.TryProcessValue(attribute);

                // Error in parsing.
                if (value == null)
                    return true;

                // Is not plain value code object.
                IPlainValueCodeObject plainValue = value as IPlainValueCodeObject;
                if (plainValue == null)
                {
                    isComponentRequired = true;
                    break;
                }

                // Is observer.
                if (context.Registry.WithObserverBuilder().TryParse(context, codeObject, attribute))
                {
                    isComponentRequired = true;
                    break;
                }
            }

            return isComponentRequired;
        }

        public override IEnumerable<ICodeObject> TryParse(IContentBuilderContext context, IXmlElement element)
        {
            if (IsComponentRequired(context, element))
                return base.TryParse(context, element);
            else
                return ProcessAsTextXmlTag(context, element);
        }

        protected IEnumerable<ICodeObject> ProcessAsTextXmlTag(IContentBuilderContext context, IXmlElement element)
        {
            CodeObjectList result = new CodeObjectList();
            if (element.ChildNodes.Any())
            {
                if (element.Attributes.Any())
                    result.AddPlainValue(String.Format("<{0} {1}>", element.Name, String.Join(" ", element.Attributes)));
                else
                    result.AddPlainValue(String.Format("<{0}>", element.Name));

                result.AddRange(context.TryProcessContentNodes(element.ChildNodes));
                result.AddPlainValue(String.Format("</{0}>", element.Name));
            }
            else
            {
                result.AddPlainValue(String.Format("<{0} />", element.Name));
            }

            return result;
        }
    }
}
