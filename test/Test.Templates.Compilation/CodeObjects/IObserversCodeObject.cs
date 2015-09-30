using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Object that has observers.
    /// </summary>
    public interface IObserversCodeObject
    {
        /// <summary>
        /// List of observers.
        /// </summary>
        List<ICodeObject> Observers { get; set; }
    }
}
