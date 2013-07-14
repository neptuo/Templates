using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers.Extensions
{
    public interface IDefaultValueExtension
    {
        bool ProvideValue(IPropertyDescriptor propertyDescriptor);
    }
}
