using Neptuo.Templates;
using Neptuo.Templates.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleNG.Controls
{
    public class ViewTemplateContent : ITemplateContent
    {
        public GeneratedView View { get; private set; }

        public ViewTemplateContent(GeneratedView view)
        {
            View = view;
        }

        public void OnInit(IComponentManager componentManager)
        {
            View.OnInit(componentManager);
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
