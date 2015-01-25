using Neptuo.ComponentModel;
using System;
using System.CodeDom;
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

        /// <summary>
        /// Creates <see cref="ICodeDomPropertyContext"/> from <paramref name="context"/> and <paramref name="propertyTarget"/>.
        /// </summary>
        /// <param name="context">Generator context.</param>
        /// <param name="propertyTarget">Object where property generator should set value.</param>
        /// <returns><see cref="ICodeDomPropertyContext"/> from <paramref name="context"/> and <paramref name="propertyTarget"/>.</returns>
        public static CodeDomDefaultPropertyContext CreatePropertyContext(this ICodeDomContext context, CodeExpression propertyTarget)
        {
            Guard.NotNull(context, "context");
            return new CodeDomDefaultPropertyContext(context, propertyTarget);
        }

        /// <summary>
        /// Creates <see cref="CodeDomDefaultObjectContext"/> from <paramref name="context"/>.
        /// </summary>
        /// <param name="context">Generator context.</param>
        /// <returns><see cref="ICodeDomObjectContext"/> from <paramref name="context"/>.</returns>
        public static CodeDomDefaultObjectContext CreateObjectContext(this ICodeDomContext context)
        {
            Guard.NotNull(context, "context");
            return new CodeDomDefaultObjectContext(context);
        }
    }
}
