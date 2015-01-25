using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Comment statement generator for <see cref="CommentCodeObject"/>.
    /// </summary>
    public class CodeDomCommentObjectGenerator : CodeDomObjectGeneratorBase<CommentCodeObject>
    {
        protected override ICodeDomObjectResult Generate(ICodeDomObjectContext context, CommentCodeObject codeObject)
        {
            return new CodeDomDefaultObjectResult(new CodeCommentStatement(codeObject.CommentText));
        }
    }
}
