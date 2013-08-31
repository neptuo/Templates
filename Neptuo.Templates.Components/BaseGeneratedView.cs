using Neptuo.Templates.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;

namespace Neptuo.Templates
{
    /// <summary>
    /// Base class for generated views.
    /// </summary>
    public abstract class BaseGeneratedView
    {
        protected IViewPage viewPage;
        protected IComponentManager componentManager;
        protected IDependencyProvider dependencyProvider;

        public ICollection<object> Content
        {
            get { return viewPage.Content; }
        }

        public void Setup(IViewPage viewPage, IComponentManager componentManager, IDependencyProvider dependencyProvider)
        {
            this.viewPage = viewPage;
            this.componentManager = componentManager;
            this.dependencyProvider = dependencyProvider;
        }

        public void CreateControls()
        {
            componentManager.AddComponent(viewPage, CreateViewPageControls);
        }

        protected abstract void CreateViewPageControls(IViewPage viewPage);

        public void Init()
        {
            componentManager.Init(viewPage);
        }

        public void Render(IHtmlWriter writer)
        {
            viewPage.Render(writer);
        }

        public void Dispose()
        {
            viewPage.Dispose();
        }

        protected IValueExtensionContext CreateValueExtensionContext(object targetObject, string targetProperty)
        {
            return new DefaultMarkupExtensionContext(
                targetObject, 
                targetObject.GetType().GetProperty(targetProperty), 
                dependencyProvider
            );
        }

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
