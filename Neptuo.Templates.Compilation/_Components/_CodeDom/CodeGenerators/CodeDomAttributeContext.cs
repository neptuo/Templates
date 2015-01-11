using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomAttributeContext
    {
        public CodeDomGenerator.Context Context { get; private set; }
        public PropertyInfo PropertyInfo { get; private set; }

        public CodeDomAttributeContext(CodeDomGenerator.Context context, PropertyInfo propertyInfo)
        {
            Context = context;
            PropertyInfo = propertyInfo;
        }
    }
}
