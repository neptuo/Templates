using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    partial class CodeDomGenerator
    {
        public class GeneratorHelper
        {
            public static class Names
            {
                public const string CodeNamespace = "Neptuo.Web.Framework";
                public const string ClassName = "GeneratedView";
                public const string BaseClassName = "BaseGeneratedView";
                public const string RequestField = "request";
                public const string ResponseField = "response";
                public const string ViewPageField = "viewPage";
                public const string ComponentManagerField = "componentManager";
                public const string ServiceProviderField = "serviceProvider";

                public const string CreateControlsMethod = "CreateViewPageControls";
                public const string InitMethod = "InitOverride";

                public static readonly Type BaseClassType = typeof(BaseGeneratedView);
            }

            private int fieldCount = 0;

            public CodeCompileUnit Unit { get; protected set; }
            public CodeNamespace CodeNamespace { get; protected set; }
            public CodeTypeDeclaration Class { get; protected set; }

            public CodeMemberMethod CreateControlsMethod { get; protected set; }
            public CodeMemberMethod InitMethod { get; protected set; }

            public GeneratorHelper()
            {
                CreateBase();
                CreateClass();
                CreateMethods();
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
                Class.BaseTypes.Add(new CodeTypeReference(typeof(BaseGeneratedView)));
                Class.BaseTypes.Add(new CodeTypeReference(typeof(IGeneratedView)));
                Class.BaseTypes.Add(new CodeTypeReference(typeof(IDisposable)));
                CodeNamespace.Types.Add(Class);
            }

            private void CreateMethods()
            {
                CreateControlsMethod = new CodeMemberMethod
                {
                    Name = Names.CreateControlsMethod,
                    Attributes = MemberAttributes.Override | MemberAttributes.Family
                };
                Class.Members.Add(CreateControlsMethod);

                InitMethod = new CodeMemberMethod
                {
                    Name = Names.InitMethod,
                    Attributes = MemberAttributes.Override | MemberAttributes.Family
                };
                Class.Members.Add(InitMethod);
            }

            public string GenerateFieldName()
            {
                return String.Format("field{0}", ++fieldCount);
            }
        }
    }
}
