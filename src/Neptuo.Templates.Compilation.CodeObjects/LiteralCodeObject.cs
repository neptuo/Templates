using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Property value (in final state) or literal object.
    /// </summary>
    public class LiteralCodeObject : ICodeObject
    {
        public object Value { get; private set; }

        public LiteralCodeObject(object value)
        {
            Value = value;
        }
    }
}
