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
        public const string CurlyName = "ntemplate_CurlyName";
        public const string CurlyAttributeName = "ntemplate_CurlyAttributeName";

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(CurlyBrace)]
        internal static ClassificationTypeDefinition CurlyBraceDefinition = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(CurlyName)]
        internal static ClassificationTypeDefinition CurlyNameDefinition = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(CurlyAttributeName)]
        internal static ClassificationTypeDefinition CurlyAttributeNameDefinition = null;
    }
}
