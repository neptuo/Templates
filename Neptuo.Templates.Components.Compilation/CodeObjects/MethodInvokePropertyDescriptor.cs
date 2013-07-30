using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    public class MethodInvokePropertyDescriptor : IPropertyDescriptor
    {
        public MethodInfo Method { get; set; }
        public IPropertyInfo PropertyName { get; set; }
        public List<ICodeObject> Parameters { get; protected set; }

        public MethodInvokePropertyDescriptor(MethodInfo method)
        {
            Method = method;
            Parameters = new List<ICodeObject>();
        }

        public void SetValue(ICodeObject value)
        {
            Parameters.Add(value);
        }
    }
}
