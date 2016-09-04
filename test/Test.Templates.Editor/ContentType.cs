using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates
{
    public class ContentType
    {
        public const string Name = "ntemplate";
        public const string FileExtension = ".nt";

        [Export(typeof(ContentTypeDefinition))]
        [Name(Name)]
        [BaseDefinition("htmlx")]
        public static ContentTypeDefinition Definition { get; set; }

        [Export(typeof(FileExtensionToContentTypeDefinition))]
        [FileExtension(FileExtension)]
        [ContentType(Name)]
        public static FileExtensionToContentTypeDefinition Extension { get; set; }
    }
}
