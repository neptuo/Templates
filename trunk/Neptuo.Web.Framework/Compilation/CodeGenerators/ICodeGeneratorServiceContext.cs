using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators
{
    public interface ICodeGeneratorServiceContext
    {
        IServiceProvider ServiceProvider { get; }
        TextWriter Output { get; }
    }
}
