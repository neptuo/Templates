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
        public XCodeDomGenerator.Context Context { get; private set; }
        public PropertyInfo PropertyInfo { get; private set; }

        public CodeDomAttributeContext(XCodeDomGenerator.Context context, PropertyInfo propertyInfo)
        {
            Context = context;
            PropertyInfo = propertyInfo;
        }
    }
}
