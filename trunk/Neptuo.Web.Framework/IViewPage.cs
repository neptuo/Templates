using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework
{
    /// <summary>
    /// Represents complete view.
    /// </summary>
    public interface IViewPage : IDisposable
    {
        /// <summary>
        /// Controls in view.
        /// </summary>
        List<object> Content { get; set; }

        void OnInit();

        void Render(HtmlTextWriter writer);
    }
}
