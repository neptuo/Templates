using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleNG.Controls
{
    public interface ITemplate : IDisposable
    {
        ITemplateContent CreateInstance();
    }
}
