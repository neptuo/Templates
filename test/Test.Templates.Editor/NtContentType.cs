using Microsoft.VisualStudio.Utilities;
using Microsoft.Web.Core.ContentTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates
{
    public class NtContentType
    {
        public const string TextValue = "ntemplate";
        public const string FileExtension = ".nt";

        [Export(typeof(ContentTypeDefinition))]
        [Name(TextValue)]
        [BaseDefinition(HtmlContentTypeDefinition.HtmlContentType)]
        public NtContentType ContentTypeDefinition { get; set; }

        [Export(typeof(FileExtensionToContentTypeDefinition))]
        [FileExtension(FileExtension)]
        [ContentType(TextValue)]
        public FileExtensionToContentTypeDefinition FileExtensionDefinition { get; set; }
    }
}
