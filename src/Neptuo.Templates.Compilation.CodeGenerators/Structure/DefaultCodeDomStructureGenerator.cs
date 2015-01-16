using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class DefaultCodeDomStructureGenerator : ICodeDomStructureGenerator
    {
        public static partial class Names
        {
            public const string CodeNamespace = "Neptuo.Templates";
            public const string ComponentManagerField = "componentManager";
            public const string DependencyProviderField = "dependencyProvider";
            public const string EntryPointFieldName = "view";

            public const string CreateViewPageControlsMethod = "BindView";
            public const string CreateValueExtensionContextMethod = "CreateValueExtensionContext";
            public const string CastValueToMethod = "CastValueTo";
        }

        public CodeTypeReference BaseType { get; set; }
        public IList<CodeTypeReference> ImplementedInterfaces { get; private set; }

        public DefaultCodeDomStructureGenerator()
        {
            ImplementedInterfaces = new List<CodeTypeReference>();
        }

        public ICodeDomStructure Generate(ICodeGeneratorContext context)
        {
            DefaultCodeDomStructure structure = new DefaultCodeDomStructure(context.DependencyProvider.Resolve<ICodeDomNaming>());
            CreateCodeUnit(structure, context);
            CreateCodeClass(structure, context);
            CreateCodeMethods(structure, context);
            return structure;
        }

        protected virtual void CreateCodeUnit(DefaultCodeDomStructure structure, ICodeGeneratorContext context)
        {
            structure.Unit = new CodeCompileUnit();
        }

        protected virtual void CreateCodeClass(DefaultCodeDomStructure structure, ICodeGeneratorContext context)
        {
            CodeNamespace codeNamespace = new CodeNamespace(structure.Naming.NamespaceName);
            //codeNamespace.Imports.Add(new CodeNamespaceImport("System"));
            //codeNamespace.Imports.Add(new CodeNamespaceImport("Neptuo"));
            //codeNamespace.Imports.Add(new CodeNamespaceImport("Neptuo.Templates"));
            structure.Unit.Namespaces.Add(codeNamespace);

            structure.Class = new CodeTypeDeclaration(structure.Naming.ClassName);
            structure.Class.IsClass = true;
            structure.Class.TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed;
            SetBaseTypes(structure.Class);
            codeNamespace.Types.Add(structure.Class);
        }

        protected virtual void SetBaseTypes(CodeTypeDeclaration typeDeclaration)
        {
            if (BaseType != null)
                typeDeclaration.BaseTypes.Add(BaseType);

            foreach (CodeTypeReference implementedInterface in ImplementedInterfaces)
                typeDeclaration.BaseTypes.Add(implementedInterface);
        }

        protected virtual void CreateCodeMethods(DefaultCodeDomStructure structure, ICodeGeneratorContext context)
        {
            structure.EntryPoint = CreateEntryPoint(structure.Naming, context);
            structure.Class.Members.Add(structure.EntryPoint);
        }

        protected virtual CodeMemberMethod CreateEntryPoint(ICodeDomNaming naming, ICodeGeneratorContext context)
        {
            CodeMemberMethod entryPoint = new CodeMemberMethod
            {
                Name = Names.CreateViewPageControlsMethod,
                Attributes = MemberAttributes.Override | MemberAttributes.Family
            };
            entryPoint.Parameters.Add(new CodeParameterDeclarationExpression(BaseType, Names.EntryPointFieldName));
            return entryPoint;
        }
    }
}
