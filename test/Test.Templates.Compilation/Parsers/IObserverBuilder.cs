using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Defines builder for observers.
    /// </summary>
    public interface IObserverBuilder
    {
        /// <summary>
        /// Parses <paramref name="attribute"/> and creates AST for it.
        /// </summary>
        /// <param name="context">Context information.</param>
        /// <param name="codeObject">Target component.</param>
        /// <param name="attribute">Attribute to be processed by this builder.</param>
        /// <returns><c>true</c> if processing was successfull; <c>false</c> otherwise.</returns>
        bool TryParse(IContentBuilderContext context, IObserversCodeObject codeObject, IXmlAttribute attribute);
    }
}
