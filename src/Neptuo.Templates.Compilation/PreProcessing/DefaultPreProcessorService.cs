using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.PreProcessing
{
    /// <summary>
    /// Implementation of <see cref="IPreProcessorService"/>.
    /// </summary>
    public class DefaultPreProcessorService : IPreProcessorService
    {
        private readonly HashSet<IVisitor> visitors = new HashSet<IVisitor>();

        public void AddVisitor(IVisitor visitor)
        {
            Ensure.NotNull(visitor, "visitor");
            visitors.Add(visitor);
        }

        public void Process(ICodeObject codeObject, IPreProcessorServiceContext context)
        {
            Ensure.NotNull(codeObject, "codeObject");
            Ensure.NotNull(context, "context");

            foreach (IVisitor visitor in visitors)
                visitor.Visit(codeObject, context.CreateVisitorContext(this));
        }
    }
}
