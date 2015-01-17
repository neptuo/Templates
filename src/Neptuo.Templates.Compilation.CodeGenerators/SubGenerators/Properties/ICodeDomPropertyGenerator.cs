using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// CodeDom generator for <see cref="ICodeObject"/>.
    /// </summary>
    public interface ICodeDomPropertyGenerator
    {
        /// <summary>
        /// Process <paramref name="codeProperty"/> and generates <see cref="CodeExpression"/> for it.
        /// </summary>
        /// <param name="context">Generator context.</param>
        /// <param name="codeProperty">Code object to process.</param>
        /// <returns><see cref="CodeExpression"/> for <paramref name="codeProperty"/>.</returns>
        CodeStatement Generate(ICodeDomPropertyContext context, ICodeProperty codeProperty);
    }
}
