using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public interface IControlCodeObject : ICodeObject
    {
        Type Type { get; }
        Dictionary<string, ICodeObject> Properties { get; }
        List<IObserverCodeObject> Observers { get; }
    }

    public interface IObserverCodeObject : ICodeObject
    {
        Type Type { get; }
        Dictionary<string, ICodeObject> Properties { get; }
    }

    /// <summary>
    /// Marker interface.
    /// Pokud je do XmlContentParseru předán RootObject toho typu, nevytváří se další prvek stromu.
    /// </summary>
    public interface IRootCodeObject : ICodeObject
    { }
}
