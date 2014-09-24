using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Neptuo.Templates.Controls
{
    /// <summary>
    /// Base interface that each control must implement. 
    /// Class name can contain suffix Control that is automaticaly appended to tag name.
    /// As addition, class can be decorated with attribute <see cref="Neptuo.Templates.Compilation.ComponentAttribute"/>.
    /// To define default property use <see cref="DefaultPropertyAttribute"/>.
    /// </summary>
    public interface IControl
    {
        /// <summary>
        /// Method invoked in init phase.
        /// Place any inicialization code here.
        /// </summary>
        void OnInit();

        /// <summary>
        /// Renders output to <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">Output writer.</param>
        void Render(IHtmlWriter writer);
    }
}
