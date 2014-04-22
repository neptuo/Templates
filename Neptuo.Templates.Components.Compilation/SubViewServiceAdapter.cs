using Neptuo.Templates.Compilation.Data;
using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    public class SubViewServiceAdapter
    {
        private CodeDomViewService mainViewService;
        private CodeDomViewService subViewService;

        public SubViewServiceAdapter(CodeDomViewService mainViewService, CodeDomViewService subViewService)
        {
            this.mainViewService = mainViewService;
            this.subViewService = subViewService;

            subViewService.ParserService.OnSearchContentParser += ParserService_OnSearchContentParser;
            subViewService.ParserService.OnSearchValueParser += ParserService_OnSearchValueParser;
            subViewService.ParserService.OnSearchDefaultValueParser += ParserService_OnSearchDefaultValueParser;

            subViewService.PreProcessorService.OnSeachVisitor += mainViewService.PreProcessorService.Process;

            subViewService.CodeGeneratorService.OnSearchGenerator += mainViewService.CodeGeneratorService.GeneratedCode;


            subViewService.NamingService = mainViewService.NamingService;
            subViewService.TempDirectory = mainViewService.TempDirectory;

            foreach (string binDirectory in mainViewService.BinDirectories)
                subViewService.BinDirectories.Add(binDirectory);

            //var binDirectories = ((NotifyList<string>)mainViewService.BinDirectories);
            //binDirectories.OnAddItem += subViewService.BinDirectories.Add;
            //binDirectories.OnRemoveItem += subViewService.BinDirectories.Remove;
            //binDirectories.OnClear += subViewService.BinDirectories.Clear;
        }

        #region Parser events

        private bool ParserService_OnSearchContentParser(string content, IParserServiceContext context)
        {
            if (mainViewService.ParserService.ContentParsers.Count == 0)
                throw new ArgumentNullException("ContentGenerator");

            IDependencyContainer provider = context.DependencyProvider.CreateChildContainer();
            provider.RegisterInstance<StorageProvider>(new StorageProvider());

            foreach (IContentParser contentParser in mainViewService.ParserService.ContentParsers)
            {
                if (contentParser.Parse(content, new DefaultParserContext(provider, subViewService.ParserService, context.PropertyDescriptor, context.Errors)))
                    return true;
            }

            return false;
        }

        private bool ParserService_OnSearchValueParser(string value, IParserServiceContext context)
        {
            IDependencyContainer provider = context.DependencyProvider.CreateChildContainer();
            provider.RegisterInstance<StorageProvider>(new StorageProvider());

            bool generated = false;
            foreach (IValueParser valueParser in mainViewService.ParserService.ValueParsers)
            {
                if (valueParser.Parse(value, new DefaultParserContext(provider, subViewService.ParserService, context.PropertyDescriptor, context.Errors)))
                {
                    generated = true;
                    break;
                }
            }

            if (!generated)
            {
                if (mainViewService.ParserService.DefaultValueParser == null)
                    throw new ArgumentNullException("DefaultValueParser");

                generated = mainViewService.ParserService.DefaultValueParser.Parse(value, new DefaultParserContext(provider, subViewService.ParserService, context.PropertyDescriptor, context.Errors));
            }

            return generated;
        }

        bool ParserService_OnSearchDefaultValueParser(string value, IParserServiceContext context)
        {
            if (mainViewService.ParserService.DefaultValueParser != null)
                return mainViewService.ParserService.DefaultValueParser.Parse(value, new DefaultParserContext(context, subViewService.ParserService));

            return false;
        }

        #endregion
    }
}
