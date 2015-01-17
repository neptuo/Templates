using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Generator for injection dependencies.
    /// </summary>
    public interface ICodeDomDependencyGenerator
    {
        /// <summary>
        /// Generates expression for dependency resolve of type <paramref name="type"/>.
        /// </summary>
        /// <param name="context">Generator context.</param>
        /// <param name="type">Type to resolve.</param>
        /// <returns>Expression for dependency resolve of type <paramref name="type"/>.</returns>
        CodeExpression GenerateCode(ICodeDomContext context, Type type);
    }
}
