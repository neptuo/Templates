using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework
{
    public static class Models
    {
        public static Stack<object> CurrentModel { get; private set; }

        static Models()
        {
            CurrentModel = new Stack<object>();
        }
    }
}
