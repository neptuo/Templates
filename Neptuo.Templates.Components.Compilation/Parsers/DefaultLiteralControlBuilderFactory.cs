using Neptuo.Linq.Expressions;
using Neptuo.Templates.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Implmenentation of <see cref="ILiteralBuilderFactory"/> which creates instances of <see cref="LiteralBuilder"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DefaultLiteralControlBuilderFactory<T> : ILiteralBuilderFactory
        where T : IControl
    {
        private readonly Type literalControlType;
        private readonly string textProperty;

        public DefaultLiteralControlBuilderFactory(Expression<Func<T, string>> textProperty)
            : this(TypeHelper.PropertyName(textProperty))
        { }

        public DefaultLiteralControlBuilderFactory(string textProperty)
        {
            Guard.NotNullOrEmpty(textProperty, "textProperty");
            this.literalControlType = typeof(T);
            this.textProperty = textProperty;
        }

        public ILiteralBuilder CreateBuilder()
        {
            return new DefaultLiteralControlBuilder<T>(textProperty);
        }
    }
}
