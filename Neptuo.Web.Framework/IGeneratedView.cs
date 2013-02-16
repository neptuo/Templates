using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Neptuo.Web.Framework
{
    public interface IGeneratedView : IDisposable
    {
        ICollection<object> Content { get; }

        void CreateControls();

        void Setup(IViewPage page, IComponentManager componentManager, IDependencyProvider dependencyProvider);

        void Init();

        void Render(HtmlTextWriter writer);
    }
}
