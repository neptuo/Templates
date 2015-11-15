using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense
{
    public class TextContext : DisposableBase
    {
        private readonly ITextBuffer textBuffer;

        public event Action<TextContext> TextChanged;

        public string Text
        {
            get { return textBuffer.CurrentSnapshot.GetText(); }
        }

        public TextContext(ITextBuffer textBuffer)
        {
            Ensure.NotNull(textBuffer, "textBuffer");
            this.textBuffer = textBuffer;

            this.textBuffer.Changed += OnTextBufferChanged;
        }

        private void OnTextBufferChanged(object sender, TextContentChangedEventArgs e)
        {
            if (TextChanged != null)
                TextChanged(this);
        }

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
            textBuffer.Changed -= OnTextBufferChanged;
        }
    }
}
