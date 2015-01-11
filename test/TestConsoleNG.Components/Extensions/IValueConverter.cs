using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleNG.Extensions
{
    public interface IValueConverter
    {
        object ConvertTo(object value);
    }
}
