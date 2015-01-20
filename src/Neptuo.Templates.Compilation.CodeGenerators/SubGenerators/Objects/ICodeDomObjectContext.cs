using Neptuo.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public interface ICodeDomObjectContext : ICodeDomContext
    {
        /// <summary>
        /// Storage for custom values.
        /// </summary>
        IReadOnlyKeyValueCollection CustomValues { get; }
    }
}
