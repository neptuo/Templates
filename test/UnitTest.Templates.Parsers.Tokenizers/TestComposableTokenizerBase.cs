using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using Neptuo.Templates.Compilation.Parsers.Tokenizers.ComponentModel;
using Neptuo.Templates.Compilation.Parsers.Tokenizers.IO;
using Neptuo.Text.Positions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Templates.Parsers.Tokenizers
{
    public abstract class TestComposableTokenizerBase : TestBase
    {
        protected ComposableTokenizer CreateTokenizer()
        {
            ComposableTokenizer tokenizer = new ComposableTokenizer();
            tokenizer.Add(new AngleTokenizer());
            tokenizer.Add(new CurlyTokenizer());
            tokenizer.Add(new PlainTokenizer());
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
                    AssertAreEqual(null, tokens[i].TextSpan);
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

        protected ITextSpan CreateContentInfo(int startIndex, int length)
        {
            return new DefaultTextSpan(startIndex, length);
        }
    }
}
