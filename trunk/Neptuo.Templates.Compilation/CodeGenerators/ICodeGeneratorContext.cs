using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public interface ICodeGeneratorContext
    {
        ICodeGeneratorService CodeGeneratorService { get; }
        IDependencyProvider DependencyProvider { get; }
        ICollection<IErrorInfo> Errors { get; }
        TextWriter Output { get; }
    }
}
