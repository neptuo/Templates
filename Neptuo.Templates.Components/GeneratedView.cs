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
        protected IComponentManager componentManager;
        protected IDependencyProvider dependencyProvider;

        /// <summary>
        /// View content.
        /// </summary>
        public ICollection<object> Content { get; private set; }

        /// <summary>
        /// Setups view page, component manager and depedency provider for this view.
        /// </summary>
        /// <param name="componentManager">Component manager for current view.</param>
        /// <param name="dependencyProvider">Dependency provider for current view.</param>
        public void Setup(IDependencyProvider dependencyProvider)
        {
            Guard.NotNull(dependencyProvider, "dependencyProvider");
            this.dependencyProvider = dependencyProvider;
        }

        /// <summary>
        /// Method where to setup root controls.
        /// </summary>
        /// <param name="view">Generated view instance to bind.</param>
        protected abstract void BindView(GeneratedView view);

        /// <summary>
        /// Starts init phase of this view.
        /// </summary>
        public void OnInit(IComponentManager componentManager)
        {
            Guard.NotNull(componentManager, "componentManager");
            this.componentManager = componentManager;
            componentManager.AddComponent(this, BindView);
            componentManager.Init(this);
        }

        /// <summary>
        /// Renders view output to <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">Output writer.</param>
        public void Render(IHtmlWriter writer)
        {
            Guard.NotNull(writer, "writer");

            foreach (object item in Content)
                componentManager.Render(item, writer);
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
