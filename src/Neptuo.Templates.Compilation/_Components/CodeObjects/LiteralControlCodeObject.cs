using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    public class LiteralControlCodeObject : LiteralCodeObject
    {
        public Type LiteralType { get; set; }
        public string TextProperty { get; set; }

        public LiteralControlCodeObject(Type literalType, string textProperty, object value)
            : base(value)
        {
            Ensure.NotNull(literalType, "literalType");
            Ensure.NotNullOrEmpty(textProperty, "textProperty");
            LiteralType = literalType;
            TextProperty = textProperty;
        }
    }
}
