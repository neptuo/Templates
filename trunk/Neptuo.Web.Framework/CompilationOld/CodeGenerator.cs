using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;

namespace Neptuo.Web.Framework.CompilationOld
{
    public class CodeGenerator : BaseCodeGenerator
    {
        private int fieldCount = 0;

        public CodeMemberField DeclareField(Type fieldType)
        {
            CodeMemberField field = new CodeMemberField
            {
                Name = GenerateFieldName(),
                Attributes = MemberAttributes.Private,
                Type = new CodeTypeReference(fieldType)
            };
            Class.Members.Add(field);
            return field;
        }

        public CodeAssignStatement CreateInstance(CodeMemberField fieldToAssign, Type instanceType)
        {
            CodeAssignStatement instance = new CodeAssignStatement(
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    fieldToAssign.Name
                ),
                new CodeObjectCreateExpression(
                    new CodeTypeReference(instanceType)
                )
            );
            CreateControlsMethod.Statements.Add(instance);
            return instance;
        }

        public CodeAssignStatement SetProperty(CodeMemberField field, string propertyName, CodeExpression assign)
        {
            CodeAssignStatement assignStatement = new CodeAssignStatement(
                new CodePropertyReferenceExpression(
                    new CodeVariableReferenceExpression(field.Name),
                    propertyName
                ),
                assign
            );
            CreateControlsMethod.Statements.Add(assignStatement);
            return assignStatement;
        }

        public CodeAssignStatement SetProperty(CodeMemberField field, string propertyName, object value)
        {
            return SetProperty(field, propertyName, new CodePrimitiveExpression(value));
        }

        public CodeMethodInvokeExpression InvokeMethod(CodeMemberField field, CodeMemberMethod invokeInMethod, string methodToInvoke, params CodeExpression[] parameters)
        {
            CodeMethodInvokeExpression invokeStatement = new CodeMethodInvokeExpression(
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    field.Name
                ),
                methodToInvoke,
                parameters
            );
            invokeInMethod.Statements.Add(invokeStatement);
            return invokeStatement;
        }

        public void AddToViewPage(CodeMemberField field)
        {
            CodeMethodInvokeExpression invokeStatement = new CodeMethodInvokeExpression(
                new CodePropertyReferenceExpression(
                    new CodePropertyReferenceExpression(
                        new CodeThisReferenceExpression(),
                        GeneratedViewPageProperty
                    ),
                    "Content"
                ),
                "Add",
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    field.Name
                )
            );
            CreateControlsMethod.Statements.Add(invokeStatement);
        }

        public void AddToParent(ParentInfo parent, CodeMemberField field)
        {
            if (parent.MethodName != null)
            {
                CodeMethodInvokeExpression invokeStatement = new CodeMethodInvokeExpression(
                    new CodeFieldReferenceExpression(
                        new CodePropertyReferenceExpression(
                            new CodeThisReferenceExpression(),
                            parent.MemberName
                        ),
                        parent.PropertyName
                    ),
                    "Add",
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        field.Name
                    )
                );
                CreateControlsMethod.Statements.Add(invokeStatement);
            }
            else
            {
                CodeAssignStatement assignStatement = new CodeAssignStatement(
                    new CodeFieldReferenceExpression(
                        new CodePropertyReferenceExpression(
                            new CodeThisReferenceExpression(),
                            parent.MemberName
                        ),
                        parent.PropertyName
                    ),
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        field.Name
                    )
                );
                CreateControlsMethod.Statements.Add(assignStatement);
            }
        }

        public CodeMethodInvokeExpression InvokeRenderMethod(CodeMemberField field)
        {
            return InvokeMethod(field, RenderMethod, "Render", new CodeVariableReferenceExpression(GeneratedRenderMethodWriterParameterName));
        }

        private string GenerateFieldName()
        {
            return String.Format("field{0}", ++fieldCount);
        }
    }
}
