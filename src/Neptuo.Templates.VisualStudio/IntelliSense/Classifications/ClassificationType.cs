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
    internal static class ClassificationType
    {
        public const string Name = "ntemplate";
        public const string CurlyBrace = "ntemplate_CurlyBrace";
        public const string CurlyContent = "ntemplate_CurlyContent";
        public const string CurlyError = "ntemplate_CurlyError";

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(CurlyBrace)]
        internal static ClassificationTypeDefinition CurlyBraceDefinition = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(CurlyContent)]
        internal static ClassificationTypeDefinition CurlyContentDefinition = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(CurlyError)]
        internal static ClassificationTypeDefinition CurlyErrorDefinition = null;
    }
}
