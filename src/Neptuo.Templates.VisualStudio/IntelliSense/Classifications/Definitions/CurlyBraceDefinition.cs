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
    [ClassificationType(ClassificationTypeNames = ClassificationType.CurlyBrace)]
    [Name(ClassificationType.CurlyBrace)]
    [UserVisible(true)]
    [Order(After = Priority.Default)]
    internal sealed class CurlyBraceDefinition : ClassificationFormatDefinition
    {
        public CurlyBraceDefinition()
        {
            BackgroundColor = Colors.Yellow;
            ForegroundColor = Colors.Blue;
            DisplayName = "Template Curly Brace";
        }
    }

}
