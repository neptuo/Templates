using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.Compilation.Parsers
{
    public class ControlContentBuilder : DefaultTypeComponentBuilder
    {
        public ControlContentBuilder(Type type)
            : base(type)
        { }

        protected override bool ProcessUnboundAttribute(IContentBuilderContext context, IXmlAttribute unboundAttribute)
        {
            if (context.Registry.WithObserverBuilder().TryParse(context, context.CodeObjectAsObservers(), unboundAttribute))
                return true;

            return base.ProcessUnboundAttribute(context, unboundAttribute);
        }
    }
}
