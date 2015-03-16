using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeCompilers
{
    /// <summary>
    /// Implementation of <see cref="ICodeCompilerService"/>.
    /// </summary>
    public class DefaultCodeCompilerService : ICodeCompilerService
    {
        private readonly Dictionary<string, ICodeCompiler> compilers = new Dictionary<string, ICodeCompiler>();

        public ICodeCompilerService AddCompiler(string name, ICodeCompiler compiler)
        {
            Ensure.NotNullOrEmpty(name, "name");
            Ensure.NotNull(compiler, "compiler");
            compilers[name] = compiler;
            return this;
        }

        public object Compile(string name, TextReader sourceCode, ICodeCompilerServiceContext context)
        {
            Ensure.NotNull(sourceCode, "sourceCode");
            Ensure.NotNull(context, "context");

            ICodeCompiler compiler;
            if (compilers.TryGetValue(name, out compiler))
                return compiler.Compile(sourceCode, new DefaultCodeCompilerContext(this, context));

            throw Ensure.Exception.ArgumentOutOfRange("name", "Requested an unregistered compiler named '{0}'.", name);
        }
    }
}
