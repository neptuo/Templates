using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Implementation of <see cref="IParserService"/>.
    /// </summary>
    public class DefaultParserService : IParserService
    {
        public IList<IContentParser> ContentParsers { get; private set; }
        public IList<IValueParser> ValueParsers { get; private set; }
        public IValueParser DefaultValueParser { get; set; }

        public DefaultParserService()
        {
            ContentParsers = new List<IContentParser>();
            ValueParsers = new List<IValueParser>();
        }

        public ICodeObject ProcessContent(string content, IParserServiceContext context)
        {
            Guard.NotNullOrEmpty(content, "content");
            Guard.NotNull(context, "context");

            if (ContentParsers.Count == 0)
                throw new ArgumentNullException("ContentGenerator");

            foreach (IContentParser contentParser in ContentParsers)
            {
                ICodeObject codeObject = contentParser.Parse(content, context.CreateContentContext(this));
                if (codeObject != null)
                    return codeObject;
            }

            return null;
        }

        public ICodeObject ProcessValue(string value, IParserServiceContext context)
        {
            Guard.NotNullOrEmpty(value, "value");
            Guard.NotNull(context, "context");

            ICodeObject codeObject = null;
            bool generated = false;
            foreach (IValueParser valueParser in ValueParsers)
            {
                codeObject = valueParser.Parse(value, context.CreateValueContext(this));
                if (codeObject != null)
                {
                    generated = true;
                    break;
                }
            }

            if (!generated)
            {
                if (DefaultValueParser == null)
                    throw new ArgumentNullException("DefaultValueParser");

                codeObject = DefaultValueParser.Parse(value, context.CreateValueContext(this));
            }

            return codeObject;
        }
    }
}
