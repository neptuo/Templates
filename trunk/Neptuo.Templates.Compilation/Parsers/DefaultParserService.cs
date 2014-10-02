using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class DefaultParserService : IParserService
    {
        public IList<IContentParser> ContentParsers { get; set; }
        public IList<IValueParser> ValueParsers { get; set; }
        public IValueParser DefaultValueParser { get; set; }

        public DefaultParserService()
        {
            ContentParsers = new List<IContentParser>();
            ValueParsers = new List<IValueParser>();
        }

        public ICodeObject ProcessContent(string content, IParserServiceContext context)
        {
            if (ContentParsers.Count == 0)
                throw new ArgumentNullException("ContentGenerator");

            IDependencyContainer provider = context.DependencyProvider.CreateChildContainer();
            //if (provider.Resolve<StorageProvider>() == null) TODO: Solve if is registered!
            provider.RegisterInstance<StorageProvider>(new StorageProvider());

            foreach (IContentParser contentParser in ContentParsers)
            {
                ICodeObject codeObject = contentParser.Parse(content, new DefaultParserContext(provider, this, context.Errors));
                if (codeObject != null)
                    return codeObject;
            }

            return null;
        }

        public ICodeObject ProcessValue(string value, IParserServiceContext context)
        {
            IDependencyContainer provider = context.DependencyProvider.CreateChildContainer();
            //if (provider.Resolve<StorageProvider>() == null) TODO: Solve if is registered!
            provider.RegisterInstance<StorageProvider>(new StorageProvider());

            ICodeObject codeObject = null;
            bool generated = false;
            foreach (IValueParser valueParser in ValueParsers)
            {
                codeObject = valueParser.Parse(value, new DefaultParserContext(provider, this, context.Errors));
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

                codeObject = DefaultValueParser.Parse(value, new DefaultParserContext(provider, this, context.Errors));
            }

            return codeObject;
        }
    }
}
