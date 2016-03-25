using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Descriptors.Features
{
    /// <summary>
    /// The default implementation of <see cref="IName"/>.
    /// </summary>
    public class DefaultName : IName
    {
        public string Prefix { get; private set; }
        public string Name { get; private set; }

        public DefaultName(string prefix, string name)
        {
            Prefix = prefix;
            Name = name;
        }
    }
}
