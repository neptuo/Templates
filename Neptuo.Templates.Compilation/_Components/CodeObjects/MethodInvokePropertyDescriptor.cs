using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    public class MethodInvokePropertyDescriptor : IPropertyDescriptor
    {
        private MethodInvokePropertyInfo property;

        public MethodInfo Method { get; set; }
        public IPropertyInfo Property
        {
            get { return property; }
            set
            {
                MethodInvokePropertyInfo newValue = value as MethodInvokePropertyInfo;
                if (newValue != null)
                    property = newValue;
            }
        }
        public List<ICodeObject> Parameters { get; protected set; }

        public MethodInvokePropertyDescriptor(MethodInfo methodInfo)
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
