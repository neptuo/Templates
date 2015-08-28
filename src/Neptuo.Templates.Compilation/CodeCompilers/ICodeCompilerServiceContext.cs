using Neptuo.Activators;
using Neptuo.Compilers.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeCompilers
{
    /// <summary>
    /// Code for code compilation using <see cref="ICodeCompilerService"/>.
    /// </summary>
    public interface ICodeCompilerServiceContext
    {
        /// <summary>
        /// Current dependency provider.
        /// </summary>
        IDependencyProvider DependencyProvider { get; }

        /// <summary>
        /// List of error messsages.
        /// </summary>
        ICollection<IErrorInfo> Errors { get; }

        /// <summary>
        /// Factory method for creating <see cref="ICodeCompilerContext"/>.
        /// </summary>
        /// <param name="service">Current compiler service.</param>
        /// <returns><see cref="ICodeCompilerContext"/>.</returns>
        ICodeCompilerContext CreateCompilerContext(ICodeCompilerService service);
    }
}
