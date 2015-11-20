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

        private TextEditorDocument changedDocument;
        private int line;
        private int position;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertStringCommand"/> class.
        /// </summary>
        /// <param name="text">Text to insert.</param>
        /// <param name="caretIndex">Index of caret in document.</param>
        public InsertStringCommand(string text, int caretIndex)
        {
            this.text = text;
            this.caretIndex = caretIndex;
        }

        /// <summary>
        /// Executes command.
        /// </summary>
        /// <param name="document">Document to run command.</param>
        public void Execute(TextEditorDocument document)
        {
            if (document == null)
            {
                return;
            }

            this.line = document.LineNumberByIndex(this.caretIndex);
            this.position = document.CaretPositionInLineByIndex(this.caretIndex);
            this.changedDocument = document;

            string paragraph = document.Lines[this.line];
            document.Lines[this.line] = paragraph.Insert(this.position, this.text);
        }

        /// <summary>
        /// Executes command on new caretIndex.
        /// </summary>
        /// <param name="document">Document to change.</param>
        /// <param name="newCaretIndex">New caretIndex.</param>
        public void Execute(TextEditorDocument document, int newCaretIndex)
        {
            this.caretIndex = newCaretIndex;
            this.Execute(document);
        }

        /// <summary>
        /// Undo command.
        /// </summary>
        public void Undo()
        {
            string paragraph = this.changedDocument.Lines.ElementAt(this.line);
            this.changedDocument.Lines[this.line] = paragraph.Remove(this.position, this.text.Length);
        }
    }
}
