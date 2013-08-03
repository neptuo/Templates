using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators.Extensions.CodeDom
{
    public class BaseStructureExtension : IBaseStructureExtension
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

        public BaseStructure GenerateCode(BaseStructureExtensionContext context)
        {
            BaseStructure baseStructure = new BaseStructure();
            CreateCodeUnit(baseStructure);
            CreateCodeClass(baseStructure);
            CreateCodeMethods(baseStructure);
            return baseStructure;
        }

        private void CreateCodeUnit(BaseStructure context)
        {
            context.Unit = new CodeCompileUnit();
        }

        private void CreateCodeClass(BaseStructure context)
        {
            CodeNamespace codeNamespace = new CodeNamespace(Names.CodeNamespace);
            codeNamespace.Imports.Add(new CodeNamespaceImport("System"));
            codeNamespace.Imports.Add(new CodeNamespaceImport("Neptuo.Web"));
            codeNamespace.Imports.Add(new CodeNamespaceImport("Neptuo.Templates"));
            context.Unit.Namespaces.Add(codeNamespace);

            context.Class = new CodeTypeDeclaration(Names.ClassName);
            context.Class.IsClass = true;
            context.Class.TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed;
            context.Class.BaseTypes.Add(new CodeTypeReference(typeof(BaseGeneratedView)));
            context.Class.BaseTypes.Add(new CodeTypeReference(typeof(IGeneratedView)));
            context.Class.BaseTypes.Add(new CodeTypeReference(typeof(IDisposable)));
            codeNamespace.Types.Add(context.Class);
        }

        private void CreateCodeMethods(BaseStructure context)
        {
            context.EntryPointMethod = new CodeMemberMethod
            {
                Name = Names.CreateViewPageControlsMethod,
                Attributes = MemberAttributes.Override | MemberAttributes.Family
            };
            context.Class.Members.Add(context.EntryPointMethod);
        }
    }

}
