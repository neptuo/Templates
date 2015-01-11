using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    public class MethodInvokePropertyInfo : IPropertyInfo
    {
        private MethodInfo methodInfo;
        private ParameterInfo[] parameters;
        private int parameterIndex;

        public string Name
        {
            get { return methodInfo.Name; }
        }

        public Type Type
        {
            get
            {
                if (parameterIndex >= parameters.Length)
                    //throw new InvalidOperationException("Parameter index is out of range.");
                    return parameters[parameters.Length - 1].ParameterType;

                return parameters[parameterIndex].ParameterType;
            }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public MethodInvokePropertyInfo(MethodInfo methodInfo)
        {
            if (methodInfo == null)
                throw new ArgumentNullException("methodInfo");

            this.methodInfo = methodInfo;
            this.parameters = methodInfo.GetParameters();
        }

        public bool CanAssign(Type type)
        {
            Type target = Type;
            if (Type.IsGenericType)
                target = Type.GetGenericArguments()[0];

            return target.IsAssignableFrom(type);
        }

        public void NextParameter()
        {
            parameterIndex++;
        }
    }
}
