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
    public class DefaultCodeDomStructure : ICodeDomStructure
    {
        public CodeCompileUnit Unit { get; private set; }
        public CodeTypeDeclaration Class { get; private set; }
        public CodeConstructor Constructor { get; private set; }
        public CodeEntryPointMethod EntryPoint { get; private set; }

        public DefaultCodeDomStructure(CodeCompileUnit unit, CodeTypeDeclaration classDeclaration, CodeConstructor constructor, CodeEntryPointMethod entryPoint)
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
