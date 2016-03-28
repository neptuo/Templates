using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes
{
    /// <summary>
    /// Raised when unexpected end for token list is found.
    /// </summary>
    public class MissingNextTokenException : NodeException
    {
        public MissingNextTokenException()
            : base("Unexcepted end to token list.")
        { }
    }
}
