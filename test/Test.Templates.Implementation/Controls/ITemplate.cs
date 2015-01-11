using Neptuo.Templates.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.Controls
{
    public interface ITemplate : IDisposable
    {
        ITemplateContent CreateInstance(IComponentManager componentManager);
    }
}
