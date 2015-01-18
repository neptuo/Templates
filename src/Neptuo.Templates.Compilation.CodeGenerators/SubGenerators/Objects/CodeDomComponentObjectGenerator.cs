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
            CodeMemberMethod createMethod = GenerateCreateMethod(context, codeObject);
            if (createMethod == null)
                return null;

            context.Structure.Class.Members.Add(createMethod);

            CodeMemberMethod bindMethod = GenerateBindMethod(context, codeObject);
            if(bindMethod == null)
                return null;

            context.Structure.Class.Members.Add(bindMethod);

            return new DefaultCodeDomObjectResult(
                new CodeMethodInvokeExpression(
                    new CodeThisReferenceExpression(),
                    createMethod.Name
                ),
                codeObject.Type
            );
        }

        protected virtual CodeMemberMethod GenerateCreateMethod(ICodeDomObjectContext context, ComponentCodeObject codeObject)
        {
            CodeMemberMethod createMethod = new CodeMemberMethod()
            {
                Name = "_Create",
                Attributes = MemberAttributes.Private,
                ReturnType = new CodeTypeReference(codeObject.Type)
            };

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

            return createMethod;
        }

        protected virtual CodeMemberMethod GenerateBindMethod(ICodeDomObjectContext context, ComponentCodeObject codeObject)
        {
            CodeMemberMethod bindMethod = new CodeMemberMethod()
            {
                Name = "_Bind",
                Attributes = MemberAttributes.Private
            };
            bindMethod.Parameters.Add(new CodeParameterDeclarationExpression(
                new CodeTypeReference(codeObject.Type),
                "_field"
            ));

            CodeDomObjectPropertyGenerator propertyGenerator = new CodeDomObjectPropertyGenerator();
            IEnumerable<CodeStatement> statements = propertyGenerator.Generate(context, codeObject, "_field");
            if (statements == null)
                return null;

            bindMethod.Statements.AddRange(statements);
            return bindMethod;
        }
    }
}
