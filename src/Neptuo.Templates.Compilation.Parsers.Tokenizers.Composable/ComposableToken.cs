﻿using Neptuo.Templates.Compilation.Parsers.Tokenizers.ComponentModel;
using Neptuo.Text.Positions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    /// <summary>
    /// Token implementation for <see cref="ComposableTokenizer"/>.
    /// </summary>
    public class ComposableToken : IToken<ComposableTokenType>
    {
        public ComposableTokenType Type { get; set; }
        public IDocumentSpan DocumentSpan { get; set; }
        public ITextSpan TextSpan { get; set; }
        public string Text { get; set; }
        public IList<IErrorMessage> Errors { get; private set; }

        public bool IsVirtual { get; set; }
        public bool IsSkipped { get; set; }

        /// <summary>
        /// Whether this token contains errors.
        /// </summary>
        public bool HasError
        {
            get { return Errors.Count > 0; }
        }

        public ComposableToken()
        { }

        public ComposableToken(ComposableTokenType type, string text)
        {
            Ensure.NotNull(type, "type");
            Type = type;
            Text = text;
            Errors = new List<IErrorMessage>();
        }

        public override string ToString()
        {
            string position = null;
            if(IsVirtual)
                position = "<Virtual>";
            else if(TextSpan != null)
                position = TextSpan.ToString();
            else if(DocumentSpan != null)
                position = DocumentSpan.ToString();

            return String.Format("{0} {1} \"{2}\"{3}", position, Type, Text, HasError ? " !" : "");
        }
    }
}
