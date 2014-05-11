using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Describes registration of observer.
    /// </summary>
    public interface IObserverRegistration
    {
        /// <summary>
        /// Observer instance scope.
        /// </summary>
        ObserverBuilderScope Scope { get; }

        /// <summary>
        /// Creates builder.
        /// </summary>
        /// <returns>Creates builder.</returns>
        IObserverBuilder CreateBuilder();
    }
}
