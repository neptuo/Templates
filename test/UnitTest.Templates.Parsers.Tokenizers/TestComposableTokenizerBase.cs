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

        protected void AssertLength(IList<ComposableToken> tokens, int count)
        {
            AssertAreEqual(tokens.Count, count);
        }

        protected void AssertTokens(IList<ComposableToken> tokens, params string[] values)
        {
            int positionIndex = 0;

            AssertLength(tokens, values.Length);
            for (int i = 0; i < values.Length; i++)
            {
                AssertAreEqual(values[i], tokens[i].Text);

                if (tokens[i].IsVirtual)
                {
                    AssertAreEqual(null, tokens[i].ContentInfo);
                }
                else
                {
                    AssertAreEqual(positionIndex, tokens[i].ContentInfo.StartIndex);
                    AssertAreEqual(values[i].Length, tokens[i].ContentInfo.Length);
                    positionIndex += tokens[i].ContentInfo.Length;
                }
            }
        }

        protected void AssertTokenTypes(IList<ComposableToken> tokens, params ComposableTokenType[] types)
        {
            AssertLength(tokens, types.Length);
            for (int i = 0; i < types.Length; i++)
            {
                AssertAreEqual(types[i], tokens[i].Type);
            }
        }

        protected void AssertContentInfo(IList<ComposableToken> tokens, params ITextPoint[] values)
        {
            AssertLength(tokens, values.Length);
            for (int i = 0; i < values.Length; i++)
            {
                Assert.IsNotNull(tokens[i].ContentInfo);
                AssertAreEqual(tokens[i].ContentInfo.StartIndex, values[i].StartIndex);
                AssertAreEqual(tokens[i].ContentInfo.Length, values[i].Length);
            }
        }

        protected ITextPoint CreateContentInfo(int startIndex, int length)
        {
            return new DefaultTextPoint(startIndex, length);
        }
    }
}
