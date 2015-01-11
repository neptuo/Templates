using Neptuo;
using Neptuo.FileSystems;
using Neptuo.Templates;
using Neptuo.Templates.Compilation;
using Neptuo.Templates.Compilation.Parsers;
using Neptuo.Templates.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.Controls
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
            ISourceContent content = new DefaultSourceContent(LocalFileSystem.FromFilePath(Path).GetContent());
            GeneratedView view = (GeneratedView)ViewService.ProcessContent("CSharp", content, new DefaultViewServiceContext(DependencyProvider));
            view.Setup(DependencyProvider);

            ViewTemplateContent templateContent = new ViewTemplateContent(view);
            componentManager.AddComponent(templateContent, null);
            return templateContent;
        }

        public void Dispose()
        { }
    }
}
