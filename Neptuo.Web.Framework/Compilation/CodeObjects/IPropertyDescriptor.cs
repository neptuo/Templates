using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeObjects
{
    public interface IPropertyDescriptor
    {
        PropertyInfo Property { get; set; }

        void SetValue(ICodeObject value);
    }
}
