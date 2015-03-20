using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio
{
    public class TemplateContentType
    {
        public const string ContentType = "ntemplate";

        [Export(typeof(ContentTypeDefinition))]
        [Name(ContentType)]
        [BaseDefinition("htmlx")]
        public TemplateContentType ContentTypeDefinition { get; set; }

        [Export(typeof(FileExtensionToContentTypeDefinition))]
        [FileExtension(".nt")]
        [ContentType(ContentType)]
        public FileExtensionToContentTypeDefinition FileExtensionDefinition { get; set; }
    }
}
