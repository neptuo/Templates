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
            public const string CreateValueExtensionContextMethod = "CreateValueExtensionContext";
            public const string CastValueToMethod = "CastValueTo";
        }

        public CodeDomStructure GenerateCode(CodeDomStructureContext context)
        {
            CodeDomStructure baseStructure = new CodeDomStructure();
            CreateCodeUnit(baseStructure, context);
            CreateCodeClass(baseStructure, context);
            CreateCodeMethods(baseStructure, context);
            return baseStructure;
        }

        protected virtual void CreateCodeUnit(CodeDomStructure structure, CodeDomStructureContext context)
        {
            structure.Unit = new CodeCompileUnit();
        }

        protected virtual void CreateCodeClass(CodeDomStructure structure, CodeDomStructureContext context)
        {
            CodeNamespace codeNamespace = new CodeNamespace(Names.CodeNamespace);
            //codeNamespace.Imports.Add(new CodeNamespaceImport("System"));
            //codeNamespace.Imports.Add(new CodeNamespaceImport("Neptuo.Templates"));
            structure.Unit.Namespaces.Add(codeNamespace);

            structure.Class = new CodeTypeDeclaration(context.Naming.ClassName);
            structure.Class.IsClass = true;
            structure.Class.TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed;
            SetBaseTypes(structure.Class);
            codeNamespace.Types.Add(structure.Class);
        }

        protected virtual void SetBaseTypes(CodeTypeDeclaration typeDeclaration)
        {
            typeDeclaration.BaseTypes.Add(new CodeTypeReference(typeof(GeneratedView)));
            typeDeclaration.BaseTypes.Add(new CodeTypeReference(typeof(IDisposable)));
        }

        protected virtual void CreateCodeMethods(CodeDomStructure structure, CodeDomStructureContext context)
        {
            structure.EntryPointMethod = new CodeMemberMethod
            {
                Name = Names.CreateViewPageControlsMethod,
                Attributes = MemberAttributes.Override | MemberAttributes.Family
            };
            structure.EntryPointMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(IViewPage), "viewPage"));
            structure.Class.Members.Add(structure.EntryPointMethod);
        }
    }

}
