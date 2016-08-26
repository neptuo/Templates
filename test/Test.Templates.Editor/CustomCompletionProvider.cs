using Microsoft.Html.Editor.Completion.Def;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Html.Editor.Completion;
using Microsoft.VisualStudio.Utilities;
using Microsoft.Html.Core.Artifacts;
using Microsoft.Web.Core.Text;
using System.Text.RegularExpressions;
using Microsoft.Web.Core.ContentTypes;

namespace Neptuo.Templates
{
    //[Export(typeof(IHtmlCompletionListProvider))]
    [HtmlCompletionProvider(CompletionTypes.Any, "*", "*")]
    [ContentType(HtmlContentTypeDefinition.HtmlContentType)]
    class CustomCompletionProvider : IHtmlCompletionListProvider
    {
        public IList<HtmlCompletion> GetEntries(HtmlCompletionContext context)
        {
            return new[] {
                new HtmlCompletion("YES", "YES", "yes", null, "", context.Session),
                new HtmlCompletion("NO", "NO", "no", null, "", context.Session),
            };
        }
    }

    //[HtmlCompletionProvider(CompletionTypes.Any, "*", "*")]
    //[ContentType(NtContentType.TextValue)]
    //class NtCompletionProvider : IHtmlCompletionListProvider
    //{
    //    public IList<HtmlCompletion> GetEntries(HtmlCompletionContext context)
    //    {
    //        return new[] {
    //            new HtmlCompletion("ui:Template", "ui:Template", "ui:Template", null, "", context.Session),
    //        };
    //    }
    //}


    //[HtmlCompletionFilter(CompletionTypes.Any)]
    //[ContentType(NtContentType.TextValue)]
    //class CustomCompletionFilter : IHtmlCompletionListFilter
    //{
    //    public void FilterCompletionList(IList<HtmlCompletion> completions, HtmlCompletionContext context)
    //    {
    //        HtmlCompletion completion = completions.FirstOrDefault(c => c.DisplayText == "NO");
    //        if (completion != null)
    //            completions.Remove(completion);
    //    }
    //}

    //[HtmlCompletionProvider(CompletionTypes.Any, "*", "*")]
    //[Export(typeof(IHtmlSnippetListProvider))]
    //[ContentType(NtContentType.TextValue)]
    //class CustomSnippetListProvider : IHtmlSnippetListProvider
    //{
    //    public IList<HtmlCompletion> GetEntries(HtmlCompletionContext context, IEnumerable<HtmlCompletion> completions)
    //    {
    //        return new[]
    //        {
    //            new HtmlCompletion("MY SNIPPET", "This is created by MY SNIPPET", "Hmm", null, "", context.Session)
    //        };
    //    }
    //}



    //[Export(typeof(IArtifactProcessorProvider))]
    //[ContentType(NtContentType.TextValue)]
    //public class MyArtifactProcessorProvider : IArtifactProcessorProvider
    //{
    //    public IArtifactProcessor GetProcessor()
    //    {
    //        return new MyArtifactProcessor();
    //    }
    //}

    //public class MyArtifactProcessor : IArtifactProcessor
    //{
    //    public bool IsReady => true;

    //    public string LeftCommentSeparator => "";

    //    public string LeftSeparator => "☃";

    //    public string RightCommentSeparator => "";

    //    public string RightSeparator => "☃";

    //    public void GetArtifacts(ITextProvider text, ArtifactCollection artifactCollection)
    //    {
    //        foreach (var a in FindArtifacts(text.GetText(new TextRange(0, text.Length))))
    //        {
    //            artifactCollection.Add(a);
    //        }
    //    }

    //    Regex regex = new Regex("☃.*?☃", RegexOptions.Compiled | RegexOptions.Singleline);
    //    IEnumerable<MyArtifact> FindArtifacts(string text)
    //    {
    //        return regex.Matches(text).OfType<Match>()
    //            .Select(m => new MyArtifact(m.Index, m.Length));
    //    }
    //}

    //class MyArtifact : Artifact
    //{
    //    public MyArtifact(int start, int len)
    //        : base(ArtifactTreatAs.Code, new TextRange(start, len), 1, 1, "Default", true)
    //    {

    //    }
    //}
}
