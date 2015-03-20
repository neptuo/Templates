using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Completions
{
    internal class TemplateCompletionSource : ICompletionSource
    {
        private TemplateCompletionSourceProvider m_sourceProvider;
        private ITextBuffer textBuffer;

        public TemplateCompletionSource(TemplateCompletionSourceProvider sourceProvider, ITextBuffer textBuffer)
        {
            m_sourceProvider = sourceProvider;
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
                if (currentToken == null || value.StartsWith(currentToken))
                    result.Add(new Completion(value));
            }

            completionSets.Add(new CompletionSet(
                "Tokens",    //the non-localized title of the tab 
                "Tokens",    //the display title of the tab
                FindTokenSpanAtPosition(session.GetTriggerPoint(textBuffer), session),
                result,
                null
            ));
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

        private bool m_isDisposed;
        public void Dispose()
        {
            if (!m_isDisposed)
            {
                GC.SuppressFinalize(this);
                m_isDisposed = true;
            }
        }
    }
}
