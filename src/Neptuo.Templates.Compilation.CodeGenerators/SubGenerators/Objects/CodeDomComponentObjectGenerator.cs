using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomComponentObjectGenerator : CodeDomObjectGeneratorBase<ComponentCodeObject>
    {
        protected override ICodeDomObjectResult Generate(ICodeDomObjectContext context, ComponentCodeObject codeObject)
        {
            CodeMemberMethod createMethod = new CodeMemberMethod()
            {
                Name = "_Create",
                Attributes = MemberAttributes.Private,
                ReturnType = new CodeTypeReference(codeObject.Type)
            };
            context.Structure.Class.Members.Add(createMethod);

            CodeDomObjectInstanceGenerator instanceGenerator = new CodeDomObjectInstanceGenerator();
            CodeExpression instanceExpression = instanceGenerator.Generate(context, codeObject.Type);
            if (instanceExpression == null)
                return null;

            createMethod.Statements.Add(new CodeVariableDeclarationStatement(
                new CodeTypeReference(codeObject.Type),
                "_field",
                instanceExpression
            ));
            createMethod.Statements.Add(new CodeMethodReturnStatement(
                new CodeVariableReferenceExpression("_field")
            ));

            return new DefaultCodeDomObjectResult(
                new CodeMethodInvokeExpression(
                    new CodeThisReferenceExpression(),
                    createMethod.Name
                ),
                codeObject.Type
            );
        }
    }
}
