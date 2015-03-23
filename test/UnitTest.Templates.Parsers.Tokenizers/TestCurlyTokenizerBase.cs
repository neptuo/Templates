using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using Neptuo.Templates.Compilation.Parsers.Tokenizers.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Templates.Parsers.Tokenizers
{
    public abstract class TestCurlyTokenizerBase : TestBase
    {
        protected void AssertLength(IList<CurlyToken> tokens, int count)
        {
            AssertAreEqual(tokens.Count, count);
        }

        protected void AssertText(IList<CurlyToken> tokens, params string[] values)
        {
            AssertLength(tokens, values.Length);
            for (int i = 0; i < values.Length; i++)
                AssertAreEqual(values[i], tokens[i].Text);
        }

        protected void AssertContentInfo(IList<CurlyToken> tokens, params IContentInfo[] values)
        {
            AssertLength(tokens, values.Length);
            for (int i = 0; i < values.Length; i++)
            {
                Assert.IsNotNull(tokens[i].ContentInfo);
                AssertAreEqual(tokens[i].ContentInfo.StartIndex, values[i].StartIndex);
                AssertAreEqual(tokens[i].ContentInfo.Length, values[i].Length);
            }
        }

        protected IContentInfo CreateContentInfo(int startIndex, int length)
        {
            return new DefaultContentInfo(startIndex, length);
        }
    }
}
