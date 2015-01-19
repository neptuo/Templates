using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomLiteralObjectGenerator : CodeDomObjectGeneratorBase<IPlainValueCodeObject>
    {
        protected override ICodeDomObjectResult Generate(ICodeDomObjectContext context, IPlainValueCodeObject codeObject)
        {
            object value = codeObject.Value;
            Type returnType = typeof(void);
            if(value != null)
                returnType = codeObject.Value.GetType();

            return new DefaultCodeDomObjectResult(new CodePrimitiveExpression(value), returnType);
        }
    }
}
