using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Default implementation of <see cref="ILiteralBuilder"/>.
    /// Creates instance of <see cref="PlainValueCodeObject"/> or <see cref="CommentCodeObject"/>.
    /// </summary>
    public class LiteralBuilder : ILiteralBuilder
    {
        public virtual ICodeObject TryParseText(IContentBuilderContext context, string text)
        {
            Guard.NotNull(context, "context");
            return new PlainValueCodeObject(text);
        }

        public virtual ICodeObject TryParseComment(IContentBuilderContext context, string commentText)
        {
            Guard.NotNull(context, "context");
            return new CommentCodeObject(commentText);
        }
    }
}
