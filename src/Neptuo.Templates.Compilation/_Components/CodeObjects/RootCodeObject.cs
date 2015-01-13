using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Describes root of AST.
    /// </summary>
    public class RootCodeObject : IComponentCodeObject
    {
        public List<ICodeProperty> Properties { get; set; }
        public List<IObserverCodeObject> Observers { get; set; }

        public RootCodeObject()
        {
            Properties = new List<ICodeProperty>();
            Observers = new List<IObserverCodeObject>();
        }
    }
}
