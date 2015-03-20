using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;

namespace Neptuo.Templates.VisualStudio.Completions
{
    internal class TestCompletionSource : ICompletionSource
    {
        private TestCompletionSourceProvider m_sourceProvider;
        private ITextBuffer m_textBuffer;
        private List<Completion> m_compList;

        public TestCompletionSource(TestCompletionSourceProvider sourceProvider, ITextBuffer textBuffer)
        {
            m_sourceProvider = sourceProvider;
            m_textBuffer = textBuffer;
        }

        void ICompletionSource.AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
        {
            List<string> strList = new List<string>();
            strList.Add("addition");
            strList.Add("adaptation");
            strList.Add("subtraction");
            strList.Add("summation");
            m_compList = new List<Completion>();
            foreach (string str in strList)
                m_compList.Add(new Completion(str));

            completionSets.Add(new CompletionSet(
                "Tokens",    //the non-localized title of the tab 
                "Tokens",    //the display title of the tab
                FindTokenSpanAtPosition(session.GetTriggerPoint(m_textBuffer),
                    session),
                m_compList,
                null));
        }

        private ITrackingSpan FindTokenSpanAtPosition(ITrackingPoint point, ICompletionSession session)
        {
            var currentPoint = session.GetTriggerPoint(m_textBuffer).GetPoint(m_textBuffer.CurrentSnapshot);
            return currentPoint.Snapshot.CreateTrackingSpan(currentPoint.Position, 0, SpanTrackingMode.EdgeInclusive);
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
