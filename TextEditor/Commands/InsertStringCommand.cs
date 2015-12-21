using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using TextEditor.FileManager;

namespace TextEditor.Commands
{
    /// <summary>
    /// Provides command to insert text into document at specified position.
    /// </summary>
    public class InsertStringCommand : ICommand
    {
        private int caretIndex;
        private string text;

        private ITextEditorDocument changedDocument;
        private int line;
        private int position;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertStringCommand"/> class.
        /// </summary>
        /// <param name="text">Text to insert.</param>
        /// <param name="caretIndex">Index of caret in document.</param>
        public InsertStringCommand(string text, int caretIndex)
        {
            if (text == null)
            {
                return;
            }

            this.text = text;
            this.caretIndex = caretIndex;
            this.CaretIndexOffset = text.Length;
        }

        /// <summary>
        /// Gets offset of the caret index after command's execution.
        /// </summary>
        public int CaretIndexOffset { get; private set; }

        /// <summary>
        /// Executes command.
        /// </summary>
        /// <param name="document">Document to run command.</param>
        public void Execute(ITextEditorDocument document)
        {
            if (document == null)
            {
                return;
            }

            this.line = document.LineNumberByIndex(this.caretIndex);
            this.position = document.CaretPositionInLineByIndex(this.caretIndex);
            this.changedDocument = document;

            if (this.line == -1)
            {
                this.line = 0;
                document.AddLine(string.Empty);
            }

            string paragraph = document.AllLines[this.line];
            document.ChangeLineAtIndex(this.line, paragraph.Insert(this.position, this.text));
        }

        /// <summary>
        /// Executes command on new caretIndex.
        /// </summary>
        /// <param name="document">Document to change.</param>
        /// <param name="newCaretIndex">New caretIndex.</param>
        public void Execute(ITextEditorDocument document, int newCaretIndex)
        {
            this.caretIndex = newCaretIndex;
            this.Execute(document);
        }

        /// <summary>
        /// Undo command.
        /// </summary>
        public void Undo()
        {
            string paragraph = this.changedDocument.AllLines.ElementAt(this.line);
            this.changedDocument.ChangeLineAtIndex(this.line, paragraph.Remove(this.position, this.text.Length));
        }
    }
}
