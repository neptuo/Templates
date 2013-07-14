using Neptuo.Templates.Compilation.CodeGenerators.Extensions;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    partial class CodeDomGenerator
    {
        public static partial class Names
        {
            public const string CodeNamespace = "Neptuo.Templates";
            public const string ClassName = "GeneratedView";
            public const string BaseClassName = "BaseGeneratedView";
            public const string RequestField = "request";
            public const string ResponseField = "response";
            public const string ViewPageField = "viewPage";
            public const string ComponentManagerField = "componentManager";
            public const string DependencyProviderField = "dependencyProvider";

            public const string CreateViewPageControlsMethod = "CreateViewPageControls";
        }

        private void CreateCodeUnit(Context context)
        {
            context.Unit = new CodeCompileUnit();
            context.CodeNamespace = new CodeNamespace(Names.CodeNamespace);
            context.CodeNamespace.Imports.Add(new CodeNamespaceImport("System"));
            context.CodeNamespace.Imports.Add(new CodeNamespaceImport("Neptuo.Web"));
            context.CodeNamespace.Imports.Add(new CodeNamespaceImport("Neptuo.Templates"));
            context.Unit.Namespaces.Add(context.CodeNamespace);
        }

        private void CreateCodeClass(Context context)
        {
            context.Class = new CodeTypeDeclaration(context.ClassName);
            context.Class.IsClass = true;
            context.Class.TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed;
            context.Class.BaseTypes.Add(new CodeTypeReference(typeof(BaseGeneratedView)));
            context.Class.BaseTypes.Add(new CodeTypeReference(typeof(IGeneratedView)));
            context.Class.BaseTypes.Add(new CodeTypeReference(typeof(IDisposable)));
            context.CodeNamespace.Types.Add(context.Class);
        }

        private void CreateCodeMethods(Context context)
        {
            context.CreateViewPageControlsMethod = new CodeMemberMethod
            {
                Name = Names.CreateViewPageControlsMethod,
                Attributes = MemberAttributes.Override | MemberAttributes.Family
            };
            context.Class.Members.Add(context.CreateViewPageControlsMethod);
        }
    }
}
