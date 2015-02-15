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
    /// Default implementation of <see cref="IXmlLiteralBuilder"/>.
    /// Creates instance of <see cref="PlainValueCodeObject"/> or <see cref="CommentCodeObject"/>.
    /// </summary>
    public class XmlLiteralBuilder : IXmlLiteralBuilder
    {
        public virtual IEnumerable<ICodeObject> TryParseText(IXmlContentBuilderContext context, string text)
        {
            Guard.NotNull(context, "context");
            return new CodeObjectList().AddPlainValue(text);
        }

        public virtual IEnumerable<ICodeObject> TryParseComment(IXmlContentBuilderContext context, string commentText)
        {
            Guard.NotNull(context, "context");
            return new CodeObjectList().AddComment(commentText);
        }
    }
}
