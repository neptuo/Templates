using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    public class MethodInvokeCodeProperty : ICodeProperty
    {
        private MethodInvokePropertyInfo property;

        public MethodInfo Method { get; set; }

        public string Name
        {
            get { return property.Name; }
        }

        public Type Type
        {
            get { return property.Type; }
        }

        public List<ICodeObject> Parameters { get; protected set; }

        public MethodInvokeCodeProperty(MethodInfo methodInfo)
        {
            Method = methodInfo;
            Parameters = new List<ICodeObject>();
            property = new MethodInvokePropertyInfo(methodInfo);
        }

        public void SetValue(ICodeObject value)
        {
            Parameters.Add(value);
            property.NextParameter();
        }
    }
}
