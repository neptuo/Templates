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
    [ClassificationType(ClassificationTypeNames = ClassificationType.CurlyAttributeName)]
    [Name(ClassificationType.CurlyAttributeName)]
    [UserVisible(true)]
    [Order(After = Priority.Default)]
    internal class CurlyAttributeNameDefinition : ClassificationFormatDefinition
    {
        public CurlyAttributeNameDefinition()
        {
            ForegroundColor = Color.FromRgb(215, 186, 125);
        }
    }
}
