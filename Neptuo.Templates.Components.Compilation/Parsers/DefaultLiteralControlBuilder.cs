using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation.CodeObjects;
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
    /// Default implementation of <see cref="ILiteralBuilder"/>.
    /// If possible, stores string values as plain strings. If target property requires <see cref="IControl"/>, creates <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of literal control.</typeparam>
    public class DefaultLiteralControlBuilder<T> : ILiteralBuilder
        where T : IControl
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

        public void Parse(IContentBuilderContext context, string text)
        {
            Guard.NotNull(context, "context");
            if (context.Parent.Property.CanAssign(typeof(string)))
            {
                context.Parent.SetValue(new PlainValueCodeObject(text));
                return;
            }

            if (context.Parent.Property.CanAssign(literalControlType))
            {
                IComponentCodeObject codeObject = new ComponentCodeObject(literalControlType);
                codeObject.Properties.Add(new SetPropertyDescriptor(
                    new TypePropertyInfo(literalControlType.GetProperty(textProperty)),
                    new PlainValueCodeObject(text)
                ));
                context.Parent.SetValue(codeObject);
            }
        }
    }
}
