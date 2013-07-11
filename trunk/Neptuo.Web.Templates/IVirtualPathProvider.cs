using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Templates
{
    public interface IVirtualUrlProvider
    {
        string ResolveUrl(string path);
    }
}
