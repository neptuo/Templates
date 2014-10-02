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
            Guard.NotNullOrEmpty(name, "name");
            Guard.NotNull(compiler, "compiler");
            compilers[name] = compiler;
            return this;
        }

        public object Compile(string name, TextReader sourceCode, ICodeCompilerServiceContext context)
        {
            Guard.NotNull(sourceCode, "sourceCode");
            Guard.NotNull(context, "context");

            ICodeCompiler compiler;
            if (compilers.TryGetValue(name, out compiler))
                return compiler.Compile(sourceCode, new DefaultCodeCompilerContext(this, context));

            throw new ArgumentOutOfRangeException("name", String.Format("Requested unregistered compiler named '{0}'.", name));
        }
    }
}
