﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public interface ICodeGeneratorContext
    {
        TextWriter Output { get; }
    }
}
