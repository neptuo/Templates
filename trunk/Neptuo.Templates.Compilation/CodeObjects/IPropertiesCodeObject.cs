using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    public interface IPropertiesCodeObject : ICodeObject
    {
        List<IPropertyDescriptor> Properties { get; set; }
    }
}
