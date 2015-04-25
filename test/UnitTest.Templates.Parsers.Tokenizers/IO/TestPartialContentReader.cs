using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neptuo.Templates.Compilation.Parsers.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Templates.Parsers.Tokenizers.IO
{
    [TestClass]
    public class TestPartialContentReader : TestBase
    {
        [TestMethod]
        public void PartialContentReader_ValidEscape()
        {
            IContentReader contentReader = new PartialContentReader(new StringContentReader("abc.ddd"), '.');

            AssertAreEqual(contentReader.Current, StringContentReader.NullChar);
            AssertAreEqual(contentReader.Position, -1);

            AssertAreEqual(contentReader.Next(), true);
            AssertAreEqual(contentReader.Next(), true);
            AssertAreEqual(contentReader.Next(), true);

            AssertAreEqual(contentReader.Current, 'c');
            AssertAreEqual(contentReader.Position, 2);

            AssertAreEqual(contentReader.Next(), false);

            AssertAreEqual(contentReader.Current, StringContentReader.NullChar);
            AssertAreEqual(contentReader.Position, 3);
        }
    }
}
