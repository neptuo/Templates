using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Classifications.Definitions
{
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = TemplateClassificationType.CurlyError)]
    [Name(TemplateClassificationType.CurlyError)]
    [UserVisible(true)]
    [Order(After = Priority.Default)]
    internal sealed class CurlyErrorDefinition : ClassificationFormatDefinition
    {
        public CurlyErrorDefinition()
        {
            BackgroundColor = Color.FromArgb(0, 0, 0, 0);
            ForegroundColor = Colors.Red;
            DisplayName = "Template Curly Error";

            TextDecorations = new TextDecorationCollection();
            TextDecorations.Add(new TextDecoration(
                TextDecorationLocation.Underline,
                new Pen(new SolidColorBrush(Colors.Red), 1),
                2,
                TextDecorationUnit.FontRecommended,
                TextDecorationUnit.FontRecommended
            ));
        }
    }
}
