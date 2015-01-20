using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Default implementation of <see cref="ICodeDomNaming"/>.
    /// </summary>
    public class CodeDomDefaultNaming : ICodeDomNaming
    {
        public string NamespaceName { get; private set; }
        public string ClassName { get; private set; }
        public string FullClassName { get; private set; }

        public CodeDomDefaultNaming(string namespaceName, string className)
        {
            Guard.NotNullOrEmpty(className, "className");
            NamespaceName = namespaceName;
            ClassName = className;

            if (!String.IsNullOrEmpty(NamespaceName))
                FullClassName = NamespaceName + "." + ClassName;
            else
                FullClassName = ClassName;
        }
    }
}
