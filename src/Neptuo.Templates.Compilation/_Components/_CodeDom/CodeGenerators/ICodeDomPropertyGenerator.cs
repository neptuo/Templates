using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public interface XICodeDomPropertyGenerator
    {
        void GenerateProperty(CodeDomPropertyContext context, ICodeProperty codeProperty);
    }
}
