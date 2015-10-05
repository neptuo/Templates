using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Descriptors
{
    /// <summary>
    /// Defines field.
    /// </summary>
    public interface IFieldDescriptor
    {
        /// <summary>
        /// Field unique name inside component.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Type of field value.
        /// </summary>
        Type FieldType { get; }

        /// <summary>
        /// Whether new value can be assigned; if <c>false</c> only 
        /// modification of current value can be processed.
        /// </summary>
        bool IsReadOnly { get; }
    }
}
