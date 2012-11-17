using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.Parsers
{
    public interface IContentParserContext
    {
        IDependencyProvider DependencyProvider { get; }
        IParserService ParserService { get; }
        IPropertyDescriptor PropertyDescriptor { get; }
    }
}
