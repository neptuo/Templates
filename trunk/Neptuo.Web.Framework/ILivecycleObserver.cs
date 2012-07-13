using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework
{
    public interface ILivecycleObserver
    {
        void Add(ILivecycle livecycleObject);

        void Add(object objectToResolve);

        void OnInit();

        void OnLoad();

        void BeforeRender();

        void BeforeUnLoad();
    }
}
