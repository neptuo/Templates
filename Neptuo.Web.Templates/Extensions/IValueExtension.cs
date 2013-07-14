using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Extensions
{
    public interface IValueExtension
    {
        object ProvideValue(IValueExtensionContext context);
    }
}
