using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.Framework.Parser;

namespace Neptuo.Web.Framework.Compilation
{
    public static class Compiler
    {
        private static CompilerRegistry<IContentParser, IContentCompiler> contentCompiler;
        private static List<CompilerRegistry<IValueContentParser, IValueCompiler>> valueCompilers;
        private static Func<IServiceProvider> serviceProviderFactory;
        private static Func<IContentParser, ContentCompilerContext> contentCompilerContextFactory;

        static Compiler()
        {
            contentCompiler = null;
            valueCompilers = new List<CompilerRegistry<IValueContentParser, IValueCompiler>>();
        }

        public static void RegisterServiceProviderFactory(Func<IServiceProvider> factory)
        {
            serviceProviderFactory = factory;
        }

        public static void RegisterContentCompilerContextFactory(Func<IContentParser, ContentCompilerContext> factory)
        {
            contentCompilerContextFactory = factory;
        }

        public static void RegisterContentCompiler<T>(Func<IContentParser<T>> parserFactory, Func<IContentCompiler<T>> compilerFactory)
        {
            contentCompiler = new CompilerRegistry<IContentParser, IContentCompiler>
            {
                Type = typeof(T),
                ParserFactory = () => CreateContentParser(parserFactory, compilerFactory),
                CompilerFactory = compilerFactory
            };
        }

        public static void RegisterValueCompiler<T>(Func<IValueContentParser<T>> parserFactory, Func<IValueCompiler> compilerFactory)
        {
            valueCompilers.Add(new CompilerRegistry<IValueContentParser, IValueCompiler>
            {
                Type = typeof(T),
                ParserFactory = () => (IValueContentParser)parserFactory(),
                CompilerFactory = compilerFactory
            });
        }

        public static void CompileContent(string content, CompilerContext context)
        {
            if (contentCompiler == null)
                throw new ArgumentNullException("contentCompiler");

            IContentParser parser = contentCompiler.ParserFactory();
            parser.Parse(content);
        }

        public static void CompileValue(string value, CompilerContext context)
        {
            foreach (var registry in valueCompilers)
            {
                IValueContentParser parser = registry.ParserFactory();
                if (parser.Parse(value))
                    break;
            }

            //TODO: What if there is no value compiler? Return plain value? Register default "empty" value compiler
        }

        private static IContentParser CreateContentParser<T>(Func<IContentParser<T>> parserFactory, Func<IContentCompiler<T>> compilerFactory)
        {
            int lastEndPosition = 0;

            IContentCompiler<T> compiler = compilerFactory();
            IContentParser<T> parser = parserFactory();

            var context = contentCompilerContextFactory(parser);

            parser.OnParsedItem += e =>
            {
                if (e.StartPosition > lastEndPosition)
                    compiler.AppendPlainText(e.OriginalContent.Substring(lastEndPosition, e.StartPosition - lastEndPosition), context);

                lastEndPosition = e.EndPosition;
                compiler.GenerateCode(e.ParsedItem, context);
            };
            parser.OnParserDone += e =>
            {
                if (lastEndPosition < e.OriginalContent.Length)
                    compiler.AppendPlainText(e.OriginalContent.Substring(lastEndPosition), context);
            };

            return parser;
        }

        private static IValueContentParser CreateValueParser<T>(Func<IValueContentParser<T>> parserFactory, Func<IValueCompiler> compilerFactory)
        {
            IValueCompiler compiler = compilerFactory();
            IValueContentParser<T> parser = parserFactory();
            parser.OnParsedItem += e =>
            {
                compiler.GenerateCode(e.ParsedItem, new ValueCompilerContext());
            };

            return parser;
        }

        class CompilerRegistry<TParser, TCompiler>
        {
            public Type Type { get; set; }

            public Func<TParser> ParserFactory { get; set; }

            public Func<TCompiler> CompilerFactory { get; set; }
        }
    }
}
