using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using Neptuo.Templates.VisualStudio.IntelliSense;
using Neptuo.Templates.VisualStudio.IntelliSense.Classifications;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.VisualStudio.IntelliSense.Classifications
{
    [Export(typeof(IClassifierProvider))]
    [ContentType(ContentType.TextValue)]
    internal class ClassifierProvider : ClassifierProviderBase
    {
        protected override ITokenizer CreateTokenizer()
        {
            return new TokenizerFactory().Create();
        }
    }
}
