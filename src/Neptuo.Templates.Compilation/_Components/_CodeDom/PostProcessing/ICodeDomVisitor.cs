using Neptuo.Templates.Compilation.CodeGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.PostProcessing
{
    public interface ICodeDomVisitor
    {
        void Visit(ICodeDomVisitorContext context);
    }
}
