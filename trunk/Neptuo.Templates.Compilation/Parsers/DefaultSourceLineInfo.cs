﻿using Neptuo.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Default (standalone) implementation of <see cref="ISourceLineInfo"/>.
    /// </summary>
    public class DefaultSourceLineInfo : ISourceLineInfo
    {
        public int LineIndex { get; private set; }
        public int ColumnIndex { get; private set; }

        public DefaultSourceLineInfo(int lineIndex, int columnIndex)
        {
            LineIndex = lineIndex;
            ColumnIndex = columnIndex;
        }
    }
}
