using System.IO;
using System.Runtime;
using System.Text;
using System.Web.UI;

namespace Neptuo.Web.Templates
{
    public class HtmlTextWriter : TextWriter
    {
        protected System.Web.UI.HtmlTextWriter InnerWriter { get; private set; }

        public HtmlTextWriter(TextWriter writer) {
            InnerWriter = new System.Web.UI.HtmlTextWriter(writer);
        }
        
        public HtmlTextWriter(TextWriter writer, string tabString) {
            InnerWriter = new System.Web.UI.HtmlTextWriter(writer, tabString);
        }

        public override Encoding Encoding
        {
            get { return InnerWriter.Encoding; }
        }

        public int Indent
        {
            get { return InnerWriter.Indent; }
            set { InnerWriter.Indent = value; }
        }
        
        public override string NewLine
        {
            get { return InnerWriter.NewLine; }
            set { InnerWriter.NewLine = value; }
        }

        public virtual void AddAttribute(HtmlTextWriterAttribute key, string value)
        {
            InnerWriter.AddAttribute(key, value);
        }

        public virtual void AddAttribute(string name, string value)
        {
            InnerWriter.AddAttribute(name, value);
        }

        public virtual void AddAttribute(HtmlTextWriterAttribute key, string value, bool fEncode)
        {
            InnerWriter.AddAttribute(key, value, fEncode);
        }

        public virtual void AddAttribute(string name, string value, bool fEncode)
        {
            InnerWriter.AddAttribute(name, value, fEncode);
        }

        public virtual void AddStyleAttribute(HtmlTextWriterStyle key, string value)
        {
            InnerWriter.AddStyleAttribute(key, value);
        }

        public virtual void AddStyleAttribute(string name, string value)
        {
            InnerWriter.AddStyleAttribute(name, value);
        }

        public virtual void BeginRender()
        {
            InnerWriter.BeginRender();
        }
        public override void Close()
        {
            InnerWriter.Close();
        }
        public virtual void EndRender()
        {
            InnerWriter.EndRender();
        }

        public override void Flush()
        {
            InnerWriter.Flush();
        }

        public virtual bool IsValidFormAttribute(string attribute)
        {
            return InnerWriter.IsValidFormAttribute(attribute);
        }

        public virtual void RenderBeginTag(HtmlTextWriterTag tagKey)
        {
            InnerWriter.RenderBeginTag(tagKey);
        }

        public virtual void RenderBeginTag(string tagName)
        {
            InnerWriter.RenderBeginTag(tagName);
        }

        public virtual void RenderEndTag()
        {
            InnerWriter.RenderEndTag();
        }

        public override void Write(bool value)
        {
            InnerWriter.Write(value);
        }

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public override void Write(char value)
        {
            InnerWriter.Write(value);
        }

        public override void Write(char[] buffer)
        {
            InnerWriter.Write(buffer);
        }

        public override void Write(double value)
        {
            InnerWriter.Write(value);
        }

        public override void Write(float value)
        {
            InnerWriter.Write(value);
        }

        public override void Write(int value)
        {
            InnerWriter.Write(value);
        }

        public override void Write(long value)
        {
            InnerWriter.Write(value);
        }

        public override void Write(object value)
        {
            InnerWriter.Write(value);
        }

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public override void Write(string s)
        {
            InnerWriter.Write(s);
        }

        public override void Write(string format, object arg0)
        {
            InnerWriter.Write(format, arg0);
        }

        public override void Write(string format, params object[] arg)
        {
            InnerWriter.Write(format, arg);
        }

        public override void Write(char[] buffer, int index, int count)
        {
            InnerWriter.Write(buffer, index, count);
        }

        public override void Write(string format, object arg0, object arg1)
        {
            InnerWriter.Write(format, arg0, arg1);
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public virtual void WriteAttribute(string name, string value)
        {
            InnerWriter.WriteAttribute(name, value);
        }

        public virtual void WriteAttribute(string name, string value, bool fEncode)
        {
            InnerWriter.WriteAttribute(name, value, fEncode);
        }

        public virtual void WriteBeginTag(string tagName)
        {
            InnerWriter.WriteBeginTag(tagName);
        }

        public virtual void WriteBreak()
        {
            InnerWriter.WriteBreak();
        }

        public virtual void WriteEncodedText(string text)
        {
            InnerWriter.WriteEncodedText(text);
        }

        public virtual void WriteEncodedUrl(string url)
        {
            InnerWriter.WriteEncodedUrl(url);
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public virtual void WriteEncodedUrlParameter(string urlText)
        {
            InnerWriter.WriteEncodedUrlParameter(urlText);
        }

        public virtual void WriteEndTag(string tagName)
        {
            InnerWriter.WriteEndTag(tagName);
        }

        public virtual void WriteFullBeginTag(string tagName)
        {
            InnerWriter.WriteFullBeginTag(tagName);
        }

        public override void WriteLine()
        {
            InnerWriter.WriteLine();
        }

        public override void WriteLine(bool value)
        {
            InnerWriter.WriteLine(value);
        }

        public override void WriteLine(char value)
        {
            InnerWriter.WriteLine(value);
        }

        public override void WriteLine(char[] buffer)
        {
            InnerWriter.WriteLine(buffer);
        }

        public override void WriteLine(double value)
        {
            InnerWriter.WriteLine(value);
        }

        public override void WriteLine(float value)
        {
            InnerWriter.WriteLine(value);
        }

        public override void WriteLine(int value)
        {
            InnerWriter.WriteLine(value);
        }

        public override void WriteLine(long value)
        {
            InnerWriter.WriteLine(value);
        }

        public override void WriteLine(object value)
        {
            InnerWriter.WriteLine(value);
        }

        public override void WriteLine(string s)
        {
            InnerWriter.WriteLine(s);
        }

        public override void WriteLine(uint value)
        {
            InnerWriter.WriteLine(value);
        }

        public override void WriteLine(string format, object arg0)
        {
            InnerWriter.WriteLine(format, arg0);
        }

        public override void WriteLine(string format, params object[] arg)
        {
            InnerWriter.WriteLine(format, arg);
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            InnerWriter.WriteLine(buffer, index, count);
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            InnerWriter.WriteLine(format, arg0, arg1);
        }

        public void WriteLineNoTabs(string s)
        {
            InnerWriter.WriteLineNoTabs(s);
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public virtual void WriteStyleAttribute(string name, string value)
        {
            InnerWriter.WriteStyleAttribute(name, value);
        }

        public virtual void WriteStyleAttribute(string name, string value, bool fEncode)
        {
            InnerWriter.WriteStyleAttribute(name, value, fEncode);
        }
    }
}
