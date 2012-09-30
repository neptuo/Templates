using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml;
using Neptuo.Web.Framework.Annotations;
using Neptuo.Web.Framework.Compilation;

namespace Neptuo.Web.Framework.Controls
{
    //[ControlBuilder(typeof(GridControlBuilder))]
    public class GridControl : BaseContentControl
    {
        public List<GridItemControl> Header { get; set; }

        [DefaultValue(2)]
        public int Repeat { get; set; }

        public GridOptions Options { get; set; }

        public GridControl()
        {
            Header = new List<GridItemControl>();
            Content = new List<object>();
        }

        public override void OnInit()
        {
            foreach (GridItemControl item in Header)
                LivecycleObserver.Init(item);

            base.OnInit();
        }

        protected override void RenderBody(HtmlTextWriter writer)
        {
            writer.WriteFullBeginTag("tr");
            foreach (GridItemControl item in Header)
            {
                writer.WriteFullBeginTag("th");
                LivecycleObserver.Render(item, writer);

                writer.WriteEndTag("th");
            }
            writer.WriteEndTag("tr");

            for (int i = 0; i < Repeat; i++)
            {
                writer.WriteFullBeginTag("tr");
                foreach (object item in Content)
                {
                    if (item is IControl)
                    {
                        IControl control = (item as IControl);
                        LiteralControl literal = control as LiteralControl;
                        if (literal != null && String.IsNullOrWhiteSpace(literal.Text))
                            continue;

                        writer.WriteFullBeginTag("td");
                        LivecycleObserver.Render(control, writer);
                        writer.WriteEndTag("td");
                    }
                }
                writer.WriteEndTag("tr");
            }
        }

        protected override string GetTagName()
        {
            return "table";
        }
    }

    public class GridControlBuilder : IXmlControlBuilder
    {
        public void GenerateControl(Type controlType, XmlElement source, XmlBuilderContext context)
        {
            DefaultContentGenerator compiler = context.ContentGenerator as DefaultContentGenerator;
            if (compiler != null)
                compiler.GenerateControl(new DefaultContentGenerator.Helper(null, context), controlType, source);
        }
    }

    [DefaultProperty("Text")]
    public class GridItemControl : BaseControl
    {
        public string Text { get; set; }

        public override void Render(HtmlTextWriter writer)
        {
            writer.Write(Text);
        }
    }

    public class GridOptions
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
