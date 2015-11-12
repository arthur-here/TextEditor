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
        private TextEditorDocument document;
        private int caretIndex;
        private int line;
        private int position;
        private int length;
        private List<string> removedLines;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveRangeCommand"/> class.
        /// </summary>
        /// <param name="document">Document insert to.</param>
        /// <param name="caretIndex">Index of caret in document.</param>
        /// <param name="length">Count of chars to delete.</param>
        public RemoveRangeCommand(TextEditorDocument document, int caretIndex, int length)
        {
            if (document == null)
            {
                throw new ArgumentException("Document shouldn't be null");
            }

            this.document = document;
            this.caretIndex = caretIndex;
            this.line = this.document.LineNumberByIndex(caretIndex);
            this.position = this.document.CaretPositionInLineByIndex(caretIndex);
            this.length = length;
        }

        /// <summary>
        /// Executes command.
        /// </summary>
        public void Execute()
        {
            int endCaretIndex = this.caretIndex + this.length;
            if (endCaretIndex > this.document.Text.Length)
            {
                endCaretIndex = this.document.Text.Length;
            }

            int endPosition = this.document.CaretPositionInLineByIndex(endCaretIndex);
            int endLineIndex = this.document.LineNumberByIndex(endCaretIndex);
            this.removedLines = this.document.Lines.GetRange(this.line, endLineIndex - this.line + 1);

            string paragraph = this.document.Lines[this.line];
            string lineToMove = this.document.Lines[endLineIndex].Substring(endPosition);
            if (paragraph.Length > this.position)
            {
                paragraph = paragraph.Remove(this.position);
            }

            this.document.Lines[this.line] = paragraph + lineToMove;
            this.document.Lines.RemoveRange(this.line + 1, endLineIndex - this.line);
        }

        /// <summary>
        /// Undo command.
        /// </summary>
        public void Undo()
        {
            this.document.Lines.RemoveAt(this.line);
            this.document.Lines.InsertRange(this.line, this.removedLines);
        }
    }
}
