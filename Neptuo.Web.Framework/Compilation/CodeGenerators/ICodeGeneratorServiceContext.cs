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
        TextWriter Output { get; }
    }
}
