using Neptuo.Activators;
using Neptuo.ComponentModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Context object for <see cref="ICodeGenerator"/>.
    /// </summary>
    public interface ICodeGeneratorContext
    {
        /// <summary>
        /// Current generator service.
        /// </summary>
        ICodeGeneratorService CodeGeneratorService { get; }

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
    }
}
