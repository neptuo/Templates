using Neptuo.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Completions
{
    /// <summary>
    /// Defines completion token trigger.
    /// </summary>
    public interface ITokenTrigger
    {
        /// <summary>
        /// Type of token to trigger at.
        /// </summary>
        TokenType Type { get; }

        /// <summary>
        /// Whether to replace token token value or append.
        /// </summary>
        bool IsValueReplaced { get; }
    }
}
