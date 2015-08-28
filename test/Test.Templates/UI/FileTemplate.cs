using Neptuo;
using Neptuo.Activators;
using Neptuo.FileSystems;
using Neptuo.Templates.Compilation;
using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.UI
{
    public class FileTemplate : ITemplate
    {
        protected IDependencyProvider DependencyProvider { get; private set; }
        protected IViewService ViewService { get; private set; }
        public string Path { get; set; }

        public FileTemplate(IDependencyProvider dependencyProvider, IViewService viewService)
        {
            DependencyProvider = dependencyProvider;
            ViewService = viewService;
        }

        public ITemplateContent CreateInstance(IComponentManager componentManager)
        {
            ISourceContent content = new DefaultSourceContent(File.ReadAllText(Path));
            GeneratedView view = (GeneratedView)ViewService.ProcessContent("CodeDom", content, new DefaultViewServiceContext(DependencyProvider));
            ViewTemplateContent templateContent = new ViewTemplateContent(view, DependencyProvider);
            componentManager.AddComponent(templateContent, null);
            return templateContent;
        }

        public void Dispose()
        { }
    }
}
