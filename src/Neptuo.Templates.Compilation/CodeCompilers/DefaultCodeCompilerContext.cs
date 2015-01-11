using Neptuo.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeCompilers
{
    /// <summary>
    /// Implementation of <see cref="ICodeCompilerContext"/>
    /// </summary>
    public class DefaultCodeCompilerContext : ICodeCompilerContext
    {
        public ICodeCompilerService CodeCompilerService { get; private set; }
        public IDependencyProvider DependencyProvider { get; private set; }
        public ICollection<IErrorInfo> Errors { get; private set; }

        public DefaultCodeCompilerContext(ICodeCompilerService codeCompilerService, ICodeCompilerServiceContext context)
        {
            Guard.NotNull(codeCompilerService, "codeCompilerService");
            Guard.NotNull(context, "context");
            CodeCompilerService = codeCompilerService;
            DependencyProvider = context.DependencyProvider;
            Errors = context.Errors;
        }
    }
}
