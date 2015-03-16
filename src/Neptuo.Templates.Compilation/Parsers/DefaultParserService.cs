using Neptuo.ComponentModel;
using Neptuo.Templates.Compilation.CodeObjects;
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
        private readonly Dictionary<string, List<IContentParser>> contentParsers;
        private readonly Dictionary<string, List<IValueParser>> valueParsers;

        public DefaultParserService()
        {
            contentParsers = new Dictionary<string, List<IContentParser>>();
            valueParsers = new Dictionary<string, List<IValueParser>>();
        }

        public IList<IContentParser> GetContentParsers(string name)
        {
            Ensure.NotNull(name, "name");

            List<IContentParser> namedParsers;
            if (!contentParsers.TryGetValue(name, out namedParsers))
                namedParsers = contentParsers[name] = new List<IContentParser>();
            
            return namedParsers;
        }

        public IList<IValueParser> GetValueParsers(string name)
        {
            Ensure.NotNull(name, "name");

            List<IValueParser> namedParsers;
            if (!valueParsers.TryGetValue(name, out namedParsers))
                namedParsers = valueParsers[name] = new List<IValueParser>();

            return namedParsers;
        }

        public ICodeObject ProcessContent(string name, ISourceContent content, IParserServiceContext context)
        {
            Ensure.NotNull(content, "content");
            Ensure.NotNull(context, "context");

            List<IContentParser> namedParsers;
            if (!contentParsers.TryGetValue(name, out namedParsers))
            {
                context.Errors.Add(new ErrorInfo(0, 0, String.Format("Unnable to parse content using name '{0}'.", name)));
                return null;
            }

            foreach (IContentParser contentParser in namedParsers)
            {
                ICodeObject codeObject = contentParser.Parse(content, context.CreateContentContext(name, this));
                if (codeObject != null)
                    return codeObject;
            }

            return null;
        }

        public ICodeObject ProcessValue(string name, ISourceContent value, IParserServiceContext context)
        {
            Ensure.NotNull(value, "value");
            Ensure.NotNull(context, "context");

            List<IValueParser> namedParsers;
            if (!valueParsers.TryGetValue(name, out namedParsers))
            {
                context.Errors.Add(new ErrorInfo(0, 0, String.Format("Unnable to parse value using name '{0}'.", name)));
                return null;
            }

            foreach (IValueParser valueParser in namedParsers)
            {
                ICodeObject codeObject = valueParser.Parse(value, context.CreateValueContext(name, this));
                if (codeObject != null)
                    return codeObject;
            }

            return null;
        }
    }
}
