using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    partial class BaseParser
    {
        public class CodeObjectContext : ICodeObjectContext
        {
            public ICodeObject ParentObject { get; protected set; }

            public CodeObjectContext(ICodeObject parentObject)
            {
                ParentObject = parentObject;
            }
        }
    }
}
