using Neptuo.Web.Framework.Compilation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.Parsers
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
            DefaultValueParser = new PlainValueParser();
        }

        public bool ProcessContent(string content, IParserServiceContext context)
        {
            if (ContentParsers.Count == 0)
                throw new ArgumentNullException("ContentGenerator");

            IDependencyContainer provider = context.DependencyProvider.CreateChildContainer();
            //if (provider.Resolve<StorageProvider>() == null) TODO: Solve if is registered!
            provider.RegisterInstance<StorageProvider>(new StorageProvider());

            foreach (IContentParser contentParser in ContentParsers)
            {
                if (contentParser.Parse(content, new DefaultParserContext(provider, this, context.PropertyDescriptor)))
                    return true;
            }

            return false;
        }

        public bool ProcessValue(string value, IParserServiceContext context)
        {
            IDependencyContainer provider = context.DependencyProvider.CreateChildContainer();
            //if (provider.Resolve<StorageProvider>() == null) TODO: Solve if is registered!
            provider.RegisterInstance<StorageProvider>(new StorageProvider());

            bool generated = false;
            foreach (IValueParser valueParser in ValueParsers)
            {
                if (valueParser.Parse(value, new DefaultParserContext(provider, this, context.PropertyDescriptor)))
                {
                    generated = true;
                    break;
                }
            }

            if (!generated)
                generated = DefaultValueParser.Parse(value, new DefaultParserContext(provider, this, context.PropertyDescriptor));

            return generated;
        }
    }
}
