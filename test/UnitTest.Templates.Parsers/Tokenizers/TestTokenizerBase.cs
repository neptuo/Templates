using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers.ComponentModel;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers.IO;
using Neptuo.Text.Positions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Templates.Parsers.Tokenizers
{
    public abstract class TestTokenizerBase : TestBase
    {
        protected DefaultTokenizer CreateTokenizer()
        {
            DefaultTokenizer tokenizer = new DefaultTokenizer();
            tokenizer.Add(new AngleTokenBuilder());
            tokenizer.Add(new CurlyTokenBuilder());
            tokenizer.Add(new LiteralTokenBuilder());
            return tokenizer;
        }

        protected IContentReader CreateContentReader(string text)
        {
            return new StringReader(text);
        }

        protected void AssertLength(IList<Token> tokens, int count)
        {
            AssertAreEqual(tokens.Count, count);
        }

        protected void AssertTokens(IList<Token> tokens, params string[] values)
        {
            int positionIndex = 0;

            AssertLength(tokens, values.Length);
            for (int i = 0; i < values.Length; i++)
            {
                AssertAreEqual(values[i], tokens[i].Text);

                if (tokens[i].IsVirtual)
                {
                    AssertAreEqual(0, tokens[i].TextSpan.Length);
                    AssertAreEqual(positionIndex, tokens[i].TextSpan.StartIndex);
                }
                else
                {
                    AssertAreEqual(positionIndex, tokens[i].TextSpan.StartIndex);
                    AssertAreEqual(values[i].Length, tokens[i].TextSpan.Length);
                    positionIndex += tokens[i].TextSpan.Length;
                }
            }
        }

        protected void AssertTokenTypes(IList<Token> tokens, params TokenType[] types)
        {
            AssertLength(tokens, types.Length);
            for (int i = 0; i < types.Length; i++)
            {
                AssertAreEqual(types[i], tokens[i].Type);
            }
        }

        protected void AssertContentInfo(IList<Token> tokens, params ITextSpan[] values)
        {
            AssertLength(tokens, values.Length);
            for (int i = 0; i < values.Length; i++)
            {
                Assert.IsNotNull(tokens[i].TextSpan);
                AssertAreEqual(tokens[i].TextSpan.StartIndex, values[i].StartIndex);
                AssertAreEqual(tokens[i].TextSpan.Length, values[i].Length);
            }
        }

        protected void AssertWithoutError(IEnumerable<Token> tokens)
        {
            foreach (Token token in tokens)
                AssertAreEqual(token.HasError, false);
        }

        protected void AssertWithoutSkipped(IEnumerable<Token> tokens)
        {
            foreach (Token token in tokens)
                AssertAreEqual(token.IsSkipped, false);
        }

        protected ITextSpan CreateContentInfo(int startIndex, int length)
        {
            return new DefaultTextSpan(startIndex, length);
        }
    }
}
