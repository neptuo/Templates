using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomStructureGenerator : ICodeDomStructureGenerator
    {
        public static partial class Names
        {
            public const string CodeNamespace = "Neptuo.Templates";
            public const string ComponentManagerField = "componentManager";
            public const string DependencyProviderField = "dependencyProvider";

            public const string CreateViewPageControlsMethod = "CreateViewPageControls";
        }

        public BaseCodeDomStructure GenerateCode(CodeDomStructureContext context)
        {
            BaseCodeDomStructure baseStructure = new BaseCodeDomStructure();
            CreateCodeUnit(baseStructure, context);
            CreateCodeClass(baseStructure, context);
            CreateCodeMethods(baseStructure, context);
            return baseStructure;
        }

        private void CreateCodeUnit(BaseCodeDomStructure structure, CodeDomStructureContext context)
        {
            structure.Unit = new CodeCompileUnit();
        }

        private void CreateCodeClass(BaseCodeDomStructure structure, CodeDomStructureContext context)
        {
            CodeNamespace codeNamespace = new CodeNamespace(Names.CodeNamespace);
            //codeNamespace.Imports.Add(new CodeNamespaceImport("System"));
            //codeNamespace.Imports.Add(new CodeNamespaceImport("Neptuo.Templates"));
            structure.Unit.Namespaces.Add(codeNamespace);

            structure.Class = new CodeTypeDeclaration(context.Naming.ClassName);
            structure.Class.IsClass = true;
            structure.Class.TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed;
            structure.Class.BaseTypes.Add(new CodeTypeReference(typeof(BaseGeneratedView)));
            structure.Class.BaseTypes.Add(new CodeTypeReference(typeof(IDisposable)));
            codeNamespace.Types.Add(structure.Class);
        }

        private void CreateCodeMethods(BaseCodeDomStructure structure, CodeDomStructureContext context)
        {
            structure.EntryPointMethod = new CodeMemberMethod
            {
                Name = Names.CreateViewPageControlsMethod,
                Attributes = MemberAttributes.Override | MemberAttributes.Family
            };
            structure.Class.Members.Add(structure.EntryPointMethod);
        }
    }

}
