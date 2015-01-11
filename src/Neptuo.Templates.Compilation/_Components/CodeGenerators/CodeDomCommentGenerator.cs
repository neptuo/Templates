using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomCommentGenerator : TypeCodeDomObjectGenerator<CommentCodeObject>
    {
        public CodeDomCommentGenerator(IFieldNameProvider fieldNameProvider, ComponentManagerDescriptor componentManager)
            : base(fieldNameProvider, componentManager)
        { }

        protected override CodeExpression GenerateCode(CodeObjectExtensionContext context, CommentCodeObject codeObject, IPropertyDescriptor propertyDescriptor)
        {
            return null;
        }
    }
}
