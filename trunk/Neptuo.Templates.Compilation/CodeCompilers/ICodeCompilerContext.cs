using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeCompilers
{
    /// <summary>
    /// Context for code compilation.
    /// </summary>
    public interface ICodeCompilerContext
    {
        /// <summary>
        /// Current compiler service.
        /// </summary>
        ICodeCompilerService CodeCompilerService { get; }

        /// <summary>
        /// Current dependency provider.
        /// </summary>
        IDependencyProvider DependencyProvider { get; }

        /// <summary>
        /// List of error messsages.
        /// </summary>
        ICollection<IErrorInfo> Errors { get; }
    }
}
