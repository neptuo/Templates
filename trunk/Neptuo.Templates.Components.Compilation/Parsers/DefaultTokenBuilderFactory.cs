using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Single type token builder factory.
    /// Creates instances of <see cref="DefaultTokenBuilder"/>.
    /// </summary>
    public class DefaultTokenBuilderFactory : ITokenBuilderFactory
    {
        protected Type TokenType { get; private set; }

        public DefaultTokenBuilderFactory(Type tokenType)
        {
            Guard.NotNull(tokenType, "tokenType");
            TokenType = tokenType;
        }

        public ITokenBuilder CreateBuilder(string prefix, string name)
        {
            return new DefaultTokenBuilder(TokenType);
        }
    }
}
