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
    /// Implementation of <see cref="ITokenValueBuilder"/> that for all tokens returns 'This token is not supported'.
    /// </summary>
    public class TokenNotSupportedValueBuilder : ITokenValueBuilder
    {
        public ICodeObject TryParse(ITokenValueBuilderContext context, Token token)
        {
            context.AddError(token, String.Format("Token '{0}' is not supported.", token.Fullname));
            return null;
        }
    }
}
