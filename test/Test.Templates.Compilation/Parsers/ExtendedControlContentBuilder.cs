using Neptuo.Models.Features;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.CodeObjects.Features;
using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.Compilation.Parsers
{
    public class ExtendedControlContentBuilder : DefaultTypeComponentBuilder
    {
        private readonly Type type;

        public ExtendedControlContentBuilder(Type type)
            : base(type)
        {
            this.type = type;
        }

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

        public override IEnumerable<ICodeObject> TryParse(IContentBuilderContext context, IXmlElement element)
        {
            return base.TryParse(context, element);
        }

        protected override bool ProcessUnboundAttribute(IContentBuilderContext context, IXmlAttribute unboundAttribute)
        {
            if (context.Registry.WithObserverBuilder().TryParse(context, (IComponentCodeObject)context.CodeObject(), unboundAttribute))
                return true;

            return base.ProcessUnboundAttribute(context, unboundAttribute);
        }
    }
}
