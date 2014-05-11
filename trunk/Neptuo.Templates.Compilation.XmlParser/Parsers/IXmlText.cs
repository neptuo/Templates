using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Abstraction of XML plain text.
    /// </summary>
    public interface IXmlText : IXmlNode
    {
        string Text { get; }
    }
}
