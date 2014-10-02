using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Property value (in final state) or literal object.
    /// </summary>
    public class PlainValueCodeObject : IPlainValueCodeObject, ITypeCodeObject
    {
        private object value;

        public object Value
        {
            get { return value; }
            set
            {
                this.value = value;

                if (this.value != null)
                    Type = this.value.GetType();
                else
                    Type = typeof(object);

            }
        }

        public Type Type { get; set; }

        public PlainValueCodeObject(object value)
        {
            Value = value;
        }
    }
}
