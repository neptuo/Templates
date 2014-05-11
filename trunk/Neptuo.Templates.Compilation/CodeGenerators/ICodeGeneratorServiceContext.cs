using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Context object for execution in <see cref="ICodeGeneratorService"/>.
    /// </summary>
    public interface ICodeGeneratorServiceContext
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
        /// Writer to write generated code to.
        /// </summary>
        TextWriter Output { get; }

        /// <summary>
        /// Factory method for creating <see cref="ICodeGeneratorContext"/>.
        /// </summary>
        /// <param name="service">Current generator service.</param>
        /// <returns><see cref="ICodeGeneratorContext"/>.</returns>
        ICodeGeneratorContext CreateGeneratorContext(ICodeGeneratorService service);
    }
}
