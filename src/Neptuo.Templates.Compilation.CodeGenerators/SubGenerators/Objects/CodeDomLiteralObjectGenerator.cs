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
        protected override ICodeDomObjectResult Generate(ICodeDomObjectContext context, LiteralCodeObject codeObject)
        {
            Type returnType = typeof(void);
            if(codeObject.Value != null)
                returnType = codeObject.Value.GetType();

            ICodeProperty codeProperty;
            if (!context.TryGetCodeProperty(out codeProperty))
                return CreateFromPrimitiveVale(codeObject.Value, returnType);

            if (codeProperty.Property.CanAssign(returnType))
                return CreateFromPrimitiveVale(codeObject.Value, returnType);

            throw Guard.Exception.NotImplemented();
        }

        private DefaultCodeDomObjectResult CreateFromPrimitiveVale(object value, Type returnType)
        {
            return new DefaultCodeDomObjectResult(new CodePrimitiveExpression(value), returnType);
        }
    }
}
