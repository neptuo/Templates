using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Default implementation of <see cref="ICodeDomStructureGenerator"/>
    /// </summary>
    public class CodeDomDefaultStructureGenerator : ICodeDomStructureGenerator
    {
        public CodeTypeReference BaseType { get; set; }
        public IList<CodeTypeReference> ImplementedInterfaces { get; private set; }

        public string EntryPointName { get; set; }
        public IList<CodeParameterDeclarationExpression> EntryPointParameters { get; private set; }

        public CodeDomDefaultStructureGenerator()
        {
            ImplementedInterfaces = new List<CodeTypeReference>();
            EntryPointParameters = new List<CodeParameterDeclarationExpression>();
        }

        public ICodeDomStructure Generate(ICodeGeneratorContext context)
        {
            CodeDomDefaultStructure structure = new CodeDomDefaultStructure(context.DependencyProvider.Resolve<ICodeDomNaming>());
            CreateCodeUnit(structure, context);
            CreateCodeClass(structure, context);
            CreateCodeMethods(structure, context);
            return structure;
        }

        protected virtual void CreateCodeUnit(CodeDomDefaultStructure structure, ICodeGeneratorContext context)
        {
            structure.Unit = new CodeCompileUnit();
        }

        protected virtual void CreateCodeClass(CodeDomDefaultStructure structure, ICodeGeneratorContext context)
        {
            CodeNamespace codeNamespace = new CodeNamespace(structure.Naming.NamespaceName);
            //codeNamespace.Imports.Add(new CodeNamespaceImport("System"));
            //codeNamespace.Imports.Add(new CodeNamespaceImport("Neptuo"));
            //codeNamespace.Imports.Add(new CodeNamespaceImport("Neptuo.Templates"));
            structure.Unit.Namespaces.Add(codeNamespace);

            structure.Class = new CodeTypeDeclaration(structure.Naming.ClassName);
            structure.Class.IsClass = true;
            structure.Class.TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed;
            SetBaseTypes(structure.Class, BaseType, ImplementedInterfaces);
            codeNamespace.Types.Add(structure.Class);
        }

        protected virtual void SetBaseTypes(CodeTypeDeclaration typeDeclaration, CodeTypeReference baseType, IList<CodeTypeReference> implementedInterfaces)
        {
            if (baseType != null)
                typeDeclaration.BaseTypes.Add(baseType);

            foreach (CodeTypeReference implementedInterface in implementedInterfaces)
                typeDeclaration.BaseTypes.Add(implementedInterface);
        }

        protected virtual void CreateCodeMethods(CodeDomDefaultStructure structure, ICodeGeneratorContext context)
        {
            structure.EntryPoint = CreateEntryPoint(structure.Naming, context);
            structure.Class.Members.Add(structure.EntryPoint);
        }

        protected virtual CodeMemberMethod CreateEntryPoint(ICodeDomNaming naming, ICodeGeneratorContext context)
        {
            if (String.IsNullOrEmpty(EntryPointName))
                throw Guard.Exception.NotImplemented("Provide entry point method name of override CreateEntryPoint method.");

            CodeMemberMethod entryPoint = new CodeMemberMethod
            {
                Name = EntryPointName,
                Attributes = MemberAttributes.Override | MemberAttributes.Family
            };

            foreach (CodeParameterDeclarationExpression parameter in EntryPointParameters)
                entryPoint.Parameters.Add(parameter);

            return entryPoint;
        }
    }
}
