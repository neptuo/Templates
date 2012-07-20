using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Neptuo.Web.Framework.Controls
{
    public class GridControl : BaseContentControl
    {
        public List<GridItemControl> Header { get; set; }

        [DefaultValue(2)]
        public int Repeat { get; set; }

        public GridControl()
        {
            Header = new List<GridItemControl>();
            Content = new List<object>();
        }

        public override void OnInit()
        {
            foreach (GridItemControl item in Header)
                item.OnInit();

            base.OnInit();
        }

        protected override void RenderBody(HtmlTextWriter writer)
        {
            writer.WriteFullBeginTag("tr");
            foreach (GridItemControl item in Header)
            {
                writer.WriteFullBeginTag("th");
                item.Render(writer);

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
                        control.Render(writer);
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

    [DefaultProperty("Text")]
    public class GridItemControl : BaseControl
    {
        public string Text { get; set; }

        public override void Render(HtmlTextWriter writer)
        {
            writer.Write(Text);
        }
    }
}
