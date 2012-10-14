using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    partial class ExtensionValueParser
    {
        public class ExtensionCodeObject : CodeObject
        {
            public Type Type { get; set; }

            public ExtensionCodeObject(Type type)
            {
                Type = type;
            }

            public override void Generate(ICodeGenerator codeGenerator, ICodeObjectContext context)
            {
                throw new NotImplementedException();
            }
        }
    }
}
