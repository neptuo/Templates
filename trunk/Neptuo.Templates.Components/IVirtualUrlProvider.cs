using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates
{
    /// <summary>
    /// Enables to resolve url in context of current application path.
    /// </summary>
    public interface IVirtualUrlProvider
    {
        /// <summary>
        /// Resolve url in context of current application path.
        /// </summary>
        /// <param name="path">Application relative url.</param>
        /// <returns>Absolute url.</returns>
        string ResolveUrl(string path);
    }
}
