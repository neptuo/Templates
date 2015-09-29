using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomLiteralObjectGenerator : CodeDomObjectGeneratorBase<ILiteralCodeObject>
    {
        protected override ICodeDomObjectResult Generate(ICodeDomObjectContext context, ILiteralCodeObject codeObject)
        {
            object value = codeObject.Value;
            Type returnType = typeof(void);
            if(value != null)
                returnType = codeObject.Value.GetType();

            return new CodeDomDefaultObjectResult(new CodePrimitiveExpression(value), returnType);
        }
    }
}
