﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using Neptuo.Web.Framework.Utils;

namespace Neptuo.Web.Framework.Compilation
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

        public CodeAssignStatement SetProperty(string fieldName, string propertyName, CodeExpression assign)
        {
            CodeAssignStatement assignStatement = new CodeAssignStatement(
                new CodePropertyReferenceExpression(
                    new CodeVariableReferenceExpression(fieldName),
                    propertyName
                ),
                assign
            );
            InitMethod.Statements.Add(assignStatement);
            return assignStatement;
        }

        public CodeAssignStatement SetProperty(string fieldName, string propertyName, object value)
        {
            return SetProperty(fieldName, propertyName, new CodePrimitiveExpression(value));
        }

        public CodeAssignStatement SetProperty(CodeMemberField field, string propertyName, CodeExpression assign)
        {
            return SetProperty(field.Name, propertyName, assign);
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

        private CodeExpression CreateFieldReferenceOrMethodCall(CodeMemberField field, string fieldMethodName = null, Type castTo = null)
        {
            CodeExpression result;

            if (fieldMethodName == null)
            {
                result = new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    field.Name
                );
            }
            else
            {
                result = new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(
                        new CodeFieldReferenceExpression(
                            new CodeThisReferenceExpression(),
                            field.Name
                        ),
                        fieldMethodName
                    )
                );
            }

            if (castTo != null)
                result = new CodeCastExpression(castTo, result);

            return result;
        }

        public void AddToParent(ParentInfo parent, CodeMemberField field, string fieldMethodName = null, bool cast = false, bool inInit = false)
        {
            if (parent.MethodName != null)
            {
                CodeMethodInvokeExpression invokeStatement = new CodeMethodInvokeExpression(
                    new CodeFieldReferenceExpression(
                        new CodePropertyReferenceExpression(
                            new CodeThisReferenceExpression(),
                            parent.Parent.Name
                        ),
                        parent.PropertyName
                    ),
                    parent.MethodName,
                    CreateFieldReferenceOrMethodCall(field, fieldMethodName, cast ? parent.RequiredType : null)
                );
                if (inInit)
                    InitMethod.Statements.Add(invokeStatement);
                else
                    CreateControlsMethod.Statements.Add(invokeStatement);
            }
            else
            {
                CodeAssignStatement assignStatement = new CodeAssignStatement(
                    new CodeFieldReferenceExpression(
                        new CodePropertyReferenceExpression(
                            new CodeThisReferenceExpression(),
                            parent.Parent.Name
                        ),
                        parent.PropertyName
                    ),
                    CreateFieldReferenceOrMethodCall(field, fieldMethodName, cast ? parent.RequiredType : null)
                );
                if (inInit)
                    InitMethod.Statements.Add(assignStatement);
                else
                    CreateControlsMethod.Statements.Add(assignStatement);
            }
        }

        public CodeMethodInvokeExpression InvokeRenderMethod(CodeMemberField field)
        {
            return InvokeMethod(field, RenderMethod, "Render", new CodeVariableReferenceExpression(GeneratedRenderMethodWriterParameterName));
        }

        public CodeMethodInvokeExpression InvokeDisposeMethod(CodeMemberField field)
        {
            return InvokeMethod(
                field, 
                DisposeMethod, 
                TypeHelper.MethodName<IDisposable>(d => d.Dispose), 
                new CodeVariableReferenceExpression(GeneratedRenderMethodWriterParameterName)
            );
        }

        private string GenerateFieldName()
        {
            return String.Format("field{0}", ++fieldCount);
        }
    }
}
