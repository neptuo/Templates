using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Implementation of <see cref="ITokenBuilder"/> that for all tokens returns 'This token is not supported'.
    /// </summary>
    public class NotSupportedTokenBuilder : ITokenBuilder
    {
        public ICodeObject TryParse(ITokenBuilderContext context, Token token)
        {
            context.AddError(token, String.Format("Token '{0}' is not supported.", token.Fullname));
            return null;
        }
    }
}
