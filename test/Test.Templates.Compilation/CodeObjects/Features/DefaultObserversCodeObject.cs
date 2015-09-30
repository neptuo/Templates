using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.Compilation.CodeObjects.Features
{
    public class DefaultObserversCodeObject : IObserversCodeObject
    {
        public List<ICodeObject> Observers { get; set; }

        public DefaultObserversCodeObject()
        {
            Observers = new List<ICodeObject>();
        }
    }
}
