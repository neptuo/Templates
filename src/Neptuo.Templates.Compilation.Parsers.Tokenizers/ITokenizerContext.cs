using Neptuo.Activators;
using Neptuo.ComponentModel;
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
        /// Collection of errors.
        /// </summary>
        ICollection<IErrorInfo> Errors { get; }

        /// <summary>
        /// Current dependency provider.
        /// </summary>
        IDependencyProvider DependencyProvider { get; }
    }
}
