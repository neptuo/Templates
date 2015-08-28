using Neptuo.Activators;
using Neptuo.Compilers.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    /// <summary>
    /// Describes context for <see cref="ITokenizer"/>.
    /// </summary>
    public interface ITokenizerContext
    {
        /// <summary>
        /// Current dependency provider.
        /// </summary>
        IDependencyProvider DependencyProvider { get; }

        /// <summary>
        /// Collection of errors.
        /// </summary>
        ICollection<IErrorInfo> Errors { get; }
    }
}
