using Neptuo.Web.Framework.Compilation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Composition
{
    public interface ITemplate
    {
        /// <summary>
        /// Vytvoří instanci kontrolů (zatím se nevolá, ale umožňí vytvořit více instancí jedné šablony).
        /// </summary>
        void CreateInstance();

        void Init(IComponentManager componentManager);
        void Render(IComponentManager componentManager, HtmlTextWriter writer);
    }

    public abstract class BaseTemplate : ITemplate
    {
        public ICollection<object> Content { get; set; }

        public void CreateInstance()
        {
            throw new NotImplementedException();
        }

        public abstract void Init(IComponentManager componentManager);

        public void Render(IComponentManager componentManager, HtmlTextWriter writer)
        {
            foreach (object content in Content)
                componentManager.Render(content, writer);
        }
    }

    [DefaultProperty("Content")]
    //[TypeConverter(typeof(TemplateTypeConverter))]
    public class Template : BaseTemplate, ITemplate
    {
        public override void Init(IComponentManager componentManager)
        {
            foreach (object content in Content)
                componentManager.Init(content);
        }
    }

    public class FileTemplate : BaseTemplate, ITemplate
    {
        private string file;
        private IViewService viewService;
        private IDependencyProvider dependencyProvider;

        public FileTemplate(string file, IViewService viewService, IDependencyProvider dependencyProvider)
        {
            this.file = file;
            this.viewService = viewService;
            this.dependencyProvider = dependencyProvider;
        }

        public override void Init(IComponentManager componentManager)
        {
            IGeneratedView view = viewService.Process(file, new DefaultViewServiceContext(dependencyProvider));
            view.Setup(new BaseViewPage(componentManager), componentManager, dependencyProvider);
            view.CreateControls();
            view.Init();

            Content = view.Content;
        }
    }
}
