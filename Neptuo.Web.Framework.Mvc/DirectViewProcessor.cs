using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Neptuo.Web.Html;
using Neptuo.Web.Html.Compilation;
using Neptuo.Web.Html.Compilation.DirectCreation;
using Neptuo.Web.Mvc.ViewEngine.Utils;
using Neptuo.Web.Parser;
using Neptuo.Web.Parser.Html;

namespace Neptuo.Web.Mvc.ViewEngine
{
    public class DirectViewProcessor : IViewProcessor, IVirtualPathProvider
    {
        public Configuration Configuration { get; set; }
        public ViewContext ViewContext { get; set; }
        public ILivecycleObserver LivecycleObserver { get; set; }
        public IDependencyContainer DependencyContainer { get; set; }

        public DirectViewProcessor(ViewContext viewContext, Configuration configuration)
        {
            Configuration = configuration;
            ViewContext = viewContext;
            LivecycleObserver = new StandartLivecycleObserver();

            DependencyContainer = new DependencyContainer();
            DependencyContainer[typeof(ViewContext)] = ViewContext;
            DependencyContainer[typeof(IViewProcessor)] = this;
            DependencyContainer[typeof(IVirtualPathProvider)] = this;
            DependencyContainer[typeof(ILivecycleObserver)] = LivecycleObserver;
            DependencyContainer[typeof(StackStorage)] = new StackStorage();
        }

        public IViewPage LoadView(string viewName)
        {
            string filePath = MapPath(viewName);
            string fileContents = File.ReadAllText(filePath);
            return Load(viewName, fileContents);
        }

        public IViewPage Load(string viewName, string viewContent)
        {
            IActivator activator = new ObjectActivator(DependencyContainer);
            ViewHandler viewHandler = new ViewHandler(activator);
            viewHandler.Name = viewName;
            IExtensionEvaluator extensionEvaluator = new HtmlExtensionEvaluator(Configuration, viewHandler);
            IEvaluator evaluator = new HtmlEvaluator(Configuration, ViewContext, viewHandler, DependencyContainer, extensionEvaluator);

            IContentParser parser = new HtmlContentParser();
            parser.Options.StripWhitespaces = true;
            IDocument document = parser.Parse(viewContent);

            foreach (IElement element in document.Childs)
            {
                ITypeHandler child = evaluator.Evaluate(element);
                if (child != null)
                    viewHandler.Childs.Add(child);
            }

            return ProcessViewHandler(viewHandler);
        }

        public string MapPath(string path)
        {
            return ViewContext.HttpContext.Server.MapPath(path);
        }

        private IViewPage ProcessViewHandler(IViewHandler viewHandler)
        {
            IViewPage view = new HtmlViewPage();
            view.Name = viewHandler.Name;
            foreach (ITypeHandler item in viewHandler.Childs)
                view.Childs.Add(ProcessTypeHandler(item as IInstanceHandler));

            return view;
        }

        private object ProcessTypeHandler(IInstanceHandler type)
        {
            //foreach (ITypeHandler item in type.Childs)
            //{
            //    type.Childs.Add(ProcessTypeHandler(item));
            //}

            return type.Instance;
        }

        public void OnInit()
        {
            LivecycleObserver.OnInit();
        }

        public void OnLoad()
        {
            LivecycleObserver.OnLoad();
        }

        public void BeforeRender()
        {
            LivecycleObserver.BeforeRender();
        }

        public void BeforeUnLoad()
        {
            LivecycleObserver.BeforeUnLoad();
        }
    }
}
