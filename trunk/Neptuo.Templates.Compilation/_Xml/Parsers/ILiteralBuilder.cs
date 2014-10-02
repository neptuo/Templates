using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// BUilds static literal value.
    /// </summary>
    public interface ILiteralBuilder
    {
        /// <summary>
        /// Parses <paramref name="text"/> and creates AST for it.
        /// </summary>
        /// <param name="context">Context information.</param>
        /// <param name="text">Text value to parse.</param>
        /// <returns>Parsed code object; <c>null</c> otherwise.</returns>
        ICodeObject Parse(IContentBuilderContext context, string text);
    }
}
