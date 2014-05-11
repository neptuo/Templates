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
        /// Parses <paramref name="attributes"/> and creates AST for it.
        /// </summary>
        /// <param name="context">Context information.</param>
        /// <param name="codeObject">Target component.</param>
        /// <param name="attributes">List attributes for observer.</param>
        void Parse(IContentBuilderContext context, IComponentCodeObject codeObject, IEnumerable<IXmlAttribute> attributes);
    }

    /// <summary>
    /// Observer instance scope.
    /// </summary>
    public enum ObserverBuilderScope
    {
        /// <summary>
        /// One instance per attribute.
        /// </summary>
        PerAttribute, 
        
        /// <summary>
        /// One instance per all attributes on component.
        /// </summary>
        PerElement, 
       
        /// <summary>
        /// Singleton per document.
        /// </summary>
        PerDocument
    }
}
