﻿using Neptuo;
using Neptuo.FileSystems;
using Neptuo.Templates;
using Neptuo.Templates.Compilation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleNG.Controls
{
    public class FileTemplate : ITemplate
    {
        protected IDependencyProvider DependencyProvider { get; private set; }
        protected IComponentManager ComponentManager { get; private set; }
        protected IViewService ViewService { get; private set; }
        public string Path { get; set; }

        public FileTemplate(IDependencyProvider dependencyProvider, IComponentManager componentManager, IViewService viewService)
        {
            DependencyProvider = dependencyProvider;
            ComponentManager = componentManager;
            ViewService = viewService;
        }

        public ITemplateContent CreateInstance()
        {
            GeneratedView view = (GeneratedView)ViewService.ProcessContent("CSharp", LocalFileSystem.FromFilePath(Path).GetContent(), new DefaultViewServiceContext(DependencyProvider));
            view.Setup(new ViewPage(DependencyProvider.Resolve<IComponentManager>()), DependencyProvider.Resolve<IComponentManager>(), DependencyProvider);
            view.CreateControls();

            ViewTemplateContent templateContent = new ViewTemplateContent(view);
            ComponentManager.AddComponent(templateContent, null);
            return templateContent;
        }

        public void Dispose()
        { }
    }
}
