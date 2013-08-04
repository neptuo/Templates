using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public interface ICodeDomStructureGenerator
    {
        BaseCodeDomStructure GenerateCode(CodeDomStructureContext context);
    }
}
