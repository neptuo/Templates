using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.Reflection;
using System.IO;
using System.CodeDom.Compiler;
using System.Web;
using Microsoft.CSharp;

namespace Neptuo.Web.Framework.Compilation
{
    public class BaseCodeGenerator
    {
        public const string GeneratedCodeNamespace = "Neptuo.Web.Framework.Generated";
        public const string GeneratedRequestField = "request";
        public const string GeneratedResponseField = "response";
        public const string GeneratedViewPageField = "viewPage";
        public const string GeneratedCreateControlsMethod = "CreateControls";
        public const string GeneratedSetupMethod = "Setup";
        public const string GeneratedInitMethod = "Init";
        public const string GeneratedRenderMethod = "Render";
        public const string GeneratedDisposeMethod = "Dispose";

        public const string GeneratedSetupMethodViewPageParameterName = "viewPage";
        public const string GeneratedSetupMethodRequestParameterName = "request";
        public const string GeneratedSetupMethodResponseParameterName = "response";

        public const string GeneratedRenderMethodWriterParameterName = "writer";

        public CodeCompileUnit Unit { get; protected set; }
        public CodeNamespace CodeNamespace { get; protected set; }
        public CodeTypeDeclaration Class { get; protected set; }

        public CodeMemberField ViewPageField { get; protected set; }
        public CodeMemberField RequestField { get; protected set; }
        public CodeMemberField ResponseField { get; protected set; }

        public CodeMemberMethod CreateControlsMethod { get; protected set; }
        public CodeMemberMethod SetupMethod { get; protected set; }
        public CodeMemberMethod InitMethod { get; protected set; }
        public CodeMemberMethod RenderMethod { get; protected set; }
        public CodeMemberMethod DisposeMethod { get; protected set; }


        public BaseCodeGenerator()
        {
            CreateBase();
            CreateClass();
            CreateFields();
            CreateMethods();
        }

        public void GenerateCode(TextWriter writer)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions
            {
                BracingStyle = "C",
                BlankLinesBetweenMembers = false,
                VerbatimOrder = false
            };

            provider.GenerateCodeFromCompileUnit(Unit, writer, options);
        }

        private void CreateBase()
        {
            Unit = new CodeCompileUnit();
            CodeNamespace = new CodeNamespace(GeneratedCodeNamespace);
            CodeNamespace.Imports.Add(new CodeNamespaceImport("System"));
            CodeNamespace.Imports.Add(new CodeNamespaceImport("Neptuo.Web"));
            CodeNamespace.Imports.Add(new CodeNamespaceImport("Neptuo.Web.Framework"));
            Unit.Namespaces.Add(CodeNamespace);
        }

        private void CreateClass()
        {
            Class = new CodeTypeDeclaration("GeneratedView");
            Class.IsClass = true;
            Class.TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed;
            Class.BaseTypes.Add(new CodeTypeReference(typeof(IGeneratedView)));
            Class.BaseTypes.Add(new CodeTypeReference(typeof(IDisposable)));
            CodeNamespace.Types.Add(Class);
        }

        private void CreateFields()
        {
            RequestField = new CodeMemberField
            {
                Name = GeneratedRequestField,
                Attributes = MemberAttributes.Private,
                Type = new CodeTypeReference(typeof(HttpRequest))
            };
            Class.Members.Add(RequestField);

            ResponseField = new CodeMemberField
            {
                Name = GeneratedResponseField,
                Attributes = MemberAttributes.Private,
                Type = new CodeTypeReference(typeof(HttpResponse))
            };
            Class.Members.Add(ResponseField);

            ViewPageField = new CodeMemberField
            {
                Name = GeneratedViewPageField,
                Attributes = MemberAttributes.Private,
                Type = new CodeTypeReference(typeof(IViewPage))
            };
            Class.Members.Add(ViewPageField);
        }

        private void CreateMethods()
        {
            CreateControlsMethod = new CodeMemberMethod
            {
                Name = GeneratedCreateControlsMethod,
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };
            Class.Members.Add(CreateControlsMethod);

            SetupMethod = new CodeMemberMethod
            {
                Name = GeneratedSetupMethod,
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };
            SetupMethod.Parameters.Add(new CodeParameterDeclarationExpression
            {
                Name = GeneratedSetupMethodViewPageParameterName,
                Type = new CodeTypeReference(typeof(IViewPage))
            });
            SetupMethod.Parameters.Add(new CodeParameterDeclarationExpression
            {
                Name = GeneratedSetupMethodRequestParameterName,
                Type = new CodeTypeReference(typeof(HttpRequest))
            });
            SetupMethod.Parameters.Add(new CodeParameterDeclarationExpression
            {
                Name = GeneratedSetupMethodResponseParameterName,
                Type = new CodeTypeReference(typeof(HttpResponse))
            });
            Class.Members.Add(SetupMethod);
            SetupMethod.Statements.Add(new CodeAssignStatement(
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    GeneratedViewPageField
                ),
                new CodeVariableReferenceExpression(GeneratedSetupMethodViewPageParameterName)
            ));
            SetupMethod.Statements.Add(new CodeAssignStatement(
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    GeneratedRequestField
                ),
                new CodeVariableReferenceExpression(GeneratedSetupMethodRequestParameterName)
            ));
            SetupMethod.Statements.Add(new CodeAssignStatement(
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    GeneratedResponseField
                ),
                new CodeVariableReferenceExpression(GeneratedSetupMethodResponseParameterName)
            ));

            InitMethod = new CodeMemberMethod
            {
                Name = GeneratedInitMethod,
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };
            Class.Members.Add(InitMethod);

            RenderMethod = new CodeMemberMethod
            {
                Name = GeneratedRenderMethod,
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };
            RenderMethod.Parameters.Add(new CodeParameterDeclarationExpression
            {
                Name = GeneratedRenderMethodWriterParameterName,
                Type = new CodeTypeReference(typeof(HtmlTextWriter))
            });
            Class.Members.Add(RenderMethod);

            DisposeMethod = new CodeMemberMethod
            {
                Name = GeneratedDisposeMethod,
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };
            Class.Members.Add(DisposeMethod);

            CodeMethodInvokeExpression invokeViewRender = new CodeMethodInvokeExpression(
                new CodePropertyReferenceExpression(
                    new CodeThisReferenceExpression(),
                    GeneratedViewPageField
                ),
                "Render",
                new CodeVariableReferenceExpression(GeneratedRenderMethodWriterParameterName)
            );
            RenderMethod.Statements.Add(invokeViewRender);
        }

        public void FinalizeClass()
        {
            CodeMethodInvokeExpression invokeViewInit = new CodeMethodInvokeExpression(
                new CodePropertyReferenceExpression(
                    new CodeThisReferenceExpression(),
                    GeneratedViewPageField
                ),
                "OnInit"
            );
            InitMethod.Statements.Add(invokeViewInit);
        }
    }
}
