using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.UI.Converters
{
    public interface IValueConverterService
    {
        IValueConverter GetConverter(string key);
        IValueConverterService SetConverter(string key, IValueConverter converter);
    }
}
