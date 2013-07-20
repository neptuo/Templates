using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates
{
    delegate TReturn Func<T, TOutput, TReturn>(T input, out TOutput output);
}
