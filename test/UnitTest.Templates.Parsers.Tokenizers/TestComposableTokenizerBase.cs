using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neptuo.ComponentModel.TextOffsets;
using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using Neptuo.Templates.Compilation.Parsers.Tokenizers.ComponentModel;
using Neptuo.Templates.Compilation.Parsers.Tokenizers.IO;
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
            tokenizer.Add(new CurlyTokenizer());
            return tokenizer;
        }

        protected IContentReader CreateContentReader(string text)
        {
            return new StringContentReader(text);
        }

        protected void AssertLength(IList<ComposableToken> tokens, int count)
        {
            AssertAreEqual(tokens.Count, count);
        }

        protected void AssertText(IList<ComposableToken> tokens, params string[] values)
        {
            AssertLength(tokens, values.Length);
            for (int i = 0; i < values.Length; i++)
                AssertAreEqual(values[i], tokens[i].Text);
        }

        protected void AssertContentInfo(IList<ComposableToken> tokens, params IContentRangeInfo[] values)
        {
            AssertLength(tokens, values.Length);
            for (int i = 0; i < values.Length; i++)
            {
                Assert.IsNotNull(tokens[i].ContentInfo);
                AssertAreEqual(tokens[i].ContentInfo.StartIndex, values[i].StartIndex);
                AssertAreEqual(tokens[i].ContentInfo.Length, values[i].Length);
            }
        }

        protected IContentRangeInfo CreateContentInfo(int startIndex, int length)
        {
            return new DefaultContentRangeInfo(startIndex, length);
        }
    }
}
