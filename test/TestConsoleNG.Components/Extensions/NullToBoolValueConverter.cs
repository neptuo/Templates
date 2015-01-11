using Neptuo.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleNG.Extensions
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
