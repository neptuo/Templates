using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Classifications
{
    /// <summary>
    /// Collection mappings from token type to classification span name.
    /// </summary>
    public interface ITokenClassificationProvider
    {
        bool TryGet(TokenType tokenType, out string classificationName);
    }
}
