using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Attribute default value generator based on usage of <see cref="DefaultValueAttribute"/>.
    /// </summary>
    public class CodeDomDefaultValueAttributeGenerator : CodeDomAttributeGeneratorBase<DefaultValueAttribute>
    {
        protected override ICodeDomAttributeResult Generate(ICodeDomContext context, DefaultValueAttribute attribute)
        {
            return new CodeDomDefaultAttributeResult(new CodePrimitiveExpression(attribute.Value));
        }
    }
}
