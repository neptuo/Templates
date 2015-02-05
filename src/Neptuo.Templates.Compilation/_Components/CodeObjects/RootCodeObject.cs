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
    public class RootCodeObject : IPropertiesCodeObject, IObserversCodeObject
    {
        public List<ICodeProperty> Properties { get; set; }
        public List<ICodeObject> Observers { get; set; }

        public RootCodeObject()
        {
            Properties = new List<ICodeProperty>();
            Observers = new List<ICodeObject>();
        }
    }
}
