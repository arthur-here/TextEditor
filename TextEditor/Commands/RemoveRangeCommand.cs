using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.Commands
{
    /// <summary>
    /// Provides command to remove string from document at specified position.
    /// </summary>
    public class RemoveRangeCommand : ICommand
    {
        private int caretIndex;
        private int length;

        private TextEditorDocument changedDocument;
        private int line;
        private int position;
        private List<string> removedLines;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveRangeCommand"/> class.
        /// </summary>
        /// <param name="caretIndex">Index of caret in document.</param>
        /// <param name="length">Count of chars to delete.</param>
        public RemoveRangeCommand(int caretIndex, int length)
        {
            this.caretIndex = caretIndex;
            this.length = length;
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

            int endCaretIndex = this.caretIndex + this.length;
            if (endCaretIndex > document.Text.Length)
            {
                endCaretIndex = document.Text.Length;
            }

            int endPosition = document.CaretPositionInLineByIndex(endCaretIndex);
            int endLineIndex = document.LineNumberByIndex(endCaretIndex);
            this.removedLines = document.Lines.GetRange(this.line, endLineIndex - this.line + 1);

            string paragraph = document.Lines[this.line];
            string lineToMove = document.Lines[endLineIndex].Substring(endPosition);
            if (paragraph.Length > this.position)
            {
                paragraph = paragraph.Remove(this.position);
            }

            document.Lines[this.line] = paragraph + lineToMove;
            document.Lines.RemoveRange(this.line + 1, endLineIndex - this.line);
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
            this.changedDocument.Lines.RemoveAt(this.line);
            this.changedDocument.Lines.InsertRange(this.line, this.removedLines);
        }
    }
}
