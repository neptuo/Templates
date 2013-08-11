﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    public interface ISourceCodeException
    {
        int LineNumber { get; }
        int LinePosition { get; }
        string Message { get; }
    }
}
