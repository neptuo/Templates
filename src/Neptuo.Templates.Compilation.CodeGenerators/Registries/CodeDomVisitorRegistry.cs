using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Registry for <see cref="ICodeDomVisitor"/>.
    /// </summary>
    public class CodeDomVisitorRegistry : ICodeDomVisitor
    {
        private readonly List<ICodeDomVisitor> storage = new List<ICodeDomVisitor>();

        public CodeDomVisitorRegistry AddVisitor(ICodeDomVisitor visitor)
        {
            Guard.NotNull(visitor, "visitor");
            storage.Add(visitor);
            return this;
        }

        public void Visit(ICodeDomContext context)
        {
            foreach (ICodeDomVisitor visitor in storage)
                visitor.Visit(context);
        }
    }
}
