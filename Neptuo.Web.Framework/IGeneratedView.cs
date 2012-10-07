﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Neptuo.Web.Framework
{
    public interface IGeneratedView : IDisposable
    {
        void CreateControls();

        void Setup(IViewPage page, ILivecycleObserver liveCycleObserver, IServiceProvider serviceProvider, HttpRequest request, HttpResponse response);

        void Init();

        void Render(HtmlTextWriter writer);
    }
}