using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Default implementation of <see cref="ISourceContent"/>
    /// </summary>
    public class DefaultSourceContent : ISourceContent
    {
        public TextReader Content { get; private set; }
        public ISourceLineInfo GlobalSourceInfo { get; private set; }

        public DefaultSourceContent(TextReader content, ISourceLineInfo globalSourceInfo)
        {
            Guard.NotNull(content, "content");
            Guard.NotNull(globalSourceInfo, "globalSourceInfo");
            Content = content;
            GlobalSourceInfo = globalSourceInfo;
        }
    }
}
