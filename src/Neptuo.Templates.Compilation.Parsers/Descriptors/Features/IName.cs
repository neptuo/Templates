using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Descriptors.Features
{
    /// <summary>
    /// Descibes component's name.
    /// </summary>
    public interface IName
    {
        /// <summary>
        /// The prefix of the name.
        /// </summary>
        string Prefix { get; }

        /// <summary>
        /// The component name.
        /// </summary>
        string Name { get; }
    }
}
