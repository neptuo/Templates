using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.Framework.Compilation;
using Neptuo.Web.Framework.Parser.HtmlContent;

namespace Neptuo.Web.Framework.Controls
{
    public class PanelControlBuilder : IControlBuilder
    {
        public void GenerateControl(Type controlType, HtmlTag parsedItem, ControlBuilderContext context)
        {
            ControlContentCompiler compiler = context.ContentCompiler as ControlContentCompiler;
            if (compiler != null)
                compiler.GenerateControl(controlType, parsedItem, context);
        }
    }
}
