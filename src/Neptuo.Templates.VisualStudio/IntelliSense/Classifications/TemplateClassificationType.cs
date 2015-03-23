using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.Windows;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Classifications
{
    internal static class TemplateClassificationType
    {
        public const string Name = "ntemplate";
        public const string CurlyBrace = "ntemplate_CurlyBrace";
        public const string CurlyContent = "ntemplate_CurlyContent";

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(CurlyBrace)]
        internal static ClassificationTypeDefinition TemplateCurlyBraceClassificationTypeDefinition = null;

        [Export(typeof (ClassificationTypeDefinition))] 
        [Name(CurlyContent)]
        internal static ClassificationTypeDefinition TemplateCurlyContentClassificationTypeDefinition = null;
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = TemplateClassificationType.CurlyBrace)]
    [Name(TemplateClassificationType.CurlyBrace)]
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

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = TemplateClassificationType.CurlyContent)]
    [Name(TemplateClassificationType.CurlyContent)]
    [UserVisible(true)]
    [Order(After = Priority.Default)]
    internal sealed class CurlyContentDefinition : ClassificationFormatDefinition
    {
        public CurlyContentDefinition()
        {
            BackgroundColor = Color.FromRgb(244, 244, 244);
            ForegroundColor = Colors.Blue;
            DisplayName = "Template Curly Content";

            //TextDecorations = new TextDecorationCollection();
            //TextDecorations.Add(new TextDecoration(
            //    TextDecorationLocation.Underline,
            //    new Pen(new SolidColorBrush(Color.FromRgb(255, 0, 0)), 1),
            //    2,
            //    TextDecorationUnit.FontRecommended,
            //    TextDecorationUnit.FontRecommended
            //));
        }
    }
}
