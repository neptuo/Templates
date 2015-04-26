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
    public class TestPartialReader : TestBase
    {
        [TestMethod]
        public void PartialContentReader_ContentWithoutEscape()
        {
            IContentReader contentReader = new PartialReader(new StringReader("abc.ddd"), c => c == '.');

            AssertAreEqual(contentReader.Current, StringReader.NullChar);
            AssertAreEqual(contentReader.Position, -1);

            AssertAreEqual(contentReader.Next(), true);
            AssertAreEqual(contentReader.Next(), true);
            AssertAreEqual(contentReader.Next(), true);

            AssertAreEqual(contentReader.Current, 'c');
            AssertAreEqual(contentReader.Position, 2);

            AssertAreEqual(contentReader.Next(), false);

            AssertAreEqual(contentReader.Current, StringReader.NullChar);
            AssertAreEqual(contentReader.Position, 3);
        }

        [TestMethod]
        public void PartialContentReader_ContentWithEscape()
        {
            IContentReader contentReader = new PartialReader(new StringReader("abc\\.ddd"), c => c == '.', '\\');

            AssertAreEqual(contentReader.Current, StringReader.NullChar);
            AssertAreEqual(contentReader.Position, -1);

            AssertAreEqual(contentReader.Next(), true);
            AssertAreEqual(contentReader.Next(), true);
            AssertAreEqual(contentReader.Next(), true);
            AssertAreEqual(contentReader.Current, 'c');
            AssertAreEqual(contentReader.Position, 2);

            AssertAreEqual(contentReader.Next(), true);
            AssertAreEqual(contentReader.Current, '\\');
            AssertAreEqual(contentReader.Position, 3);

            AssertAreEqual(contentReader.Next(), true);
            AssertAreEqual(contentReader.Current, '.');
            AssertAreEqual(contentReader.Position, 4);

            AssertAreEqual(contentReader.Next(), true);
            AssertAreEqual(contentReader.Current, 'd');
            AssertAreEqual(contentReader.Position, 5);
        }

        [TestMethod]
        public void PartialContentReader_ContentWithOffset()
        {
            IContentReader source = new StringReader("abc.ddd");
            source.Next();
            source.Next();
            IContentReader contentReader = new PartialReader(source, c => c == '.', '\\');

            AssertAreEqual(contentReader.Current, StringReader.NullChar);
            AssertAreEqual(contentReader.Position, -1);

            AssertAreEqual(contentReader.Next(), true);
            AssertAreEqual(contentReader.Current, 'b');
            AssertAreEqual(contentReader.Position, 1);

            AssertAreEqual(contentReader.Next(), true);
            AssertAreEqual(contentReader.Current, 'c');
            AssertAreEqual(contentReader.Position, 2);

            AssertAreEqual(contentReader.Next(), false);
            AssertAreEqual(contentReader.Current, StringReader.NullChar);
            AssertAreEqual(contentReader.Position, 3);


        }

    }
}
