using Neptuo.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public static class _CodeDomContextExtensions
    {
        public static void AddError(this ICodeDomContext context, string errorTextFormat, params object[] parameters)
        {
            Guard.NotNull(context, "context");
            context.GeneratorContext.Errors.Add(new ErrorInfo(0, 0, String.Format(errorTextFormat, parameters)));
        }
    }
}
