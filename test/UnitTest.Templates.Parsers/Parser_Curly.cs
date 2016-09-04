using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neptuo.Models.Features;
using Neptuo.Templates.Compilation;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers;
using Neptuo.Templates.Compilation.Parsers.Descriptors;
using Neptuo.Templates.Compilation.Parsers.Descriptors.Features;
using Neptuo.Templates.Compilation.Parsers.Normalization;
using Neptuo.Templates.Compilation.Parsers.Syntax;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using Neptuo.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Templates.Parsers.Extensions;
using UnitTest.Templates.Parsers.Tokenizers;

namespace UnitTest.Templates.Parsers
{
    [TestClass]
    public class Parser_Curly : TestTokenizerBase
    {
        [TestMethod]
        public void Basic()
        {
            DefaultComponentDescriptor bindingDescriptor = new DefaultComponentDescriptor();
            bindingDescriptor
                .Add<IFieldEnumerator>(new TypePropertyFieldEnumerator(typeof(BindingExtension)));

            CodeObjectBuilderCollection codeObjectBuilders = new CodeObjectBuilderCollection();
            codeObjectBuilders
                 .Add(typeof(NodeCollection), new CollectionCodeObjectBuilder(codeObjectBuilders))
                 .Add(typeof(LiteralNode), new TextCodeObjectBuilder())
                 .Add(typeof(CurlyNode), new CurlyCodeObjectBuilder(bindingDescriptor));

            CodePropertyBuilderCollection codePropertyBuilders = new CodePropertyBuilderCollection();
            codePropertyBuilders
                .AddSearchHandler(CreateCodePropertyBuilder);

            IParserProvider parserProvider = new DefaultParserCollection()
                .Add<ICodeObjectBuilder>(codeObjectBuilders)
                .Add<ICodePropertyBuilder>(codePropertyBuilders)
                .AddPropertyNormalizer(new LowerInvariantNameNormalizer());

            SyntaxParserService parserService = new SyntaxParserService(parserProvider);
            parserService
                .AddCurlyTokenBuilder()
                .AddAngleTokenBuilder()
                .AddLiteralTokenBuilder();

            parserService.NodeBuilders
                .Add(CurlyTokenType.OpenBrace, new CurlyNodeBuilder())
                .Add(TokenType.Literal, new LiteralNodeBuilder());

            ICodeObject codeObject = parserService.ProcessContent(
                "Default", 
                new DefaultSourceContent("Text {data:Binding Path=ID, Converter=Static} Text {ui:Template Path=~/Test.nt}"), 
                new DefaultParserServiceContext(new FakeDependencyProvider())
            );
            Console.WriteLine(codeObject);
        }

        private bool CreateCodePropertyBuilder(Type propertyType, out ICodePropertyBuilder builder)
        {
            builder = new DefaultCodePropertyBuilder();
            return true;
        }
    }
}
