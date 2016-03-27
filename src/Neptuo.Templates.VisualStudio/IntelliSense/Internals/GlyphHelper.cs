using Microsoft.VisualStudio.Language.Intellisense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Internals
{
    internal class GlyphHelper
    {
        private readonly IGlyphService glyphService;

        public GlyphHelper(IGlyphService glyphService)
        {
            Ensure.NotNull(glyphService, "glyphService");
            this.glyphService = glyphService;
        }

        public ImageSource Get(StandardGlyphGroup glyphGroup, StandardGlyphItem glyphItem = StandardGlyphItem.GlyphItemPublic)
        {
            return glyphService.GetGlyph(glyphGroup, glyphItem);
        }

        public ImageSource GetExtensionMethod()
        {
            return Get(StandardGlyphGroup.GlyphExtensionMethod);
        }
    }
}
