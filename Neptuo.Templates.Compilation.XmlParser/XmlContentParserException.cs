using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// Exception used in <see cref="XmlContentParser"/>.
    /// </summary>
    [Serializable]
    public class XmlContentParserException : BaseSourceCodeException
    {
        public XmlContentParserException(string message, int lineNumber, int linePosition)
            : base(message, lineNumber, linePosition)
        { }
    }
}
