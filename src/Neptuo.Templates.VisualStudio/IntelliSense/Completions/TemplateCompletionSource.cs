using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using Neptuo.ComponentModel;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Completions
{
    internal class TemplateCompletionSource : DisposableBase, ICompletionSource
    {
        public const string Moniker = "ntemplate";

        private readonly ITextBuffer textBuffer;

        public TemplateCompletionSource(ITextBuffer textBuffer)
        {
            this.textBuffer = textBuffer;
        }

        public void AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
        {
            List<string> supportedValues = new List<string>();
            supportedValues.Add("addition");
            supportedValues.Add("adaptation");
            supportedValues.Add("subtraction");
            supportedValues.Add("summation");
            List<Completion> result = new List<Completion>();

            string currentToken = textBuffer.CurrentSnapshot.GetText().Split(' ').LastOrDefault();

            foreach (string value in supportedValues)
            {
                string currentValue = currentToken == null ? value : value.Substring(Math.Min(currentToken.Length, value.Length));

                if (currentToken == null || value.StartsWith(currentToken))
                    result.Add(new Completion(value, currentValue, String.Format("Value of '{0}'.", value), null, ""));
            }

            CompletionSet newCompletionSet = new CompletionSet(
                Moniker,
                "Neptuo Templates",
                FindTokenSpanAtPosition(session.GetTriggerPoint(textBuffer), session),
                result,
                null
            );
            //completionSets.Add(MergeCompletionSets(completionSets, newCompletionSet));

            if (completionSets.Any())
                completionSets.RemoveAt(0);

            completionSets.Add(newCompletionSet);
        }

        private CompletionSet MergeCompletionSets(IList<CompletionSet> completionSets, CompletionSet newCompletionSet)
        {
            CompletionSet htmlCompletionSet = completionSets.FirstOrDefault();
            if (htmlCompletionSet != null)
            {
                CompletionSet mergedCompletionSet = new CompletionSet(
                    newCompletionSet.Moniker,
                    newCompletionSet.DisplayName,
                    newCompletionSet.ApplicableTo,
                    newCompletionSet.Completions.Concat(htmlCompletionSet.Completions).OrderBy(n => n.DisplayText),
                    htmlCompletionSet.CompletionBuilders
                );

                completionSets.Remove(htmlCompletionSet);
                return mergedCompletionSet;
            }

            return newCompletionSet;
        }

        private ITrackingSpan FindTokenSpanAtPosition(ITrackingPoint point, ICompletionSession session)
        {
            SnapshotPoint currentPoint = session
                .GetTriggerPoint(textBuffer)
                .GetPoint(textBuffer.CurrentSnapshot);

            return currentPoint.Snapshot.CreateTrackingSpan(
                currentPoint.Position, 
                0, 
                SpanTrackingMode.EdgeInclusive
            );
        }
    }
}
