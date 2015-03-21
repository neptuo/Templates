using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neptuo.Templates.Compilation.Parsers.Tokenizers.IO;

namespace UnitTest.Templates.Parsers.Tokenizers
{
    [TestClass]
    public class TestStringContentReader
    {
        [TestMethod]
        public void ReadingValue()
        {
            IContentReader contentReader = new StringContentReader("value");

            Assert.AreEqual(contentReader.Current, StringContentReader.NullChar);
            Assert.AreEqual(contentReader.Next(), true);
            Assert.AreEqual(contentReader.Current, 'v');
            Assert.AreEqual(contentReader.Next(), true);
            Assert.AreEqual(contentReader.Current, 'a');
            Assert.AreEqual(contentReader.Next(), true);
            Assert.AreEqual(contentReader.Current, 'l');
            Assert.AreEqual(contentReader.Next(), true);
            Assert.AreEqual(contentReader.Current, 'u');
            Assert.AreEqual(contentReader.Next(), true);
            Assert.AreEqual(contentReader.Current, 'e');
            Assert.AreEqual(contentReader.Next(), false);
            Assert.AreEqual(contentReader.Current, StringContentReader.NullChar);
        }

        [TestMethod]
        public void ReadingNull()
        {
            IContentReader contentReader = new StringContentReader(null);

            Assert.AreEqual(contentReader.Current, StringContentReader.NullChar);
            Assert.AreEqual(contentReader.Next(), false);
        }
    }
}
