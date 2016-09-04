using Neptuo.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Completions
{
    /// <summary>
    /// Default implementation of <see cref="ITokenTrigger"/>.
    /// </summary>
    public class DefaultTokenTrigger : ITokenTrigger
    {
        public TokenType Type { get; private set; }
        public bool IsValueReplaced { get; private set; }

        public DefaultTokenTrigger(TokenType type, bool isValueReplaced)
        {
            Type = type;
            IsValueReplaced = isValueReplaced;
        }
    }
}
