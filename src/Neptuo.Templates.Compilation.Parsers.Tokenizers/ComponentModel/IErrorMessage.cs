﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers.ComponentModel
{
    /// <summary>
    /// Describes error message.
    /// </summary>
    public interface IErrorMessage
    {
        /// <summary>
        /// Text value of error message.
        /// </summary>
        string Text { get; }
    }
}
