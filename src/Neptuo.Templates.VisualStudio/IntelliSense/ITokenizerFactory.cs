using Microsoft.VisualStudio.Text;
using Neptuo.Activators;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense
{
    public interface ITokenizerFactory : IFactory<ITokenizer, ITextBuffer>
    { }
}
