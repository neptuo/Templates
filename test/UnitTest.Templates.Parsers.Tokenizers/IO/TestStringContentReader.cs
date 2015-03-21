using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neptuo.Templates.Compilation.Parsers.Tokenizers.IO;

namespace UnitTest.Templates.Parsers.Tokenizers.IO
{
    [TestClass]
    public class TestStringContentReader : TestBase
    {
        [TestMethod]
        public void ReadingValue()
        {
            IContentReader contentReader = new StringContentReader("value");

            AssertAreEqual(contentReader.Current, StringContentReader.NullChar);
            AssertAreEqual(contentReader.Position, -1);
            AssertAreEqual(contentReader.Next(), true);
            AssertAreEqual(contentReader.Current, 'v');
            AssertAreEqual(contentReader.Position, 0);
            AssertAreEqual(contentReader.Next(), true);
            AssertAreEqual(contentReader.Current, 'a');
            AssertAreEqual(contentReader.Position, 1);
            AssertAreEqual(contentReader.Next(), true);
            AssertAreEqual(contentReader.Current, 'l');
            AssertAreEqual(contentReader.Position, 2);
            AssertAreEqual(contentReader.Next(), true);
            AssertAreEqual(contentReader.Current, 'u');
            AssertAreEqual(contentReader.Position, 3);
            AssertAreEqual(contentReader.Next(), true);
            AssertAreEqual(contentReader.Current, 'e');
            AssertAreEqual(contentReader.Position, 4);
            AssertAreEqual(contentReader.Next(), false);
            AssertAreEqual(contentReader.Current, StringContentReader.NullChar);
            AssertAreEqual(contentReader.Position, -1);
        }

        [TestMethod]
        public void ReadingNull()
        {
            IContentReader contentReader = new StringContentReader(null);

            AssertAreEqual(contentReader.Current, StringContentReader.NullChar);
            AssertAreEqual(contentReader.Position, -1);
            AssertAreEqual(contentReader.Next(), false);
            AssertAreEqual(contentReader.Current, StringContentReader.NullChar);
            AssertAreEqual(contentReader.Position, -1);
        }
    }
}
