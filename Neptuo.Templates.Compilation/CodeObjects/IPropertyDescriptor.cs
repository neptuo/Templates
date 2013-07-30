using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    public interface IPropertyDescriptor
    {
        IPropertyInfo PropertyName { get; set; }

        void SetValue(ICodeObject value);
    }
}
