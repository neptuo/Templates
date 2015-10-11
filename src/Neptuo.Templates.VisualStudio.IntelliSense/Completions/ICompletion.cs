using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Completions
{
    public interface ICompletion
    {
        string DisplayText { get; }
        string InsertionText { get; }
        string DescriptionText { get; }
        ImageSource IconSource { get; }
    }
}
