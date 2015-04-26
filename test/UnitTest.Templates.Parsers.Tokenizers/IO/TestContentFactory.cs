using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neptuo.Activators;
using Neptuo.Templates.Compilation.Parsers.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Templates.Parsers.Tokenizers.IO
{
    [TestClass]
    public class TestContentFactory : TestBase
    {
        [TestMethod]
        public void ContentFactory_BaseTest()
        {
            IContentReader source = new StringReader("abcdef");
            IActivator<IContentReader> factory = new ContentFactory(source);

            IContentReader reader1 = factory.Create();
            AssertAreEqual(reader1.Current, StringReader.NullChar);
            AssertAreEqual(source.Current, StringReader.NullChar);

            IContentReader reader2 = factory.Create();
            AssertAreEqual(reader2.Current, StringReader.NullChar);
            AssertAreEqual(source.Current, StringReader.NullChar);

            AssertAreEqual(reader1.Next(), true);
            AssertAreEqual(reader1.Current, 'a');
            AssertAreEqual(reader1.Next(), true);
            AssertAreEqual(reader1.Current, 'b');
            AssertAreEqual(source.Current, 'b');

            AssertAreEqual(reader2.Current, StringReader.NullChar);
            AssertAreEqual(reader2.Next(), true);
            AssertAreEqual(reader2.Current, 'a');
            AssertAreEqual(reader2.Next(), true);
            AssertAreEqual(reader2.Current, 'b');
            AssertAreEqual(source.Current, 'b');
            AssertAreEqual(reader2.Next(), true);
            AssertAreEqual(reader2.Current, 'c');
            AssertAreEqual(source.Current, 'c');

            AssertAreEqual(reader1.Current, 'b');
            AssertAreEqual(reader1.Next(), true);
            AssertAreEqual(reader1.Current, 'c');
            AssertAreEqual(reader1.Next(), true);
            AssertAreEqual(reader1.Current, 'd');
            AssertAreEqual(reader1.Next(), true);
            AssertAreEqual(reader1.Current, 'e');
            AssertAreEqual(reader1.Next(), true);
            AssertAreEqual(reader1.Current, 'f');
            AssertAreEqual(source.Current, 'f');
            AssertAreEqual(reader1.Next(), false);
            AssertAreEqual(reader1.Current, StringReader.NullChar);
            AssertAreEqual(source.Current, StringReader.NullChar);

            AssertAreEqual(reader2.Next(), true);
            AssertAreEqual(reader2.Current, 'd');
            AssertAreEqual(reader2.Next(), true);
            AssertAreEqual(reader2.Current, 'e');
            AssertAreEqual(reader2.Next(), true);
            AssertAreEqual(reader2.Current, 'f');
            AssertAreEqual(source.Current, StringReader.NullChar);
            AssertAreEqual(reader2.Next(), false);
            AssertAreEqual(reader2.Current, StringReader.NullChar);
            AssertAreEqual(source.Current, StringReader.NullChar);
        }
    }
}
