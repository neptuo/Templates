using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework
{
    /// <summary>
    /// Represents complete view.
    /// </summary>
    public interface IViewPage : IControl, IDisposable
    {
        /// <summary>
        /// Controls in view.
        /// </summary>
        ICollection<object> Content { get; set; }
    }
}
