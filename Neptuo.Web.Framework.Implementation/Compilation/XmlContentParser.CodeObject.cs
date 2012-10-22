using Neptuo.Web.Framework.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    partial class XmlContentParser
    {
        /// <summary>
        /// Hodnota jako literal, buď LiteralControl nebo string.
        /// </summary>
        public class LiteralCodeObject : ControlCodeObject
        {
            public LiteralTypeDescriptor Descriptor { get; set; }
            public string Value { get; set; }

            public LiteralCodeObject(LiteralTypeDescriptor descriptor, string value)
                : base(descriptor.Type)
            {
                Descriptor = descriptor;
                Value = value;
            }

            public override void Generate(CodeDomGenerator codeGenerator, ICodeObjectContext context)
            {
                throw new NotImplementedException();
                //base.Generate(codeGenerator, context);
            }
        }
    }
}
