using Neptuo.Templates.Compilation;
using Neptuo.Templates.Compilation.CodeGenerators;
using Neptuo.Templates.Compilation.Parsers;
using Neptuo.Templates.Compilation.PreProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation
{
    public class MvcViewService : IViewService
    {
        public IParserService ParserService { get; private set; }
        public IPreProcessorService PreProcessorService { get; private set; }
        public IFileProvider FileProvider { get; private set; }

        public INamingService NamingService { get; set; }
        public ICollection<string> BinDirectories { get; set; }
        public string TempDirectory { get; set; }
        public bool DebugMode { get; set; }

        public MvcViewService()
        {
            ParserService = new DefaultParserService();
            PreProcessorService = new DefaultPreProcessorService();

            BinDirectories = new List<string>();

            //CodeDomGenerator.SetBaseStructureGenerator(new MvcStructureGenerator<T>());
        }

        public object Process(string fileName, IViewServiceContext context)
        {
            throw new NotImplementedException();
        }

        public object ProcessContent(string viewContent, IViewServiceContext context)
        {
            throw new NotImplementedException();
        }
    }
}
