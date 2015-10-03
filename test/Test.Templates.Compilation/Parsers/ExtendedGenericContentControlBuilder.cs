using Neptuo.Models.Features;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.CodeObjects.Features;
using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Templates.UI;

namespace Test.Templates.Compilation.Parsers
{
    public class ExtendedGenericContentControlBuilder : GenericContentControlBuilder<GenericContentControl>
    {
        public ExtendedGenericContentControlBuilder()
            : base(c => c.TagName)
        { }

        protected override IComponentCodeObject CreateCodeObject(IContentBuilderContext context, IXmlElement element)
        {
            IComponentCodeObject codeObject = base.CreateCodeObject(context, element);
            ComponentCodeObject component = codeObject as ComponentCodeObject;
            if (component == null)
                return null;

            component
                .Add<IObserverCollectionCodeObject>(new ObserverCollectionCodeObject());

            return component;
        }

        protected override bool IsComponentRequired(IContentBuilderContext context, IXmlElement element)
        {
            if (base.IsComponentRequired(context, element))
                return true;

            ComponentCodeObject codeObject = new ComponentCodeObject();
            codeObject
                .Add<ITypeCodeObject>(new TypeCodeObject(typeof(GenericContentControl)))
                .Add<IFieldCollectionCodeObject>(new FieldCollectionCodeObject());

            bool isComponentRequired = false;
            foreach (IXmlAttribute attribute in element.Attributes)
            {
                // Is observer.
                if (context.Registry.WithObserverBuilder().TryParse(context, codeObject, attribute))
                {
                    isComponentRequired = true;
                    break;
                }
            }

            return isComponentRequired;
        }

        protected override bool ProcessUnboundAttribute(IContentBuilderContext context, IXmlAttribute unboundAttribute)
        {
            if (context.Registry.WithObserverBuilder().TryParse(context, (IComponentCodeObject)context.CodeObject(), unboundAttribute))
                return true;

            return base.ProcessUnboundAttribute(context, unboundAttribute);
        }

        public override IEnumerable<ICodeObject> TryParse(IContentBuilderContext context, IXmlElement element)
        {
            IEnumerable<ICodeObject> result = base.TryParse(context, element);
            return result;
        }
    }
}
