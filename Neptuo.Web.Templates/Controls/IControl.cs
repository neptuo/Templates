using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Neptuo.Templates.Controls
{
    /// <summary>
    /// Base interface that each HtmlControl must implement. 
    /// Class name can contain suffix Control that automaticaly appended to tag name.
    /// As addition, class can be decorated with attribute <see cref="ControlAttribute"/>.
    /// To define default property use <see cref="DefaultPropertyAttribute"/>.
    /// </summary>
    public interface IControl
    {
        void OnInit();

        void Render(HtmlTextWriter writer);
    }
}
