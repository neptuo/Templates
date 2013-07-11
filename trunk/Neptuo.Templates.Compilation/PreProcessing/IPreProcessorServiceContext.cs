using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.PreProcessing
{
    public interface IPreProcessorServiceContext
    {
        IDependencyProvider DependencyProvider { get; }
    }
}
