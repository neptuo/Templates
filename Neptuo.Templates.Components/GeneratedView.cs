using Neptuo.ComponentModel;
using Neptuo.Templates.Controls;
using Neptuo.Templates.Extensions;
using Neptuo.Templates.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace Neptuo.Templates
{
    /// <summary>
    /// Base class for generated views.
    /// </summary>
    public abstract class GeneratedView : DisposableBase
    {
        protected IContentControl rootContainer;
        protected IComponentManager componentManager;
        protected IDependencyProvider dependencyProvider;

        /// <summary>
        /// View content.
        /// </summary>
        public ICollection<object> Content
        {
            get { return rootContainer.Content; }
        }

        /// <summary>
        /// Setups view page, component manager and depedency provider for this view.
        /// </summary>
        /// <param name="rootContainer">Root content control for current view.</param>
        /// <param name="componentManager">Component manager for current view.</param>
        /// <param name="dependencyProvider">Dependency provider for current view.</param>
        public void Setup(IContentControl rootContainer, IDependencyProvider dependencyProvider)
        {
            Guard.NotNull(rootContainer, "rootContainer");
            Guard.NotNull(dependencyProvider, "dependencyProvider");
            this.rootContainer = rootContainer;
            this.dependencyProvider = dependencyProvider;
        }

        /// <summary>
        /// Method where to setup root controls.
        /// </summary>
        /// <param name="rootContainer">Root content control for current view.</param>
        protected abstract void CreateViewPageControls(IContentControl rootContainer);

        /// <summary>
        /// Starts init phase of this view.
        /// </summary>
        public void Init(IComponentManager componentManager)
        {
            Guard.NotNull(componentManager, "componentManager");
            this.componentManager = componentManager;
            componentManager.AddComponent(rootContainer, CreateViewPageControls);
            componentManager.Init(rootContainer);
        }

        /// <summary>
        /// Renders view output to <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">Output writer.</param>
        public void Render(IHtmlWriter writer)
        {
            Guard.NotNull(writer, "writer");
            rootContainer.Render(writer);
        }

        /// <summary>
        /// Disposes this view.
        /// </summary>
        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
            componentManager.DisposeAll();
        }

        /// <summary>
        /// Utility method for creating <see cref="IValueExtensionContext"/> for <paramref name="targetObject"/> and <paramref name="targetProperty"/>.
        /// </summary>
        /// <param name="targetObject">Target object for <see cref="IValueExtensionContext"/>.</param>
        /// <param name="targetProperty">Target property for <see cref="IValueExtensionContext"/>.</param>
        /// <returns><see cref="IValueExtensionContext"/> for <paramref name="targetObject"/> and <paramref name="targetProperty"/>.</returns>
        protected IValueExtensionContext CreateValueExtensionContext(object targetObject, string targetProperty)
        {
            Guard.NotNull(targetObject, "targetObject");

            PropertyInfo propertyInfo = null;
            if (!String.IsNullOrEmpty(targetProperty))
                propertyInfo = targetObject.GetType().GetProperty(targetProperty);

            return new DefaultExtensionContext(
                targetObject,
                propertyInfo, 
                dependencyProvider
            );
        }

        /// <summary>
        /// Utility method for casting <paramref name="value"/> to <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Target type.</typeparam>
        /// <param name="value">Source value.</param>
        /// <returns><paramref name="value"/> as <typeparamref name="T"/>.</returns>
        protected virtual T CastValueTo<T>(object value)
        {
            if (value == null)
                return default(T);

            Type sourceType = value.GetType();
            Type targetType = typeof(T);

            if (sourceType == targetType)
                return (T)value;

            TypeConverter converter = TypeDescriptor.GetConverter(value);
            if (converter.CanConvertTo(targetType))
                return (T)converter.ConvertTo(value, targetType);

            throw new InvalidOperationException(String.Format("Unnable to convert to target type! Source type: {0}, target type: {1}", sourceType, targetType));
        }
    }
}
