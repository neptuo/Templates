using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// List of registered builders.
    /// </summary>
    public interface IContentBuilderRegistry
    {
        /// <summary>
        /// Get component builder for <paramref name="prefix"/> and <paramref name="name"/> or null.
        /// </summary>
        /// <param name="prefix">Component prefix.</param>
        /// <param name="name">Component name.</param>
        /// <returns>Get component builder for <paramref name="prefix"/> and <paramref name="name"/> or null.</returns>
        IComponentBuilder GetComponentBuilder(string prefix, string name);

        /// <summary>
        /// Gets generic component builder.
        /// </summary>
        /// <param name="name">Component name.</param>
        /// <returns>Gets generic component builder.</returns>
        IComponentBuilder GetGenericContentBuilder(string name);

        /// <summary>
        /// Gets static text builder.
        /// </summary>
        /// <returns>Gets static text builder.</returns>
        ILiteralBuilder GetLiteralBuilder();

        /// <summary>
        /// Gets observer builder for <paramref name="prefix"/> and <paramref name="name"/>.
        /// </summary>
        /// <param name="prefix">Observer prefix.</param>
        /// <param name="name">Observer name.</param>
        /// <returns>Gets observer builder for <paramref name="prefix"/> and <paramref name="name"/>.</returns>
        IObserverRegistration GetObserverBuilder(string prefix, string name);

        /// <summary>
        /// Gets property builder for <paramref name="propertyInfo"/>.
        /// </summary>
        /// <param name="propertyInfo">Property info definition.</param>
        /// <returns>Gets property builder for <paramref name="propertyInfo"/>.</returns>
        IPropertyBuilder GetPropertyBuilder(IPropertyInfo propertyInfo);

        /// <summary>
        /// Test if <paramref name="prefix"/> and <paramref name="name"/> is registered component.
        /// </summary>
        /// <param name="prefix">Component prefix.</param>
        /// <param name="name">Component name.</param>
        /// <returns>True if <paramref name="prefix"/> andm <paramref name="name"/> is registered component.</returns>
        bool ContainsComponent(string prefix, string name);

        /// <summary>
        /// Test if <paramref name="prefix"/> and <paramref name="name"/> is registered observer.
        /// </summary>
        /// <param name="prefix">Observer prefix.</param>
        /// <param name="name">Observer name.</param>
        /// <returns>True if <paramref name="prefix"/> andm <paramref name="name"/> is registered observer.</returns>
        bool ContainsObserver(string prefix, string name);

        /// <summary>
        /// Test if <paramref name="propertyInfo"/> has registered builder.
        /// </summary>
        /// <param name="propertyInfo">Property info definition.</param>
        /// <returns>True if <paramref name="propertyInfo"/> has registered builder.</returns>
        bool ContainsProperty(IPropertyInfo propertyInfo);
        
        /// <summary>
        /// Registers namespace.
        /// </summary>
        /// <param name="namespaceDeclaration">Namespace definition.</param>
        void RegisterNamespace(NamespaceDeclaration namespaceDeclaration);

        /// <summary>
        /// Gets list of registered namespaces.
        /// </summary>
        /// <returns>Gets list of registered namespaces.</returns>
        IEnumerable<NamespaceDeclaration> GetRegisteredNamespaces();

        /// <summary>
        /// Creates child registry including current registrations.
        /// </summary>
        /// <returns>Creates child registry including current registrations.</returns>
        IContentBuilderRegistry CreateChildRegistry();
    }
}
