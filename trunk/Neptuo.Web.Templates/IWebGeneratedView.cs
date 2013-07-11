using Neptuo.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Web.Templates
{
    public interface IWebGeneratedView : IGeneratedView
    {
        void CreateControls();

        void Setup(IViewPage page, IComponentManager componentManager, IDependencyProvider dependencyProvider);

        void Init();

        void Render(HtmlTextWriter writer);
    }
}
