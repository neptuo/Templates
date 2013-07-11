using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    public interface IValueParserContext
    {
        IDependencyProvider DependencyProvider { get; }
        IParserService ParserService { get; }
        IPropertyDescriptor PropertyDescriptor { get; }
        ICollection<IErrorInfo> Errors { get; }
    }
}
