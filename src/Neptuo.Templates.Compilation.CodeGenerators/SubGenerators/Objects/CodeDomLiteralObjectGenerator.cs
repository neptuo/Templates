using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomLiteralObjectGenerator : CodeDomObjectGeneratorBase<LiteralCodeObject>
    {
        protected override ICodeDomObjectResult Generate(ICodeDomContext context, LiteralCodeObject codeObject)
        {
            Type returnType = typeof(void);
            if(codeObject.Value != null)
                returnType = codeObject.Value.GetType();

            return new DefaultCodeDomObjectResult(new CodePrimitiveExpression(codeObject.Value), returnType);
        }
    }
}
