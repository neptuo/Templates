using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Completions
{
    /// <summary>
    /// Default implementation of <see cref="ICompletion"/>.
    /// </summary>
    public class DefaultCompletion : ICompletion
    {
        public string DisplayText { get; private set; }
        public string InsertionText { get; private set; }
        public string DescriptionText { get; private set; }
        public ImageSource IconSource { get; private set; }

        public DefaultCompletion(string displayText, string descriptionText, ImageSource iconSource)
            : this(displayText, displayText, descriptionText, iconSource)
        { }

        public DefaultCompletion(string displayText, ImageSource iconSource)
            : this(displayText, displayText, String.Empty, iconSource)
        { }

        public DefaultCompletion(string displayText, string insertionText, string descriptionText, ImageSource iconSource)
        {
            DisplayText = displayText;
            InsertionText = insertionText;
            DescriptionText = descriptionText;
            IconSource = iconSource;
        }
    }
}
