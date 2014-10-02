using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Factory for <see cref="ILiteralBuilder"/>.
    /// </summary>
    public interface ILiteralBuilderFactory
    {
        /// <summary>
        /// Creates instances of <see cref="ILiteralBuilder"/>.
        /// </summary>
        /// <returns><see cref="ILiteralBuilder"/>.</returns>
        ILiteralBuilder CreateBuilder();
    }
}
