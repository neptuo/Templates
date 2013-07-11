using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestConsole
{
    class JavascriptCodeGenerator : CodeGenerator
    {
        protected override string CreateEscapedIdentifier(string value)
        {
            throw new NotImplementedException();
        }

        protected override string CreateValidIdentifier(string value)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateArgumentReferenceExpression(CodeArgumentReferenceExpression e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateArrayCreateExpression(CodeArrayCreateExpression e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateArrayIndexerExpression(CodeArrayIndexerExpression e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateAssignStatement(CodeAssignStatement e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateAttachEventStatement(CodeAttachEventStatement e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateAttributeDeclarationsEnd(CodeAttributeDeclarationCollection attributes)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateAttributeDeclarationsStart(CodeAttributeDeclarationCollection attributes)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateBaseReferenceExpression(CodeBaseReferenceExpression e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateCastExpression(CodeCastExpression e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateComment(CodeComment e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateConditionStatement(CodeConditionStatement e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateConstructor(CodeConstructor e, CodeTypeDeclaration c)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateDelegateCreateExpression(CodeDelegateCreateExpression e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateDelegateInvokeExpression(CodeDelegateInvokeExpression e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateEntryPointMethod(CodeEntryPointMethod e, CodeTypeDeclaration c)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateEvent(CodeMemberEvent e, CodeTypeDeclaration c)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateEventReferenceExpression(CodeEventReferenceExpression e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateExpressionStatement(CodeExpressionStatement e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateField(CodeMemberField e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateFieldReferenceExpression(CodeFieldReferenceExpression e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateGotoStatement(CodeGotoStatement e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateIndexerExpression(CodeIndexerExpression e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateIterationStatement(CodeIterationStatement e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateLabeledStatement(CodeLabeledStatement e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateLinePragmaEnd(CodeLinePragma e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateLinePragmaStart(CodeLinePragma e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateMethod(CodeMemberMethod e, CodeTypeDeclaration c)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateMethodInvokeExpression(CodeMethodInvokeExpression e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateMethodReferenceExpression(CodeMethodReferenceExpression e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateMethodReturnStatement(CodeMethodReturnStatement e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateNamespaceEnd(CodeNamespace e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateNamespaceImport(CodeNamespaceImport e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateNamespaceStart(CodeNamespace e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateObjectCreateExpression(CodeObjectCreateExpression e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateProperty(CodeMemberProperty e, CodeTypeDeclaration c)
        {
            throw new NotImplementedException();
        }

        protected override void GeneratePropertyReferenceExpression(CodePropertyReferenceExpression e)
        {
            throw new NotImplementedException();
        }

        protected override void GeneratePropertySetValueReferenceExpression(CodePropertySetValueReferenceExpression e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateRemoveEventStatement(CodeRemoveEventStatement e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateSnippetExpression(CodeSnippetExpression e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateSnippetMember(CodeSnippetTypeMember e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateThisReferenceExpression(CodeThisReferenceExpression e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateThrowExceptionStatement(CodeThrowExceptionStatement e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateTryCatchFinallyStatement(CodeTryCatchFinallyStatement e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateTypeConstructor(CodeTypeConstructor e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateTypeEnd(CodeTypeDeclaration e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateTypeStart(CodeTypeDeclaration e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateVariableDeclarationStatement(CodeVariableDeclarationStatement e)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateVariableReferenceExpression(CodeVariableReferenceExpression e)
        {
            throw new NotImplementedException();
        }

        protected override string GetTypeOutput(CodeTypeReference value)
        {
            throw new NotImplementedException();
        }

        protected override bool IsValidIdentifier(string value)
        {
            throw new NotImplementedException();
        }

        protected override string NullToken
        {
            get { throw new NotImplementedException(); }
        }

        protected override void OutputType(CodeTypeReference typeRef)
        {
            throw new NotImplementedException();
        }

        protected override string QuoteSnippetString(string value)
        {
            throw new NotImplementedException();
        }

        protected override bool Supports(GeneratorSupport support)
        {
            throw new NotImplementedException();
        }
    }
}
