using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Default implementation of <see cref="ICodeDomStructure"/>
    /// </summary>
    public class CodeDomDefaultStructure : ICodeDomStructure
    {
        public ICodeDomNaming Naming { get; set; }
        public CodeCompileUnit Unit { get; set; }
        public CodeTypeDeclaration Class { get; set; }
        public CodeConstructor Constructor { get; set; }
        public CodeMemberMethod EntryPoint { get; set; }

        public CodeDomDefaultStructure(ICodeDomNaming naming)
        {
            Guard.NotNull(naming, "naming");
            Naming = naming;
        }

        public CodeDomDefaultStructure(ICodeDomNaming naming, CodeCompileUnit unit, CodeTypeDeclaration classDeclaration, CodeConstructor constructor, CodeEntryPointMethod entryPoint)
            : this(naming)
        {
            Guard.NotNull(unit, "unit");
            Guard.NotNull(classDeclaration, "classDeclaration");
            Guard.NotNull(constructor, "constructor");
            Guard.NotNull(entryPoint, "entryPoint");
            Unit = unit;
            Class = classDeclaration;
            Constructor = constructor;
            EntryPoint = entryPoint;
        }
    }
}
