﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    public interface INamingService
    {
        INaming FromFile(string fileName);
        INaming FromContent(string viewContent);
    }
}