﻿using Neptuo.ComponentModel;
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
        public ISourceLineInfo GlobalSourceInfo { get; private set; }

        public DefaultSourceContent(TextReader content, ISourceLineInfo globalSourceInfo)
        {
            Guard.NotNull(content, "content");
            Guard.NotNull(globalSourceInfo, "globalSourceInfo");
            TextContent = content.ReadToEnd();
            GlobalSourceInfo = globalSourceInfo;
        }

        public DefaultSourceContent(TextReader content)
            : this(content, new DefaultSourceLineInfo(0, 0))
        { }

        public DefaultSourceContent(string textContent, ISourceLineInfo globalSourceInfo)
        {
            Guard.NotNullOrEmpty(textContent, "content");
            Guard.NotNull(globalSourceInfo, "globalSourceInfo");
            GlobalSourceInfo = globalSourceInfo;
            TextContent = textContent;
        }
        
        public DefaultSourceContent(string textContent)
            : this(textContent, new DefaultSourceLineInfo(0, 0))
        { }
    }
}