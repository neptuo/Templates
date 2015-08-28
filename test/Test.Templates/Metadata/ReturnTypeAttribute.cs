using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.Metadata
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ReturnTypeAttribute : Attribute
    {
        public Type ReturnType { get; set; }

        public ReturnTypeAttribute(Type returnType)
        {
            ReturnType = returnType;
        }
    }
}
