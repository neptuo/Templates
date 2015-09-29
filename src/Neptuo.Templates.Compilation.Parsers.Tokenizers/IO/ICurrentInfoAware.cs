﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers.IO
{
    /// <summary>
    /// Provides information about current position.
    /// </summary>
    public interface ICurrentInfoAware
    {
        int Position { get; }
        int LineIndex { get; }
        int ColumnIndex { get; }
    }
}