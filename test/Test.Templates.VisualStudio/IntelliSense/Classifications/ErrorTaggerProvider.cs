using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
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
    [Export(typeof(ITaggerProvider))]
    [ContentType(ContentType.TextValue)]
    [TagType(typeof(ErrorTag))]
    internal class ErrorTaggerProvider : ITaggerProvider
    {
        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) 
            where T : ITag
        {
            TokenContext tokenContext = buffer.Properties.GetOrCreateSingletonProperty(() => new TokenContext(buffer));
            return (ITagger<T>)buffer.Properties.GetOrCreateSingletonProperty(() => new ErrorTagger(tokenContext, buffer));
        }
    }
}
