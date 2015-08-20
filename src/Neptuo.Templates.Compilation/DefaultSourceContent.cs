using Neptuo.Text.Positions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// Default implementation of <see cref="ISourceContent"/>
    /// </summary>
    public class DefaultSourceContent : ISourceContent
    {
        public string TextContent { get; private set; }
        public IDocumentPoint GlobalSourceInfo { get; private set; }

        public DefaultSourceContent(TextReader content, IDocumentPoint globalSourceInfo)
        {
            Ensure.NotNull(content, "content");
            Ensure.NotNull(globalSourceInfo, "globalSourceInfo");
            TextContent = content.ReadToEnd();
            GlobalSourceInfo = globalSourceInfo;
        }

        public DefaultSourceContent(TextReader content)
            : this(content, new DefaultSourceLineInfo(0, 0))
        { }

        public DefaultSourceContent(string textContent, IDocumentPoint globalSourceInfo)
        {
            Ensure.NotNullOrEmpty(textContent, "content");
            Ensure.NotNull(globalSourceInfo, "globalSourceInfo");
            GlobalSourceInfo = globalSourceInfo;
            TextContent = textContent;
        }
        
        public DefaultSourceContent(string textContent)
            : this(textContent, new DefaultSourceLineInfo(0, 0))
        { }
    }
}
