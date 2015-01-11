using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    partial class XmlContentParser
    {
        /// <summary>
        /// Some static usefull methods.
        /// </summary>
        public static class Utils
        {
            //public static IEnumerable<NamespaceDeclaration> GetXmlNsNamespace(IXmlElement element)
            //{
            //    List<NamespaceDeclaration> result = new List<NamespaceDeclaration>();
            //    foreach (IXmlAttribute attribute in element.Attributes)
            //    {
            //        if (attribute.Prefix.ToLowerInvariant() == "xmlns")
            //            result.Add(new NamespaceDeclaration(attribute.LocalName, attribute.Value));
            //    }
            //    return result;
            //}

            ///// <summary>
            ///// Creates child builder registry if needed (has any <paramref name="declarations"/>).
            ///// </summary>
            ///// <param name="currentBuilderRegistry">Current builder registry.</param>
            ///// <param name="declarations">New namespace declaration.</param>
            ///// <returns>Child builder registry or current one.</returns>
            //public static IContentBuilderRegistry CreateChildRegistry(IContentBuilderRegistry currentBuilderRegistry, IEnumerable<NamespaceDeclaration> declarations)
            //{
            //    //if (declarations.Any())
            //    //{
            //    //    IContentBuilderRegistry newBuilderRegistry = currentBuilderRegistry.CreateChildRegistry();
            //    //    foreach (NamespaceDeclaration decl in declarations)
            //    //        newBuilderRegistry.RegisterNamespace(decl);

            //    //    return newBuilderRegistry;
            //    //}
            //    return currentBuilderRegistry;
            //}
        }
    }
}
