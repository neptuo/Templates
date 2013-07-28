using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.PreProcessing
{
    public interface IVisitorContext
    {
        IDependencyProvider DependencyProvider { get; }
        IPreProcessorService PreProcessorService { get; }
    }
}
