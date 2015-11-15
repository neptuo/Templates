using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers.ComponentModel
{
    /// <summary>
    /// Default implementation of <see cref="IErrorMessage"/>.
    /// </summary>
    public class DefaultErrorMessage : IErrorMessage
    {
        public string Text { get; private set; }

        public DefaultErrorMessage(string text)
        {
            Text = text;
        }

        public override string ToString()
        {
            return String.Format("Error: {0}.", Text);
        }
    }
}
