using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomSetPropertyGenerator : XCodeDomPropertyGeneratorBase<SetCodeProperty>
    {
        public CodeDomSetPropertyGenerator(Type requiredComponentType, ComponentManagerDescriptor componentManagerDescriptor)
            : base(requiredComponentType, componentManagerDescriptor)
        { }

        protected override void GenerateProperty(CodeDomPropertyContext context, SetCodeProperty codeProperty)
        {
            CodeExpression codeExpression = context.CodeGenerator.GenerateCodeObject(
                new CodeObjectExtensionContext(context.Context, context.FieldName), 
                codeProperty.Value, 
                codeProperty
            );
            if (codeExpression != null)
            {
                context.Statements.Add(
                    new CodeAssignStatement(
                        new CodePropertyReferenceExpression(
                            GetPropertyTarget(context),
                            codeProperty.Property.Name
                        ),
                        codeExpression
                    )
                );
            }
        }
    }
}
