using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates
{
    public interface IVirtualPathProvider
    {
        string MapPath(string path);
    }
}
