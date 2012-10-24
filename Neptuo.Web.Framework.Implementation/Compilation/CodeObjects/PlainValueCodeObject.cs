using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeObjects
{
    /// <summary>
    /// Property value (in final state) or literal object.
    /// </summary>
    public class PlainValueCodeObject : IPlainValueCodeObject
    {
        public object Value { get; set; }

        public PlainValueCodeObject(object value)
        {
            Value = value;
        }
    }
}
