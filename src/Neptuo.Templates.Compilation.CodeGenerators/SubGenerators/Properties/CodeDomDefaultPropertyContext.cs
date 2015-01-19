using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Default implementation of <see cref="ICodeDomPropertyContext"/>.
    /// </summary>
    public class CodeDomDefaultPropertyContext : CodeDomDefaultContext, ICodeDomPropertyContext
    {
        public CodeExpression PropertyTarget { get; private set; }

        public CodeDomDefaultPropertyContext(ICodeDomContext context, CodeExpression propertyTarget)
            : base(context.GeneratorContext, context.Configuration, context.Structure, context.Registry)
        {
            Guard.NotNull(propertyTarget, "propertyTarget");
            PropertyTarget = propertyTarget;
        }
    }
}
