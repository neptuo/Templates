using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers;
using Neptuo.Templates.Controls;
using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleNG.Controls
{
    [JsType(Export = false)]
    public class LiteralControlBuilder<T> : ILiteralBuilder
        where T : IControl
    {
        private Type literalControlType;
        private string textProperty;

        public LiteralControlBuilder(Expression<Func<T, string>> textProperty)
        {
            this.literalControlType = typeof(T);
            this.textProperty = TypeHelper.PropertyName(textProperty);
        }

        public void Parse(IContentBuilderContext context, string text)
        {
            if (context.Parent.Property.CanAssign(typeof(string)))
            {
                context.Parent.SetValue(new PlainValueCodeObject(text));
                return;
            }

            IComponentCodeObject codeObject = new ComponentCodeObject(literalControlType);
            codeObject.Properties.Add(new SetPropertyDescriptor(
                new TypePropertyInfo(literalControlType.GetProperty(textProperty)), 
                new PlainValueCodeObject(text)
            ));
            context.Parent.SetValue(codeObject);
        }
    }
}
