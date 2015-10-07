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
    [ClassificationType(ClassificationTypeNames = ClassificationType.CurlyContent)]
    [Name(ClassificationType.CurlyContent)]
    [UserVisible(true)]
    [Order(After = Priority.Default)]
    internal sealed class CurlyContentDefinition : ClassificationFormatDefinition
    {
        public CurlyContentDefinition()
        {
            BackgroundColor = Color.FromRgb(244, 244, 244);
            ForegroundColor = Colors.Blue;
            DisplayName = "Template Curly Content";
        }
    }
}
