using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Classifications.Definitions
{
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = ClassificationType.CurlyName)]
    [Name(ClassificationType.CurlyName)]
    [UserVisible(true)]
    [Order(After = Priority.Default)]
    internal class CurlyNameDefinition : ClassificationFormatDefinition
    {
        public CurlyNameDefinition()
        {
            ForegroundColor = Color.FromRgb(187, 160, 140);
            DisplayName = "Template Curly Name";
        }
    }
}
