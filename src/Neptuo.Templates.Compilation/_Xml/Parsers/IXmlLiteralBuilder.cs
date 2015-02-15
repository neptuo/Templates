using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Builds static literal value.
    /// </summary>
    public interface IXmlLiteralBuilder
    {
        /// <summary>
        /// Parses <paramref name="text"/> and creates AST for it.
        /// </summary>
        /// <param name="context">Context information.</param>
        /// <param name="text">Text value to parse.</param>
        /// <returns>Parsed code objects; <c>null</c> otherwise.</returns>
        IEnumerable<ICodeObject> TryParseText(IXmlContentBuilderContext context, string text);

        /// <summary>
        /// Parses <paramref name="commentText"/> and creates AST for it.
        /// </summary>
        /// <param name="context">Content information.</param>
        /// <param name="commentText">Text value of comment.</param>
        /// <returns>Parsed code objects; <c>null</c> otherwise.</returns>
        IEnumerable<ICodeObject> TryParseComment(IXmlContentBuilderContext context, string commentText);
    }
}
