using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.PreProcessing
{
    public interface IPreProcessorContext
    {
        IDependencyProvider DependencyProvider { get; }
        IPreProcessorService PreProcessorService { get; }
    }
}
