using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.Parsers.Extensions
{
    public interface IDefaultValueExtension
    {
        bool ProvideValue(IPropertyDescriptor propertyDescriptor);
    }
}
