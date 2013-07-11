using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators
{
    public interface ICodeGeneratorServiceContext
    {
        IDependencyProvider DependencyProvider { get; }
        ICollection<IErrorInfo> Errors { get; }
        TextWriter Output { get; }
    }
}
