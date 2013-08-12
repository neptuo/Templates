using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class MvcStructureGenerator<T> : ICodeDomStructureGenerator
    {
        public BaseCodeDomStructure GenerateCode(CodeDomStructureContext context)
        {
            BaseCodeDomStructure structure = new BaseCodeDomStructure();

            structure.Unit = new CodeCompileUnit();

            CodeNamespace codeNamespace = new CodeNamespace("Neptuo.Templates.Mvc");
            structure.Unit.Namespaces.Add(codeNamespace);

            structure.Class = new CodeTypeDeclaration(context.Naming.ClassName);
            structure.Class.IsClass = true;
            structure.Class.TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed;
            structure.Class.BaseTypes.Add(new CodeTypeReference(typeof(WebViewPage<T>)));
            structure.Class.BaseTypes.Add(new CodeTypeReference(typeof(IDisposable)));
            codeNamespace.Types.Add(structure.Class);

            structure.EntryPointMethod = new CodeMemberMethod
            {
                Name = "Execute",
                Attributes = MemberAttributes.Override | MemberAttributes.Public
            };

            return structure;
        }
    }
}
