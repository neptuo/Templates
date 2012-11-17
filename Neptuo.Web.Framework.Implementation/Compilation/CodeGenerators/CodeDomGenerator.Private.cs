using Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators
{
    partial class CodeDomGenerator
    {
        public static partial class Names
        {
            public const string CodeNamespace = "Neptuo.Web.Framework";
            public const string ClassName = "GeneratedView";
            public const string BaseClassName = "BaseGeneratedView";
            public const string RequestField = "request";
            public const string ResponseField = "response";
            public const string ViewPageField = "viewPage";
            public const string ComponentManagerField = "componentManager";
            public const string DependencyProviderField = "dependencyProvider";

            public const string CreateViewPageControlsMethod = "CreateViewPageControls";
        }

        public CodeCompileUnit Unit { get; private set; }
        public CodeNamespace CodeNamespace { get; private set; }
        public CodeTypeDeclaration Class { get; private set; }
        public CodeMemberMethod CreateViewPageControlsMethod { get; private set; }

        private void CreateCodeUnit()
        {
            Unit = new CodeCompileUnit();
            CodeNamespace = new CodeNamespace(Names.CodeNamespace);
            CodeNamespace.Imports.Add(new CodeNamespaceImport("System"));
            CodeNamespace.Imports.Add(new CodeNamespaceImport("Neptuo.Web"));
            CodeNamespace.Imports.Add(new CodeNamespaceImport("Neptuo.Web.Framework"));
            Unit.Namespaces.Add(CodeNamespace);
        }

        private void CreateCodeClass()
        {
            Class = new CodeTypeDeclaration(Names.ClassName);
            Class.IsClass = true;
            Class.TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed;
            Class.BaseTypes.Add(new CodeTypeReference(typeof(BaseGeneratedView)));
            Class.BaseTypes.Add(new CodeTypeReference(typeof(IGeneratedView)));
            Class.BaseTypes.Add(new CodeTypeReference(typeof(IDisposable)));
            CodeNamespace.Types.Add(Class);
        }

        private void CreateCodeMethods()
        {
            CreateViewPageControlsMethod = new CodeMemberMethod
            {
                Name = Names.CreateViewPageControlsMethod,
                Attributes = MemberAttributes.Override | MemberAttributes.Family
            };
            Class.Members.Add(CreateViewPageControlsMethod);
        }
    }
}
