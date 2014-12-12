using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Implementation of <see cref="IContentBuilder"/> that for all xml elements returns 'This element is not supported'.
    /// </summary>
    public class NotSupportedContentBuilder : IContentBuilder
    {
        public ICodeObject TryParse(IContentBuilderContext context, IXmlElement element)
        {
            context.AddError(String.Format("Xml element '{0}' is not supported.", element.Name));
            return null;
        }
    }
}
