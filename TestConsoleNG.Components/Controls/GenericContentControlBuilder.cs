using Neptuo;
using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers;
using Neptuo.Templates.Controls;
using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleNG.Controls
{
    [JsType(Export = false)]
    public class GenericContentControlBuilder<T> : DefaultTypeComponentBuilder
        where T : IControl
    {
        private readonly string tagNameProperty;

        public GenericContentControlBuilder(Expression<Func<T, string>> tagNameProperty, IPropertyBuilder propertyFactory, IObserverBuilder observerFactory)
            : base(typeof(T), propertyFactory, observerFactory)
        {
            this.tagNameProperty = TypeHelper.PropertyName(tagNameProperty);
        }

        protected override IComponentCodeObject CreateCodeObject(IContentBuilderContext context, IXmlElement element)
        {
            IComponentCodeObject codeObject = base.CreateCodeObject(context, element);
            codeObject.Properties.Add(new SetPropertyDescriptor(new TypePropertyInfo(GetControlType(element).GetProperty(tagNameProperty)), new PlainValueCodeObject(element.Name)));
            return codeObject;
        }

        public override IEnumerable<ICodeObject> TryParse(IContentBuilderContext context, IXmlElement element)
        {
            ComponentCodeObject codeObject = new ComponentCodeObject(typeof(T));

            bool isComponentRequired = false;
            foreach (IXmlAttribute attribute in element.Attributes)
            {
                ICodeObject value = context.TryProcessValue(attribute);

                // Error in parsing.
                if (value == null)
                    return null;

                // Is contains extension.
                ExtensionCodeObject extension = value as ExtensionCodeObject;
                if (extension != null)
                {
                    isComponentRequired = true;
                    break;
                }

                if (ObserverFactory.TryParse(context, codeObject, attribute))
                {
                    isComponentRequired = true;
                    break;
                }
            }

            if (isComponentRequired)
                return base.TryParse(context, element);
            else
                return ProcessAsTextXmlTag(context, element);
        }

        protected IEnumerable<ICodeObject> ProcessAsTextXmlTag(IContentBuilderContext context, IXmlElement element)
        {
            CodeObjectList result = new CodeObjectList();
            result.AddPlainValue(String.Format("<{0} {1}>", element.Name, String.Join(" ", element.Attributes)));
            result.AddRange(context.TryProcessContentNodes(element.ChildNodes));
            result.AddPlainValue(String.Format("</{0}>", element.Name));
            return result;
        }
    }
}
