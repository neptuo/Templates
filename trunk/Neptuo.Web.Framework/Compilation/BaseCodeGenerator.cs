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
using Neptuo.Web.Framework.Utils;

namespace Neptuo.Web.Framework.Compilation
{
    public class BaseCodeGenerator
    {
        public static class Names
        {
            public const string CodeNamespace = "Neptuo.Web.Framework.Generated";
            public const string ClassName = "GeneratedView";
            public const string RequestField = "request";
            public const string ResponseField = "response";
            public const string ViewPageField = "viewPage";
            public const string LivecycleObserverField = "livecycleObserver";
            public const string ServiceProviderField = "serviceProvider";
            public const string CreateControlsMethod = "CreateControls";
            public const string SetupMethod = "Setup";
            public const string InitMethod = "Init";
            public const string RenderMethod = "Render";
            public const string DisposeMethod = "Dispose";

            public const string SetupMethodViewPageParameter = "viewPage";
            public const string SetupMethodLivecycleObserverParameter = "livecycleObserver";
            public const string SetupMethodServiceProviderParameter = "serviceProvider";
            public const string SetupMethodRequestParameter = "request";
            public const string SetupMethodResponseParameter = "response";

            public const string RenderMethodWriterParameter = "writer";
        }

        public CodeCompileUnit Unit { get; protected set; }
        public CodeNamespace CodeNamespace { get; protected set; }
        public CodeTypeDeclaration Class { get; protected set; }

        public CodeMemberField ViewPageField { get; protected set; }
        public CodeMemberField LivecycleObserverField { get; protected set; }
        public CodeMemberField ServiceProviderField { get; protected set; }
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
            CodeNamespace = new CodeNamespace(Names.CodeNamespace);
            CodeNamespace.Imports.Add(new CodeNamespaceImport("System"));
            CodeNamespace.Imports.Add(new CodeNamespaceImport("Neptuo.Web"));
            CodeNamespace.Imports.Add(new CodeNamespaceImport("Neptuo.Web.Framework"));
            Unit.Namespaces.Add(CodeNamespace);
        }

        private void CreateClass()
        {
            Class = new CodeTypeDeclaration(Names.ClassName);
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
                Name = Names.RequestField,
                Attributes = MemberAttributes.Private,
                Type = new CodeTypeReference(typeof(HttpRequest))
            };
            Class.Members.Add(RequestField);

            ResponseField = new CodeMemberField
            {
                Name = Names.ResponseField,
                Attributes = MemberAttributes.Private,
                Type = new CodeTypeReference(typeof(HttpResponse))
            };
            Class.Members.Add(ResponseField);

            ViewPageField = new CodeMemberField
            {
                Name = Names.ViewPageField,
                Attributes = MemberAttributes.Private,
                Type = new CodeTypeReference(typeof(IViewPage))
            };
            Class.Members.Add(ViewPageField);

            LivecycleObserverField = new CodeMemberField
            {
                Name = Names.LivecycleObserverField,
                Attributes = MemberAttributes.Private,
                Type = new CodeTypeReference(typeof(ILivecycleObserver))
            };
            Class.Members.Add(LivecycleObserverField);

            ServiceProviderField = new CodeMemberField
            {
                Name = Names.ServiceProviderField,
                Attributes = MemberAttributes.Private,
                Type = new CodeTypeReference(typeof(IServiceProvider))
            };
            Class.Members.Add(ServiceProviderField);
        }

        private void CreateMethods()
        {
            CreateControlsMethod = new CodeMemberMethod
            {
                Name = Names.CreateControlsMethod,
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };
            Class.Members.Add(CreateControlsMethod);

            SetupMethod = new CodeMemberMethod
            {
                Name = Names.SetupMethod,
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };
            SetupMethod.Parameters.Add(new CodeParameterDeclarationExpression
            {
                Name = Names.SetupMethodViewPageParameter,
                Type = new CodeTypeReference(typeof(IViewPage))
            });
            SetupMethod.Parameters.Add(new CodeParameterDeclarationExpression
            {
                Name = Names.SetupMethodLivecycleObserverParameter,
                Type = new CodeTypeReference(typeof(ILivecycleObserver))
            });
            SetupMethod.Parameters.Add(new CodeParameterDeclarationExpression
            {
                Name = Names.SetupMethodServiceProviderParameter,
                Type = new CodeTypeReference(typeof(IServiceProvider))
            });
            SetupMethod.Parameters.Add(new CodeParameterDeclarationExpression
            {
                Name = Names.SetupMethodRequestParameter,
                Type = new CodeTypeReference(typeof(HttpRequest))
            });
            SetupMethod.Parameters.Add(new CodeParameterDeclarationExpression
            {
                Name = Names.SetupMethodResponseParameter,
                Type = new CodeTypeReference(typeof(HttpResponse))
            });
            Class.Members.Add(SetupMethod);
            SetupMethod.Statements.Add(new CodeAssignStatement(
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    Names.ViewPageField
                ),
                new CodeVariableReferenceExpression(Names.SetupMethodViewPageParameter)
            ));
            SetupMethod.Statements.Add(new CodeAssignStatement(
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    Names.LivecycleObserverField
                ),
                new CodeVariableReferenceExpression(Names.SetupMethodLivecycleObserverParameter)
            ));
            SetupMethod.Statements.Add(new CodeAssignStatement(
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    Names.ServiceProviderField
                ),
                new CodeVariableReferenceExpression(Names.SetupMethodServiceProviderParameter)
            ));
            SetupMethod.Statements.Add(new CodeAssignStatement(
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    Names.RequestField
                ),
                new CodeVariableReferenceExpression(Names.SetupMethodRequestParameter)
            ));
            SetupMethod.Statements.Add(new CodeAssignStatement(
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    Names.ResponseField
                ),
                new CodeVariableReferenceExpression(Names.SetupMethodResponseParameter)
            ));

            InitMethod = new CodeMemberMethod
            {
                Name = Names.InitMethod,
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };
            Class.Members.Add(InitMethod);

            RenderMethod = new CodeMemberMethod
            {
                Name = Names.RenderMethod,
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };
            RenderMethod.Parameters.Add(new CodeParameterDeclarationExpression
            {
                Name = Names.RenderMethodWriterParameter,
                Type = new CodeTypeReference(typeof(HtmlTextWriter))
            });
            Class.Members.Add(RenderMethod);

            DisposeMethod = new CodeMemberMethod
            {
                Name = Names.DisposeMethod,
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };
            Class.Members.Add(DisposeMethod);

            InitMethod.Statements.Add(new CodeMethodInvokeExpression(
                new CodeMethodReferenceExpression(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        CodeGenerator.Names.LivecycleObserverField
                    ),
                    TypeHelper.MethodName<ILivecycleObserver, object, Action<object>>(l => l.Register)
                ),
                new CodePrimitiveExpression(null),
                new CodeVariableReferenceExpression(ViewPageField.Name),
                new CodeVariableReferenceExpression("viewPage_Bind")
            ));

            CodeMethodInvokeExpression invokeViewRender = new CodeMethodInvokeExpression(
                new CodePropertyReferenceExpression(
                    new CodeThisReferenceExpression(),
                    Names.ViewPageField
                ),
                TypeHelper.MethodName<IViewPage, HtmlTextWriter>(p => p.Render),
                new CodeVariableReferenceExpression(Names.RenderMethodWriterParameter)
            );
            RenderMethod.Statements.Add(invokeViewRender);

            DisposeMethod.Statements.Add(new CodeMethodInvokeExpression(
                new CodePropertyReferenceExpression(
                    new CodeThisReferenceExpression(),
                    Names.ViewPageField
                ),
                TypeHelper.MethodName<IViewPage>(p => p.Dispose)
            ));
        }

        public void FinalizeClass()
        {
            CodeMethodInvokeExpression invokeViewInit = new CodeMethodInvokeExpression(
                new CodePropertyReferenceExpression(
                    new CodeThisReferenceExpression(),
                    Names.ViewPageField
                ),
                TypeHelper.MethodName<IViewPage>(p => p.OnInit)
            );
            InitMethod.Statements.Add(invokeViewInit);
        }
    }
}
