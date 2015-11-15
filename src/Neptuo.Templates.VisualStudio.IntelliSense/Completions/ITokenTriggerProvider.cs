using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Completions
{
    /// <summary>
    /// Completion start and commit token triggers provider.
    /// </summary>
    public interface ITokenTriggerProvider
    {
        /// <summary>
        /// Returns the enumeration of token triggers for starting completion.
        /// </summary>
        /// <returns>The enumeration of token triggers for starting completion.</returns>
        IEnumerable<ITokenTrigger> GetStartTriggers();

        /// <summary>
        /// Returns the enumeration of token triggers for committing completion.
        /// </summary>
        /// <returns>The enumeration of token triggers for committing completion.</returns>
        IEnumerable<ITokenTrigger> GetCommitTriggers();
    }
}
