using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    public class LiteralCodeObject : PlainValueCodeObject
    {
        public Type LiteralType { get; set; }
        public string TextProperty { get; set; }

        public LiteralCodeObject(Type literalType, string textProperty, object value)
            : base(value)
        {
            Guard.NotNull(literalType, "literalType");
            Guard.NotNullOrEmpty(textProperty, "textProperty");
            LiteralType = literalType;
            TextProperty = textProperty;
        }
    }
}
