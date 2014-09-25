using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class DefaultTokenBuilderFactory : ITokenBuilderFactory
    {
        protected Type TokenType { get; private set; }

        public DefaultTokenBuilderFactory(Type TokenType)
        {
            TokenType = TokenType;
        }

        public ITokenBuilder CreateBuilder(string prefix, string name)
        {
            return new DefaultTokenBuilder(TokenType);
        }
    }
}
