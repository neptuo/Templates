using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Completions
{
    public interface ITokenTriggerProvider
    {
        /// <summary>
        /// Returns enumeration of completion token triggers.
        /// </summary>
        /// <returns>Enumeration of completion token triggers.</returns>
        IEnumerable<ITokenTrigger> GetTriggers();
    }
}
