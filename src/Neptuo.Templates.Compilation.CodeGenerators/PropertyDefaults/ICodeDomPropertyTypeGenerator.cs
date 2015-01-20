using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Generator for processing all unbound properties of concrete type.
    /// </summary>
    public interface ICodeDomPropertyTypeGenerator
    {
        /// <summary>
        /// Generates expression for unbound property <paramref name="propertyInfo" />.
        /// </summary>
        /// <param name="context">Generator context.</param>
        /// <param name="propertyInfo">Unbound property to provide value for.</param>
        /// <returns>Expression for <paramref name="propertyInfo"/>.</returns>
        ICodeDomPropertyTypeResult Generate(ICodeDomContext context, PropertyInfo propertyInfo);
    }
}
