using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Templates.Metadata;

namespace Test.Templates.UI.Converters
{
    public class NullToBoolValueConverter : IValueConverter
    {
        [ReturnType(typeof(bool))]
        public object ConvertTo(object value)
        {
            return value != null;
        }
    }
}
