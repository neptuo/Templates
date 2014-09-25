using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Factory for <see cref="IPropertyBuilder"/>.
    /// </summary>
    public interface IPropertyBuilderFactory
    {
        /// <summary>
        /// Creates property builder for <paramref name="propertyInfo"/>.
        /// </summary>
        /// <param name="propertyInfo">Property info.</param>
        /// <returns>Creates property builder for <paramref name="propertyInfo"/>.</returns>
        IPropertyBuilder CreateBuilder(IPropertyInfo propertyInfo);
    }
}
