using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.PreProcessing
{
    /// <summary>
    /// Actual implementation of <see cref="IPreProcessorService"/>.
    /// </summary>
    public class DefaultPreProcessorService : IPreProcessorService
    {
        private HashSet<IVisitor> visitors = new HashSet<IVisitor>();

        public void AddVisitor(IVisitor visitor)
        {
            visitors.Add(visitor);
        }

        public void Process(ICodeObject codeObject, IPreProcessorServiceContext context)
        {
            foreach (IVisitor visitor in visitors)
                visitor.Visit(codeObject, new DefaultPreProcessorContext(context.DependencyProvider, this));
        }
    }
}
