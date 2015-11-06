using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.VisualStudio
{
    public class ContentType
    {
        public const string TextValue = "ntemplate";

        [Export(typeof(ContentTypeDefinition))]
        [Name(TextValue)]
        [BaseDefinition("code")]
        public ContentType ContentTypeDefinition { get; set; }

        [Export(typeof(FileExtensionToContentTypeDefinition))]
        [FileExtension(".nt")]
        [ContentType(TextValue)]
        public FileExtensionToContentTypeDefinition FileExtensionDefinition { get; set; }
    }
}
