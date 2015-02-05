using Neptuo.Linq.Expressions;
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
    /// If possible, stores string values as plain strings. If target property requires instance of object instead of string, creates <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of literal control.</typeparam>
    public class DefaultLiteralControlBuilder<T> : LiteralBuilder, ILiteralBuilder
    {
        private readonly Type literalControlType;
        private readonly string textProperty;

        public DefaultLiteralControlBuilder(Expression<Func<T, string>> textProperty)
            : this(TypeHelper.PropertyName(textProperty))
        { }

        public DefaultLiteralControlBuilder(string textProperty)
        {
            Guard.NotNullOrEmpty(textProperty, "textProperty");
            this.literalControlType = typeof(T);
            this.textProperty = textProperty;
        }

        public override IEnumerable<ICodeObject> TryParseText(IContentBuilderContext context, string text)
        {
            Guard.NotNull(context, "context");
            return new CodeObjectList().AddLiteral(literalControlType, textProperty, text);
        }
    }
}
