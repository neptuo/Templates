﻿using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers
{
    /// <summary>
    /// Describes component that passes throught reader and creates list of syntactic tokens.
    /// </summary>
    public interface ITokenizer
    {
        /// <summary>
        /// Processes <paramref name="reader"/> and creates list of syntactic tokens.
        /// </summary>
        /// <param name="reader">Input content reader.</param>
        /// <param name="context">Context for input processing.</param>
        /// <returns>List of syntactic tokens from <paramref name="reader"/>.</returns>
        IList<Token> Tokenize(IContentReader reader, ITokenizerContext context);
    }
}