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

namespace Neptuo.Web.Framework.CompilationOld
{
    public class BaseCodeGenerator
    {
        public const string GeneratedCodeNamespace = "Neptuo.Web.Framework.Generated";
        public const string GeneratedRequestField = "request";
        public const string GeneratedResponseField = "response";
        public const string GeneratedRequestProperty = "Request";
        public const string GeneratedResponseProperty = "Response";
        public const string GeneratedViewPageField = "viewPage";
        public const string GeneratedViewPageProperty = "ViewPage";
        public const string GeneratedCreateControlsMethod = "CreateControls";
        public const string GeneratedInitMethod = "Init";
        public const string GeneratedRenderMethod = "Render";

        public const string GeneratedRenderMethodWriterParameterName = "writer";

        public CodeCompileUnit Unit { get; protected set; }

        public CodeTypeDeclaration Class { get; protected set; }

        public CodeMemberField ViewPageField { get; protected set; }
        public CodeMemberProperty ViewPageProperty { get; protected set; }

        public CodeMemberField RequestField { get; protected set; }
        public CodeMemberProperty RequestProperty { get; protected set; }

        public CodeMemberField ResponseField { get; protected set; }
        public CodeMemberProperty ResponseProperty { get; protected set; }

        public CodeMemberMethod CreateControlsMethod { get; protected set; }

        public CodeMemberMethod InitMethod { get; protected set; }

        public CodeMemberMethod RenderMethod { get; protected set; }

        public CodeNamespace CodeNamespace { get; protected set; }

        public BaseCodeGenerator()
        {
            CreateBase();
            CreateClass();
            CreateFields();
            CreateProperties();
            CreateMethods();
        }

        public void GenerateCode(TextWriter writer)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions
            {
                BracingStyle = "C",
            };

            provider.GenerateCodeFromCompileUnit(Unit, writer, options);
            CompileCSharpCode("GeneratedView.dll");
        }

        public bool CompileCSharpCode(string exeFile)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters cp = new CompilerParameters();
            cp.ReferencedAssemblies.Add("System.dll");
            cp.ReferencedAssemblies.Add("System.Web.dll");
            cp.ReferencedAssemblies.Add("Neptuo.Web.Framework.dll");
            cp.GenerateExecutable = false;
            cp.OutputAssembly = exeFile;
            cp.GenerateInMemory = false;

            //CompilerResults cr = provider.CompileAssemblyFromFile(cp, sourceFile);
            CompilerResults cr = provider.CompileAssemblyFromDom(cp, Unit);

            if (cr.Errors.Count > 0)
            {
                // Display compilation errors.
                Console.WriteLine("Errors building {0}", cr.PathToAssembly);
                foreach (CompilerError ce in cr.Errors)
                {
                    Console.WriteLine("  {0}", ce.ToString());
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("{0} built successfully.", cr.PathToAssembly);
            }

            // Return the results of compilation.
            if (cr.Errors.Count > 0)
                return false;
            else
                return true;
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

        private void CreateProperties()
        {
            RequestProperty = new CodeMemberProperty
            {
                Name = GeneratedRequestProperty,
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Type = new CodeTypeReference(typeof(HttpRequest)),
                HasGet = true,
                HasSet = true
            };
            RequestProperty.GetStatements.Add(new CodeMethodReturnStatement(
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    GeneratedRequestField
                )
            ));
            RequestProperty.SetStatements.Add(new CodeAssignStatement(
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), GeneratedRequestField),
                new CodePropertySetValueReferenceExpression()
            ));
            Class.Members.Add(RequestProperty);


            ResponseProperty = new CodeMemberProperty
            {
                Name = GeneratedResponseProperty,
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Type = new CodeTypeReference(typeof(HttpResponse))
            };
            ResponseProperty.GetStatements.Add(new CodeMethodReturnStatement(
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    GeneratedResponseField
                )
            ));
            ResponseProperty.SetStatements.Add(new CodeAssignStatement(
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), GeneratedResponseField),
                new CodePropertySetValueReferenceExpression()
            ));
            Class.Members.Add(ResponseProperty);


            ViewPageProperty = new CodeMemberProperty
            {
                Name = GeneratedViewPageProperty,
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Type = new CodeTypeReference(typeof(IViewPage))
            };
            ViewPageProperty.GetStatements.Add(new CodeMethodReturnStatement(
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    GeneratedViewPageField
                )
            ));
            ViewPageProperty.SetStatements.Add(new CodeAssignStatement(
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), GeneratedViewPageField),
                new CodePropertySetValueReferenceExpression()
            ));
            Class.Members.Add(ViewPageProperty);
        }

        private void CreateMethods()
        {
            CreateControlsMethod = new CodeMemberMethod
            {
                Name = GeneratedCreateControlsMethod,
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };
            Class.Members.Add(CreateControlsMethod);

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


            CodeMethodInvokeExpression invokeViewInit = new CodeMethodInvokeExpression(
                new CodePropertyReferenceExpression(
                    new CodeThisReferenceExpression(),
                    GeneratedViewPageProperty
                ),
                "OnInit"
            );
            InitMethod.Statements.Add(invokeViewInit);

            CodeMethodInvokeExpression invokeViewRender = new CodeMethodInvokeExpression(
                new CodePropertyReferenceExpression(
                    new CodeThisReferenceExpression(),
                    GeneratedViewPageProperty
                ),
                "Render",
                new CodeVariableReferenceExpression(GeneratedRenderMethodWriterParameterName)
            );
            RenderMethod.Statements.Add(invokeViewRender);

            //TODO: Remove this ...
            //RenderMethod.Statements.Add(
            //    new CodeMethodInvokeExpression(
            //        new CodeVariableReferenceExpression(GeneratedRenderMethodWriterParameterName),
            //        "WriteLine",
            //        new CodePrimitiveExpression("Hello, World!")
            //    )
            //);
        }
    }
}
