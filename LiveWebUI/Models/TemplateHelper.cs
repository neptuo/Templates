using LiveWebUI.Models;
using Neptuo.Security.Cryptography;
using Neptuo.Templates;
using Neptuo.Templates.Compilation;
using Neptuo.Templates.Compilation.CodeGenerators;
using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestConsoleNG;
using TestConsoleNG.Controls;
using TestConsoleNG.Data;
using TestConsoleNG.Extensions;
using TestConsoleNG.Observers;
using TestConsoleNG.SimpleContainer;

namespace LiveWebUI.Models
{
    public static class TemplateHelper
    {
        public static readonly IDependencyContainer Container;
        public static readonly ExtendedViewService ViewService;
        public static readonly IViewRepository ViewRepository;

        static TemplateHelper()
        {
            ViewRepository = new MemoryViewRepository();


            Container = new SimpleObjectBuilder();
            Container.RegisterInstance<IFileProvider>(new FileProvider(new CurrentDirectoryVirtualPathProvider()));
            Container.RegisterInstance<DataStorage>(new DataStorage(new PersonModel("Jon", "Doe", new AddressModel("Dlouhá street", 23, "Prague", 10001))));
            Container.RegisterInstance<IValueConverterService>(new ValueConverterService().SetConverter("NullToBool", new NullToBoolValueConverter()));


            TypeBuilderRegistry registry = new TypeBuilderRegistry(
                new TypeBuilderRegistryConfiguration(Container),
                new LiteralControlBuilder<LiteralControl>(c => c.Text),
                new GenericContentControlBuilder<GenericContentControl>(c => c.TagName)
            );
            registry.RegisterNamespace(new NamespaceDeclaration("h", "TestConsoleNG.Controls, TestConsoleNG.Components"));
            registry.RegisterNamespace(new NamespaceDeclaration(null, "TestConsoleNG.Extensions, TestConsoleNG.Components"));
            registry.RegisterObserverBuilder("data", "*", new DefaultTypeObserverBuilderFactory(typeof(DataContextObserver)));
            registry.RegisterObserverBuilder("ui", "Visible", new DefaultTypeObserverBuilderFactory(typeof(VisibleObserver)));

            ViewService = new ExtendedViewService();
            ViewService.ParserService.ContentParsers.Add(new XmlContentParser(registry));
            ViewService.ParserService.DefaultValueParser = new PlainValueParser();
            ViewService.ParserService.ValueParsers.Add(new MarkupExtensionValueParser(registry));
            ViewService.NamingService = new UniqueNamingService();
            //viewService.DebugMode = true;
            ViewService.TempDirectory = @"C:\Temp\NeptuoLiveTemplates";
            ViewService.BinDirectories.Add(HttpContext.Current.Server.MapPath("~/Bin"));
            CODEDOMREGISTEREXTENSIONS.Register(ViewService.CodeDomGenerator);

            Container.RegisterInstance<IViewService>(ViewService);
            Container.RegisterInstance<IComponentManager>(new ComponentManager());
        }

    }
}