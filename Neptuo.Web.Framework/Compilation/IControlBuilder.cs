using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.Framework.Parser.HtmlContent;

namespace Neptuo.Web.Framework.Compilation
{
    public interface IControlBuilder
    {
        void GenerateControl(Type controlType, HtmlTag parsedItem, ControlBuilderContext context);
    }
}
