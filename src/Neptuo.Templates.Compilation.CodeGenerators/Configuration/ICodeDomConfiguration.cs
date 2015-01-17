using Neptuo.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Configuration for <see cref="CodeDomGenerator"/> and sub generators.
    /// </summary>
    public interface ICodeDomConfiguration : IReadOnlyKeyValueCollection
    {
    }
}
