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
        public virtual IEnumerable<ICodeObject> TryParseText(IContentBuilderContext context, string text)
        {
            Ensure.NotNull(context, "context");
            return new CodeObjectList().AddPlainValue(text);
        }

        public virtual IEnumerable<ICodeObject> TryParseComment(IContentBuilderContext context, string commentText)
        {
            Ensure.NotNull(context, "context");
            return new CodeObjectList().AddComment(commentText);
        }
    }
}
