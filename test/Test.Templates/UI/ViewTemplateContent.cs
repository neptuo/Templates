using Neptuo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.UI
{
    public class ViewTemplateContent : ITemplateContent
    {
        public GeneratedView View { get; private set; }

        private readonly IDependencyProvider dependencyProvider;

        public ViewTemplateContent(GeneratedView view, IDependencyProvider dependencyProvider)
        {
            View = view;
            this.dependencyProvider = dependencyProvider;
        }

        public void OnInit(IComponentManager componentManager)
        {
            View.Init(dependencyProvider, componentManager);
        }

        public void Render(IHtmlWriter writer)
        {
            View.Render(writer);
        }

        public void Dispose()
        {
            View.Dispose();
        }
    }
}
