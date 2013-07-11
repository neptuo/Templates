using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    public interface IObserversCodeObject
    {
        List<IObserverCodeObject> Observers { get; set; }
    }
}
