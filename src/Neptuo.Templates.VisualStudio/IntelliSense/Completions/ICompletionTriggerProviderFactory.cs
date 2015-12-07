using Microsoft.VisualStudio.Text;
using Neptuo.Activators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Completions
{
    public interface ICompletionTriggerProviderFactory : IFactory<ICompletionTriggerProvider, ITextBuffer>
    { }
}
