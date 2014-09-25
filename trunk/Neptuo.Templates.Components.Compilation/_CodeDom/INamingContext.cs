using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// Defines object that has <see cref="INaming"/>.
    /// </summary>
    public interface INamingContext
    {
        INaming Naming { get; }
    }
}
