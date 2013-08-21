using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates
{
    public class HtmlTextWriter : IHtmlWriter
    {
        public static class Html
        {
            public const char StartTag = '<';
            public const char CloseTag = '>';
            public const char Slash = '/';
            public const char Space = ' ';

            public const char Equal = '=';
            public const char DoubleQuote = '"';
            public const char Quote = '\'';
        }

        protected TextWriter InnerWriter { get; private set; }
        protected Stack<string> OpenTags { get; private set; }
        protected bool IsOpenTag { get; set; }
        protected bool HasContent { get; set; }
        protected bool CanWriteAttribute { get; set; }

        public HtmlTextWriter(TextWriter innerWriter)
        {
            InnerWriter = innerWriter;
            OpenTags = new Stack<string>();
        }

        public IHtmlWriter Content(object content)
        {
            EnsureCloseOpeningTag();
            InnerWriter.Write(content);
            return this;
        }

        public IHtmlWriter Content(string content)
        {
            EnsureCloseOpeningTag();
            InnerWriter.Write(content);
            return this;
        }

        public IHtmlWriter Tag(string name)
        {
            EnsureCloseOpeningTag();

            CanWriteAttribute = true;
            HasContent = false;
            IsOpenTag = true;

            OpenTags.Push(name);
            InnerWriter.Write(Html.StartTag);
            InnerWriter.Write(name);
            return this;
        }

        public IHtmlWriter CloseTag()
        {
            WriteCloseTag(HasContent);
            return this;
        }

        public IHtmlWriter CloseFullTag()
        {
            WriteCloseTag(true);
            return this;
        }

        public IHtmlWriter Attribute(string name, string value)
        {
            if (!CanWriteAttribute)
                throw new HtmlTextWriterException("Unnable to write attribute in current state!");

            InnerWriter.Write(Html.Space);
            InnerWriter.Write(name);
            InnerWriter.Write(Html.Equal);
            InnerWriter.Write(Html.DoubleQuote);
            InnerWriter.Write(value);
            InnerWriter.Write(Html.DoubleQuote);

            return this;
        }

        protected void WriteCloseTag(bool hasContent)
        {
            if (OpenTags.Count == 0)
                throw new HtmlTextWriterException("Unnable to close tag! All tags has been closed.");

            string name = OpenTags.Pop();
            if (hasContent)
            {
                EnsureCloseOpeningTag();
                InnerWriter.Write(Html.StartTag);
                InnerWriter.Write(Html.Slash);
                InnerWriter.Write(name);
                InnerWriter.Write(Html.CloseTag);
            }
            else
            {
                IsOpenTag = false;
                InnerWriter.Write(Html.Space);
                InnerWriter.Write(Html.Slash);
                InnerWriter.Write(Html.CloseTag);
            }

            HasContent = true;
        }

        protected void EnsureCloseOpeningTag()
        {
            if (IsOpenTag)
                InnerWriter.Write(Html.CloseTag);

            IsOpenTag = false;
            CanWriteAttribute = false;
            HasContent = true;
        }
    }
}
