using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using Neptuo.Templates.VisualStudio.IntelliSense;
using Neptuo.Templates.VisualStudio.IntelliSense.Classifications;
using Neptuo.Templates.VisualStudio.IntelliSense.Completions;
using Neptuo.Templates.VisualStudio.IntelliSense.Completions.Sources;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.VisualStudio.IntelliSense
{
    [Export(typeof(IVsTextViewCreationListener))]
    [Export(typeof(ICompletionSourceProvider))]
    [Export(typeof(IClassifierProvider))]
    [Export(typeof(ITaggerProvider))]
    [TagType(typeof(ErrorTag))]
    [Name(NtContentType.TextValue)]
    [ContentType(NtContentType.TextValue)]
    [TextViewRole(PredefinedTextViewRoles.Interactive)]
    public class NtEditorService : IVsTextViewCreationListener, ICompletionSourceProvider, IClassifierProvider, ITaggerProvider
    {
        [Import]
        internal ViewCommandHandlerFactory ViewCommandHandlerFactory { get; set; }

        [Import]
        internal CompletionSourceFactory CompletionSourceFactory { get; set; }

        [Import]
        internal ClassifierFactory ClassifierFactory { get; set; }

        [Import]
        internal ErrorTaggerFactory ErrorTaggerFactory { get; set; }

        [Import]
        internal IGlyphService GlyphService { get; set; }

        private TokenizerFactory tokenizerFactory;

        private void Initialize()
        {
            tokenizerFactory = new TokenizerFactory();
        }

        private ICompletionTriggerProvider CreateTokenTriggerProvider(ITextBuffer textBuffer)
        {
            return textBuffer.Properties.GetOrCreateSingletonProperty<ICompletionTriggerProvider>(
                () => new CompletionTriggerProviderCollection()
                    .Add(new AngleProvider(GlyphService))
                    .Add(new CurlyProvider(GlyphService, new DummyCurlyCompletionSource()))
            );
        }

        private IAutomaticCompletionProvider CreateAutomaticCompletionProvider(ITextBuffer textBuffer)
        {
            return textBuffer.Properties.GetOrCreateSingletonProperty<IAutomaticCompletionProvider>(
                () => new AutomaticCompletionProviderCollection()
                    .Add(new AngleProvider(GlyphService))
                    .Add(new CurlyProvider(GlyphService, new DummyCurlyCompletionSource()))
            );
        }

        private ICompletionProvider CreateCompletionProvider(ITextBuffer textBuffer)
        {
            return textBuffer.Properties.GetOrCreateSingletonProperty<ICompletionProvider>(
                () => new CompletionProviderCollection()
                    .Add(new AngleProvider(GlyphService))
                    .Add(new CurlyProvider(GlyphService, new DummyCurlyCompletionSource()))
            );
        }

        private ITokenClassificationProvider CreateTokenClassifierProvider(ITextBuffer textBuffer)
        {
            return textBuffer.Properties.GetOrCreateSingletonProperty<ITokenClassificationProvider>(
                () => new TokenClassificationProviderCollection()
                    .Add(new AngleProvider(GlyphService))
                    .Add(new CurlyProvider(GlyphService, new DummyCurlyCompletionSource()))
            );
        }

        private ISyntaxNodeFactory CreateSyntaxNodeFactory()
        {
            ISyntaxNodeFactory builder = new SyntaxNodeBuilderCollection()
                .Add(AngleTokenType.OpenBrace, new AngleSyntaxNodeBuilder())
                .Add(CurlyTokenType.OpenBrace, new CurlySyntaxNodeBuilder())
                .Add(TokenType.Literal, new LiteralSyntaxNodeBuilder());

            return builder;
        }

        private SyntaxContext CreateSyntaxContext(ITextBuffer textBuffer)
        {
            return textBuffer.Properties.GetOrCreateSingletonProperty(
                () =>
                {
                    SyntaxContext context = new SyntaxContext(
                        textBuffer.Properties.GetProperty<TokenContext>(typeof(TokenContext)),
                        CreateSyntaxNodeFactory()
                    );
                    context.RootNodeChanged += OnSyntaxRootNodeChanged;
                    return context;
                }
            );
        }

        private void OnSyntaxRootNodeChanged(SyntaxContext context)
        {

        }

        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            IViewCommandHandler viewCommandHandler;
            if (!ViewCommandHandlerFactory.TryGet(textViewAdapter, out viewCommandHandler))
            {
                ITextBuffer textBuffer;
                if (ViewCommandHandlerFactory.TryGetTextBuffer(textViewAdapter, out textBuffer))
                {
                    Initialize();
                    viewCommandHandler = ViewCommandHandlerFactory.Create(
                        textViewAdapter,
                        tokenizerFactory.Create(textBuffer),
                        CreateTokenTriggerProvider(textBuffer),
                        CreateAutomaticCompletionProvider(textBuffer)
                    );

                    CreateSyntaxContext(textBuffer);
                }
            }
        }

        public ICompletionSource TryCreateCompletionSource(ITextBuffer textBuffer)
        {
            ICompletionSource completionSource;
            if (!CompletionSourceFactory.TryGet(textBuffer, out completionSource))
            {
                CreateSyntaxContext(textBuffer);

                Initialize();
                completionSource = CompletionSourceFactory.Create(
                    textBuffer, 
                    tokenizerFactory.Create(textBuffer),
                    CreateSyntaxNodeFactory(),
                    new SyntaxVisitorFactory().Create(),
                    CreateCompletionProvider(textBuffer),
                    CreateTokenTriggerProvider(textBuffer)
                );
            }

            return completionSource;
        }

        public IClassifier GetClassifier(ITextBuffer textBuffer)
        {
            IClassifier classifier;
            if (!ClassifierFactory.TryGet(textBuffer, out classifier))
            {
                Initialize();
                classifier = ClassifierFactory.Create(
                    textBuffer,
                    tokenizerFactory.Create(textBuffer),
                    CreateTokenClassifierProvider(textBuffer)
                );

                CreateSyntaxContext(textBuffer);
            }

            return classifier;
        }

        public ITagger<T> CreateTagger<T>(ITextBuffer textBuffer) 
            where T : ITag
        {
            ITagger<IErrorTag> tagger;
            if (!ErrorTaggerFactory.TryGet(textBuffer, out tagger))
            {
                tagger = ErrorTaggerFactory.Create(
                    textBuffer,
                    tokenizerFactory.Create(textBuffer)
                );

                CreateSyntaxContext(textBuffer);
            }

            return (ITagger<T>)tagger;
        }
    }
}
