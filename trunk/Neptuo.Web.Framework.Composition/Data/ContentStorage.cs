using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Composition.Data
{
    public class ContentStorage : Dictionary<string, ContentControl>
    {
        public static ContentStorage Instance = new ContentStorage();
    }
}
