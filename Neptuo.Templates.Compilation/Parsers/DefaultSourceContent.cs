using Neptuo.ComponentModel;
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
        private readonly string originalContent;

        public ISourceLineInfo GlobalSourceInfo { get; private set; }

        public DefaultSourceContent(TextReader content, ISourceLineInfo globalSourceInfo)
        {
            Guard.NotNull(content, "content");
            Guard.NotNull(globalSourceInfo, "globalSourceInfo");
            GlobalSourceInfo = globalSourceInfo;
            originalContent = content.ReadToEnd();
        }

        public DefaultSourceContent(TextReader content)
            : this(content, new DefaultSourceLineInfo(0, 0))
        { }

        public DefaultSourceContent(string content, ISourceLineInfo globalSourceInfo)
            : this(new StringReader(content), globalSourceInfo)
        {
            Guard.NotNull(content, "content");
            Guard.NotNull(globalSourceInfo, "globalSourceInfo");
            GlobalSourceInfo = globalSourceInfo;
            originalContent = content;
        }
        
        public DefaultSourceContent(string content)
            : this(content, new DefaultSourceLineInfo(0, 0))
        { }

        public TextReader CreateContentReader()
        {
            return new StringReader(originalContent);
        }
    }
}
