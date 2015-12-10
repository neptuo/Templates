using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Templates.Parsers.Tokenizers
{
    public abstract class TestBase
    {
        [DebuggerStepThrough]
        protected void AssertAreEqual(object value, object expected)
        {
            Assert.AreEqual(expected, value);
        }
    }
}
