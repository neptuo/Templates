using Neptuo.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes
{
    /// <summary>
    /// Raised when not supported token type was found at some position.
    /// </summary>
    public class InvalidTokenTypeException : NodeException
    {
        public Token FoundToken { get; private set; }
        public IEnumerable<TokenType> SupportedTypes {get; private set;}

        public InvalidTokenTypeException(Token foundToken, params TokenType[] supportedTypes)
            : base(String.Format("Found token of type '{0}', but supported types are '{1}'.", foundToken.Type.UniqueName, String.Join(", ", supportedTypes.Select(t => t.UniqueName))))
        {
            Ensure.NotNull(foundToken, "foundToken");
            Ensure.NotNull(supportedTypes, "supportedTypes");
            FoundToken = foundToken;
            SupportedTypes = supportedTypes;
        }
    }
}
