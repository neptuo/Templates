using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Context for <see cref="IPropertyBuilder"/>.
    /// </summary>
    public interface IPropertyBuilderContext : IParserServiceContext
    {
        /// <summary>
        /// Name of parsers to use.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Current parser service.
        /// </summary>
        IParserService ParserService { get; }

        /// <summary>
        /// Property to build.
        /// </summary>
        IFieldDescriptor FieldDescriptor { get; }
    }
}
